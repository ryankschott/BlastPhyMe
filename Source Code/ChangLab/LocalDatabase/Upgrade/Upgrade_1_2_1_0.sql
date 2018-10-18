SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('BlastN.Alignment') AND c.name = 'LastUpdatedAt') BEGIN
	ALTER TABLE BlastN.Alignment ADD LastUpdatedAt datetime2(7) NOT NULL CONSTRAINT DF_BlastN_Alignment_LastUpdatedAt DEFAULT (sysdatetime())
END
GO
ALTER PROCEDURE [RecordSet].[Import_BlastN_Alignment]
	@ID uniqueidentifier = NULL OUTPUT
	,@NeedsExons bit = 0 OUTPUT
	,@QueryID uniqueidentifier
	,@SubjectID uniqueidentifier
	,@Rank int
	,@LastUpdatedAt datetime2(7)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @ExistingLastUpdatedAt datetime2(7)
			,@DeleteExons bit = 0

	IF EXISTS (SELECT * FROM BlastN.Alignment ex
					WHERE ex.QueryID = @QueryID
						AND ex.SubjectID = @SubjectID) BEGIN
		SELECT @ID = ex.ID
				,@ExistingLastUpdatedAt = ex.LastUpdatedAt
			FROM BlastN.Alignment ex
				WHERE ex.QueryID = @QueryID
					AND ex.SubjectID = @SubjectID

		IF @ExistingLastUpdatedAt < @LastUpdatedAt BEGIN
			SET @DeleteExons = 1
		END
	END

	IF @ID IS NULL OR @DeleteExons = 1 BEGIN
		SET @NeedsExons = 1
		EXEC BlastN.Alignment_Edit @ID OUTPUT, @QueryID, @SubjectID, @Rank, @DeleteExons
	END
END
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
				,LastUpdatedAt = sysdatetime()
			WHERE ID = @ID
	END

	IF @ClearExons = 1 BEGIN
		DELETE FROM BlastN.AlignmentExon
			WHERE AlignmentID = @ID
	END
END
GO

-- For for slow performance on large jobs
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
				,CAST(MAX(Common.CalculatePercentageFromInt(al_ex.IdentitiesCount, al_ex.AlignmentLength)) AS int) AS AlignmentPercentage
			FROM AlignmentExons al_ex
			GROUP BY al_ex.AlignmentID
	), QueryCover AS (
		SELECT al_ex.AlignmentID
				,CAST(Common.CalculatePercentageFromInt(SUM(al_ex.AlignmentLength), al.QuerySequenceLength) AS int) AS QueryCover
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
			,MAX(perc.AlignmentPercentage) AS AlignmentPercentage
			,MAX(qcover.QueryCover) AS QueryCover
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

-- Utility functions
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('RecordSet.RecordSet_IDByName')) BEGIN
	DROP FUNCTION RecordSet.RecordSet_IDByName
END
GO
CREATE FUNCTION RecordSet.RecordSet_IDByName (@Name varchar(200))
RETURNS uniqueidentifier
AS
BEGIN
	RETURN (SELECT ID FROM RecordSet.RecordSet WHERE Name = @Name)
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('RecordSet.SubSet_IDByName')) BEGIN
	DROP FUNCTION RecordSet.SubSet_IDByName
END
GO
CREATE FUNCTION RecordSet.SubSet_IDByName (@Name varchar(100), @RecordSetID uniqueidentifier)
RETURNS uniqueidentifier
AS
BEGIN
	RETURN (SELECT ID FROM RecordSet.SubSet WHERE Name = @Name AND RecordSetID = @RecordSetID)
END
GO

-- Excluding Job Genes from inactive subsets
ALTER PROCEDURE [RecordSet].[RecordSet_Export]
	@RecordSetID uniqueidentifier
	,@JobHistory bit = 0
AS
BEGIN
	SET NOCOUNT ON

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
					FROM (
						SELECT DISTINCT [Gene].*
							FROM RecordSet.RecordSet rs
							JOIN RecordSet.SubSet sub ON sub.RecordSetID = rs.ID
							JOIN RecordSet.SubSetGene sg ON sg.SubSetID = sub.ID
							JOIN Gene.Gene [Gene] ON [Gene].ID = sg.GeneID
							WHERE rs.ID = @RecordSetID
								AND sub.Active = 1
								AND [Gene].Active = 1
					) [Gene]
					ORDER BY ID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Gene'))
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
						,[Request-Gene].DirectionID
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
		SELECT (SELECT [Gene].*
					FROM (
						SELECT DISTINCT [Gene].* -- Pick up the Genes from BLASTN results that aren't already in the main Gene pool
							FROM Job.Job j
							JOIN NCBI.Request req ON req.JobID = j.ID
							JOIN NCBI.BlastNAlignment n_al ON n_al.RequestID = req.ID
							JOIN BlastN.Alignment al ON al.ID = n_al.AlignmentID
							JOIN Gene.Gene [Gene] ON [Gene].ID = al.SubjectID
							WHERE j.RecordSetID = @RecordSetID
								AND [Gene].Active = 1
								AND NOT EXISTS (SELECT * -- This will gather the genes that aren't in an active subset
													FROM RecordSet.SubSetGene g
													JOIN RecordSet.SubSet sub ON sub.ID = g.SubSetID
													WHERE g.GeneID = [Gene].ID
														AND sub.RecordSetID = @RecordSetID
														AND sub.Active = 1)
					) [Gene]
					ORDER BY ID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Subject-Gene'))
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

UPDATE Common.ApplicationProperty
	SET Value = '1.2.1.0'
	WHERE [Key] = 'DatabaseVersion'
GO