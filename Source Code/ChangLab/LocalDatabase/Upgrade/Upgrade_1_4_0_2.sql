SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.name = 'SequenceCount' AND c.object_id = OBJECT_ID('PAML.Tree')) BEGIN
	ALTER TABLE PAML.Tree
		ADD SequenceCount int NOT NULL CONSTRAINT DF_PAML_Tree_SequenceCount DEFAULT ((0))
		,SequenceLength int NOT NULL CONSTRAINT DF_PAML_Tree_SequenceLength DEFAULT ((0))

	ALTER TABLE PAML.Tree DROP CONSTRAINT DF_PAML_Tree_SequenceCount
	ALTER TABLE PAML.Tree DROP CONSTRAINT DF_PAML_Tree_SequenceLength
END
GO
ALTER PROCEDURE [PAML].[Tree_Edit]
	@JobID uniqueidentifier
	,@TreeFilePath varchar(250)
	,@SequencesFilePath varchar(250)
	,@Title varchar(250)
	,@ParentID int
	,@Rank int
	,@StatusID int
	,@SequenceCount int
	,@SequenceLength int
	,@ID int = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		INSERT INTO PAML.Tree (JobID, TreeFilePath, SequencesFilePath, Title, [Rank], ParentID, StatusID, SequenceCount, SequenceLength)
		VALUES (@JobID, @TreeFilePath, @SequencesFilePath, @Title, @Rank, @ParentID, @StatusID, @SequenceCount, @SequenceLength)

		SET @ID = @@IDENTITY
	END
	ELSE BEGIN
		UPDATE PAML.Tree
			SET [Rank] = @Rank
				,StatusID = @StatusID
			WHERE ID = @ID
	END
END
GO
ALTER PROCEDURE [PAML].[Tree_List]
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT t.ID
			,t.TreeFilePath
			,t.SequencesFilePath
			,t.Title
			,t.[Rank]
			,t.ParentID
			,t.StatusID
			,t.SequenceCount
			,t.SequenceLength
			,t_st.Name AS TreeStatusName
			,cf.ID AS AnalysisConfigurationID
			,cf.Model
			,cf.ModelPresetID
			,cf.NCatG
			,cf.KStart
			,cf.KEnd
			,cf.KInterval
			,cf.KFixed
			,cf.WStart
			,cf.WEnd
			,cf.WInterval
			,cf.WFixed
			,cf.[Rank]
			,cf.StatusID
			,cf_st.Name AS ConfigurationStatusName
			,PAML.GetNSSitesListForAnalysisConfiguration(cf.ID) AS NSSites
		FROM PAML.Tree t
		JOIN Job.[Status] t_st ON t_st.ID = t.StatusID
		JOIN PAML.AnalysisConfiguration cf ON cf.TreeID = t.ID
		JOIN PAML.ModelPreset mp ON mp.ID = cf.ModelPresetID
		JOIN Job.[Status] cf_st ON cf_st.ID = cf.StatusID
		WHERE t.JobID = @JobID
		ORDER BY (CASE WHEN t.ParentID IS NULL THEN 1 ELSE 0 END), t.[Rank]
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.name = 'CompletedAt' AND c.object_id = OBJECT_ID('PAML.Result')) BEGIN
	ALTER TABLE PAML.Result ADD 
		CompletedAt datetime2(7) NOT NULL CONSTRAINT DF_PAML_Result_CompletedAt DEFAULT ((sysdatetime())),
		Active bit NOT NULL CONSTRAINT DF_PAML_Result_Active DEFAULT ((1))

	EXEC ('UPDATE r
			SET CompletedAt = ISNULL(j.EndedAt, CompletedAt)
			FROM PAML.Result r
			JOIN PAML.Tree t ON t.ID = r.TreeID
			JOIN Job.Job j ON j.ID = t.JobID')
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
		ORDER BY t.[Rank], ResultRank

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
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.Result_Details')) BEGIN
	DROP PROCEDURE PAML.Result_Details
END
GO
CREATE PROCEDURE PAML.Result_Details
	@ResultID int
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @TreeID int
			,@AnalysisConfigurationID int
			,@NSSite int

	SELECT @TreeID = r.TreeID
			,@AnalysisConfigurationID = r.AnalysisConfigurationID
			,@NSSite = r.NSSite
		FROM PAML.Result r
		WHERE r.ID = @ResultID

	SELECT t.ID AS TreeID
			,t.Title AS TreeTitle
			,t.TreeFilePath
			,t.SequenceCount
			,t.SequenceLength
			,t.SequencesFilePath
			,cf.ModelPresetID
			,mp.[Key] AS ModelPresetKey
			,r.NSSite
			,cf.NCatG
			,cf.KStart, cf.KEnd, cf.KInterval, cf.KFixed
			,cf.WStart, cf.WEnd, cf.WInterval, cf.WFixed
		FROM PAML.Result r
		JOIN PAML.Tree t ON t.ID = r.TreeID
		JOIN PAML.AnalysisConfiguration cf ON cf.ID = r.AnalysisConfigurationID
		JOIN PAML.ModelPreset mp ON mp.ID = cf.ModelPresetID
		WHERE r.ID = @ResultID

	SELECT r.ID AS ResultID
			,r.np
			,r.lnL
			,r.k
			,r.Kappa
			,r.Omega
			,ROW_NUMBER() OVER (ORDER BY r.lnL DESC, r.Kappa, r.Omega) AS RowNumber
		FROM PAML.Result r
		WHERE r.TreeID = @TreeID
			AND r.AnalysisConfigurationID = @AnalysisConfigurationID
			AND r.NSSite = @NSSite
			AND r.Active = 1
		ORDER BY RowNumber

	SELECT r.ID AS ResultID
			,vt.[Rank] AS TypeRank
			,val.[Rank] AS ValueRank
			,ISNULL(val.SiteClass, '0') AS SiteClass
			,vt.Name AS ValueTypeName
			,vt.[Key] AS ValueTypeKey
			,val.Value
		FROM PAML.Result r
		JOIN PAML.ResultdNdSValue val ON val.ResultID = r.ID
		JOIN PAML.ResultdNdSValueType vt ON vt.ID = val.ValueTypeID
		WHERE r.TreeID = @TreeID
			AND r.AnalysisConfigurationID = @AnalysisConfigurationID
			AND r.NSSite = @NSSite
			AND r.Active = 1
		ORDER BY r.ID, TypeRank, ValueRank, SiteClass
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.Result_Delete')) BEGIN
	DROP PROCEDURE PAML.Result_Delete
END
GO
CREATE PROCEDURE PAML.Result_Delete
	@ResultID int
	,@DeleteRelated bit = 1
AS
BEGIN
	SET NOCOUNT ON

	IF (@DeleteRelated = 1) BEGIN
		DECLARE @TreeID int
				,@AnalysisConfigurationID int
				,@NSSite int

		SELECT @TreeID = r.TreeID
				,@AnalysisConfigurationID = r.AnalysisConfigurationID
				,@NSSite = r.NSSite
			FROM PAML.Result r
			WHERE r.ID = @ResultID

		UPDATE r
			SET r.Active = 0
			FROM PAML.Result r
			WHERE r.TreeID = @TreeID
				AND r.AnalysisConfigurationID = @AnalysisConfigurationID
				AND r.NSSite = @NSSite
	END
	ELSE BEGIN
		UPDATE r
			SET r.Active = 0
			FROM PAML.Result r
			WHERE r.ID = @ResultID
	END
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('RecordSet.DataType')) BEGIN
	CREATE TABLE RecordSet.DataType (
		ID int IDENTITY(1,1) NOT NULL,
		Name varchar(30) NOT NULL,
		[Key] varchar(30) NOT NULL,

		CONSTRAINT PK_RecordSet_DataType PRIMARY KEY (ID ASC)
	)

	INSERT INTO RecordSet.DataType (Name, [Key])
	VALUES ('Gene Sequence', 'GeneSequence'), ('CodeML Result', 'CodeMLResult')
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.name = 'DataTypeID' AND c.object_id = OBJECT_ID('RecordSet.SubSet')) BEGIN
	ALTER TABLE RecordSet.SubSet ADD DataTypeID int NOT NULL 
		CONSTRAINT FK_RecordSet_SubSet_DataTypeID FOREIGN KEY REFERENCES RecordSet.DataType (ID)
		CONSTRAINT DF_RecordSet_SubSet_DataTypeID DEFAULT ((1))

		ALTER TABLE RecordSet.SubSet DROP CONSTRAINT DF_RecordSet_SubSet_DataTypeID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.DataType_List')) BEGIN
	DROP PROCEDURE RecordSet.DataType_List
END
GO
CREATE PROCEDURE RecordSet.DataType_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT dt.ID
			,dt.Name
			,dt.[Key]
		FROM RecordSet.DataType dt
END
GO
ALTER PROCEDURE [RecordSet].[Import_SubSet]
	@RecordSetID uniqueidentifier
	,@ID uniqueidentifier = NULL OUTPUT
	,@Name varchar(100)
	,@DataTypeID int
	,@LastOpenedAt datetime2(7) = NULL
	,@Open bit
AS
BEGIN
	SET NOCOUNT ON
	SET @ID = NEWID()

	INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DataTypeID, LastOpenedAt, [Open])
	VALUES (@ID, @RecordSetID, @DataTypeID, @Name, @LastOpenedAt, @Open)
END
GO
ALTER PROCEDURE [RecordSet].[RecordSet_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@Name varchar(200),
	@Active bit = NULL
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @NowTime datetime2(7) = SYSDATETIME();

	IF (@ID IS NULL) BEGIN
		SET @ID = NEWID()
	END

	IF NOT EXISTS (SELECT * FROM RecordSet.RecordSet WHERE ID = @ID) BEGIN
		INSERT INTO RecordSet.RecordSet (ID, Name, CreatedAt, ModifiedAt)
		VALUES (@ID, @Name, @NowTime, @NowTime)

		DECLARE @DataTypeID_GeneSequence int = (SELECT dt.ID FROM RecordSet.DataType dt WHERE dt.[Key] = 'GeneSequence')
				,@DataTypeID_CodeMLResult int = (SELECT dt.ID FROM RecordSet.DataType dt WHERE dt.[Key] = 'CodeMLResult')

		INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DataTypeID)
		VALUES (NEWID(), @ID, 'All', @DataTypeID_GeneSequence),
				(NEWID(), @ID, 'Excluded', @DataTypeID_GeneSequence),
				(NEWID(), @ID, 'Duplicates', @DataTypeID_GeneSequence)
	
		INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DataTypeID)
			VALUES (NEWID(), @ID, 'All', @DataTypeID_CodeMLResult)
	END
	ELSE BEGIN
		IF (@Active IS NOT NULL) AND ((SELECT rs.Active FROM RecordSet.RecordSet rs WHERE rs.ID = @ID) <> @Active) BEGIN
			UPDATE RecordSet.RecordSet
				SET ModifiedAt = @NowTime,
					Active = @Active
				WHERE ID = @ID
		END
		ELSE BEGIN
			UPDATE RecordSet.RecordSet
				SET Name = @Name,
					ModifiedAt = @NowTime
				WHERE ID = @ID
		END
	END
END
GO
ALTER PROCEDURE [RecordSet].[SubSet_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@RecordSetID uniqueidentifier = NULL,
	@Name varchar(200),
	@DataTypeID int,
	@Active bit = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		SET @ID = NEWID()
	END

	IF NOT EXISTS (SELECT * FROM RecordSet.SubSet WHERE ID = @ID) BEGIN
		INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DataTypeID, DisplayIndex)
		SELECT @ID, @RecordSetID, @Name, @DataTypeID
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
ALTER FUNCTION [RecordSet].[SubSet_IDByName] (@Name varchar(100), @RecordSetID uniqueidentifier, @DataTypeID int)
RETURNS uniqueidentifier
AS
BEGIN
	RETURN (SELECT ID FROM RecordSet.SubSet WHERE Name = @Name AND RecordSetID = @RecordSetID AND DataTypeID = @DataTypeID)
END
GO
ALTER PROCEDURE [RecordSet].[SubSet_List]
	@RecordSetID uniqueidentifier
	,@DataTypeID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT sub.ID
			,sub.Name
			,sub.DataTypeID
			,sub.LastOpenedAt
			,sub.[Open]
			,sub.DisplayIndex
			,(SELECT COUNT(*)
					FROM RecordSet.SubSetGene sg
					WHERE sg.SubSetID = sub.ID) AS GeneCount
		FROM RecordSet.SubSet sub
		WHERE sub.RecordSetID = @RecordSetID
			AND sub.DataTypeID = @DataTypeID
			AND sub.Active = 1
		ORDER BY sub.LastOpenedAt, sub.Name
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
ALTER PROCEDURE [RecordSet].[RecordSet_Opened]
	@ID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	UPDATE RecordSet.RecordSet
		SET LastOpenedAt = SYSDATETIME()
		WHERE ID = @ID
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.SubSetResult')) BEGIN
	CREATE TABLE PAML.SubSetResult (
		SubSetID uniqueidentifier NOT NULL
		,ResultID int NOT NULL

		,CONSTRAINT PK_PAML_SubSetResult PRIMARY KEY (SubSetID ASC, ResultID ASC)
	)

	ALTER TABLE PAML.SubSetResult ADD CONSTRAINT FK_PAML_SubSetResult_SubSetID FOREIGN KEY (SubSetID) REFERENCES RecordSet.SubSet (ID)
	ALTER TABLE PAML.SubSetResult ADD CONSTRAINT FK_PAML_SubSetResult_ResultID FOREIGN KEY (ResultID) REFERENCES PAML.Result (ID)

	INSERT INTO PAML.SubSetResult (SubSetID, ResultID)
	SELECT sub.ID, r.ID
		FROM PAML.Result r
		JOIN PAML.Tree t ON t.ID = r.TreeID
		JOIN Job.Job j ON j.ID = t.JobID
		JOIN RecordSet.SubSet sub ON sub.RecordSetID = j.RecordSetID
		WHERE sub.DataTypeID = 2
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.SubSetResult_Edit')) BEGIN
	DROP PROCEDURE PAML.SubSetResult_Edit
END
GO
CREATE PROCEDURE PAML.SubSetResult_Edit
	@SubSetID uniqueidentifier
	,@ResultID int
AS
BEGIN
	SET NOCOUNT ON
	
	IF NOT EXISTS (SELECT * FROM PAML.SubSetResult WHERE SubSetID = @SubSetID AND ResultID = @ResultID) BEGIN
		INSERT INTO PAML.SubSetResult (SubSetID, ResultID)
		VALUES (@SubSetID, @ResultID)
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.SubSetResult_EditMultiple')) BEGIN
	DROP PROCEDURE PAML.SubSetResult_EditMultiple
END
GO
CREATE PROCEDURE PAML.SubSetResult_EditMultiple
	@SubSetID uniqueidentifier
	,@ResultIDs Common.ListInt READONLY
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO PAML.SubSetResult (SubSetID, ResultID)
	SELECT @SubSetID, r.Value
		FROM @ResultIDs r
		WHERE NOT EXISTS (SELECT * FROM PAML.SubSetResult ex WHERE ex.SubSetID = @SubSetID AND ex.ResultID = r.Value)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.SubSetResult_DeleteMultiple')) BEGIN
	DROP PROCEDURE PAML.SubSetResult_DeleteMultiple
END
GO
CREATE PROCEDURE PAML.SubSetResult_DeleteMultiple
	@SubSetID uniqueidentifier
	,@ResultIDs Common.ListInt READONLY
AS
BEGIN
	SET NOCOUNT ON

	DELETE sr
		FROM PAML.SubSetResult sr
		JOIN @ResultIDs r ON r.Value = sr.ResultID
		WHERE sr.SubSetID = @SubSetID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.Result_List')) BEGIN
	DROP PROCEDURE PAML.Result_List
END
GO
CREATE PROCEDURE PAML.Result_List
	@SubSetID uniqueidentifier 
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @TopResults TABLE (TreeID int, ResultID int);

	WITH AllResults AS (
		SELECT t.ID AS TreeID
				,r.ID AS ResultID
				,ROW_NUMBER() OVER (PARTITION BY t.ID, cf.ModelPresetID, r.NSSite ORDER BY lnL DESC, r.Kappa, r.Omega) AS RowNumber
			FROM PAML.SubSetResult sr
			JOIN PAML.Result r ON r.ID = sr.ResultID
			JOIN PAML.Tree t ON t.ID = r.TreeID
			JOIN Job.Job j ON j.ID = t.JobID
			JOIN PAML.AnalysisConfiguration cf ON cf.ID = r.AnalysisConfigurationID
			WHERE sr.SubSetID = @SubSetID
				AND r.Active = 1
				AND j.Active = 1
	)

	INSERT INTO @TopResults
	SELECT r.TreeID, r.ResultID
		FROM AllResults r
		WHERE r.RowNumber = 1

	SELECT r.TreeID
			,r.ID AS ResultID
			,r.AnalysisConfigurationID
			,j.Title AS JobTitle
			,t.Title AS TreeTitle
			,Common.FileNameFromPath(t.TreeFilePath) AS TreeFileName
			,Common.FileNameFromPath(t.SequencesFilePath) AS SequencesFileName
			,mp.[Key] AS ModelPresetKey
			,r.NSSite
			,r.Kappa
			,r.Omega
			,r.lnL
			,r.k
			,CASE WHEN mp.[Key] LIKE 'Model%' THEN 0
				  WHEN mp.[Key] LIKE 'BranchSite%' THEN 2
				  WHEN mp.[Key] LIKE 'Branch%' THEN 1
				  WHEN mp.[Key] LIKE 'CmC%' THEN 3
				  END AS ModelGrossRank
			,CAST(CASE mp.[Key]
				WHEN 'Model0' THEN '0.' + (CASE WHEN r.NSSite = 8 THEN '81' ELSE CONVERT(varchar(5), r.NSSite) END)
				WHEN 'Model2a' THEN '0.21'
				WHEN 'Model8a' THEN '0.82'
				WHEN 'Branch' THEN '2.00'
				WHEN 'BranchNull' THEN '2.02'
				WHEN 'BranchSiteNull' THEN '2.20'
				WHEN 'BranchSite' THEN '2.21'
				WHEN 'CmC' THEN '3.0'
				WHEN 'CmCNull' THEN '3.1'
				ELSE CONVERT(varchar(5), mp.[Rank])
				END AS decimal(5,2)) AS ResultRank
			,j.StartedAt
		FROM @TopResults tr
		JOIN PAML.Result r ON r.ID = tr.ResultID
		JOIN PAML.Tree t ON t.ID = r.TreeID
		JOIN PAML.AnalysisConfiguration cf ON cf.ID = r.AnalysisConfigurationID
		JOIN PAML.ModelPreset mp ON mp.ID = cf.ModelPresetID
		JOIN Job.Job j ON j.ID = t.JobID
		ORDER BY j.StartedAt DESC, ModelGrossRank, t.Title, ResultRank, r.NSSite

END
GO

IF NOT EXISTS (SELECT * FROM sys.index_columns ic
				JOIN sys.indexes i ON i.object_id = ic.object_id AND i.index_id = ic.index_id
				JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
				WHERE i.object_id = OBJECT_ID('RecordSet.SubSet')
					AND i.name = 'IX_RecordSet_SubSet_Name'
					AND c.name = 'DataTypeID') BEGIN
	EXEC ('DROP INDEX [IX_RecordSet_SubSet_Name] ON [RecordSet].[SubSet]')
	EXEC ('CREATE UNIQUE NONCLUSTERED INDEX [IX_RecordSet_SubSet_Name] ON [RecordSet].[SubSet]
			(
				[RecordSetID] ASC,
				[Name] ASC,
				[DataTypeID] ASC
			)')

	DECLARE @DataTypeID_CodeMLResult int = (SELECT dt.ID FROM RecordSet.DataType dt WHERE dt.[Key] = 'CodeMLResult')

	INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DataTypeID)
	SELECT NEWID(), rs.ID, 'All', @DataTypeID_CodeMLResult
		FROM RecordSet.RecordSet rs
		WHERE NOT EXISTS (SELECT * FROM RecordSet.SubSet sub WHERE sub.RecordSetID = rs.ID AND sub.DataTypeID = @DataTypeID_CodeMLResult)
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.0.2'
	WHERE [Key] = 'DatabaseVersion'
GO