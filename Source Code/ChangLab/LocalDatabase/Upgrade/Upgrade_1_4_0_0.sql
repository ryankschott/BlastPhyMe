SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'PAML') BEGIN
	EXEC ('CREATE SCHEMA [PAML]')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.Tree')) BEGIN
	CREATE TABLE PAML.Tree (
		ID int IDENTITY (1,1) NOT NULL
		,JobID uniqueidentifier NOT NULL
		,TreeFilePath varchar(250) NOT NULL
		,SequencesFilePath varchar(250) NOT NULL
		,Title varchar(250) NOT NULL
		,[Rank] int NOT NULL
		,ParentID int NULL
		,StatusID int NOT NULL

		,CONSTRAINT PK_PAML_Tree PRIMARY KEY CLUSTERED (ID ASC)
	)

	ALTER TABLE PAML.Tree ADD CONSTRAINT FK_PAML_Tree_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
	ALTER TABLE PAML.Tree ADD CONSTRAINT FK_PAML_Tree_ParentID FOREIGN KEY (ParentID) REFERENCES PAML.Tree (ID)
	ALTER TABLE PAML.Tree ADD CONSTRAINT FK_PAML_Tree_StatusID FOREIGN KEY (StatusID) REFERENCES Job.[Status] (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.AnalysisConfiguration')) BEGIN
	CREATE TABLE PAML.AnalysisConfiguration (
		ID int IDENTITY (1,1) NOT NULL
		,TreeID int NOT NULL
		,Model int NOT NULL CONSTRAINT CK_PAML_AnalysisConfiguration_Model CHECK ([Model] IN (0, 2, 3))
		,NCatG int NOT NULL
		,KStart decimal(9,3) NOT NULL
		,KEnd decimal(9,3) NULL
		,KInterval decimal(9,3) NULL
		,KFixed bit NOT NULL CONSTRAINT DF_PAML_AnalysisConfiguration_KFixed DEFAULT (0)
		,WStart decimal(9,3) NOT NULL
		,WEnd decimal(9,3) NULL
		,WInterval decimal(9,3) NULL
		,WFixed bit NOT NULL CONSTRAINT DF_PAML_AnalysisConfiguration_WFixed DEFAULT (0)
		,[Rank] int NOT NULL
		,StatusID int NOT NULL

		,CONSTRAINT PK_PAML_AnalysisConfiguration PRIMARY KEY CLUSTERED (ID ASC)
	)

	ALTER TABLE PAML.AnalysisConfiguration ADD CONSTRAINT FK_PAML_AnalysisConfiguration_TreeID FOREIGN KEY (TreeID) REFERENCES PAML.Tree (ID)
	ALTER TABLE PAML.AnalysisConfiguration ADD CONSTRAINT FK_PAML_AnalysisConfiguration_StatusID FOREIGN KEY (StatusID) REFERENCES Job.[Status] (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.AnalysisConfigurationNSSite')) BEGIN
	CREATE TABLE PAML.AnalysisConfigurationNSSite (
		ID int IDENTITY (1,1) NOT NULL
		,AnalysisConfigurationID int NOT NULL
		,NSSite int NOT NULL

		,CONSTRAINT PK_PAML_AnalysisConfigurationNSSite PRIMARY KEY CLUSTERED (ID ASC)
	)

	ALTER TABLE PAML.AnalysisConfigurationNSSite ADD CONSTRAINT FK_PAML_AnalysisConfigurationNSSite_AnalysisConfigurationID FOREIGN KEY (AnalysisConfigurationID) REFERENCES PAML.AnalysisConfiguration (ID)
END
GO
--IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.AnalysisControlOption')) BEGIN
--	CREATE TABLE PAML.AnalysisControlOption (
--		ID int IDENTITY (1,1) NOT NULL
--		,AnalysisConfigurationID int NOT NULL
--		,NSSite int NOT NULL
--		,Kappa decimal(9,3) NOT NULL
--		,Omega decimal(9,3) NOT NULL

--		,CONSTRAINT PK_PAML_AnalysisControlOption PRIMARY KEY CLUSTERED (ID ASC)
--	)

--	ALTER TABLE PAML.AnalysisControlOption ADD CONSTRAINT FK_PAML_AnalysisControlOption_AnalysisConfigurationID FOREIGN KEY (AnalysisConfigurationID) REFERENCES PAML.AnalysisConfiguration (ID)
--END
--GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.Result')) BEGIN
	CREATE TABLE PAML.Result (
		ID int IDENTITY (1,1) NOT NULL
		,TreeID int NOT NULL -- Not strictly necessary, because I could derive it from AnalysisConfiguration, but convenient.
		,AnalysisConfigurationID int NOT NULL
		,NSSite int NOT NULL
		,Kappa decimal(9,3) NOT NULL
		,Omega decimal(9,3) NOT NULL
		,np int NOT NULL
		,lnL decimal(19,8) NOT NULL
		,k decimal(9,6) NOT NULL
		,Duration time NOT NULL

		,CONSTRAINT PK_PAML_Result PRIMARY KEY CLUSTERED (ID ASC)
	)

	ALTER TABLE PAML.Result ADD CONSTRAINT FK_PAML_Result_TreeID FOREIGN KEY (TreeID) REFERENCES PAML.Tree (ID)
	ALTER TABLE PAML.Result ADD CONSTRAINT FK_PAML_Result_AnalysisConfigurationID FOREIGN KEY (AnalysisConfigurationID) REFERENCES PAML.AnalysisConfiguration (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.ResultdNdSValueType')) BEGIN
	CREATE TABLE PAML.ResultdNdSValueType (
		ID int IDENTITY(1,1) NOT NULL
		,Name varchar(20) NOT NULL
		,[Key] varchar(20) NOT NULL
		,[Rank] int NOT NULL

		,CONSTRAINT PK_PAML_ResultdNdSValueType PRIMARY KEY CLUSTERED (ID ASC)
	)

	INSERT INTO PAML.ResultdNdSValueType ([Key], Name, [Rank])
	VALUES ('p_value', 'p', 1), ('q_value', 'q', 2), ('p1_value', 'p1', 3), ('w_value', 'w', 4)
			,('background_w', 'Background W', 5), ('foreground_w', 'Foreground W', 6), ('branch_type', 'Branch Type', 7)
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.ResultdNdSValue')) BEGIN
	CREATE TABLE PAML.ResultdNdSValue (
		ID int IDENTITY (1,1) NOT NULL
		,ResultID int NOT NULL
		,SiteClass varchar(2) NULL
		,ValueTypeID int NOT NULL
		,[Rank] int NOT NULL
		,Value decimal(9,6) NOT NULL

		,CONSTRAINT PK_PAML_ResultdNdSValue PRIMARY KEY CLUSTERED (ID ASC)
	)

	ALTER TABLE PAML.ResultdNdSValue ADD CONSTRAINT FK_PAML_ResultdNdSValue_ResultID FOREIGN KEY (ResultID) REFERENCES PAML.Result (ID)
	ALTER TABLE PAML.ResultdNdSValue ADD CONSTRAINT FK_PAML_ResultdNdSValue_ValueTypeID FOREIGN KEY (ValueTypeID) REFERENCES PAML.ResultdNdSValueType (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Job.Job') AND c.name = 'Title') BEGIN
	ALTER TABLE Job.Job ADD Title varchar(200) NULL
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('PAML.GetNSSitesListForAnalysisConfiguration')) BEGIN
	DROP FUNCTION PAML.GetNSSitesListForAnalysisConfiguration
END
GO
CREATE FUNCTION PAML.GetNSSitesListForAnalysisConfiguration (@AnalysisConfigurationID int)
RETURNS varchar(250)
AS
BEGIN
	DECLARE @nssites varchar(250) = ''

	SELECT @nssites = (CASE WHEN @nssites = '' THEN '' ELSE ',' END) + CAST(ns.NSSite AS varchar(5))
		FROM PAML.AnalysisConfigurationNSSite ns
		WHERE ns.AnalysisConfigurationID = @AnalysisConfigurationID
		ORDER BY ns.NSSite

	RETURN @nssites
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.Tree_Edit')) BEGIN
	DROP PROCEDURE PAML.Tree_Edit
END
GO
CREATE PROCEDURE PAML.Tree_Edit
	@JobID uniqueidentifier
	,@TreeFilePath varchar(250)
	,@SequencesFilePath varchar(250)
	,@Title varchar(250)
	,@ParentID int
	,@Rank int
	,@StatusID int
	,@ID int = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		INSERT INTO PAML.Tree (JobID, TreeFilePath, SequencesFilePath, Title, [Rank], ParentID, StatusID)
		VALUES (@JobID, @TreeFilePath, @SequencesFilePath, @Title, @Rank, @ParentID, @StatusID)

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
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.Tree_UpdateStatus')) BEGIN
	DROP PROCEDURE PAML.Tree_UpdateStatus
END
GO
CREATE PROCEDURE PAML.Tree_UpdateStatus
	@ID int
	,@StatusID int
AS
BEGIN
	SET NOCOUNT ON

	UPDATE PAML.Tree
		SET StatusID = @StatusID
		WHERE ID = @ID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.Tree_List')) BEGIN
	DROP PROCEDURE PAML.Tree_List
END
GO
CREATE PROCEDURE PAML.Tree_List
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT t.ID
			,t.TreeFilePath
			,t.SequencesFilePath
			,t.[Rank]
			,t.ParentID
			,t.StatusID
			,stat.Name AS TreeStatus
			,cg.ID AS AnalysisConfigurationID
			,cg.Model
			,cg.NCatG
			,cg.KStart
			,cg.KEnd
			,cg.KInterval
			,cg.KFixed
			,cg.WStart
			,cg.WEnd
			,cg.WInterval
			,cg.WFixed
			,PAML.GetNSSitesListForAnalysisConfiguration(cg.ID) AS NSSites
		FROM PAML.Tree t
		JOIN Job.[Status] stat ON stat.ID = t.StatusID
		JOIN PAML.AnalysisConfiguration cg ON cg.TreeID = t.ID
		WHERE t.JobID = @JobID
		ORDER BY (CASE WHEN t.ParentID IS NULL THEN 1 ELSE 0 END), t.[Rank]
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.AnalysisConfiguration_Edit')) BEGIN
	DROP PROCEDURE PAML.AnalysisConfiguration_Edit
END
GO
CREATE PROCEDURE PAML.AnalysisConfiguration_Edit
	@TreeID int
	,@Model int
	,@NCatG int
	,@KStart decimal(9,3)
	,@KEnd decimal(9,3)
	,@KInterval decimal(9,3)
	,@KFixed bit
	,@WStart decimal(9,3)
	,@WEnd decimal(9,3)
	,@WInterval decimal(9,3)
	,@WFixed bit
	,@Rank int
	,@StatusID int
	,@NSSites Common.ListInt READONLY
	,@ID int = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		INSERT INTO PAML.AnalysisConfiguration (TreeID, Model, NCatG
												,KStart, KEnd, KInterval, KFixed
												,WStart, WEnd, WInterval, WFixed
												,[Rank], StatusID)
		VALUES (@TreeID, @Model, @NCatG
				,@KStart, @KEnd, @KInterval, @KFixed
				,@WStart, @WEnd, @WInterval, @WFixed
				,@Rank, @StatusID)

		SET @ID = @@IDENTITY
	END
	ELSE BEGIN
		UPDATE PAML.AnalysisConfiguration
			SET Model = @Model
				,NCatG = @NCatG
				,KStart = @KStart
				,KEnd = @KEnd
				,KInterval = @KInterval
				,KFixed = @KFixed
				,WStart = @WStart
				,WEnd = @WEnd
				,WInterval = @WInterval
				,WFixed = @WFixed
				,StatusID = @StatusID
			WHERE ID = @ID

		DELETE FROM PAML.AnalysisConfigurationNSSite
			WHERE AnalysisConfigurationID = @ID
	END

	INSERT INTO PAML.AnalysisConfigurationNSSite (AnalysisConfigurationID, NSSite)
	SELECT @ID, ns.Value
		FROM @NSSites ns
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.AnalysisConfiguration_UpdateStatus')) BEGIN
	DROP PROCEDURE PAML.AnalysisConfiguration_UpdateStatus
END
GO
CREATE PROCEDURE PAML.AnalysisConfiguration_UpdateStatus
	@ID int
	,@StatusID int
AS
BEGIN
	SET NOCOUNT ON

	UPDATE PAML.AnalysisConfiguration
		SET StatusID = @StatusID
		WHERE ID = @ID
END
GO
--IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.AnalysisControlOption_Add')) BEGIN
--	DROP PROCEDURE PAML.AnalysisControlOption_Add
--END
--GO
--CREATE PROCEDURE PAML.AnalysisControlOption_Add
--	@AnalysisConfigurationID int
--	,@NSSite int
--	,@Kappa decimal(9,3)
--	,@Omega decimal(9,3)
--	,@ID int = NULL OUTPUT
--AS
--BEGIN
--	SET NOCOUNT ON

--	IF (@ID IS NULL) BEGIN
--		INSERT INTO PAML.AnalysisControlOption (AnalysisConfigurationID, NSSite, Kappa, Omega)
--		VALUES (@AnalysisConfigurationID, @NSSite, @Kappa, @Omega)

--		SET @ID = @@IDENTITY
--	END
--	-- No use case yet for an UPDATE
--END
--GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.Result_Add')) BEGIN
	DROP PROCEDURE PAML.Result_Add
END
GO
CREATE PROCEDURE PAML.Result_Add
	@TreeID int = NULL
	,@AnalysisConfigurationID int
	,@NSSite int
	,@Kappa decimal(9,3)
	,@Omega decimal(9,3)
	,@np int
	,@lnL decimal(19,8)
	,@k decimal(9,6)
	,@Duration time
	,@ID int = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF (@TreeID IS NULL) BEGIN
		SELECT @TreeID = cf.TreeID
			FROM PAML.AnalysisConfiguration cf
			WHERE cf.ID = @AnalysisConfigurationID
	END

	IF (@ID IS NULL) BEGIN
		INSERT INTO PAML.Result (TreeID, AnalysisConfigurationID, NSSite, Kappa, Omega, np, lnL, k, Duration)
		VALUES (@TreeID, @AnalysisConfigurationID, @NSSite, @Kappa, @Omega, @np, @lnL, @k, @Duration)

		SET @ID = @@IDENTITY
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.ResultdNdSValueType_List')) BEGIN
	DROP PROCEDURE PAML.ResultdNdSValueType_List
END
GO
CREATE PROCEDURE PAML.ResultdNdSValueType_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT vt.ID
			,vt.Name
			,vt.[Key]
		FROM PAML.ResultdNdSValueType vt
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.ResultdNdSValue_Add')) BEGIN
	DROP PROCEDURE PAML.ResultdNdSValue_Add
END
GO
CREATE PROCEDURE PAML.ResultdNdSValue_Add
	@ResultID int
	,@SiteClass varchar(2)
	,@ValueTypeID int
	,@Rank int
	,@Value decimal(9,6)
	,@ID int OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		INSERT INTO PAML.ResultdNdSValue (ResultID, SiteClass, ValueTypeID, [Rank], Value)
		VALUES (@ResultID, @SiteClass, @ValueTypeID, @Rank, @Value)

		SET @ID = @@IDENTITY
	END
END
GO
ALTER PROCEDURE [Job].[Job_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@SubSetID uniqueidentifier = NULL, -- Not used in an UPDATE
	@TargetID int = NULL, -- Not used in an UPDATE
	@Title varchar(250) = NULL,
	@StatusID int = NULL,
	@StartedAt datetime2(7) = NULL, -- Not used in an UPDATE
	@EndedAt datetime2(7) = NULL,
	@Active bit = NULL
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @RecordSetID uniqueidentifier

	IF NOT EXISTS (SELECT * FROM Job.Job j WHERE j.ID = @ID) BEGIN
		SET @ID = NEWID()

		SELECT @RecordSetID = sub.RecordSetID
			FROM RecordSet.SubSet sub
			WHERE sub.ID = @SubSetID

		INSERT INTO Job.Job (ID, RecordSetID, SubSetID, TargetID, Title, StartedAt, EndedAt)
		VALUES (@ID, @RecordSetID, @SubSetID, @TargetID, @Title, @StartedAt, @EndedAt)
	END
	ELSE BEGIN
		UPDATE Job.Job
			SET StatusID = ISNULL(@StatusID, StatusID)
				,Title = @Title
				,EndedAt = ISNULL(@EndedAt, EndedAt)
				,Active = ISNULL(@Active, Active)
			WHERE ID = @ID
	END
END
GO
IF NOT EXISTS (SELECT * FROM Job.[Target] WHERE [Key] = 'CodeML') BEGIN
	INSERT INTO Job.[Target] VALUES ('CodeML', 'CodeML')
END
GO
IF NOT EXISTS (SELECT * FROM Job.[Status] WHERE [Key] = 'Pending') BEGIN
	INSERT INTO Job.[Status] VALUES ('Pending', 'Pending')
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.0.0'
	WHERE [Key] = 'DatabaseVersion'
GO