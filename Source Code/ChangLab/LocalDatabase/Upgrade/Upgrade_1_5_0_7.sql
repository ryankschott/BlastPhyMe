SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Job].[BlastN_ListAlignmentsForJob]
	@RequestID int = NULL,
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
			AND
			(
				(@RequestID IS NULL OR req.ID = @RequestID)
			)
			AND (
					(@FilterByID = 1)
					OR
					(@FilterByID = 2 AND NOT EXISTS (SELECT *
														FROM RecordSet.Gene rs_g
														JOIN Gene.Gene g ON g.ID = rs_g.GeneID
														WHERE rs_g.RecordSetID = @RecordSetID
															AND g.Accession = sbj.Accession))
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
			,sbj.Accession
			,sbj.Name
			,sbj.[Definition]
			,sbj.Nucleotides
			,stat.*
			,Gene.IsInRecordSet(sbj.ID, @RecordSetID) AS InRecordSet
		FROM Gene.Gene sbj
		JOIN @SubjectIDs id ON id.SubjectID = sbj.ID
		CROSS APPLY BlastN.Alignment_StatisticsTopForSubject(sbj.ID, @RecordSetID, @JobID) stat
		ORDER BY stat.MaxScore DESC

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
ALTER PROCEDURE [Job].[BlastN_ListNotProcessedGenes]
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT qry.ID
			,qry.GenBankID
			,qry.Accession
			,qry.Name
			,qry.[Definition]
			,qry.Organism
			,qry.Nucleotides -- Used to calculate the sequence length
		FROM Job.Gene jg
		JOIN Job.GeneDirection dir ON dir.ID = jg.DirectionID
		JOIN Gene.Gene qry ON qry.ID = jg.GeneID
		WHERE jg.JobID = @JobID
			AND dir.[Key] = 'Input' 
			AND NOT EXISTS (SELECT *
								FROM NCBI.Gene ng
								JOIN NCBI.Request req ON req.JobID = @JobID AND req.ID = ng.RequestID
								JOIN Job.GeneStatus stat ON stat.ID = ng.StatusID
								WHERE ng.GeneID = jg.GeneID
									AND stat.[Key] IN ('Processed'))
		ORDER BY qry.Organism, qry.[Definition]
END
GO
ALTER PROCEDURE [Job].[BlastN_ListSubjectGenesForQueryGene]
	@QueryGeneID uniqueidentifier
	,@RecordSetID uniqueidentifier
	,@JobID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON
		
	SELECT sbj.*
			,ssrc.Name AS QuerySourceName
			,stat.*
			,Gene.IsInRecordSet(sbj.ID, @RecordSetID) AS InRecordSet
		FROM BlastN.Alignment al
		JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
		JOIN Gene.[Source] ssrc ON ssrc.ID = sbj.SourceID
		JOIN NCBI.BlastNAlignment ncbi ON ncbi.AlignmentID = al.ID
		JOIN NCBI.Request req ON req.ID = ncbi.RequestID
		CROSS APPLY BlastN.Alignment_Statistics(al.QueryID, al.SubjectID) stat
		WHERE al.QueryID = @QueryGeneID
			AND (@JobID IS NULL OR req.JobID = @JobID)
		ORDER BY stat.[Rank], stat.MaxScore DESC
END
GO
ALTER PROCEDURE [Job].[BlastN_ListAnnotationGenesForJob]
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT qry.*
			,sbj.ID AS SubjectID
			,sbj.SourceID AS SubjectSourceID
			,sbj.[Definition] AS SubjectDefinition
			,sbj.GenBankID AS SubjectGenBankID
			,sbj.Accession AS SubjectAccession
			,sbj.LastUpdatedAt AS SubjectLastUpdatedAt
			,sbj.SequenceIdentityMatchPercentage
		FROM Job.Gene j
		JOIN Gene.Gene qry ON qry.ID = j.GeneID

		LEFT OUTER JOIN (
				SELECT al.QueryID
						,sbj.ID
						,sbj.SourceID
						,sbj.[Definition]
						,sbj.GenBankID
						,sbj.Accession
						,sbj.LastUpdatedAt
						,CONVERT(int, AVG(ROUND((CONVERT(float, al_ex.IdentitiesCount) / CONVERT(float, al_ex.AlignmentLength)), 2) * 100.0)) AS SequenceIdentityMatchPercentage
					FROM Gene.Gene sbj
					JOIN BlastN.Alignment al ON al.SubjectID = sbj.ID AND al.[Rank] = 0 -- Return just the top match
					JOIN BlastN.AlignmentExon al_ex ON al_ex.AlignmentID = al.ID
					JOIN NCBI.BlastNAlignment ncbi ON ncbi.AlignmentID = al.ID
					JOIN NCBI.Request req ON req.ID = ncbi.RequestID
					WHERE req.JobID = @JobID
					GROUP BY al.QueryID, sbj.ID, sbj.SourceID, sbj.[Definition], sbj.GenBankID, sbj.Accession, sbj.LastUpdatedAt
			) sbj ON sbj.QueryID = qry.ID

		WHERE j.JobID = @JobID
			AND j.DirectionID = 1

END
GO
ALTER PROCEDURE [Job].[BlastN_ListQueryGenesForAlignment]
	@SubjectGeneID uniqueidentifier,
	@RequestID int,
	@JobID uniqueidentifier,
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF @RecordSetID IS NULL AND @JobID IS NULL AND @RequestID IS NULL BEGIN
		RAISERROR('A Request ID, Job ID, or RecordSet ID must be provided', 11, 1)
	END
		
	IF @RecordSetID IS NULL BEGIN
		IF @JobID IS NOT NULL BEGIN
			SELECT @RecordSetID = j.RecordSetID
				FROM Job.Job j
				WHERE j.ID = @JobID
		END
		ELSE BEGIN
			SELECT @RecordSetID = j.RecordSetID
					,@JobID = j.ID
				FROM NCBI.Request req
				JOIN Job.Job j ON j.ID = req.JobID
				WHERE req.ID = @RequestID
		END
	END

	IF @SubjectGeneID IS NOT NULL BEGIN
		SELECT qry.ID
				,qsrc.Name AS QuerySourceName
				,qry.GenBankID
				,qry.Accession
				,qry.Name
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
				,qry.Accession
				,qry.Name
				,qry.[Definition]
				,qry.Organism
				,qry.Nucleotides -- Used to calculate the sequence length
			FROM NCBI.Request r
			JOIN NCBI.Gene ng ON ng.RequestID = r.ID
			JOIN Gene.Gene qry ON qry.ID = ng.GeneID
			JOIN Job.Job j ON j.ID = r.JobID
			WHERE j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
				AND (@RequestID IS NULL OR r.ID = @RequestID)
			ORDER BY qry.Organism, qry.[Definition]
	END
END
GO

UPDATE Common.ApplicationProperty
	SET Value = '1.5.0.7'
	WHERE [Key] = 'DatabaseVersion'
GO