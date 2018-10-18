SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('NCBI.LocalDatabaseRequest')) BEGIN
	CREATE TABLE NCBI.LocalDatabaseRequest (
		RequestID uniqueidentifier NOT NULL
		,TargetDatabase varchar(250) NOT NULL
		,CONSTRAINT PK_NCBI_LocalDatabaseRequest PRIMARY KEY (RequestID ASC)
	)
	ALTER TABLE NCBI.LocalDatabaseRequest ADD CONSTRAINT FK_NCBI_LocalDatabaseRequest_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('NCBI.LocalDatabaseRequest_Edit')) BEGIN
	DROP PROCEDURE NCBI.LocalDatabaseRequest_Edit
END
GO
CREATE PROCEDURE NCBI.LocalDatabaseRequest_Edit
	@RequestID uniqueidentifier
	,@TargetDatabase varchar(250)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO NCBI.LocalDatabaseRequest (RequestID, TargetDatabase)
	VALUES (@RequestID, @TargetDatabase)
END
GO

IF NOT EXISTS (SELECT * FROM Gene.[Source] s WHERE s.[Key] = 'BLASTN_Local') BEGIN
	INSERT INTO Gene.[Source] (Name, [Key])
	VALUES ('BLASTN (Local)', 'BLASTN_Local')
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Gene_EditMultiple')) BEGIN
	DROP PROCEDURE RecordSet.Gene_EditMultiple
END
GO
CREATE PROCEDURE RecordSet.Gene_EditMultiple
	@RecordSetID uniqueidentifier,
	@SubSetID uniqueidentifier,
	@GeneIDs Common.ListUniqueIdentifier READONLY
AS
BEGIN
	SET NOCOUNT ON

	-- Update the ModifiedAt field for the genes we already have, then insert the new ones.
	UPDATE g
		SET ModifiedAt = SYSDATETIME()
		FROM RecordSet.Gene g
		JOIN @GeneIDs id ON id.Value = g.GeneID
		WHERE g.RecordSetID = @RecordSetID

	INSERT INTO RecordSet.Gene (RecordSetID, GeneID)
	SELECT @RecordSetID, g.Value
		FROM @GeneIDs g
		WHERE NOT EXISTS (SELECT * FROM RecordSet.Gene ex
							WHERE ex.RecordSetID = @RecordSetID
								AND ex.GeneID = g.Value)

	UPDATE g
		SET ModifiedAt = SYSDATETIME()
		FROM RecordSet.SubSetGene g
		JOIN @GeneIDs id ON id.Value = g.GeneID
		WHERE g.SubSetID = @SubSetID

	INSERT INTO RecordSet.SubSetGene (SubSetID, GeneID)
	SELECT @SubSetID, g.Value
		FROM @GeneIDs g
		WHERE NOT EXISTS (SELECT * FROM RecordSet.SubSetGene ex
							WHERE ex.SubSetID = @SubSetID
								AND ex.GeneID = g.Value)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Gene_DeleteMultiple')) BEGIN
	DROP PROCEDURE RecordSet.Gene_DeleteMultiple
END
GO
CREATE PROCEDURE RecordSet.Gene_DeleteMultiple
	@RecordSetID uniqueidentifier,
	@SubSetID uniqueidentifier,
	@GeneIDs Common.ListUniqueIdentifier READONLY
AS
BEGIN
	SET NOCOUNT ON

	DELETE sg
		FROM RecordSet.SubSetGene sg
		JOIN @GeneIDs g ON g.Value = sg.GeneID
		WHERE SubSetID = @SubSetID

	-- The gene is considered orphaned if it is not associated with any subsets.
	DELETE rg
		FROM RecordSet.Gene rg
		WHERE RecordSetID = @RecordSetID
			AND NOT EXISTS (SELECT * 
								FROM RecordSet.SubSetGene sg
								JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
								WHERE sub.RecordSetID = @RecordSetID
									AND sg.GeneID = rg.GeneID)
END
GO

-- Made obsolete by the GenBank search history feature
IF EXISTS (SELECT * FROM RecordSet.ApplicationProperty
				WHERE [Key] = 'LastGenBankSearch') BEGIN
	DELETE FROM RecordSet.ApplicationProperty
		WHERE [Key] = 'LastGenBankSearch'
END
GO

-- Capping query cover at 100% and converting from int to percentage
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.MinimumMaximumFloat')) BEGIN
	DROP FUNCTION Common.MinimumMaximumFloat
END
GO
CREATE FUNCTION Common.MinimumMaximumFloat (@Value float, @Maximum float)
RETURNS float
AS
BEGIN
	-- Caps a given number at a smallest (minimum) maximum value.

	DECLARE @Return float

	IF @Value > @Maximum BEGIN
		SET @Return = @Maximum
	END
	ELSE BEGIN
		SET @Return = @Value
	END

	RETURN @Return
END
GO
ALTER FUNCTION [Common].[CalculatePercentageFromInt](@Numerator int, @Denominator int)
RETURNS float
AS
BEGIN
	RETURN (SELECT (CAST(@Numerator AS float) / CAST(@Denominator AS float)))
END
GO
ALTER FUNCTION [BlastN].[Alignment_QueryCover](@AlignmentID uniqueidentifier)
RETURNS float
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
				,al.[Rank]
				,ex.AlignmentLength
				,ex.BitScore
				,Common.MinimumMaximumFloat(Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength), 1.0) AS AlignmentPercentage
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
	ELSE BEGIN
		SELECT DISTINCT
				qry.ID
				,qry.GenBankID
				,qry.[Definition]
				,qry.Organism
				,qry.CodingSequenceStart
				,qry.CodingSequenceEnd
			FROM NCBI.Request r
			JOIN NCBI.Gene ng ON ng.RequestID = r.ID
			JOIN Gene.Gene qry ON qry.ID = ng.GeneID
			JOIN Job.Job j ON j.ID = r.JobID
			WHERE j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
				AND ng.DirectionID = 1
			ORDER BY qry.Organism, qry.[Definition]
	END
END
GO
ALTER PROCEDURE [Job].[BlastN_ListSubjectGenesForQueryGene]
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
			,Common.MinimumMaximumFloat(Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength), 1.0) AS AlignmentPercentage
			,BlastN.Alignment_QueryCover(al.ID) AS QueryCover
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

	WITH Alignments AS (
		SELECT al.ID AS AlignmentID
				,al.SubjectID
				,MAX(al.[Rank]) AS [Rank]
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
			GROUP BY al.ID, al.SubjectID, qry.Nucleotides
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
END
GO

UPDATE Common.ApplicationProperty
	SET Value = '1.3.0.0'
	WHERE [Key] = 'DatabaseVersion'
GO