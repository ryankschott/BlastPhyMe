SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM Gene.[Source] WHERE [Key] = 'Trinity') BEGIN
	INSERT INTO Gene.[Source] (Name, [Key]) VALUES ('Trinity', 'Trinity')
END
GO
ALTER PROCEDURE Job.BlastN_ListSubjectGenesForQueryGene
	@QueryGeneID uniqueidentifier
	,@RecordSetID uniqueidentifier
	,@JobID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON
		
	SELECT sbj.*
			,ssrc.Name AS QuerySourceName
			--,sbj.GenBankID
			--,sbj.Name
			--,sbj.[Definition]
			--,sbj.Organism
			--,sbj.Taxonomy
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
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.BlastN_ListAnnotationGenesForJob')) BEGIN
	DROP PROCEDURE Job.BlastN_ListAnnotationGenesForJob
END
GO
CREATE PROCEDURE Job.BlastN_ListAnnotationGenesForJob
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT qry.*
			,sbj.ID AS SubjectID
			,sbj.SourceID AS SubjectSourceID
			,sbj.[Definition] AS SubjectDefinition
			,sbj.GenBankID AS SubjectGenBankID
			,sbj.LastUpdatedAt AS SubjectLastUpdatedAt
		FROM Job.Gene j
		JOIN Gene.Gene qry ON qry.ID = j.GeneID

		LEFT OUTER JOIN (
				SELECT al.QueryID
						,sbj.*
					FROM Gene.Gene sbj
					JOIN BlastN.Alignment al ON al.SubjectID = sbj.ID AND al.[Rank] = 0 -- Return just the top match
					JOIN NCBI.BlastNAlignment ncbi ON ncbi.AlignmentID = al.ID
					JOIN NCBI.Request req ON req.ID = ncbi.RequestID
					WHERE req.JobID = @JobID
			) sbj ON sbj.QueryID = qry.ID

		WHERE j.JobID = @JobID
			AND j.DirectionID = 1

END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.SubSet_ListSubSetIDsForGeneIDs')) BEGIN
	DROP PROCEDURE RecordSet.SubSet_ListSubSetIDsForGeneIDs
END
GO
CREATE PROCEDURE RecordSet.SubSet_ListSubSetIDsForGeneIDs
	@GeneIDs [Common].[ListUniqueIdentifier] READONLY
	,@Open bit = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT DISTINCT sub.ID
		FROM RecordSet.SubSet sub
		JOIN RecordSet.SubSetGene sg ON sg.SubSetID = sub.ID
		JOIN @GeneIDs g ON g.Value = sg.GeneID
		WHERE sub.Active = 1
			AND (@Open IS NULL OR sub.[Open] = @Open)
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.4.0'
	WHERE [Key] = 'DatabaseVersion'
GO
