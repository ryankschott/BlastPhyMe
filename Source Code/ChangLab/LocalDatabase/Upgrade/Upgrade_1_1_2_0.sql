SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [BlastN].[Alignment_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@QueryID uniqueidentifier,
	@SubjectID uniqueidentifier,
	@Rank int,
	@ClearExons bit = 0
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM BlastN.Alignment ex
						WHERE ex.QueryID = @QueryID
							AND ex.SubjectID = @SubjectID) BEGIN
		SET @ID = NEWID()

		INSERT INTO BlastN.Alignment (ID, QueryID, SubjectID, [Rank])
		VALUES (@ID, @QueryID, @SubjectID, @Rank)
	END
	ELSE BEGIN
		SELECT @ID = ex.ID 
			FROM BlastN.Alignment ex
			WHERE ex.QueryID = @QueryID
				AND ex.SubjectID = @SubjectID

		UPDATE BlastN.Alignment
			SET [Rank] = @Rank
			WHERE ID = @ID
	END

	IF @ClearExons = 1 BEGIN
		DELETE FROM BlastN.AlignmentExon
			WHERE AlignmentID = @ID
	END
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('BlastN.Alignment_QueryCover')) BEGIN
	DROP FUNCTION BlastN.Alignment_QueryCover
END
GO
CREATE FUNCTION BlastN.Alignment_QueryCover(@AlignmentID uniqueidentifier)
RETURNS int
AS
BEGIN

	RETURN (
		SELECT Common.CalculatePercentageFromInt(

			(SELECT SUM(ex.AlignmentLength)
				FROM BlastN.AlignmentExon ex
				WHERE ex.AlignmentID = @AlignmentID)
			,
			(SELECT LEN(g.Nucleotides)
				FROM BlastN.Alignment al
				JOIN Gene.Gene g ON g.ID = al.QueryID
				WHERE al.ID = @AlignmentID)
	))
END
GO
ALTER PROCEDURE [Job].[BlastN_ListAlignmentsForJob]
	@JobID uniqueidentifier,
	@RecordSetID uniqueidentifier,
	@FilterByID int = NULL
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @JobStatusID_Archived int

	IF @RecordSetID IS NULL AND @JobID IS NULL BEGIN
		RAISERROR('A Job ID or a RecordSet ID must be provided', 11, 1)
	END

	IF @RecordSetID IS NULL BEGIN
		SELECT @RecordSetID = j.RecordSetID
			FROM Job.Job j
			WHERE j.ID = @JobID
	END

	SELECT @JobStatusID_Archived = ID 
		FROM Job.[Status] 
		WHERE [Key] = 'Archived'

	SELECT sbj.ID
			,sbj.SourceID
			,sbj.GenBankID
			,sbj.[Definition]
			,sbj.SequenceStart
			,sbj.SequenceEnd
			,MAX(al.[Rank]) AS [Rank]
			,MAX(BlastN.Alignment_MaxAlignmentPercentage(al.ID)) AS AlignmentPercentage
			,MAX(BlastN.Alignment_QueryCover(al.ID)) AS QueryCover

			,CAST(CASE WHEN EXISTS (SELECT * FROM RecordSet.Gene rs_g WHERE rs_g.GeneID = sbj.ID)
					   THEN 1
					   ELSE 0
					   END AS bit) AS InRecordSet
		FROM NCBI.Request r
		JOIN NCBI.BlastNAlignment nal ON nal.RequestID = r.ID
		JOIN BlastN.Alignment al ON al.ID = nal.AlignmentID
		JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
		JOIN Job.Job j ON j.ID = r.JobID
		WHERE
			j.RecordSetID = @RecordSetID
			AND 
			(
				(@JobID IS NOT NULL AND r.JobID = @JobID)
				OR
				(@JobID IS NULL AND j.StatusID <> @JobStatusID_Archived)
			)
			AND (
					(@FilterByID = 1)
					OR
					(@FilterByID = 2 AND NOT EXISTS (SELECT *
														FROM RecordSet.Gene rs_g
														JOIN Gene.Gene g ON g.ID = rs_g.GeneID
														WHERE rs_g.RecordSetID = @RecordSetID
															AND g.GenBankID = sbj.GenBankID))
					OR
					(@FilterByID = 3 AND NOT EXISTS (SELECT *
														FROM RecordSet.Gene rs_g
														JOIN Gene.Gene g ON g.ID = rs_g.GeneID
														WHERE rs_g.RecordSetID = @RecordSetID
															AND g.Organism = sbj.Organism))
			)
		GROUP BY sbj.ID, sbj.SourceID, sbj.GenBankID, sbj.[Definition], sbj.SequenceStart, sbj.SequenceEnd
		ORDER BY [Rank], AlignmentPercentage DESC
END
GO
ALTER PROCEDURE [Job].[BlastN_ListQueryGenesForAlignment]
	@SubjectGeneID uniqueidentifier,
	@JobID uniqueidentifier,
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF @RecordSetID IS NULL AND @JobID IS NULL BEGIN
		RAISERROR('A Job ID or a RecordSet ID must be provided', 11, 1)
	END
		
	IF @RecordSetID IS NULL BEGIN
		SELECT @RecordSetID = j.RecordSetID
			FROM Job.Job j
			WHERE j.ID = @JobID
	END

	SELECT qry.ID
			,qsrc.Name AS QuerySourceName
			,qry.GenBankID
			,qry.[Definition]
			,qry.Organism
			,qry.Taxonomy
			,al.[Rank]
			,ex.AlignmentLength
			,ex.BitScore
			,CAST(Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength) AS int) AS AlignmentPercentage
			,BlastN.Alignment_QueryCover(al.ID) AS QueryCover
			,ex.Gaps
		FROM NCBI.Request r
		JOIN NCBI.BlastNAlignment nal ON nal.RequestID = r.ID
		JOIN BlastN.Alignment al ON al.ID = nal.AlignmentID
		JOIN BlastN.AlignmentExon ex ON ex.ID = BlastN.AlignmentExon_First(al.ID)
		JOIN Gene.Gene qry ON qry.ID = al.QueryID
		JOIN Gene.[Source] qsrc ON qsrc.ID = qry.SourceID
		JOIN Job.Job j ON j.ID = r.JobID
		WHERE al.SubjectID = @SubjectGeneID
			AND j.RecordSetID = @RecordSetID
			AND (@JobID IS NULL OR r.JobID = @JobID)
		ORDER BY AlignmentPercentage DESC, BitScore DESC
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.BlastN_ListSubjectGenesForQueryGene')) BEGIN
	DROP PROCEDURE Job.BlastN_ListSubjectGenesForQueryGene
END
GO
CREATE PROCEDURE Job.BlastN_ListSubjectGenesForQueryGene
	@QueryGeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON
		
	SELECT sbj.ID
			,ssrc.Name AS QuerySourceName
			,sbj.GenBankID
			,sbj.[Definition]
			,sbj.Organism
			,sbj.Taxonomy
			,al.[Rank]
			,ex.AlignmentLength
			,ex.BitScore
			,CAST(Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength) AS int) AS AlignmentPercentage
			,ex.Gaps

			,CAST(CASE WHEN EXISTS (SELECT * FROM RecordSet.Gene rs_g WHERE rs_g.GeneID = sbj.ID)
					   THEN 1
					   ELSE 0
					   END AS bit) AS InRecordSet
		FROM BlastN.Alignment al
		JOIN BlastN.AlignmentExon ex ON ex.ID = BlastN.AlignmentExon_First(al.ID)
		JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
		JOIN Gene.[Source] ssrc ON ssrc.ID = sbj.SourceID
		WHERE al.QueryID = @QueryGeneID
		ORDER BY AlignmentPercentage DESC, BitScore DESC
END
GO

ALTER PROCEDURE [RecordSet].[SubSetGene_List]
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.ID
			,g.[Definition]
			,g.SourceID
			,g.LastUpdatedAt
			,rs_g.ModifiedAt

			,g.GenBankID
			,g.Locus
			,g.Accession
			,g.Organism
			,g.Taxonomy

			,g.Nucleotides
			,g.SequenceTypeID
			,g.SequenceStart
			,g.SequenceEnd
			,g.CodingSequenceStart
			,g.CodingSequenceEnd

			,CASE WHEN (t.ID IS NOT NULL)
				  THEN t.HID.ToString()
				  ELSE ''
				  END AS TaxonomyHierarchy
			
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.QueryID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedSubjectSequences
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.SubjectID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedQuerySequences

		FROM RecordSet.SubSetGene sg
		JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
		JOIN RecordSet.Gene rs_g ON rs_g.GeneID = sg.GeneID AND rs_g.RecordSetID = sub.RecordSetID
		JOIN Gene.Gene g ON g.ID = rs_g.GeneID
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		
		WHERE sg.SubSetID = @SubSetID
			AND g.Active = 1
		ORDER BY rs_g.ModifiedAt DESC, g.Organism, g.Accession, g.GenBankID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_Get')) BEGIN
	DROP PROCEDURE Gene.Gene_Get
END
GO
CREATE PROCEDURE Gene.Gene_Get
	@GeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.ID
			,g.[Definition]
			,g.SourceID
			,g.LastUpdatedAt

			,g.GenBankID
			,g.Locus
			,g.Accession
			,g.Organism
			,g.Taxonomy

			,g.Nucleotides
			,g.SequenceTypeID
			,g.SequenceStart
			,g.SequenceEnd
			,g.CodingSequenceStart
			,g.CodingSequenceEnd

			,CASE WHEN (t.ID IS NOT NULL)
				  THEN t.HID.ToString()
				  ELSE ''
				  END AS TaxonomyHierarchy
			
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.QueryID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedSubjectSequences
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.SubjectID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedQuerySequences

		FROM Gene.Gene g
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		
		WHERE g.ID = @GeneID
		ORDER BY g.Organism, g.Accession, g.GenBankID
END
GO
ALTER PROCEDURE [RecordSet].[SubSet_Opened]
	@ID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	UPDATE RecordSet.SubSet
		SET [Open] = 1,
			LastOpenedAt = SYSDATETIME()
		WHERE ID = @ID
END
GO

IF NOT EXISTS (SELECT * FROM sys.types t WHERE t.schema_id = SCHEMA_ID('Common') AND t.name = 'ListInt') BEGIN
	CREATE TYPE Common.ListInt AS TABLE  (Value int NOT NULL)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_ExistsByGenBankID')) BEGIN
	DROP PROCEDURE Gene.Gene_ExistsByGenBankID
END
GO
CREATE PROCEDURE Gene.Gene_ExistsByGenBankID
	@GenBankIDs Common.ListInt READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT l.Value AS ID
			,CAST(CASE WHEN g.ID IS NOT NULL THEN 1 ELSE 0 END AS bit) AS [Exists]
		FROM @GenBankIDs l
		LEFT OUTER JOIN Gene.Gene g ON g.GenBankID = l.Value
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('RecordSet.SubSetGene') AND c.name = 'ModifiedAt') BEGIN
	ALTER TABLE RecordSet.SubSetGene ADD ModifiedAt datetime2(7) NOT NULL CONSTRAINT DF_RecordSet_SubSet_ModifiedAt DEFAULT (sysdatetime())

	EXEC('UPDATE ssg
			SET ModifiedAt = g.ModifiedAt
			FROM RecordSet.SubSetGene ssg
			JOIN RecordSet.SubSet sub ON sub.ID = ssg.SubSetID
			JOIN RecordSet.Gene g ON g.GeneID = ssg.GeneID AND g.RecordSetID = sub.RecordSetID')
END
GO
ALTER PROCEDURE [RecordSet].[Gene_Edit]
	@RecordSetID uniqueidentifier,
	@GeneID uniqueidentifier,
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM RecordSet.Gene WHERE RecordSetID = @RecordSetID AND GeneID = @GeneID) BEGIN
		INSERT INTO RecordSet.Gene (RecordSetID, GeneID)
		VALUES (@RecordSetID, @GeneID)
	END
	ELSE BEGIN
		UPDATE RecordSet.Gene
			SET ModifiedAt = SYSDATETIME()
			WHERE RecordSetID = @RecordSetID
				AND GeneID = @GeneID
	END

	IF NOT EXISTS (SELECT * FROM RecordSet.SubSetGene WHERE GeneID = @GeneID AND SubSetID = @SubSetID) BEGIN
		INSERT INTO RecordSet.SubSetGene (SubSetID, GeneID)
		VALUES (@SubSetID, @GeneID)
	END
	ELSE BEGIN
		UPDATE RecordSet.Gene
			SET ModifiedAt = SYSDATETIME()
			WHERE RecordSetID = @RecordSetID
				AND GeneID = @GeneID
	END
END
GO
ALTER PROCEDURE [RecordSet].[SubSetGene_List]
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.ID
			,g.[Definition]
			,g.SourceID
			,g.LastUpdatedAt
			,sg.ModifiedAt

			,g.GenBankID
			,g.Locus
			,g.Accession
			,g.Organism
			,g.Taxonomy

			,g.Nucleotides
			,g.SequenceTypeID
			,g.SequenceStart
			,g.SequenceEnd
			,g.CodingSequenceStart
			,g.CodingSequenceEnd

			,CASE WHEN (t.ID IS NOT NULL)
				  THEN t.HID.ToString()
				  ELSE ''
				  END AS TaxonomyHierarchy
			
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.QueryID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedSubjectSequences
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.SubjectID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedQuerySequences

		FROM RecordSet.SubSetGene sg
		JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
		JOIN RecordSet.Gene rs_g ON rs_g.GeneID = sg.GeneID AND rs_g.RecordSetID = sub.RecordSetID
		JOIN Gene.Gene g ON g.ID = rs_g.GeneID
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		
		WHERE sg.SubSetID = @SubSetID
			AND g.Active = 1
		ORDER BY rs_g.ModifiedAt DESC, g.Organism, g.Accession, g.GenBankID
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_ListAllOrganismNames')) BEGIN
	DROP PROCEDURE Gene.Gene_ListAllOrganismNames
END
GO
CREATE PROCEDURE Gene.Gene_ListAllOrganismNames
	@RecordSetID uniqueidentifier = NULL
	,@SubSetID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON

	-- This is a little clunky (on account of being repetitive), but saves us doing OUTER JOINs.
	IF @SubSetID IS NOT NULL BEGIN
		SELECT DISTINCT g.Organism
			FROM RecordSet.SubSetGene sg
			JOIN Gene.Gene g ON g.ID = sg.GeneID
			WHERE sg.SubSetID = @SubSetID
				AND g.Active = 1
			ORDER BY Organism
	END
	ELSE IF @RecordSetID IS NOT NULL BEGIN
		SELECT DISTINCT g.Organism
			FROM RecordSet.Gene rg
			JOIN Gene.Gene g ON g.ID = rg.GeneID
			WHERE rg.RecordSetID = @RecordSetID
				AND g.Active = 1
			ORDER BY Organism
	END
	ELSE BEGIN
		SELECT DISTINCT g.Organism
			FROM Gene.Gene g
			WHERE g.Active = 1
			ORDER BY Organism
	END
END
GO

IF NOT EXISTS (SELECT * 
					FROM Common.ApplicationProperty app
					WHERE app.[Key] = 'NCBIEmailAddress') BEGIN
	INSERT INTO Common.ApplicationProperty (ID, [Key], Value)
	VALUES (newid(), 'NCBIEmailAddress', 'daniel.gow@mail.utoronto.ca')
END
GO

UPDATE Common.ApplicationProperty
	SET Value = '1.1.2.0'
	WHERE [Key] = 'DatabaseVersion'
GO