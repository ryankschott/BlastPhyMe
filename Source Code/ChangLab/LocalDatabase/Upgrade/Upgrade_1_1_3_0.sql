SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Add SubSetID to Job.Job so that the UI can show which subset the gene sequences for the job were used from.  It's not sufficient to link back via
-- Job.Gene and RecordSet.SubSetGene, because genes can be moved out of subsets and exist in multiple subsets.
-- RecordSetID is maintained for backwards compatibility and convenience.
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Job.Job') AND c.name = 'SubSetID') BEGIN
	ALTER TABLE Job.Job ADD SubSetID uniqueidentifier NULL
END
GO
ALTER PROCEDURE [Job].[Job_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@SubSetID uniqueidentifier = NULL, -- Not used in an UPDATE
	@TargetID int = NULL, -- Not used in an UPDATE
	@StatusID int = NULL,
	@StartedAt datetime2(7) = NULL, -- Not used in an UPDATE
	@EndedAt datetime2(7) = NULL
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @RecordSetID uniqueidentifier

	IF NOT EXISTS (SELECT * FROM Job.Job j WHERE j.ID = @ID) BEGIN
		SET @ID = NEWID()

		SELECT @RecordSetID = sub.RecordSetID
			FROM RecordSet.SubSet sub
			WHERE sub.ID = @SubSetID

		INSERT INTO Job.Job (ID, RecordSetID, SubSetID, TargetID, StartedAt, EndedAt)
		VALUES (@ID, @RecordSetID, @SubSetID, @TargetID, @StartedAt, @EndedAt)
	END
	ELSE BEGIN
		UPDATE Job.Job
			SET StatusID = @StatusID
				,EndedAt = @EndedAt
			WHERE ID = @ID
	END
END
GO
ALTER PROCEDURE [Job].[Job_List]
	@RecordSetID uniqueidentifier,
	@TargetID int,
	@StatusID int = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT j.*
			,jt.Name AS TargetName
			,js.[Key] AS StatusKey
			,js.Name AS StatusName
			,(SELECT COUNT(*)
					FROM Job.Gene g
					WHERE g.JobID = j.ID
						AND g.DirectionID = 1) AS InputGenesCount
			,ISNULL(sub.Name, '') AS SubSetName
		FROM Job.Job j
		JOIN Job.[Target] jt ON jt.ID = j.TargetID
		JOIN Job.[Status] js ON js.ID = j.StatusID
		LEFT OUTER JOIN RecordSet.SubSet sub ON sub.ID = j.SubSetID
		WHERE j.RecordSetID = @RecordSetID
			AND j.TargetID = @TargetID
			AND ((@StatusID IS NULL) OR (j.StatusID = @StatusID))
		ORDER BY j.StartedAt DESC
END
GO

-- Switch to default sorting by Organism and Definition, since that's what I'm most often using.
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
		ORDER BY g.Organism, g.[Definition], rs_g.ModifiedAt DESC
END
GO

-- Fix for the InRecordSet column in GenBank search results
ALTER PROCEDURE [Gene].[Gene_ExistsByGenBankID]
	@GenBankIDs Common.ListInt READONLY
	,@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	WITH RecordSetGenBankIDs AS (
		SELECT DISTINCT g.GenBankID -- Should already be unique, but why not...one day there'll be NULLs in there.
			FROM RecordSet.Gene rs_g
			JOIN Gene.Gene g ON g.ID = rs_g.GeneID
			WHERE rs_g.RecordSetID = @RecordSetID
	)

	SELECT l.Value AS ID
			,CAST(CASE WHEN g.GenBankID IS NOT NULL THEN 1 ELSE 0 END AS bit) AS [Exists]
		FROM @GenBankIDs l
		LEFT OUTER JOIN RecordSetGenBankIDs g ON g.GenBankID = l.Value
END
GO

UPDATE Common.ApplicationProperty
	SET Value = '1.1.3.0'
	WHERE [Key] = 'DatabaseVersion'
GO