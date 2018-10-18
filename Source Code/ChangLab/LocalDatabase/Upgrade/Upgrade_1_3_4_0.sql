SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('RecordSet.GeneStatus')) BEGIN
	DROP TABLE RecordSet.GeneStatus
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.GeneStatus')) BEGIN
	CREATE TABLE Job.GeneStatus (
		ID int IDENTITY(1,1) NOT NULL
		,Name varchar(20) NOT NULL
		,[Key] varchar(20) NOT NULL

		,CONSTRAINT PK_Job_GeneStatus PRIMARY KEY CLUSTERED (ID ASC)
	)

	INSERT INTO Job.GeneStatus (Name, [Key])
	VALUES ('Not Submitted', 'NotSubmitted')
			,('Submitted', 'Submitted')
			,('Processed', 'Processed')
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.GeneStatus_List')) BEGIN
	DROP PROCEDURE Job.GeneStatus_List
END
GO
CREATE PROCEDURE Job.GeneStatus_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT s.ID
			,s.Name
			,s.[Key]
		FROM Job.GeneStatus s
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('NCBI.Gene') AND c.name = 'StatusID') BEGIN
	ALTER TABLE NCBI.Gene DROP CONSTRAINT FK_NCBI_Gene_DirectionID
	ALTER TABLE NCBI.Gene DROP CONSTRAINT FK_NCBI_Gene_GeneID
	ALTER TABLE NCBI.Gene DROP CONSTRAINT FK_NCBI_Gene_RequestID

	CREATE TABLE NCBI.Tmp_Gene (
		RequestID uniqueidentifier NOT NULL,
		GeneID uniqueidentifier NOT NULL,
		StatusID int NOT NULL
	)

	IF EXISTS(SELECT * FROM NCBI.Gene) BEGIN
		 EXEC('INSERT INTO NCBI.Tmp_Gene (RequestID, GeneID, StatusID)
			SELECT RequestID, GeneID, 1 FROM NCBI.Gene WITH (HOLDLOCK TABLOCKX)')
	END

	EXEC ('DROP TABLE NCBI.Gene')
	EXECUTE sp_rename N'NCBI.Tmp_Gene', N'Gene', 'OBJECT' 

	ALTER TABLE NCBI.Gene ADD CONSTRAINT PK_NCBI_Gene PRIMARY KEY CLUSTERED (RequestID, GeneID, StatusID)
	ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
	ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)
	ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_StatusID FOREIGN KEY (StatusID) REFERENCES Job.GeneStatus (ID)

	EXEC ('UPDATE ng
		SET ng.StatusID = 3
		FROM NCBI.Gene ng
		WHERE EXISTS (SELECT *
						FROM BLASTN.Alignment al
						WHERE al.QueryID = ng.GeneID)')
END
GO

GO
ALTER PROCEDURE [NCBI].[Gene_Edit]
	@RequestID uniqueidentifier,
	@GeneID uniqueidentifier,
	@StatusID int
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM NCBI.Gene ex
					WHERE ex.RequestID = @RequestID
						AND ex.GeneID = @GeneID) BEGIN
		INSERT INTO NCBI.Gene (RequestID, GeneID, StatusID)
		VALUES (@RequestID, @GeneID, @StatusID)
	END
	ELSE BEGIN
		UPDATE NCBI.Gene
			SET StatusID = @StatusID
			WHERE RequestID = @RequestID
				AND GeneID = @GeneID
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('NCBI.Gene_EditMultiple')) BEGIN
	DROP PROCEDURE NCBI.Gene_EditMultiple
END
GO
CREATE PROCEDURE NCBI.Gene_EditMultiple
	@RequestID uniqueidentifier,
	@GeneIDs Common.ListUniqueIdentifier READONLY,
	@StatusID int
AS
BEGIN
	SET NOCOUNT ON

	UPDATE ng
		SET ng.StatusID = @StatusID
		FROM NCBI.Gene ng
		JOIN @GeneIDs g ON g.Value = ng.GeneID
		WHERE ng.RequestID = @RequestID

	INSERT INTO NCBI.Gene (RequestID, GeneID, StatusID)
	SELECT @RequestID, g.Value, @StatusID
		FROM @GeneIDs g
		WHERE NOT EXISTS (SELECT * FROM NCBI.Gene ex
							WHERE ex.RequestID = @RequestID
								AND ex.GeneID = g.Value)
END
GO
ALTER PROCEDURE [RecordSet].[RecordSet_Export]
	@RecordSetID uniqueidentifier
	,@JobHistory bit = 0
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @GeneIDs TABLE (GeneID uniqueidentifier)
	INSERT INTO @GeneIDs
	SELECT g.ID
		FROM Gene.Gene g
		JOIN RecordSet.SubSetGene sg ON sg.GeneID = g.ID
		JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
		JOIN RecordSet.RecordSet rs ON rs.ID = sub.RecordSetID
		WHERE rs.ID = @RecordSetID
			AND sub.Active = 1
			AND g.Active = 1
	UNION
	-- Pick up the Genes from BLASTN results that aren't already in the main Gene pool; UNION will DISTINCT this all for us
	SELECT g.ID
		FROM Gene.Gene g
		JOIN BlastN.Alignment al ON al.SubjectID = g.ID
		JOIN NCBI.BlastNAlignment n_al ON n_al.AlignmentID = al.ID
		JOIN NCBI.Request req ON req.ID = n_al.RequestID
		JOIN Job.Job j ON j.ID = req.JobID
		WHERE j.RecordSetID = @RecordSetID
			AND g.Active = 1

	IF (@JobHistory = 0) BEGIN
		SELECT (SELECT [Properties].Value AS [DatabaseVersion]
					FROM Common.ApplicationProperty [Properties]
					WHERE [Properties].[Key] = 'DatabaseVersion'
					FOR XML AUTO, ELEMENTS)
		UNION ALL
		SELECT (SELECT [RecordSet].*
						,[Properties].[Key]
						,[Properties].Value
					FROM RecordSet.RecordSet [RecordSet]
					JOIN RecordSet.ApplicationProperty [Properties] ON [Properties].RecordSetID = [RecordSet].ID
					WHERE [RecordSet].ID = @RecordSetID
					FOR XML AUTO, ELEMENTS)
		UNION ALL 
		SELECT (SELECT [SubSet].ID
						,[SubSet].Name
						,[SubSet].LastOpenedAt
						,[SubSet].[Open]
						,[SubSet-Gene].GeneID
						,[SubSet-Gene].ModifiedAt
					FROM RecordSet.RecordSet [RecordSet]
					JOIN RecordSet.SubSet [SubSet] ON [SubSet].RecordSetID = [RecordSet].ID
					JOIN RecordSet.SubSetGene [SubSet-Gene] ON [SubSet-Gene].SubSetID = [SubSet].ID
					JOIN Gene.Gene g ON g.ID = [SubSet-Gene].GeneID
					WHERE [RecordSet].ID = @RecordSetID
						AND [SubSet].Active = 1
						AND g.Active = 1
					ORDER BY [SubSet].Name
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-SubSet'))
		UNION ALL
		SELECT (SELECT [Gene].*
						FROM Gene.Gene [Gene]
						JOIN @GeneIDs g ON g.GeneID = [Gene].ID
					ORDER BY ID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Gene'))
		UNION ALL
		SELECT (SELECT [Sequence].*
					FROM Gene.NucleotideSequence [Sequence]
					JOIN @GeneIDs g ON g.GeneID = [Sequence].GeneID
					ORDER BY g.GeneID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Gene-Sequence'))
		UNION ALL
		SELECT (SELECT [Feature].*
						,[Feature-Interval].ID
						,[Feature-Interval].Start
						,[Feature-Interval].[End]
						,[Feature-Interval].IsComplement
						,[Feature-Interval].StartModifier
						,[Feature-Interval].EndModifier
					FROM Gene.Feature [Feature]
					JOIN Gene.FeatureInterval [Feature-Interval] ON [Feature-Interval].FeatureID = [Feature].ID
					JOIN @GeneIDs g ON g.GeneID = [Feature].GeneID
					ORDER BY [Feature].ID, [Feature-Interval].ID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Gene-Feature'))
	END
	ELSE BEGIN
		SELECT (SELECT [Job].ID
						,[Job].TargetID
						,[Job].StartedAt
						,[Job].EndedAt
						,[Job].StatusID
						,[Job].SubSetID
						,[Job-Gene].GeneID
						,[Job-Gene].DirectionID
					FROM Job.Job [Job]
					JOIN Job.Gene [Job-Gene] ON [Job-Gene].JobID = [Job].ID
					WHERE [Job].RecordSetID = @RecordSetID
						AND EXISTS (SELECT *
										FROM RecordSet.SubSetGene sg
										JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
										WHERE sg.GeneID = [Job-Gene].GeneID
											AND sub.RecordSetID = @RecordSetID
											AND sub.Active = 1)
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Job'))
		UNION ALL
		SELECT (SELECT [Request].*
						,[Request-Gene].GeneID
						,[Request-Gene].StatusID
					FROM NCBI.Request [Request]
					JOIN Job.Job j ON j.ID = [Request].JobID
					JOIN NCBI.Gene [Request-Gene] ON [Request-Gene].RequestID = [Request].ID
					WHERE j.RecordSetID = @RecordSetID
						AND EXISTS (SELECT *
										FROM RecordSet.SubSetGene sg
										JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
										WHERE sg.GeneID = [Request-Gene].GeneID
											AND sub.RecordSetID = @RecordSetID
											AND sub.Active = 1)
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-NCBI-Request'))
		UNION ALL
		SELECT (SELECT DISTINCT [Alignment].*
						,[Alignment-Exon].OrientationID
						,[Alignment-Exon].BitScore
						,[Alignment-Exon].AlignmentLength
						,[Alignment-Exon].IdentitiesCount
						,[Alignment-Exon].Gaps
						,[Alignment-Exon].QueryRangeStart
						,[Alignment-Exon].QueryRangeEnd
						,[Alignment-Exon].SubjectRangeStart
						,[Alignment-Exon].SubjectRangeEnd
					FROM BlastN.Alignment [Alignment]
					JOIN BlastN.AlignmentExon [Alignment-Exon] ON [Alignment-Exon].AlignmentID = [Alignment].ID
					JOIN NCBI.BlastNAlignment n_al ON n_al.AlignmentID = [Alignment].ID
					JOIN NCBI.Request req ON req.ID = n_al.RequestID
					JOIN Job.Job j ON j.ID = req.JobID
					JOIN Gene.Gene qry ON qry.ID = Alignment.QueryID
					JOIN Gene.Gene sbj ON sbj.ID = Alignment.SubjectID
					WHERE qry.Active = 1
						AND sbj.Active = 1
						AND j.RecordSetID = @RecordSetID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Alignment'))
		UNION ALL
		SELECT (SELECT [Request].ID
						,(SELECT [Alignment].AlignmentID
								FROM NCBI.BlastNAlignment [Alignment] 
								WHERE [Alignment].RequestID = [Request].ID
								FOR XML PATH(''), TYPE) AS "Alignments"
					FROM NCBI.Request [Request]
					JOIN Job.Job j ON j.ID = [Request].JobID
					WHERE j.RecordSetID = @RecordSetID
						AND EXISTS (SELECT * FROM NCBI.BlastNAlignment ex WHERE ex.RequestID = [Request].ID)
					FOR XML PATH('Request'), ROOT ('RecordSet-Request-Alignment'))
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_NCBI_Gene')) BEGIN
	DROP PROCEDURE RecordSet.Import_NCBI_Gene
END
GO

-- Improved statistics in results views
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('BlastN.Alignment_Statistics')) BEGIN
	DROP FUNCTION BlastN.Alignment_Statistics
END
GO
CREATE FUNCTION BlastN.Alignment_Statistics (@QueryGeneID uniqueidentifier, @SubjectGeneID uniqueidentifier)
RETURNS @Statistics TABLE ([Rank] int, MaxScore float, TotalScore float, AlignmentLength int, AlignmentPercentage float, QueryCover float)
AS
BEGIN
	INSERT INTO @Statistics
	SELECT al.[Rank]
			,MAX(al_ex.BitScore)
			,SUM(al_ex.BitScore)
			,SUM(al_ex.AlignmentLength)
			,Common.CalculatePercentageFromInt(SUM(al_ex.IdentitiesCount), SUM(al_ex.AlignmentLength))
			,Common.CalculatePercentageFromInt(SUM(al_ex.AlignmentLength), LEN(qry.Nucleotides))
		FROM BlastN.Alignment al
		JOIN BlastN.AlignmentExon al_ex ON al_ex.AlignmentID = al.ID
		JOIN Gene.Gene qry ON qry.ID = al.QueryID
		WHERE al.QueryID = @QueryGeneID
			AND al.SubjectID = @SubjectGeneID
		GROUP BY al.[Rank], LEN(qry.Nucleotides)

	RETURN
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('BlastN.Alignment_StatisticsTopForSubject')) BEGIN
	DROP FUNCTION BlastN.Alignment_StatisticsTopForSubject
END
GO
CREATE FUNCTION BlastN.Alignment_StatisticsTopForSubject (@SubjectGeneID uniqueidentifier)
RETURNS @Statistics TABLE ([Rank] int, MaxScore float, TotalScore float, AlignmentLength int, AlignmentPercentage float, QueryCover float)
AS
BEGIN
	INSERT INTO @Statistics
	SELECT TOP 1
			al.[Rank]
			,MAX(al_ex.BitScore) AS MaxScore
			,SUM(al_ex.BitScore)
			,SUM(al_ex.AlignmentLength)
			,Common.CalculatePercentageFromInt(SUM(al_ex.IdentitiesCount), SUM(al_ex.AlignmentLength))
			,Common.CalculatePercentageFromInt(SUM(al_ex.AlignmentLength), LEN(qry.Nucleotides))
		FROM BlastN.Alignment al
		JOIN BlastN.AlignmentExon al_ex ON al_ex.AlignmentID = al.ID
		JOIN Gene.Gene qry ON qry.ID = al.QueryID
		WHERE al.SubjectID = @SubjectGeneID
		GROUP BY al.ID, al.[Rank], LEN(qry.Nucleotides)
		ORDER BY al.[Rank], MaxScore DESC

	RETURN
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
		WHERE [Key] = 'Archived';

	DECLARE @SubjectIDs TABLE (SubjectID uniqueidentifier)

	INSERT INTO @SubjectIDs
	SELECT DISTINCT sbj.ID
	FROM BlastN.Alignment al
	JOIN Gene.Gene qry ON qry.ID = al.QueryID
	JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
	JOIN NCBI.BlastNAlignment nal ON nal.AlignmentID = al.ID
	JOIN NCBI.Request req ON req.ID = nal.RequestID
	JOIN Job.Job j ON j.ID = req.JobID
	WHERE j.RecordSetID = @RecordSetID
		AND 
		(
			(@JobID IS NOT NULL AND req.JobID = @JobID)
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

	SELECT sbj.ID
			,sbj.SourceID
			,sbj.GenBankID
			,sbj.[Definition]
			,sbj.SequenceStart
			,sbj.SequenceEnd
			,stat.*
			,CAST(CASE WHEN EXISTS (SELECT * FROM RecordSet.Gene rs_g WHERE rs_g.RecordSetID = @RecordSetID AND rs_g.GeneID = sbj.ID)
						   THEN 1
						   ELSE 0
						   END AS bit) AS InRecordSet
		FROM Gene.Gene sbj
		JOIN @SubjectIDs id ON id.SubjectID = sbj.ID
		CROSS APPLY BlastN.Alignment_StatisticsTopForSubject(sbj.ID) stat
		ORDER BY stat.[Rank], stat.MaxScore DESC

/*
	WITH Alignments AS (
		SELECT al.ID AS AlignmentID
				,al.SubjectID
				,al.[Rank] AS [Rank]
				,LEN(qry.Nucleotides) AS QuerySequenceLength
			FROM BlastN.Alignment al
			JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
			JOIN Gene.Gene qry ON qry.ID = al.QueryID
			JOIN NCBI.BlastNAlignment nal ON nal.AlignmentID = al.ID
			JOIN NCBI.Request req ON req.ID = nal.RequestID
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE
				j.RecordSetID = @RecordSetID
				AND 
				(
					(@JobID IS NOT NULL AND req.JobID = @JobID)
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
	), AlignmentExons AS (
		SELECT al_ex.AlignmentID
				,al_ex.IdentitiesCount
				,al_ex.AlignmentLength
			FROM BlastN.AlignmentExon al_ex
			JOIN Alignments al ON al.AlignmentID = al_ex.AlignmentID
	), AlignmentPercentage AS (
		SELECT al_ex.AlignmentID
				,MAX(Common.CalculatePercentageFromInt(al_ex.IdentitiesCount, al_ex.AlignmentLength)) AS AlignmentPercentage
			FROM AlignmentExons al_ex
			GROUP BY al_ex.AlignmentID
	), QueryCover AS (
		SELECT al_ex.AlignmentID
				,Common.CalculatePercentageFromInt(SUM(al_ex.AlignmentLength), al.QuerySequenceLength) AS QueryCover
			FROM AlignmentExons al_ex
			JOIN Alignments al ON al.AlignmentID = al_ex.AlignmentID
			GROUP BY al_ex.AlignmentID, al.QuerySequenceLength
	)

	SELECT sbj.ID
			,sbj.SourceID
			,sbj.GenBankID
			,sbj.[Definition]
			,sbj.SequenceStart
			,sbj.SequenceEnd
			,MAX(al.[Rank]) AS [Rank]
			,Common.MinimumMaximumFloat(MAX(perc.AlignmentPercentage), 1.0) AS AlignmentPercentage
			,Common.MinimumMaximumFloat(MAX(qcover.QueryCover), 1.0) AS QueryCover
			,CAST(CASE WHEN EXISTS (SELECT * FROM RecordSet.Gene rs_g WHERE rs_g.GeneID = sbj.ID)
					   THEN 1
					   ELSE 0
					   END AS bit) AS InRecordSet
		FROM Alignments al
		JOIN AlignmentPercentage perc ON perc.AlignmentID = al.AlignmentID
		JOIN QueryCover qcover ON qcover.AlignmentID = al.AlignmentID
		JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
		GROUP BY sbj.ID, sbj.SourceID, sbj.GenBankID, sbj.[Definition], sbj.SequenceStart, sbj.SequenceEnd
		ORDER BY [Rank], AlignmentPercentage DESC
*/
END
GO
ALTER PROCEDURE [Job].[BlastN_ListSubjectGenesForQueryGene]
	@QueryGeneID uniqueidentifier
	,@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON
		
	SELECT sbj.ID
			,ssrc.Name AS QuerySourceName
			,sbj.GenBankID
			,sbj.[Definition]
			,sbj.Organism
			,sbj.Taxonomy
			,stat.*
			,CAST(CASE WHEN EXISTS (SELECT * FROM RecordSet.Gene rs_g WHERE rs_g.RecordSetID = @RecordSetID AND rs_g.GeneID = sbj.ID)
					   THEN 1
					   ELSE 0
					   END AS bit) AS InRecordSet
		FROM BlastN.Alignment al
		JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
		JOIN Gene.[Source] ssrc ON ssrc.ID = sbj.SourceID
		CROSS APPLY BlastN.Alignment_Statistics(al.QueryID, al.SubjectID) stat
		WHERE al.QueryID = @QueryGeneID
		ORDER BY stat.[Rank], stat.MaxScore DESC
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

	IF @SubjectGeneID IS NOT NULL BEGIN
		SELECT qry.ID
				,qsrc.Name AS QuerySourceName
				,qry.GenBankID
				,qry.[Definition]
				,qry.Organism
				,qry.Taxonomy
				,stat.*
			FROM NCBI.Request r
			JOIN NCBI.BlastNAlignment nal ON nal.RequestID = r.ID
			JOIN BlastN.Alignment al ON al.ID = nal.AlignmentID
			JOIN BlastN.AlignmentExon ex ON ex.ID = BlastN.AlignmentExon_First(al.ID)
			JOIN Gene.Gene qry ON qry.ID = al.QueryID
			JOIN Gene.[Source] qsrc ON qsrc.ID = qry.SourceID
			JOIN Job.Job j ON j.ID = r.JobID
			CROSS APPLY BlastN.Alignment_Statistics(al.QueryID, al.SubjectID) stat
			WHERE al.SubjectID = @SubjectGeneID
				AND j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
			ORDER BY stat.[Rank], stat.MaxScore DESC
	END
	ELSE BEGIN
		SELECT DISTINCT
				qry.ID
				,qry.GenBankID
				,qry.[Definition]
				,qry.Organism
				,qry.Nucleotides -- Used to calculate the sequence length
			FROM NCBI.Request r
			JOIN NCBI.Gene ng ON ng.RequestID = r.ID
			JOIN Gene.Gene qry ON qry.ID = ng.GeneID
			JOIN Job.Job j ON j.ID = r.JobID
			WHERE j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
			ORDER BY qry.Organism, qry.[Definition]
	END
END
GO
ALTER PROCEDURE [RecordSet].[SubSetGene_List]
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @GeneStatusID_Processed int = (SELECT s.ID FROM Job.GeneStatus s WHERE s.[Key] = 'Processed')

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

			,CASE WHEN (t.ID IS NOT NULL)
				  THEN t.HID.ToString()
				  ELSE ''
				  END AS TaxonomyHierarchy
			,CAST(CASE WHEN EXISTS (SELECT * 
										FROM NCBI.Gene ng
										WHERE ng.GeneID = sg.GeneID
											AND ng.StatusID = @GeneStatusID_Processed)
				-- We're not doing a LEFT OUTER JOIN for this because a gene could be submitted multiple times, thus multiple requests
						THEN 1
						ELSE 0
						END AS bit) AS ProcessedThroughBLASTNAtNCBI
			
		FROM RecordSet.SubSetGene sg
		JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
		JOIN RecordSet.Gene rs_g ON rs_g.GeneID = sg.GeneID AND rs_g.RecordSetID = sub.RecordSetID
		JOIN Gene.Gene g ON g.ID = rs_g.GeneID
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		
		WHERE sg.SubSetID = @SubSetID
			AND g.Active = 1
		ORDER BY ISNULL(g.Organism, g.[Definition])
				,LEN(ISNULL(g.Nucleotides, ''))
				,g.[Definition]
				,rs_g.ModifiedAt DESC
END
GO

-- SubSet view re-ordering
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('RecordSet.SubSet') AND c.name = 'DisplayIndex') BEGIN
	ALTER TABLE RecordSet.SubSet ADD DisplayIndex int NOT NULL CONSTRAINT DF_RecordSet_SubSet_DisplayIndex DEFAULT ((0))
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('RecordSet.RecordSet') AND c.name = 'SelectedSubSetID') BEGIN
	ALTER TABLE RecordSet.RecordSet ADD SelectedSubSetID uniqueidentifier NULL
END
GO
IF NOT EXISTS (SELECT * FROM sys.table_types t WHERE t.user_type_id = TYPE_ID('Common.DictionaryUniqueIdentifierInt')) BEGIN
	CREATE TYPE Common.DictionaryUniqueIdentifierInt AS TABLE (
		[Key] uniqueidentifier NOT NULL
		,[Value] int NOT NULL
	)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.SubSets_Reorder')) BEGIN
	DROP PROCEDURE RecordSet.SubSets_Reorder
END
GO
CREATE PROCEDURE RecordSet.SubSets_Reorder
	@SubSets Common.DictionaryUniqueIdentifierInt READONLY
AS
BEGIN
	SET NOCOUNT ON

	UPDATE sub
		SET DisplayIndex = ids.Value
		FROM RecordSet.SubSet sub
		JOIN @SubSets ids ON ids.[Key] = sub.ID
END
GO
ALTER PROCEDURE [RecordSet].[SubSet_List]
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT sub.ID
			,sub.Name
			,sub.LastOpenedAt
			,sub.[Open]
			,sub.DisplayIndex
			,(SELECT COUNT(*)
					FROM RecordSet.SubSetGene sg
					WHERE sg.SubSetID = sub.ID) AS GeneCount
		FROM RecordSet.SubSet sub
		WHERE sub.RecordSetID = @RecordSetID
			AND sub.Active = 1
		ORDER BY sub.LastOpenedAt, sub.Name
END
GO
ALTER PROCEDURE [RecordSet].[SubSet_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@RecordSetID uniqueidentifier = NULL,
	@Name varchar(200),
	@Active bit = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		SET @ID = NEWID()
	END

	IF NOT EXISTS (SELECT * FROM RecordSet.SubSet WHERE ID = @ID) BEGIN
		INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DisplayIndex)
		SELECT @ID, @RecordSetID, @Name
				,MAX(sub.DisplayIndex) + 1
			FROM RecordSet.SubSet sub
			WHERE sub.RecordSetID = @RecordSetID
	END
	ELSE BEGIN
		IF (@Active IS NOT NULL) AND ((SELECT rs.Active FROM RecordSet.SubSet rs WHERE rs.ID = @ID) <> @Active) BEGIN
			UPDATE RecordSet.SubSet
				SET Active = @Active
				WHERE ID = @ID
		END
		ELSE BEGIN
			UPDATE RecordSet.SubSet
				SET Name = @Name
				WHERE ID = @ID
		END
	END
END
GO
ALTER PROCEDURE [RecordSet].[RecordSet_Opened]
	@ID uniqueidentifier
	,@SelectedSubSetID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF @SelectedSubSetID IS NULL BEGIN
		UPDATE RecordSet.RecordSet
			SET LastOpenedAt = SYSDATETIME()
			WHERE ID = @ID
	END
	ELSE BEGIN
		-- Piggy-backing on this stored procedure for the convenience of not creating another one.
		UPDATE RecordSet.RecordSet
			SET SelectedSubSetID = @SelectedSubSetID
			WHERE ID = @ID
	END
END
GO
ALTER PROCEDURE [RecordSet].[RecordSet_List]
	@Active bit = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT rs.ID
			,rs.Name
			,rs.CreatedAt
			,rs.LastOpenedAt
			,rs.ModifiedAt
			,rs.Active
			,rs.SelectedSubSetID
			,(SELECT COUNT(*)
				FROM RecordSet.Gene g
				WHERE g.RecordSetID = rs.ID) AS GeneCountFinal
		FROM RecordSet.RecordSet rs
		WHERE (@Active IS NULL OR rs.Active = @Active)
		ORDER BY rs.LastOpenedAt DESC, rs.ModifiedAt DESC
END
GO

-- Sequence Range values were made obsolete by the notion of multiple feature intervals
GO
IF (SELECT CHARINDEX('{Coding Sequence Range Start}-{Coding Sequence Range End}', Value) FROM Common.ApplicationProperty WHERE [Key] = 'FASTAHeaderFormatString') <> 0 BEGIN
	UPDATE Common.ApplicationProperty
		SET Value = REPLACE(Value, '{Coding Sequence Range Start}-{Coding Sequence Range End}', '{Coding Sequence Length}')
		WHERE [Key] = 'FASTAHeaderFormatString'
END
IF (SELECT CHARINDEX('{Coding Sequence Range Start}-{Coding Sequence Range End}', Value) FROM Common.ApplicationProperty WHERE [Key] = 'FASTAFileNameFormatString') <> 0 BEGIN
	UPDATE Common.ApplicationProperty
		SET Value = REPLACE(Value, '{Coding Sequence Range Start}-{Coding Sequence Range End}', '{Coding Sequence Length}')
		WHERE [Key] = 'FASTAFileNameFormatString'
END
GO

-- Export to Excel
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_ForExport')) BEGIN
	DROP PROCEDURE Gene.Gene_ForExport
END
GO
CREATE PROCEDURE Gene.Gene_ForExport
	@GeneIDs Common.ListUniqueIdentifier READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.[Definition]
			,g.Organism
			,g.Taxonomy
			,g.Locus
			,g.Accession
			,g.GenBankID
			,src.Name AS [Source]
			,LEN(ISNULL(g.Nucleotides, '')) AS [Length]
			,g.Nucleotides
		FROM Gene.Gene g
		JOIN Gene.[Source] src ON src.ID = g.SourceID
		JOIN @GeneIDs ids ON ids.Value = g.ID
		ORDER BY g.Organism, LEN(ISNULL(g.Nucleotides, '')), g.[Definition]
END
GO

UPDATE Common.ApplicationProperty
	SET Value = '1.3.4.0'
	WHERE [Key] = 'DatabaseVersion'
GO