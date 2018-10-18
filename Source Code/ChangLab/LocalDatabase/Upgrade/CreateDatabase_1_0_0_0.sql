IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'Gene') BEGIN
	EXEC ('CREATE SCHEMA [Gene]')
END
GO
IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'RecordSet') BEGIN
	EXEC ('CREATE SCHEMA [RecordSet]')
END
GO
IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'Common') BEGIN
	EXEC ('CREATE SCHEMA [Common]')
END
GO
IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'BlastN') BEGIN
	EXEC ('CREATE SCHEMA [BlastN]')
END
GO
IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'Job') BEGIN
	EXEC ('CREATE SCHEMA [Job]')
END
GO
IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'NCBI') BEGIN
	EXEC ('CREATE SCHEMA [NCBI]')
END
GO
IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'Taxonomy') BEGIN
	EXEC ('CREATE SCHEMA [Taxonomy]')
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Common.ExceptionLevel')) BEGIN
	CREATE TABLE Common.ExceptionLevel
	(
		ID int NOT NULL IDENTITY(1,1),
		Name varchar(20) NOT NULL,
		[Level] int NOT NULL,

		CONSTRAINT PK_Common_ExceptionLevel PRIMARY KEY (ID ASC)
	)

	INSERT INTO Common.ExceptionLevel (Name, [Level])
	VALUES ('Exception', 1), ('Warning', 11)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Common.Exception')) BEGIN
	CREATE TABLE Common.Exception
	(
		ID uniqueidentifier NOT NULL,
		[Message] varchar(MAX) NOT NULL,
		[Source] varchar(MAX) NOT NULL,
		StackTrace varchar(MAX) NOT NULL,
		ParentID uniqueidentifier NOT NULL,

		CONSTRAINT PK_Common_Exception PRIMARY KEY (ID ASC)
	)
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.Source')) BEGIN
	CREATE TABLE Gene.[Source]
	(
		ID int NOT NULL IDENTITY(1,1),
		Name varchar(20) NOT NULL,
		[Key] varchar(20) NOT NULL,

		CONSTRAINT PK_Gene_Source PRIMARY KEY (ID ASC)
	)

	CREATE UNIQUE INDEX IX_Gene_Source_Key ON Gene.[Source] ([Key] ASC)

	INSERT INTO Gene.[Source] (Name, [Key])
	VALUES ('FASTA', 'FASTA'), ('BLASTN (NCBI)', 'BLASTN_NCBI'), ('GenBank Search', 'GenBank'), ('Ensembl Search', 'Ensembl')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.SequenceType')) BEGIN
	CREATE TABLE Gene.SequenceType
	(
		ID int NOT NULL IDENTITY(0,1),
		Name varchar(20) NOT NULL,
		[Key] varchar(20) NOT NULL,

		CONSTRAINT PK_Gene_SequenceType PRIMARY KEY (ID ASC)
	)

	CREATE UNIQUE INDEX IX_Gene_SequenceType_Key ON Gene.SequenceType ([Key] ASC)

	INSERT INTO Gene.SequenceType (Name, [Key])
	VALUES ('Not Defined', 'NotDefined'), ('Source', 'Source'), ('Gene', 'Gene'), ('Coding Sequence', 'Coding'), ('Alignment', 'Alignment')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.Gene')) BEGIN
	CREATE TABLE Gene.Gene
	(
		ID uniqueidentifier NOT NULL,
		[Definition] varchar(1000) NOT NULL,
		SourceID int NOT NULL,

		/* GenBank specific fields
			We're putting these here because presumably most of our records will come from GenBank.  Some of the fields could be filled out from 
			searching Ensembl and local BlastN databases.
		*/
		GenBankID int NULL,
		Locus varchar(100) NULL,
		Accession varchar(20) NULL,
		Organism varchar(250) NULL,
		Taxonomy varchar(4000) NULL, -- Yup, I'm going to need to parse this.

		Nucleotides varchar(MAX) NULL,
		SequenceTypeID int NULL,

		SequenceStart int NULL,
		SequenceEnd int NULL,
		CodingSequenceStart int NULL,
		CodingSequenceEnd int NULL,

		LastUpdatedAt datetime2(7) NULL,
		Active bit NOT NULL CONSTRAINT DF_Gene_Gene_Active DEFAULT (1),

		CONSTRAINT PK_Gene_Gene PRIMARY KEY (ID ASC)
	)

	ALTER TABLE Gene.Gene ADD CONSTRAINT FK_Gene_Gene_SourceID FOREIGN KEY (SourceID) REFERENCES Gene.[Source] (ID)
	ALTER TABLE Gene.Gene ADD CONSTRAINT FK_Gene_Gene_SequenceTypeID FOREIGN KEY (SequenceTypeID) REFERENCES Gene.SequenceType (ID)
	CREATE UNIQUE INDEX IX_Gene_Gene_GenBankID ON Gene.Gene (GenBankID ASC)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.GeneHistory')) BEGIN
	CREATE TABLE Gene.GeneHistory
	(
		RevisionID int NOT NULL,
		ID uniqueidentifier NOT NULL,
		SourceID int NOT NULL,
		GenBankID int NULL,
		Locus varchar(100) NULL,
		[Definition] varchar(1000) NULL,
		Accession varchar(20) NULL,
		Organism varchar(250) NULL,
		Taxonomy varchar(4000) NULL,
		Nucleotides varchar(MAX) NULL,
		SequenceTypeID int NULL,
		SequenceStart int NULL,
		SequenceEnd int NULL,
		CodingSequenceStart int NULL,
		CodingSequenceEnd int NULL,
		LastUpdatedAt datetime2(7) NULL,
		Active bit NOT NULL

		CONSTRAINT PK_Gene_GeneHistory PRIMARY KEY (RevisionID ASC, ID ASC)
	)
	
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_ID FOREIGN KEY (ID) REFERENCES Gene.Gene (ID)
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_SourceID FOREIGN KEY (SourceID) REFERENCES Gene.[Source] (ID)
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_SequenceTypeID FOREIGN KEY (SequenceTypeID) REFERENCES Gene.SequenceType (ID)
END
GO
--IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.BlastN')) BEGIN
--	-- Additional data about a Gene, as retrieved from a BlastN output file.
--	CREATE TABLE Gene.BlastN
--	(
--		ID uniqueidentifier NOT NULL, -- Should be 1-1 with a row in Gene.Gene
--		ContigNumber varchar(100) NULL,
--		SourceFile varchar(250) NULL,

--		CONSTRAINT PK_Gene_BlastN PRIMARY KEY (ID ASC)
--	)
--END
--GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.ExonOrientation')) BEGIN
	CREATE TABLE Gene.ExonOrientation
	(
		ID int NOT NULL IDENTITY(1,1),
		Name varchar(10) NOT NULL,
		[Key] varchar(10) NOT NULL,

		CONSTRAINT PK_Gene_ExonOrientation PRIMARY KEY (ID ASC)
	)

	INSERT INTO Gene.ExonOrientation (Name, [Key])
	VALUES ('Plus/Plus', 'PlusPlus'), ('Plus/Minus', 'PlusMinus')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.Exception')) BEGIN
	CREATE TABLE Gene.Exception
	(
		GeneID uniqueidentifier NOT NULL,
		ExceptionID uniqueidentifier NOT NULL,
		ExceptionLevel int NOT NULL,

		CONSTRAINT PK_Gene_Exception PRIMARY KEY (GeneID ASC, ExceptionID ASC)
	)

	ALTER TABLE Gene.Exception ADD CONSTRAINT FK_Gene_Exception_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)
	ALTER TABLE Gene.Exception ADD CONSTRAINT FK_Gene_Exception_ExceptionID FOREIGN KEY (ExceptionID) REFERENCES Common.Exception (ID)
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.Alignment')) BEGIN
	CREATE TABLE BlastN.Alignment
	(
		ID uniqueidentifier NOT NULL,
		QueryID uniqueidentifier NOT NULL,
		SubjectID uniqueidentifier NOT NULL,
		[Rank] int NOT NULL,
		
		CONSTRAINT PK_BlastN_Alignment PRIMARY KEY (ID ASC)
	)

	ALTER TABLE BlastN.Alignment ADD CONSTRAINT FK_BlastN_Alignment_QueryID FOREIGN KEY (QueryID) REFERENCES Gene.Gene (ID)
	ALTER TABLE BlastN.Alignment ADD CONSTRAINT FK_BlastN_Alignment_SubjectID FOREIGN KEY (SubjectID) REFERENCES Gene.Gene (ID)
	
	CREATE UNIQUE INDEX IX_BlastN_Alignment ON BlastN.Alignment (QueryID ASC, SubjectID ASC)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.AlignmentExon')) BEGIN
	CREATE TABLE BlastN.AlignmentExon
	(
		ID uniqueidentifier NOT NULL,
		AlignmentID uniqueidentifier NOT NULL,
		OrientationID int NOT NULL,
		BitScore float NOT NULL,
		AlignmentLength int NOT NULL,
		IdentitiesCount int NOT NULL,
		Gaps int NOT NULL,
		QueryRangeStart int NOT NULL,
		QueryRangeEnd int NOT NULL,
		SubjectRangeStart int NOT NULL,
		SubjectRangeEnd int NOT NULL,
		
		CONSTRAINT PK_BlastN_AlignmentExon PRIMARY KEY (ID ASC)
	)

	ALTER TABLE BlastN.AlignmentExon ADD CONSTRAINT FK_BlastN_AlignmentExon_AlignmentID FOREIGN KEY (AlignmentID) REFERENCES BlastN.Alignment (ID)
	ALTER TABLE BlastN.AlignmentExon ADD CONSTRAINT FK_BlastN_AlignmentExon_OrientationID FOREIGN KEY (OrientationID) REFERENCES Gene.ExonOrientation (ID)
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('RecordSet.RecordSet')) BEGIN
	CREATE TABLE RecordSet.RecordSet
	(
		ID uniqueidentifier NOT NULL,
		Name varchar(200) NOT NULL,
		CreatedAt datetime2(7) CONSTRAINT DF_RecordSet_RecordSet_CreatedAt DEFAULT (sysdatetime()),
		LastOpenedAt datetime2(7) NULL,
		ModifiedAt datetime2(7) CONSTRAINT DF_RecordSet_RecordSet_ModifiedAt DEFAULT (sysdatetime()),
		Active bit NOT NULL CONSTRAINT DF_RecordSet_RecordSet_Active DEFAULT(1),

		CONSTRAINT PK_RecordSet_RecordSet PRIMARY KEY (ID ASC)
	)
	CREATE UNIQUE INDEX IX_RecordSet_RecordSet_Name ON RecordSet.RecordSet (Name ASC, Active ASC)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('RecordSet.GeneStatus')) BEGIN
	CREATE TABLE RecordSet.GeneStatus
	(
		ID int NOT NULL IDENTITY(1,1),
		Name varchar(20) NOT NULL,
		[Key] varchar(20) NOT NULL,

		CONSTRAINT PK_RecordSet_GeneStatus PRIMARY KEY (ID ASC)
	)

	CREATE UNIQUE INDEX IX_RecordSet_GeneStatus_Key ON RecordSet.GeneStatus ([Key] ASC)

	INSERT INTO RecordSet.GeneStatus (Name, [Key])
	VALUES ('Committed', 'Committed'), ('Pending', 'Pending')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('RecordSet.Gene')) BEGIN
	CREATE TABLE RecordSet.Gene
	(
		RecordSetID uniqueidentifier NOT NULL,
		GeneID uniqueidentifier NOT NULL,
		GeneStatusID int NOT NULL,
		ModifiedAt datetime2(7) NOT NULL CONSTRAINT DF_RecordSet_Gene_ModifiedAt DEFAULT (sysdatetime()),

		CONSTRAINT PK_RecordSet_Gene PRIMARY KEY (RecordSetID ASC, GeneID ASC, GeneStatusID ASC)
	)

	ALTER TABLE RecordSet.Gene ADD CONSTRAINT FK_RecordSet_Gene_RecordSetID FOREIGN KEY (RecordSetID) REFERENCES RecordSet.RecordSet (ID)
	ALTER TABLE RecordSet.Gene ADD CONSTRAINT FK_RecordSet_Gene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)
	ALTER TABLE RecordSet.Gene ADD CONSTRAINT FK_RecordSet_Gene_GeneStatusID FOREIGN KEY (GeneStatusID) REFERENCES RecordSet.GeneStatus (ID)
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.[Target]')) BEGIN
	CREATE TABLE Job.[Target]
	(
		ID int NOT NULL IDENTITY(1,1),
		Name varchar(20) NOT NULL,
		[Key] varchar(20) NOT NULL,

		CONSTRAINT PK_Job_Target PRIMARY KEY (ID ASC)
	)

	INSERT INTO Job.[Target] (Name, [Key])
	VALUES ('BLASTN (NCBI)', 'BLASTN_NCBI'), ('BLASTN (Local)', 'BLASTN_Local'), ('Ensembl', 'Ensembl')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.[Status]')) BEGIN
	CREATE TABLE Job.[Status]
	(
		ID int NOT NULL IDENTITY(1,1),
		Name varchar(20) NOT NULL,
		[Key] varchar(20) NOT NULL,

		CONSTRAINT PK_Job_Status PRIMARY KEY (ID ASC)
	)

	INSERT INTO Job.[Status] (Name, [Key])
	VALUES ('New', 'New'), ('Running', 'Running'), ('Completed', 'Completed'), ('Cancelled', 'Cancelled'), ('Reviewed', 'Reviewed'), ('Archived', 'Archived')
END
GO
--SELECT * FROM Job.[Status]
--DBCC CHECKIDENT ('Job.Status', RESEED, 3)
--DELETE FROM Job.[Status] WHERE ID > 3
--INSERT INTO Job.[Status] (Name, [Key])
--	VALUES ('Cancelled', 'Cancelled'), ('Reviewed', 'Reviewed'), ('Archived', 'Archived')
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.Job')) BEGIN
	CREATE TABLE Job.Job
	(
		ID uniqueidentifier NOT NULL,
		RecordSetID uniqueidentifier NULL,
		TargetID int NOT NULL,
		StartedAt datetime2(7) NULL, -- No default on this; long-term, a user might want to set up a job and then run it later, in which case it hasn't started yet.
		EndedAt datetime2(7) NULL,
		StatusID int NOT NULL CONSTRAINT DF_Job_Job_StatusID DEFAULT (1),

		CONSTRAINT PK_Job_Job PRIMARY KEY (ID ASC)
	)

	ALTER TABLE Job.Job ADD CONSTRAINT FK_Job_Job_TargetID FOREIGN KEY (TargetID) REFERENCES Job.[Target] (ID)
	ALTER TABLE Job.Job ADD CONSTRAINT FK_Job_Job_StatusID FOREIGN KEY (StatusID) REFERENCES Job.[Status] (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.GeneDirection')) BEGIN
	CREATE TABLE Job.GeneDirection
	(
		ID int NOT NULL IDENTITY(1,1),
		Name varchar(20) NOT NULL,
		[Key] varchar(20) NOT NULL,

		CONSTRAINT PK_Job_GeneDirection PRIMARY KEY (ID ASC)
	)

	INSERT INTO Job.GeneDirection (Name, [Key])
	VALUES ('Input', 'Input'), ('Output', 'Output')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.Gene')) BEGIN
	CREATE TABLE Job.Gene
	(
		JobID uniqueidentifier NOT NULL,
		GeneID uniqueidentifier NOT NULL,
		DirectionID int NOT NULL,

		CONSTRAINT PK_Job_Gene PRIMARY KEY (JobID ASC, GeneID ASC, DirectionID ASC)
	)

	ALTER TABLE Job.Gene ADD CONSTRAINT FK_Job_Gene_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
	ALTER TABLE Job.Gene ADD CONSTRAINT FK_Job_Gene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)
	ALTER TABLE Job.Gene ADD CONSTRAINT FK_Job_Gene_DirectionID FOREIGN KEY (DirectionID) REFERENCES Job.GeneDirection (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.InputFileFormat')) BEGIN
	CREATE TABLE Job.InputFileFormat
	(
		ID int NOT NULL IDENTITY(1,1),
		Name varchar(20) NOT NULL,

		CONSTRAINT PK_Job_InputFileFormat PRIMARY KEY (ID ASC)
	)

	INSERT INTO Job.InputFileFormat (Name)
	VALUES ('FASTA'), ('BLASTN'), ('GenBank')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.InputFile')) BEGIN
	CREATE TABLE Job.InputFile
	(
		ID uniqueidentifier NOT NULL,
		JobID uniqueidentifier NOT NULL,
		Name varchar(250) NOT NULL,
		FileFormatID int NOT NULL,
		LastModifiedAt datetime2(7) NOT NULL,

		CONSTRAINT PK_Job_InputFile PRIMARY KEY (ID ASC)
	)

	ALTER TABLE Job.InputFile ADD CONSTRAINT FK_Job_InputFile_FileFormatID FOREIGN KEY (FileFormatID) REFERENCES Job.InputFileFormat (ID)
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('NCBI.Request')) BEGIN
	CREATE TABLE NCBI.Request
	(
		ID uniqueidentifier NOT NULL, -- I'm not 100% certain whether NCBI recycles RequestIDs
		RequestID varchar(20) NOT NULL,
		JobID uniqueidentifier NOT NULL,
		StartTime datetime2(7) NOT NULL CONSTRAINT DF_NCBI_Request_StartTime DEFAULT (sysdatetime()),
		EndTime datetime2(7) NULL,
		LastStatus varchar(8) NULL,
		LastUpdatedAt datetime2(7) NOT NULL CONSTRAINT DF_NCBI_Request_LastUpdatedAt DEFAULT (sysdatetime()),
		StatusInformation varchar(MAX) NULL,

		CONSTRAINT PK_NCBI_Request PRIMARY KEY (ID ASC)
	)

	ALTER TABLE NCBI.Request ADD CONSTRAINT FK_NCBI_Request_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('NCBI.Gene')) BEGIN
	CREATE TABLE NCBI.Gene
	(
		RequestID uniqueidentifier NOT NULL,
		GeneID uniqueidentifier NOT NULL,
		DirectionID int NOT NULL,

		CONSTRAINT PK_NCBI_Gene PRIMARY KEY (RequestID ASC, GeneID ASC, DirectionID ASC)
	)

	ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
	ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)
	ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_DirectionID FOREIGN KEY (DirectionID) REFERENCES Job.GeneDirection (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('NCBI.BlastNAlignment')) BEGIN
	CREATE TABLE NCBI.BlastNAlignment
	(
		RequestID uniqueidentifier NOT NULL,
		AlignmentID uniqueidentifier NOT NULL,

		CONSTRAINT PK_NCBI_BlastNAlignment PRIMARY KEY (RequestID ASC, AlignmentID ASC)
	)

	ALTER TABLE NCBI.BlastNAlignment ADD CONSTRAINT FK_NCBI_BlastNAlignment_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
	ALTER TABLE NCBI.BlastNAlignment ADD CONSTRAINT FK_NCBI_BlastNAlignment_AlignmentID FOREIGN KEY (AlignmentID) REFERENCES BlastN.Alignment (ID)
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Common.ApplicationProperty')) BEGIN
	CREATE TABLE Common.ApplicationProperty
	(
		ID uniqueidentifier NOT NULL,
		[Key] varchar(30) NOT NULL,
		Value varchar(MAX) NULL,

		CONSTRAINT PK_Common_ApplicationProperty PRIMARY KEY (ID ASC)
	)

	INSERT INTO Common.ApplicationProperty (ID, [Key], Value)
	VALUES (newid(), 'FASTAFileNameFormatString', '{Definition} {Coding Sequence Range Start}-{Coding Sequence Range End}'),
			(newid(), 'FASTAHeaderFormatString', '{Definition} {Coding Sequence Range Start}-{Coding Sequence Range End}'),
			(newid(), 'DatabaseVersion', '1.0.0.0')

	CREATE UNIQUE INDEX IX_Common_ApplicationProperty_Key ON Common.ApplicationProperty ([Key] ASC)
END
GO

IF EXISTS (SELECT * FROM sys.triggers t WHERE t.object_id = OBJECT_ID('Gene.Gene_LogHistory')) BEGIN
	DROP TRIGGER Gene.Gene_LogHistory
END
GO
CREATE TRIGGER Gene.Gene_LogHistory ON Gene.Gene
AFTER INSERT, UPDATE
AS
BEGIN

	INSERT INTO Gene.GeneHistory (RevisionID, ID, SourceID,
									GenBankID, Locus, [Definition], Accession, Organism, Taxonomy,
									Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd,
									LastUpdatedAt, Active)
		SELECT ISNULL((SELECT MAX(RevisionID) FROM Gene.GeneHistory g WHERE g.ID = i.ID), 0) + 1
				,i.ID
				,i.SourceID
				,i.GenBankID
				,i.Locus
				,i.[Definition]
				,i.Accession
				,i.Organism
				,i.Taxonomy
				,i.Nucleotides
				,i.SequenceTypeID
				,i.SequenceStart
				,i.SequenceEnd
				,i.CodingSequenceStart
				,i.CodingSequenceEnd
				,i.LastUpdatedAt
				,i.Active
			FROM inserted i

END
GO

IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.CalculatePercentageFromInt')) BEGIN
	DROP FUNCTION Common.CalculatePercentageFromInt
END
GO
CREATE FUNCTION Common.CalculatePercentageFromInt(@Numerator int, @Denominator int)
RETURNS float
AS
BEGIN
	RETURN (SELECT (CAST(@Numerator AS float) / CAST(@Denominator AS float) * CAST(100 AS float)))
END
GO

IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Gene.Gene_CodingSequenceForSequence')) BEGIN
	DROP FUNCTION Gene.Gene_CodingSequenceForSequence
END
GO
CREATE FUNCTION Gene.Gene_CodingSequenceForSequence(@ID uniqueidentifier)
RETURNS varchar(MAX)
AS
BEGIN

	DECLARE @SequenceStart int
			,@CDSStart int
			,@CDSEnd int
			,@Nucleotides varchar(MAX)

	SELECT @SequenceStart = g.SequenceStart
			,@CDSStart = g.CodingSequenceStart
			,@CDSEnd = g.CodingSequenceEnd
			,@Nucleotides = g.Nucleotides
		FROM Gene.Gene g
		WHERE g.ID = @ID

	IF (@CDSStart IS NULL) OR (@CDSStart = -1)
		OR (@CDSEnd IS NULL) OR (@CDSEnd = -1)
		OR (@CDSStart >= @CDSEnd) BEGIN
		RETURN @Nucleotides
	END
	ELSE BEGIN
		RETURN SUBSTRING(@Nucleotides, (@CDSStart - @SequenceStart) + 1, (@CDSEnd - @CDSStart) + 1)
	END

	RETURN @Nucleotides
END
GO

IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('BlastN.AlignmentExon_First')) BEGIN
	DROP FUNCTION BlastN.AlignmentExon_First
END
GO
CREATE FUNCTION BlastN.AlignmentExon_First(@AlignmentID uniqueidentifier)
RETURNS uniqueidentifier
AS
BEGIN	
	RETURN (SELECT TOP 1 ID
				FROM BlastN.AlignmentExon ex
				WHERE ex.AlignmentID = @AlignmentID
				ORDER BY ex.BitScore DESC, Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength) DESC)
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('BlastN.Alignment_MaxAlignmentPercentage')) BEGIN
	DROP FUNCTION BlastN.Alignment_MaxAlignmentPercentage
END
GO
CREATE FUNCTION BlastN.Alignment_MaxAlignmentPercentage(@AlignmentID uniqueidentifier)
RETURNS int
AS
BEGIN
	RETURN (SELECT MAX(Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength))
				FROM BlastN.AlignmentExon ex
				WHERE ex.AlignmentID = @AlignmentID)
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Common.ApplicationProperty_List')) BEGIN
	DROP PROCEDURE Common.ApplicationProperty_List
END
GO
CREATE PROCEDURE Common.ApplicationProperty_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT ap.ID
			,ap.[Key]
			,ap.Value
		FROM Common.ApplicationProperty ap
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Common.ApplicationProperty_Edit')) BEGIN
	DROP PROCEDURE Common.ApplicationProperty_Edit
END
GO
CREATE PROCEDURE Common.ApplicationProperty_Edit
	@Key varchar(30)
	,@Value varchar(MAX)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE Common.ApplicationProperty
		SET Value = @Value
		WHERE [Key] = @Key
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Source_List')) BEGIN
	DROP PROCEDURE Gene.Source_List
END
GO
CREATE PROCEDURE Gene.Source_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT s.ID
			,s.Name
			,s.[Key]
		FROM Gene.[Source] s
 
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.SequenceType_List')) BEGIN
	DROP PROCEDURE Gene.SequenceType_List
END
GO
CREATE PROCEDURE Gene.SequenceType_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT s.ID
			,s.Name
			,s.[Key]
		FROM Gene.SequenceType s
 
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.ExonOrientation_List')) BEGIN
	DROP PROCEDURE Gene.ExonOrientation_List
END
GO
CREATE PROCEDURE Gene.ExonOrientation_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.ID
			,g.Name
			,g.[Key]
		FROM Gene.[ExonOrientation] g
 
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_Edit')) BEGIN
	DROP PROCEDURE Gene.Gene_Edit
END
GO
CREATE PROCEDURE Gene.Gene_Edit
	@ID uniqueidentifier = NULL OUTPUT,
	@Definition varchar(1000),
	@SourceID int,
	@GenBankID int,
	@Locus varchar(100),
	@Accession varchar(20),
	@Organism varchar(250),
	@Taxonomy varchar(4000),
	@Nucleotides varchar(MAX),
	@SequenceTypeID int,
	@SequenceStart int,
	@SequenceEnd int,
	@CodingSequenceStart int = NULL,
	@CodingSequenceEnd int = NULL,
	@LastUpdatedAt datetime2(7) = NULL,
	@AllowOverwrite bit = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @NowTime datetime2(7) = SYSDATETIME()
			,@Overwrite bit = 1

	IF @GenBankID <> 0 
		AND EXISTS (SELECT * FROM Gene.Gene g WHERE g.GenBankID = @GenBankID)
		AND NOT EXISTS (SELECT * FROM Gene.Gene g WHERE g.ID = @ID)
	BEGIN
		SELECT @ID = g.ID FROM Gene.Gene g WHERE g.GenBankID = @GenBankID

		IF @AllowOverwrite = 0 BEGIN
			SET @Overwrite = 0
		END
	END

	IF NOT EXISTS (SELECT * FROM Gene.Gene g WHERE g.ID = @ID) BEGIN
		IF (@ID IS NULL) BEGIN
			SET @ID = NEWID()
		END

		INSERT INTO Gene.Gene (ID, [Definition], SourceID
								,GenBankID, Locus, Accession, Organism, Taxonomy
								,Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd
								,LastUpdatedAt)
		VALUES (@ID, @Definition, @SourceID
				,@GenBankID, @Locus, @Accession, @Organism, @Taxonomy
				,@Nucleotides, @SequenceTypeID, @SequenceStart, @SequenceEnd, @CodingSequenceStart, @CodingSequenceEnd
				,@LastUpdatedAt)
	END
	ELSE BEGIN
		IF (@Overwrite = 1) BEGIN
			UPDATE Gene.Gene
				SET SourceID = ISNULL(@SourceID, SourceID)
					,[Definition] = ISNULL(@Definition, [Definition])
					,Locus = ISNULL(@Locus, Locus)
					,Accession = ISNULL(@Accession, Accession)
					,Organism = ISNULL(@Organism, Organism)
					,Taxonomy = ISNULL(@Taxonomy, Taxonomy)
					,Nucleotides = ISNULL(@Nucleotides, Nucleotides)
					,SequenceTypeID = ISNULL(@SequenceTypeID, SequenceTypeID)
					,SequenceStart = ISNULL(@SequenceStart, SequenceStart)
					,SequenceEnd = ISNULL(@SequenceEnd, SequenceEnd)
					,CodingSequenceStart = ISNULL(@CodingSequenceStart, CodingSequenceStart)
					,CodingSequenceEnd = ISNULL(@CodingSequenceEnd, CodingSequenceEnd)
					,LastUpdatedAt = ISNULL(@LastUpdatedAt, LastUpdatedAt)
				WHERE ID = @ID
		END
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_EditSimple')) BEGIN
	DROP PROCEDURE Gene.Gene_EditSimple
END
GO
CREATE PROCEDURE Gene.Gene_EditSimple
	@ID uniqueidentifier = NULL OUTPUT,
	@Definition varchar(1000),
	@SourceID int,
	@Nucleotides varchar(MAX),
	@SequenceStart int,
	@SequenceEnd int,
	@CodingSequenceStart int = NULL,
	@CodingSequenceEnd int = NULL
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @NowTime datetime2(7) = SYSDATETIME();

	IF (@ID IS NULL) BEGIN
		SET @ID = NEWID()
	END

	IF NOT EXISTS (SELECT * FROM Gene.Gene WHERE ID = @ID) BEGIN
		INSERT INTO Gene.Gene (ID, [Definition], SourceID, Nucleotides, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd)
		VALUES (@ID, @Definition, @SourceID, @Nucleotides, @SequenceStart, @SequenceEnd, @CodingSequenceStart, @CodingSequenceEnd)
	END
	ELSE BEGIN
		UPDATE Gene.Gene
			SET [Definition] = @Definition
				,SourceID = @SourceID
				,Nucleotides = @Nucleotides
				,SequenceStart = @SequenceStart
				,SequenceEnd = @SequenceEnd
				,CodingSequenceStart = @CodingSequenceStart
				,CodingSequenceEnd = @CodingSequenceEnd
				,LastUpdatedAt = SYSDATETIME()
			WHERE ID = @ID
	END
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.RecordSet_Edit')) BEGIN
	DROP PROCEDURE RecordSet.RecordSet_Edit
END
GO
CREATE PROCEDURE RecordSet.RecordSet_Edit
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
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.RecordSet_Opened')) BEGIN
	DROP PROCEDURE RecordSet.RecordSet_Opened
END
GO
CREATE PROCEDURE RecordSet.RecordSet_Opened
	@ID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON
	UPDATE RecordSet.RecordSet
		SET LastOpenedAt = SYSDATETIME()
		WHERE ID = @ID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.RecordSet_List')) BEGIN
	DROP PROCEDURE RecordSet.RecordSet_List
END
GO
CREATE PROCEDURE RecordSet.RecordSet_List
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
			,(SELECT COUNT(*)
				FROM RecordSet.Gene g
				WHERE g.RecordSetID = rs.ID
					AND g.GeneStatusID = 1) AS GeneCountFinal
		FROM RecordSet.RecordSet rs
		WHERE (@Active IS NULL OR rs.Active = @Active)
		ORDER BY rs.LastOpenedAt DESC, rs.ModifiedAt DESC
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.GeneStatus_List')) BEGIN
	DROP PROCEDURE RecordSet.GeneStatus_List
END
GO
CREATE PROCEDURE RecordSet.GeneStatus_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT *
		FROM RecordSet.GeneStatus
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Gene_Edit')) BEGIN
	DROP PROCEDURE RecordSet.Gene_Edit
END
GO
CREATE PROCEDURE RecordSet.Gene_Edit
	@RecordSetID uniqueidentifier,
	@GeneID uniqueidentifier,
	@GeneStatusID int
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM RecordSet.Gene WHERE RecordSetID = @RecordSetID AND GeneID = @GeneID) BEGIN
		INSERT INTO RecordSet.Gene (RecordSetID, GeneID, GeneStatusID)
		VALUES (@RecordSetID, @GeneID, @GeneStatusID)
	END
	ELSE BEGIN
		UPDATE RecordSet.Gene
			SET GeneStatusID = @GeneStatusID
				,ModifiedAt = SYSDATETIME()
			WHERE RecordSetID = @RecordSetID
				AND GeneID = @GeneID
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Gene_Delete')) BEGIN
	DROP PROCEDURE RecordSet.Gene_Delete
END
GO
CREATE PROCEDURE RecordSet.Gene_Delete
	@RecordSetID uniqueidentifier,
	@GeneID uniqueidentifier,
	@GeneStatusID int
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM RecordSet.Gene
		WHERE RecordSetID = @RecordSetID
			AND GeneID = @GeneID
			AND GeneStatusID = @GeneStatusID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Gene_List')) BEGIN
	DROP PROCEDURE RecordSet.Gene_List
END
GO
CREATE PROCEDURE RecordSet.Gene_List
	@RecordSetID uniqueidentifier,
	@GeneStatusID int = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.ID
			,g.[Definition]
			,g.SourceID
			,g.LastUpdatedAt
			,rs_g.GeneStatusID
			,rs_g.ModifiedAt

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

		FROM RecordSet.Gene rs_g
		JOIN RecordSet.GeneStatus rs_gs ON rs_gs.ID = rs_g.GeneStatusID
		JOIN Gene.Gene g ON g.ID = rs_g.GeneID
		
		WHERE rs_g.RecordSetID = @RecordSetID
			AND ((@GeneStatusID IS NULL) OR (rs_g.GeneStatusID = @GeneStatusID))
			AND g.Active = 1
		ORDER BY rs_g.ModifiedAt DESC, g.Organism, g.Accession, g.GenBankID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Gene_FetchSequence')) BEGIN
	DROP PROCEDURE RecordSet.Gene_FetchSequence
END
GO
CREATE PROCEDURE RecordSet.Gene_FetchSequence
	@ID uniqueidentifier,
	@CDS bit = 1,
	@Sequence varchar(MAX) = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF @CDS = 1 BEGIN
		SELECT @Sequence = Gene.Gene_CodingSequenceForSequence(@ID)
	END
	ELSE BEGIN
		SELECT @Sequence = g.Nucleotides
			FROM Gene.Gene g
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.GeneStatus_List')) BEGIN
	DROP PROCEDURE RecordSet.GeneStatus_List
END
GO
CREATE PROCEDURE RecordSet.GeneStatus_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT s.ID
			,s.Name
			,s.[Key]
		FROM RecordSet.GeneStatus s
 
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Target_List')) BEGIN
	DROP PROCEDURE Job.Target_List
END
GO
CREATE PROCEDURE Job.Target_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT t.ID
			,t.Name
			,t.[Key]
		FROM Job.[Target] t
 
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Status_List')) BEGIN
	DROP PROCEDURE Job.Status_List
END
GO
CREATE PROCEDURE Job.Status_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT t.ID
			,t.Name
			,t.[Key]
		FROM Job.[Status] t
 
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.GeneDirection_List')) BEGIN
	DROP PROCEDURE Job.GeneDirection_List
END
GO
CREATE PROCEDURE Job.GeneDirection_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT d.ID
			,d.Name
			,d.[Key]
		FROM Job.GeneDirection d
 
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Job_Edit')) BEGIN
	DROP PROCEDURE Job.Job_Edit
END
GO
CREATE PROCEDURE Job.Job_Edit
	@ID uniqueidentifier = NULL OUTPUT,
	@RecordSetID uniqueidentifier = NULL, -- Not used in an UPDATE
	@TargetID int = NULL, -- Not used in an UPDATE
	@StatusID int = NULL,
	@StartedAt datetime2(7) = NULL, -- Not used in an UPDATE
	@EndedAt datetime2(7) = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM Job.Job j WHERE j.ID = @ID) BEGIN
		SET @ID = NEWID()

		INSERT INTO Job.Job (ID, RecordSetID, TargetID, StartedAt, EndedAt)
		VALUES (@ID, @RecordSetID, @TargetID, @StartedAt, @EndedAt)
	END
	ELSE BEGIN
		UPDATE Job.Job
			SET StatusID = @StatusID
				,EndedAt = @EndedAt
			WHERE ID = @ID
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Job_UpdateStatus')) BEGIN
	DROP PROCEDURE Job.Job_UpdateStatus
END
GO
CREATE PROCEDURE Job.Job_UpdateStatus
	@ID uniqueidentifier,
	@StatusID int
AS
BEGIN
	SET NOCOUNT ON

	UPDATE Job.Job
		SET StatusID = @StatusID
		WHERE ID = @ID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Gene_Edit')) BEGIN
	DROP PROCEDURE Job.Gene_Edit
END
GO
CREATE PROCEDURE Job.Gene_Edit
	@JobID uniqueidentifier,
	@GeneID uniqueidentifier,
	@DirectionID int
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM Job.Gene ex
					WHERE ex.JobID = @JobID
						AND ex.GeneID = @GeneID
						AND ex.DirectionID = @DirectionID) BEGIN
		INSERT INTO Job.Gene (JobID, GeneID, DirectionID)
		VALUES (@JobID, @GeneID, @DirectionID)
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Job_List')) BEGIN
	DROP PROCEDURE Job.Job_List
END
GO
CREATE PROCEDURE Job.Job_List
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
		FROM Job.Job j
		JOIN Job.[Target] jt ON jt.ID = j.TargetID
		JOIN Job.[Status] js ON js.ID = j.StatusID
		WHERE j.RecordSetID = @RecordSetID
			AND j.TargetID = @TargetID
			AND ((@StatusID IS NULL) OR (j.StatusID = @StatusID))
		ORDER BY j.StartedAt DESC
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Gene_List')) BEGIN
	DROP PROCEDURE Job.Gene_List
END
GO
CREATE PROCEDURE Job.Gene_List
	@JobID uniqueidentifier,
	@GeneDirectionID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.*
		FROM Job.Gene jg
		JOIN Gene.Gene g ON g.ID = jg.GeneID
		WHERE jg.JobID = @JobID
			AND jg.DirectionID = @GeneDirectionID
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('BlastN.Alignment_Edit')) BEGIN
	DROP PROCEDURE BlastN.Alignment_Edit
END
GO
CREATE PROCEDURE BlastN.Alignment_Edit
	@ID uniqueidentifier = NULL OUTPUT,
	@QueryID uniqueidentifier,
	@SubjectID uniqueidentifier,
	@Rank int
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
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('BlastN.AlignmentExon_Edit')) BEGIN
	DROP PROCEDURE BlastN.AlignmentExon_Edit
END
GO
CREATE PROCEDURE BlastN.AlignmentExon_Edit
	@ID uniqueidentifier = NULL OUTPUT,
	@AlignmentID uniqueidentifier,
	@OrientationID int,
	@BitScore float,
	@AlignmentLength int,
	@IdentitiesCount int,
	@Gaps int,
	@QueryRangeStart int,
	@QueryRangeEnd int,
	@SubjectRangeStart int,
	@SubjectRangeEnd int
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM BlastN.AlignmentExon ae WHERE ae.ID = @ID) BEGIN
		SET @ID = NEWID()

		INSERT INTO BlastN.AlignmentExon (ID, AlignmentID, OrientationID, 
											BitScore, AlignmentLength, IdentitiesCount, Gaps, 
											QueryRangeStart, QueryRangeEnd, SubjectRangeStart, SubjectRangeEnd)
		VALUES (@ID, @AlignmentID, @OrientationID,
				@BitScore, @AlignmentLength, @IdentitiesCount, @Gaps,
				@QueryRangeStart, @QueryRangeEnd, @SubjectRangeStart, @SubjectRangeEnd)
	END
	-- I'm not sure that there's a use case for an UPDATE.
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('BlastN.AlignmentExon_ListForGenes')) BEGIN
	DROP PROCEDURE BlastN.AlignmentExon_ListForGenes
END
GO
CREATE PROCEDURE BlastN.AlignmentExon_ListForGenes
	@QueryID uniqueidentifier,
	@SubjectID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT ex.AlignmentID
			,ex.OrientationID
			,eor.Name AS Orientation
			,ex.BitScore
			,ex.AlignmentLength
			,ex.IdentitiesCount
			,ex.Gaps
			,ex.QueryRangeStart
			,ex.QueryRangeEnd
			,ex.SubjectRangeStart
			,ex.SubjectRangeEnd
			,qry.Nucleotides AS QueryNucleotides
			,qry.SequenceTypeID AS QuerySequenceTypeID
			,qry.SequenceStart AS QuerySequenceStart
			,qry.SequenceEnd AS QuerySequenceEnd
			,sbj.Nucleotides AS SubjectNucleotides
			,sbj.SequenceTypeID AS SubjectSequenceTypeID
			,sbj.SequenceStart AS SubjectSequenceStart
			,sbj.SequenceEnd AS SubjectSequenceEnd
		FROM BlastN.Alignment a
		JOIN BlastN.AlignmentExon ex ON ex.AlignmentID = a.ID
		JOIN Gene.ExonOrientation eor ON eor.ID = ex.OrientationID
		JOIN Gene.Gene qry ON qry.ID = a.QueryID
		JOIN Gene.Gene sbj ON sbj.ID = a.SubjectID
		WHERE a.QueryID = @QueryID
			AND a.SubjectID = @SubjectID
		ORDER BY Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength) DESC, ex.BitScore DESC
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('NCBI.Request_Edit')) BEGIN
	DROP PROCEDURE NCBI.Request_Edit
END
GO
CREATE PROCEDURE NCBI.Request_Edit
	@ID uniqueidentifier = NULL OUTPUT,
	@RequestID varchar(20),
	@JobID uniqueidentifier = NULL, -- One-off requests might not be associated with a job?  That might never happen.
	@StartTime datetime2(7) = NULL,
	@EndTime datetime2(7) = NULL,
	@LastStatus varchar(8) = NULL,
	@StatusInformation varchar(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) OR (NOT EXISTS (SELECT * FROM NCBI.Request r WHERE r.ID = @ID)) BEGIN
		SET @ID = NEWID()
		IF (@StartTime IS NULL) BEGIN
			SET @StartTime = SYSDATETIME()
		END

		INSERT INTO NCBI.Request (ID, RequestID, JobID, StartTime, EndTime, LastStatus, StatusInformation)
		VALUES (@ID, @RequestID, @JobID, @StartTime, @EndTime, @LastStatus, @StatusInformation)
	END
	ELSE BEGIN
		UPDATE NCBI.Request
			SET StartTime = @StartTime,
				EndTime = @EndTime,
				LastStatus = @LastStatus,
				LastUpdatedAt = SYSDATETIME(),
				StatusInformation = @StatusInformation
			WHERE ID = @ID
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('NCBI.Gene_Edit')) BEGIN
	DROP PROCEDURE NCBI.Gene_Edit
END
GO
CREATE PROCEDURE NCBI.Gene_Edit
	@RequestID uniqueidentifier,
	@GeneID uniqueidentifier,
	@DirectionID int
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM NCBI.Gene ex
					WHERE ex.RequestID = @RequestID
						AND ex.GeneID = @GeneID
						AND ex.DirectionID = @DirectionID) BEGIN
		INSERT INTO NCBI.Gene (RequestID, GeneID, DirectionID)
		VALUES (@RequestID, @GeneID, @DirectionID)
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('NCBI.BlastNAlignment_Edit')) BEGIN
	DROP PROCEDURE NCBI.BlastNAlignment_Edit
END
GO
CREATE PROCEDURE NCBI.BlastNAlignment_Edit
	@RequestID uniqueidentifier,
	@AlignmentID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM NCBI.BlastNAlignment ex WHERE ex.RequestID = @RequestID AND ex.AlignmentID = @AlignmentID) BEGIN
		INSERT INTO NCBI.BlastNAlignment (RequestID, AlignmentID)
		VALUES (@RequestID, @AlignmentID)
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE P.object_id = OBJECT_ID('Job.BlastN_ListAlignmentsForJob')) BEGIN
	DROP PROCEDURE Job.BlastN_ListAlignmentsForJob
END
GO
CREATE PROCEDURE Job.BlastN_ListAlignmentsForJob
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT sbj.ID
			,sbj.SourceID
			,sbj.GenBankID
			,sbj.[Definition]
			,sbj.SequenceStart
			,sbj.SequenceEnd
			,MAX(al.[Rank]) AS [Rank]
			,MAX(BlastN.Alignment_MaxAlignmentPercentage(al.ID)) AS AlignmentPercentage
		FROM NCBI.Request r
		JOIN NCBI.BlastNAlignment nal ON nal.RequestID = r.ID
		JOIN BlastN.Alignment al ON al.ID = nal.AlignmentID
		JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
		WHERE r.JobID = @JobID
		GROUP BY sbj.ID, sbj.SourceID, sbj.GenBankID, sbj.[Definition], sbj.SequenceStart, sbj.SequenceEnd
		ORDER BY AlignmentPercentage DESC, [Rank]
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE P.object_id = OBJECT_ID('Job.BlastN_ListQueryGenesForAlignment')) BEGIN
	DROP PROCEDURE Job.BlastN_ListQueryGenesForAlignment
END
GO
CREATE PROCEDURE Job.BlastN_ListQueryGenesForAlignment
	@SubjectGeneID uniqueidentifier,
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON
		
	SELECT qry.ID
			,qsrc.Name AS QuerySourceName
			,qry.GenBankID
			,qry.[Definition]
			,qry.Organism
			,qry.Taxonomy
			,al.[Rank]
			,ex.AlignmentLength
			,ex.BitScore
			,CAST(Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength) AS int) AS AlignmentPercentage
			,ex.Gaps
		FROM NCBI.Request r
		JOIN NCBI.BlastNAlignment nal ON nal.RequestID = r.ID
		JOIN BlastN.Alignment al ON al.ID = nal.AlignmentID
		JOIN BlastN.AlignmentExon ex ON ex.ID = BlastN.AlignmentExon_First(al.ID)
		JOIN Gene.Gene qry ON qry.ID = al.QueryID
		JOIN Gene.[Source] qsrc ON qsrc.ID = qry.SourceID
		WHERE r.JobID = @JobID
			AND al.SubjectID = @SubjectGeneID
		ORDER BY AlignmentPercentage DESC, BitScore DESC
END
GO

-- Set database version
UPDATE Common.ApplicationProperty
	SET Value = '1.0.0.0'
	WHERE [Key] = 'DatabaseVersion'