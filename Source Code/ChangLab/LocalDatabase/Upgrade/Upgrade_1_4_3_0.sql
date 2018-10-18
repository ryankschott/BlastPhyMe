SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM Gene.[Source] s WHERE s.[Key] = 'MUSCLE') BEGIN
	INSERT INTO Gene.[Source] (Name, [Key])
	VALUES ('MUSCLE', 'MUSCLE')
END
GO
IF NOT EXISTS (SELECT * FROM Job.[Target] s WHERE s.[Key] = 'MUSCLE') BEGIN
	INSERT INTO Job.[Target] (Name, [Key])
	VALUES ('MUSCLE', 'MUSCLE')
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.AlignedGeneSource')) BEGIN
	CREATE TABLE Gene.AlignedGeneSource (
		JobID uniqueidentifier NOT NULL,
		InputGeneID uniqueidentifier NOT NULL,
		OutputGeneID uniqueidentifier NOT NULL,

		CONSTRAINT PK_Gene_AlignedGeneSource PRIMARY KEY CLUSTERED (JobID ASC, InputGeneID ASC, OutputGeneID ASC)
	)

	ALTER TABLE Gene.AlignedGeneSource ADD CONSTRAINT FK_Gene_AlignedGeneSource_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
	ALTER TABLE Gene.AlignedGeneSource ADD CONSTRAINT FK_Gene_AlignedGeneSource_InputGeneID FOREIGN KEY (InputGeneID) REFERENCES Gene.Gene (ID)
	ALTER TABLE Gene.AlignedGeneSource ADD CONSTRAINT FK_Gene_AlignedGeneSource_OutputGeneID FOREIGN KEY (OutputGeneID) REFERENCES Gene.Gene (ID)

	DECLARE @v sql_variant 
	SET @v = N'Stores a link between the gene that''s sequence was aligned and the output gene created with the new nucleotide sequence. '
			+ N'This provides a way for us to offer the user the option to overwrite the original record with the new nucleotide sequence instead '
			+ N'of creating new Gene.Gene records.  That''s slighlty complicated by the fact that the Gene.Gene records already exist in the '
			+ N'by the point at which they''ve been offered the opportunity, and we have to go back and clean-up the unnecessary records after the '
			+ N'fact, but doing it this way gives us the capacity to allow the user to close and re-open Pilgrimage and all that good stuff before '
			+ N'they''ve dealt with the results of the alignment.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Gene', N'TABLE', N'AlignedGeneSource', NULL, NULL

	INSERT INTO Gene.AlignedGeneSource (JobID, InputGeneID, OutputGeneID)
	SELECT src.JobID, src.InputGeneID, src.OutputGeneID
		FROM PRANK.GeneSource src

	DELETE FROM PRANK.GeneSource
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.AlignedGeneSource_Edit')) BEGIN
	DROP PROCEDURE Gene.AlignedGeneSource_Edit
END
GO
CREATE PROCEDURE Gene.AlignedGeneSource_Edit
	@JobID uniqueidentifier
	,@InputGeneID uniqueidentifier
	,@OutputGeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Gene.AlignedGeneSource (JobID, InputGeneID, OutputGeneID)
	VALUES (@JobID, @InputGeneID, @OutputGeneID)
END
GO
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PRANK.GeneSource')) BEGIN
	DROP TABLE PRANK.GeneSource
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PRANK.GeneSource_Edit')) BEGIN
	DROP PROCEDURE PRANK.GeneSource_Edit
END
GO

IF NOT EXISTS (SELECT * FROM Common.ThirdPartyComponentReference ref WHERE ref.Name = 'MUSCLE') BEGIN
	INSERT INTO Common.ThirdPartyComponentReference
		(Name, [Version], Creator, ProductURL, LastUpdatedAt, LastRetrievedAt, Copyright, LicenseURL, LicenseText, Modified, Logo, Packaged, Citation)
	VALUES
		('MUSCLE', '3.8.31', 'Robert Edgar', 'http://www.drive5.com/muscle/',
			'2010-05-03', '2015-06-02 1:00 AM', NULL, 
			'http://www.drive5.com/muscle/manual/license.html',
'MUSCLE is public domain software
The MUSCLE software, including object and source code and documentation, is donated to the public domain. You may therefore freely copy it for any legal purpose you wish. Acknowledgement of authorship and citation in publications is appreciated.

Disclaimer of warranty
THIS SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.',
			0, NULL, 0,
			'Edgar, R.C. (2004) MUSCLE: multiple sequence alignment with high accuracy and high throughput.Nucleic Acids Res. 32(5):1792-1797.')
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.3.0'
	WHERE [Key] = 'DatabaseVersion'
GO