SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [RecordSet].[Import_SubSet]
	@RecordSetID uniqueidentifier
	,@ID uniqueidentifier = NULL OUTPUT
	,@Name varchar(100)
	,@DataTypeID int
	,@LastOpenedAt datetime2(7) = NULL
	,@Open bit
	,@DisplayIndex int = 0
AS
BEGIN
	SET NOCOUNT ON
	SET @ID = NEWID()

	INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DataTypeID, LastOpenedAt, [Open], DisplayIndex)
	VALUES (@ID, @RecordSetID, @Name, @DataTypeID, @LastOpenedAt, @Open, @DisplayIndex)
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
						,[SubSet].DataTypeID
						,[SubSet].LastOpenedAt
						,[SubSet].[Open]
						,[SubSet].DisplayIndex
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
						,[Feature-Interval].Accession
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
ALTER PROCEDURE [PAML].[Job_ListTopResults]
	@RecordSetID uniqueidentifier
	,@JobID uniqueidentifier = NULL
	,@ResultIDs Common.ListInt READONLY
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TopResults TABLE (TreeID int, ResultID int);

	IF EXISTS (SELECT * FROM @ResultIDs) BEGIN
		INSERT INTO @TopResults
		SELECT r.TreeID, r.ID
			FROM @ResultIDs r_id
			JOIN PAML.Result r ON r.ID = r_id.Value
	END
	ELSE BEGIN
		WITH AllResults AS (
			SELECT t.ID AS TreeID
					,r.ID AS ResultID
					,ROW_NUMBER() OVER (PARTITION BY t.ID, cf.ModelPresetID, r.NSSite ORDER BY lnL DESC, r.Kappa, r.Omega) AS RowNumber
			FROM PAML.Tree t
			JOIN PAML.Result r ON r.TreeID = t.ID
			JOIN PAML.AnalysisConfiguration cf ON cf.ID = r.AnalysisConfigurationID
			JOIN Job.Job j ON j.ID = t.JobID
			WHERE r.Active = 1
				AND (@RecordSetID IS NULL OR j.RecordSetID = @RecordSetID)
				AND ((@JobID IS NULL AND j.Active = 1)
						OR
					(@JobID IS NOT NULL AND (t.JobID = @JobID)))
		)

		INSERT INTO @TopResults
		SELECT r.TreeID, r.ResultID
			FROM AllResults r
			WHERE r.RowNumber = 1
	END

	SELECT tr.TreeID
			,tr.ResultID
			,r.AnalysisConfigurationID
			,t.Title
			,t.[Rank]
			,mp.[Key] AS ModelPresetKey
			,r.NSSite
			,r.Kappa
			,r.Omega
			,r.np
			,r.lnL
			,CASE mp.[Key]
				WHEN 'Model0' THEN (CASE WHEN r.NSSite = 8 THEN '8b' ELSE CONVERT(varchar(5), r.NSSite) END)
				WHEN 'Model2a' THEN '2a'
				WHEN 'Model8a' THEN '8a'
				WHEN 'BranchNull' THEN '9'
				WHEN 'Branch' THEN '10'
				WHEN 'BranchSiteNull' THEN '11'
				WHEN 'BranchSite' THEN '12'
				WHEN 'CmC' THEN '13'
				WHEN 'CmCNull' THEN '14'
				ELSE CONVERT(varchar(5), mp.[Rank])
				END AS ResultRank
			,r.k
			,r.CompletedAt
		FROM @TopResults tr
		JOIN PAML.Tree t ON t.ID = tr.TreeID
		JOIN PAML.Result r ON r.ID = tr.ResultID
		JOIN PAML.AnalysisConfiguration cf ON cf.ID = r.AnalysisConfigurationID
		JOIN PAML.ModelPreset mp ON mp.ID = cf.ModelPresetID
		ORDER BY t.Title, ResultRank

	SELECT tr.ResultID
			,vt.[Rank] AS TypeRank
			,val.[Rank] AS ValueRank
			,ISNULL(val.SiteClass, '0') AS SiteClass
			,vt.Name AS ValueTypeName
			,vt.[Key] AS ValueTypeKey
			,val.Value
		FROM @TopResults tr
		JOIN PAML.ResultdNdSValue val ON val.ResultID = tr.ResultID
		JOIN PAML.ResultdNdSValueType vt ON vt.ID = val.ValueTypeID
		ORDER BY tr.ResultID, TypeRank, ValueRank, SiteClass

	-- PIVOT
	/*
	SELECT t.Title
			,t.[Rank]
			,r.NSSite
			,r.Kappa
			,r.Omega
			,r.np
			,r.lnL
			,r.k
			,pvt.ValueTypeName
			,pvt.[0], pvt.[1], pvt.[2], pvt.[2a], pvt.[2b]
		FROM TopResults tr
		JOIN PAML.Tree t ON t.ID = tr.TreeID
		JOIN PAML.Result r ON r.ID = tr.ResultID
		JOIN (SELECT * 
				FROM (SELECT tr.ResultID
							,vt.[Rank] AS TypeRank
							,val.[Rank] AS ValueRank
							,ISNULL(val.SiteClass, 0) AS SiteClass
							,vt.Name AS ValueTypeName
							,val.Value
						FROM TopResults tr
						JOIN PAML.ResultdNdSValue val ON val.ResultID = tr.ResultID
						JOIN PAML.ResultdNdSValueType vt ON vt.ID = val.ValueTypeID) p
						PIVOT (MAX(Value) FOR SiteClass IN ([0], [1], [2], [2a], [2b])) pvt) pvt ON pvt.ResultID = r.ID
		ORDER BY t.[Rank], r.NSSite, r.Kappa, r.Omega, pvt.TypeRank, pvt.ValueRank
	*/
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.0.5'
	WHERE [Key] = 'DatabaseVersion'
GO