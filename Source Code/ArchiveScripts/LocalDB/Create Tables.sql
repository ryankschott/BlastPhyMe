USE [LocalDB]
GO
/*
IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'Component') BEGIN
	EXEC ('CREATE SCHEMA [Component]')
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
IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'GenBank') BEGIN
	EXEC ('CREATE SCHEMA [GenBank]')
END
GO
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.JobSequence')) BEGIN
	DROP TABLE BlastN.JobSequence
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.Job')) BEGIN
	DROP TABLE BlastN.Job
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.MatchExon')) BEGIN
	DROP TABLE BlastN.MatchExon
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.Exon')) BEGIN
	DROP TABLE BlastN.Exon
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.ExonOrientation')) BEGIN
	DROP TABLE BlastN.ExonOrientation
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.Match')) BEGIN
	DROP TABLE BlastN.Match
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Component.SequenceException')) BEGIN
	DROP TABLE Component.SequenceException
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Common.ExceptionLevel')) BEGIN
	DROP TABLE Common.ExceptionLevel
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Component.SequenceAlignment')) BEGIN
	DROP TABLE Component.SequenceAlignment
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Component.Sequence')) BEGIN
	DROP TABLE Component.[Sequence]
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Component.SequenceSource')) BEGIN
	DROP TABLE Component.SequenceSource
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Common.Exception')) BEGIN
	DROP TABLE Common.Exception
END
GO
-- SequenceSource provides an indication of how the sequence got into the database.
-- For example: 'BlastN NCBI' tells us that there will be a record in BlastN.Match with additional information about the sequence.
--	Similarly, 'GenBank' tells us we got the record from GenBank (be it file-based or a download, if the latter ever becomes possible).
--	'FASTA' would tell us it came from a FASTA query, such that there's very little data available (just the header and the sequence).
CREATE TABLE Component.SequenceSource
(
	ID int NOT NULL IDENTITY(1,1),
	Name varchar(20) NOT NULL,

	CONSTRAINT PK_Components_SequenceSource PRIMARY KEY (ID ASC)
)
GO
INSERT INTO Component.SequenceSource (Name) 
VALUES ('FASTA'), ('BlastN NCBI'), ('GenBank'), ('Ensembl')
GO
CREATE TABLE Component.[Sequence]
(
	ID uniqueidentifier NOT NULL,
	SourceID int NOT NULL,
	Nucleotides varchar(MAX) NULL,
	SequenceStart int NOT NULL,
	SequenceEnd int NOT NULL,
	CodingSequenceStart int NULL,
	CodingSequenceEnd int NULL,
	FASTAHeader varchar(250) NOT NULL,
	GenBankID varchar(20) NULL,
	LastUpdatedAt datetime2(7) CONSTRAINT DF_Component_Sequence_LastUpdatedAt DEFAULT (sysdatetime()),
	
	CONSTRAINT PK_Components_Sequence PRIMARY KEY (ID ASC)
)
GO
CREATE TABLE Component.SequenceAlignment
(
	SequenceID uniqueidentifier NOT NULL,
	AlignmentID uniqueidentifier NOT NULL,

	CONSTRAINT PKComponents_SequenceAlignment PRIMARY KEY (SequenceID ASC, AlignmentID ASC)
)
ALTER TABLE Component.SequenceAlignment ADD CONSTRAINT FK_Components_SequenceAlignment_SequenceID FOREIGN KEY (SequenceID) REFERENCES Component.[Sequence] (ID)
ALTER TABLE Component.SequenceAlignment ADD CONSTRAINT FK_Components_SequenceAlignment_AlignmentID FOREIGN KEY (AlignmentID) REFERENCES Component.[Sequence] (ID)
GO
CREATE TABLE Common.Exception
(
	ID uniqueidentifier NOT NULL,
	[Message] varchar(MAX) NOT NULL,
	[Source] varchar(MAX) NOT NULL,
	StackTrace varchar(MAX) NOT NULL,
	ParentID uniqueidentifier NULL

	CONSTRAINT PK_Common_Exception PRIMARY KEY (ID ASC)
)
ALTER TABLE Common.Exception ADD CONSTRAINT FK_Common_Exception_ExceptionID FOREIGN KEY (ParentID) REFERENCES Common.Exception (ID)
GO
CREATE TABLE Common.ExceptionLevel
(
	ID int NOT NULL IDENTITY(1,1),
	Name varchar(50) NOT NULL,

	CONSTRAINT PK_Common_ExceptionLevel PRIMARY KEY (ID ASC)
)
GO
INSERT INTO Common.ExceptionLevel (Name)
VALUES ('Exception'), ('Warning')
GO
CREATE TABLE Component.SequenceException
(
	SequenceID uniqueidentifier NOT NULL,
	ExceptionID uniqueidentifier NOT NULL,
	ExceptionLevelID int NOT NULL,
	CONSTRAINT PK_Components_SequenceException PRIMARY KEY (SequenceID ASC, ExceptionID ASC)
)
ALTER TABLE Component.SequenceException ADD CONSTRAINT FK_Components_SequenceException_SequenceID FOREIGN KEY (SequenceID) REFERENCES Component.[Sequence] (ID)
ALTER TABLE Component.SequenceException ADD CONSTRAINT FK_Components_SequenceException_ExceptionID FOREIGN KEY (ExceptionID) REFERENCES Common.Exception (ID)
GO
CREATE TABLE BlastN.Match
(
	SequenceID uniqueidentifier NOT NULL,
	[Index] int NOT NULL,
	Header varchar(MAX) NOT NULL,
	SourceRecordID uniqueidentifier NULL

	CONSTRAINT PK_BlastN_Match PRIMARY KEY (SequenceID ASC)
)
ALTER TABLE BlastN.Match ADD CONSTRAINT FK_BlastN_Match_SequenceID FOREIGN KEY (SequenceID) REFERENCES Component.Sequence (ID)
GO
CREATE TABLE BlastN.ExonOrientation
(
	ID int NOT NULL IDENTITY(1,1),
	Name varchar(10) NOT NULL

	CONSTRAINT PK_BlastN_ExonOrientation PRIMARY KEY (ID ASC)
)
GO
INSERT INTO BlastN.ExonOrientation (Name) VALUES ('Plus/Plus'), ('Plus/Minus')
GO
CREATE TABLE BlastN.Exon
(
	ID uniqueidentifier NOT NULL,
	BitScore float NOT NULL,
	AlignmentLength int NOT NULL,
	IdentitiesCount int NOT NULL,
	Gaps int NOT NULL,
	SubjectIndexStart int NOT NULL,
	SubjectIndexEnd int NOT NULL,
	QueryIndexStart int NOT NULL,
	QueryIndexEnd int NOT NULL,
	OrientationID int NOT NULL

	CONSTRAINT PK_BlastN_Exon PRIMARY KEY (ID ASC)
)
ALTER TABLE BlastN.Exon ADD CONSTRAINT FK_BlastN_Exon_OrientationID FOREIGN KEY (OrientationID) REFERENCES BlastN.ExonOrientation (ID)
GO
CREATE TABLE BlastN.MatchExon
(
	MatchID uniqueidentifier NOT NULL,
	ExonID uniqueidentifier NOT NULL,

	CONSTRAINT PK_BlastN_MatchExon PRIMARY KEY (MatchID ASC, ExonID ASC)
)
ALTER TABLE BlastN.MatchExon ADD CONSTRAINT FK_BlastN_MatchExon_MatchID FOREIGN KEY (MatchID) REFERENCES BlastN.Match (SequenceID)
ALTER TABLE BlastN.MatchExon ADD CONSTRAINT FK_BlastN_MatchExon_ExonID FOREIGN KEY (ExonID) REFERENCES BlastN.Exon (ID)
GO
-- Stores information about a job that was sent to BlastN@NCBI
CREATE TABLE BlastN.Job
(
	ID int NOT NULL IDENTITY(1,1), -- Surrogate primary key to make indexing faster.
	RequestID varchar(12) NOT NULL,
	SubmittedAt datetime2(7) NOT NULL,
	DatabaseName varchar(250) NOT NULL, -- 'nr', whatever local database file the user specified, etc
	ExpiresAt datetime2(7) NULL, -- NULL for local database jobs, because those don't expire.
	TargetDatabase varchar(5) NOT NULL -- 'NCBI' or 'Local'

	CONSTRAINT PK_BlastN_Job PRIMARY KEY (ID ASC)
)
GO
CREATE UNIQUE INDEX IX_BlastN_Job_RequestID ON BlastN.Job (RequestID)
GO
CREATE TABLE BlastN.JobSequence
(
	JobID int NOT NULL,
	SequenceID uniqueidentifier NOT NULL,

	CONSTRAINT PK_BlastN_JobSequence PRIMARY KEY (JobID ASC, SequenceID ASC)
)
ALTER TABLE BlastN.JobSequence ADD CONSTRAINT FK_BlastN_JobSequence_JobID FOREIGN KEY (JobID) REFERENCES BlastN.Job (ID)
ALTER TABLE BlastN.JobSequence ADD CONSTRAINT FK_BlastN_JobSequence_SequenceID FOREIGN KEY (SequenceID) REFERENCES Component.[Sequence] (ID)
GO

IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.ResultSetData')) BEGIN
	DROP TABLE BlastN.ResultSetData
END
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('BlastN.ResultSet')) BEGIN
	DROP TABLE BlastN.ResultSet
END
GO
-- Temporary tables for figuring out in-memory merging
CREATE TABLE BlastN.ResultSet
(
	id int NOT NULL IDENTITY(1,1),
	created_at datetime2(7) NOT NULL CONSTRAINT DF_BlastN_ResultSet_created_at DEFAULT (sysdatetime()),

	CONSTRAINT PK_BlastN_ResultSet PRIMARY KEY (id ASC)
)
GO
CREATE TABLE BlastN.ResultSetData
(
	id int NOT NULL IDENTITY(1,1),
	result_set_id int NOT NULL,
	input_index int NOT NULL,
	original_sequence_source varchar(500) NOT NULL,
	contig_number varchar(100) NULL,
	accession varchar(20) NULL,
	original_sequence_submitted varchar(500) NOT NULL,
	subject_sequence_range_start int NOT NULL,
	subject_sequence_range_end int NOT NULL,
	subject_query_match varchar(500) NULL,
	subject_query_match_rank int NULL,
	percentage_sequence_match int NULL,
	genbank_id varchar(20) NULL,
	request_id varchar(12) NULL,
	source_file varchar(250) NULL,
	original_sequence_submitted_nucleotides varchar(MAX) NULL,
	alignment_nucleotides varchar(MAX) NULL,
	created_at datetime2(7) NOT NULL CONSTRAINT DF_BlastN_ResultSetData_created_at DEFAULT (sysdatetime())

	CONSTRAINT PK_BlastN_ResultSetData PRIMARY KEY (id ASC),
	CONSTRAINT FK_BlastN_ResultSetData_id FOREIGN KEY (result_set_id) REFERENCES BlastN.ResultSet (id)
)
GO
*/
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('GenBank.Record')) BEGIN
	DROP TABLE GenBank.Record
END
GO
CREATE TABLE GenBank.Record
(
	ID int NOT NULL IDENTITY(1,1), -- Surrogate key to make JOINs faster
	GenBankID varchar(10) NOT NULL,
	Locus varchar(20) NOT NULL,
	[Definition] varchar(250) NOT NULL,
	Accession varchar(20) NOT NULL,
	FullTaxonomy varchar(1000) NOT NULL,
	LastUpdatedAt datetime2(7) NOT NULL CONSTRAINT DF_GenBank_Record_LastDownloadedAt DEFAULT (sysdatetime())

	CONSTRAINT PK_GenBank_Record PRIMARY KEY (ID ASC)
)
GO
CREATE UNIQUE INDEX IX_GenBank_Record ON GenBank.Record (GenBankID ASC)
GO