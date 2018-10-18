SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM Gene.[Source] WHERE [Key] = 'NEXUS') BEGIN
	INSERT INTO Gene.[Source] (Name, [Key]) VALUES ('NEXUS', 'NEXUS')
END
GO
IF NOT EXISTS (SELECT * FROM Gene.[Source] WHERE [Key] = 'PHYLIP') BEGIN
	INSERT INTO Gene.[Source] (Name, [Key]) VALUES ('PHYLIP', 'PHYLIP')
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.name = 'ControlConfiguration' AND c.object_id = OBJECT_ID('PAML.Tree')) BEGIN
	ALTER TABLE PAML.Tree ADD ControlConfiguration xml NULL
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
	,@ControlConfiguration xml = NULL
	,@ID int = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		INSERT INTO PAML.Tree (JobID, TreeFilePath, SequencesFilePath, Title, [Rank], ParentID, StatusID, SequenceCount, SequenceLength, ControlConfiguration)
		VALUES (@JobID, @TreeFilePath, @SequencesFilePath, @Title, @Rank, @ParentID, @StatusID, @SequenceCount, @SequenceLength, @ControlConfiguration)

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
			,t.ControlConfiguration
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

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.ProcessOutput')) BEGIN
	CREATE TABLE PAML.ProcessOutput (
		ID int IDENTITY(1,1) NOT NULL,
		TreeID int NOT NULL,
		AnalysisConfigurationID int NOT NULL,
		Kappa decimal(9,3) NOT NULL,
		Omega decimal(9,3) NOT NULL,
		StatusID int NOT NULL,
		ProcessDirectory varchar(250) NULL,
		OutputData varchar(MAX) SPARSE NULL,
		ErrorData varchar(MAX) SPARSE NULL,
		CONSTRAINT PK_PAML_ProcessOutput PRIMARY KEY CLUSTERED (ID ASC)
	)

	ALTER TABLE PAML.ProcessOutput ADD CONSTRAINT FK_PAML_ProcessOutput_TreeID FOREIGN KEY (TreeID) REFERENCES PAML.Tree (ID)
	ALTER TABLE PAML.ProcessOutput ADD CONSTRAINT FK_PAML_ProcessOutput_AnalysisConfigurationID FOREIGN KEY (AnalysisConfigurationID) REFERENCES PAML.AnalysisConfiguration (ID)
	ALTER TABLE PAML.ProcessOutput ADD CONSTRAINT FK_PAML_ProcessOutput_StatusID FOREIGN KEY (StatusID) REFERENCES Job.[Status] (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PAML.ProcessException')) BEGIN
	CREATE TABLE PAML.ProcessException (
		ExceptionID int NOT NULL,
		ProcessOutputID int NOT NULL, 
		CONSTRAINT PK_PAML_ProcessException PRIMARY KEY CLUSTERED (ExceptionID ASC)
	)

	ALTER TABLE PAML.ProcessException ADD CONSTRAINT FK_PAML_ProcessException_ExceptionID FOREIGN KEY (ExceptionID) REFERENCES Job.Exception (ID)
	ALTER TABLE PAML.ProcessException ADD CONSTRAINT FK_PAML_ProcessException_ProcessOutputID FOREIGN KEY (ProcessOutputID) REFERENCES PAML.ProcessOutput (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.name = 'ExceptionType' AND c.object_id = OBJECT_ID('Job.Exception')) BEGIN
	ALTER TABLE Job.Exception ADD ExceptionType varchar(250) NULL
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.ProcessOutput_Add')) BEGIN
	DROP PROCEDURE PAML.ProcessOutput_Add
END
GO
CREATE PROCEDURE PAML.ProcessOutput_Add
	@TreeID int,
	@AnalysisConfigurationID int,
	@Kappa decimal(9,3),
	@Omega decimal(9,3),
	@StatusID int,
	@ProcessDirectory varchar(250) = NULL,
	@OutputData varchar(MAX) = NULL,
	@ErrorData varchar(MAX) = NULL,
	@ID int OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO PAML.ProcessOutput (TreeID, AnalysisConfigurationID, Kappa, Omega, StatusID, ProcessDirectory, OutputData, ErrorData)
	VALUES (@TreeID, @AnalysisConfigurationID, @Kappa, @Omega, @StatusID, @ProcessDirectory, @OutputData, @ErrorData)

	SET @ID = @@IDENTITY
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.ProcessOutput_List')) BEGIN
	DROP PROCEDURE PAML.ProcessOutput_List
END
GO
CREATE PROCEDURE PAML.ProcessOutput_List
	@JobID uniqueidentifier = NULL,
	@TreeID int = NULL,
	@AnalysisConfigurationID int = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT p.ID
			,t.Title
			,s.Name AS [Status]
			,t.TreeFilePath
			,t.SequencesFilePath
			,cf.ModelPresetID
			,p.Kappa
			,p.Omega
			,p.ProcessDirectory
			,cf.NCatG
			,PAML.GetNSSitesListForAnalysisConfiguration(cf.ID) AS NSSites
			,p.OutputData
			,p.ErrorData
			,CAST((CASE WHEN EXISTS (SELECT * FROM PAML.ProcessException pex WHERE pex.ProcessOutputID = p.ID) THEN 1 ELSE 0 END) AS bit)
				AS HasExceptions

		FROM PAML.ProcessOutput p
		JOIN PAML.AnalysisConfiguration cf ON cf.ID = p.AnalysisConfigurationID
		JOIN Job.[Status] s ON s.ID = p.StatusID
		JOIN PAML.Tree t ON t.ID = p.TreeID
		WHERE
			((@JobID IS NULL) OR (t.JobID = @JobID))
			AND
			((@TreeID IS NULL) OR (t.ID = @TreeID))
			AND
			((@AnalysisConfigurationID IS NULL) OR (cf.ID = @AnalysisConfigurationID))
		ORDER BY p.Kappa, p.Omega
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.ProcessException_Add')) BEGIN
	DROP PROCEDURE PAML.ProcessException_Add
END
GO
CREATE PROCEDURE PAML.ProcessException_Add
	@ExceptionID int,
	@ProcessOutputID int
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO PAML.ProcessException (ExceptionID, ProcessOutputID)
	VALUES (@ExceptionID, @ProcessOutputID)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PAML.ProcessException_List')) BEGIN
	DROP PROCEDURE PAML.ProcessException_List
END
GO
CREATE PROCEDURE PAML.ProcessException_List
	@ProcessOutputID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT jex.ID
			,jex.[Message]
			,jex.[Source]
			,jex.StackTrace
			,jex.ParentID
			,jex.ExceptionAt
			,jex.ExceptionType
		FROM PAML.ProcessException pex
		JOIN Job.Exception jex ON jex.ID = pex.ExceptionID
		WHERE pex.ProcessOutputID = @ProcessOutputID
		ORDER BY jex.ExceptionAt
END
GO

ALTER PROCEDURE Job.Exception_Add
	@ID int OUTPUT
	,@JobID uniqueidentifier
	,@RequestID int = NULL
	,@Message varchar(MAX)
	,@Source varchar(MAX) = NULL
	,@StackTrace varchar(MAX) = NULL
	,@ExceptionType varchar(250) = NULL
	,@ParentID int = NULL
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Job.Exception (JobID, RequestID, [Message], [Source], StackTrace, ParentID, ExceptionType)
	VALUES (@JobID, @RequestID, @Message, @Source, @StackTrace, @ParentID, @ExceptionType)

	SET @ID = @@IDENTITY
END
GO

GO
-- Add LongName property to Common.ThirdPartyComponentReference
BEGIN TRANSACTION
GO
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.name = 'LongName' AND c.object_id = OBJECT_ID('Common.ThirdPartyComponentReference')) BEGIN
	ALTER TABLE Common.ThirdPartyComponentReference DROP CONSTRAINT DF_Common_ThirdPartyComponentReference_Modified
	ALTER TABLE Common.ThirdPartyComponentReference DROP CONSTRAINT DF_Common_ThirdPartyComponentReference_Packaged

	CREATE TABLE Common.Tmp_ThirdPartyComponentReference (
		ID int NOT NULL IDENTITY (1, 1),
		Name varchar(200) NOT NULL,
		LongName varchar(1000) NULL,
		Version varchar(100) NOT NULL,
		Creator varchar(200) NOT NULL,
		ProductURL varchar(2048) NOT NULL,
		LastUpdatedAt datetime2(7) NOT NULL,
		LastRetrievedAt datetime2(7) NOT NULL,
		Copyright varchar(9) NULL,
		LicenseType varchar(100) NULL,
		LicenseURL varchar(2048) NULL,
		LicenseText nvarchar(MAX) NULL,
		Modified bit NOT NULL,
		Logo varchar(100) NULL,
		Packaged bit NOT NULL,
		Citation nvarchar(MAX) NULL
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	DECLARE @v sql_variant 
	SET @v = N'This is the date the component itself was last updated or published.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'Tmp_ThirdPartyComponentReference', N'COLUMN', N'LastUpdatedAt'

	SET @v = N'This is the date the component was downloaded by us.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'Tmp_ThirdPartyComponentReference', N'COLUMN', N'LastRetrievedAt'

	SET @v = N'This is free-text for now just to make things easier, but arguably could be converted to a list of things like GNU, Creative Commons, Apache, etc.  However, we''d then need to address the license''s verison number as well, potentially spawning a Common.ThirdPartyComponentLicense table.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'Tmp_ThirdPartyComponentReference', N'COLUMN', N'LicenseType'

	SET @v = N'Unfortunately not all products will have a specific license on them.  Some, like PAML, just have a copyright blurb, and that textgoes here.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'Tmp_ThirdPartyComponentReference', N'COLUMN', N'LicenseText'

	SET @v = N'.NET Bio was modified to implement a configurable timeout for NCBI web services.  This kind of modification is allowable under its license, but should be noted in our reference to it.  When this bit is set true, a message should appear in the UI stating something to the effect of: This component has been modified by Chang Lab within the constraints of the component''s license.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'Tmp_ThirdPartyComponentReference', N'COLUMN', N'Modified'

	SET @v = N'Indicates that the component is included in the Pilgrimage installation package, and does not need to be seaprately installed.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'Tmp_ThirdPartyComponentReference', N'COLUMN', N'Packaged'

	ALTER TABLE Common.Tmp_ThirdPartyComponentReference ADD CONSTRAINT DF_Common_ThirdPartyComponentReference_Modified DEFAULT ((0)) FOR Modified
	ALTER TABLE Common.Tmp_ThirdPartyComponentReference ADD CONSTRAINT DF_Common_ThirdPartyComponentReference_Packaged DEFAULT ((0)) FOR Packaged

	SET IDENTITY_INSERT Common.Tmp_ThirdPartyComponentReference ON
	EXEC('INSERT INTO Common.Tmp_ThirdPartyComponentReference (ID, Name, Version, Creator, ProductURL, LastUpdatedAt, LastRetrievedAt, Copyright, LicenseType, LicenseURL, LicenseText, Modified, Logo, Packaged, Citation)
			SELECT ID, Name, Version, Creator, ProductURL, LastUpdatedAt, LastRetrievedAt, Copyright, LicenseType, LicenseURL, LicenseText, Modified, Logo, Packaged, Citation FROM Common.ThirdPartyComponentReference WITH (HOLDLOCK TABLOCKX)')
	SET IDENTITY_INSERT Common.Tmp_ThirdPartyComponentReference OFF

	EXEC ('DROP TABLE Common.ThirdPartyComponentReference')
	EXECUTE sp_rename N'Common.Tmp_ThirdPartyComponentReference', N'ThirdPartyComponentReference', 'OBJECT' 

	ALTER TABLE Common.ThirdPartyComponentReference ADD CONSTRAINT PK_Common_ThirdPartyComponentReference PRIMARY KEY CLUSTERED (ID) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO
COMMIT
GO
IF NOT EXISTS (SELECT * FROM Common.ThirdPartyComponentReference WHERE Name = 'PAML') BEGIN
	UPDATE Common.ThirdPartyComponentReference
		SET LongName = 'Phylogenetic Analysis by Maximum Likelihood (PAML)'
			,Name = 'PAML'
		WHERE Name = 'Phylogenetic Analysis by Maximum Likelihood (PAML)'
END
GO
ALTER PROCEDURE [Common].[ThirdPartyComponentReference_List]
AS
BEGIN
	SET NOCOUNT ON

	SELECT r.ID
			,r.Name
			,r.LongName
			,r.[Version]
			,r.Creator
			,r.ProductURL
			,r.LastUpdatedAt
			,r.LastRetrievedAt 
			,r.Copyright
			,r.LicenseType
			,r.LicenseURL
			,r.LicenseText
			,r.Modified
			,r.Logo
			,r.Packaged
			,r.Citation
		FROM Common.ThirdPartyComponentReference r
		ORDER BY r.Name
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.5.0'
	WHERE [Key] = 'DatabaseVersion'
GO