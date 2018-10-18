SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Wasn't being used anywhere, just cleaning house.
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_EditSimple')) BEGIN
	DROP PROCEDURE Gene.Gene_EditSimple
END
GO

GO
-- Removal of the constraint on GenBankID, to allow for creating new Gene.Gene records with the same GenBankID but different sequence info after
--	alignment in MEGA, for example.
IF EXISTS (SELECT * FROM sys.indexes i WHERE i.object_id = OBJECT_ID('Gene.Gene') AND i.name = 'IX_Gene_Gene_GenBankID') BEGIN
	DROP INDEX [IX_Gene_Gene_GenBankID] ON [Gene].[Gene]
END
GO
IF NOT EXISTS (SELECT * FROM Gene.[Source] s WHERE s.[Key] = 'MEGA') BEGIN
	INSERT INTO Gene.[Source] ([Key], Name) VALUES ('MEGA', 'MEGA')
END
GO
ALTER PROCEDURE [Gene].[Gene_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@Definition varchar(1000),
	@SourceID int,
	@GenBankID int,
	@Locus varchar(100),
	@Accession varchar(20),
	@Organism varchar(250),
	@Taxonomy varchar(4000),
	@Description varchar(MAX),
	@Nucleotides varchar(MAX),
	@SequenceTypeID int,
	@SequenceStart int,
	@SequenceEnd int,
	@CodingSequenceStart int = NULL,
	@CodingSequenceEnd int = NULL,
	@LastUpdatedAt datetime2(7) = NULL,
	@LastUpdateSourceID int = NULL,
	@AllowOverwrite bit = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @NowTime datetime2(7) = SYSDATETIME()
			,@OriginalTaxonomyID int
			,@OriginalTaxonomy varchar(MAX)
			,@TaxonomyID int = NULL

	IF @LastUpdateSourceID IS NULL BEGIN
		SET @LastUpdateSourceID = @SourceID
	END

	IF NOT EXISTS (SELECT * FROM Gene.Gene g WHERE g.ID = @ID) BEGIN
		IF (@ID IS NULL) BEGIN
			SET @ID = NEWID()
		END

		IF (@Taxonomy IS NOT NULL) AND (LTRIM(RTRIM(@Taxonomy)) <> '') BEGIN
			EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT
		END

		INSERT INTO Gene.Gene (ID, [Definition], SourceID
								,GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID, [Description]
								,Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd
								,LastUpdatedAt, LastUpdateSourceID)
		VALUES (@ID, @Definition, @SourceID
				,@GenBankID, @Locus, @Accession, @Organism, @Taxonomy, @TaxonomyID, @Description
				,@Nucleotides, @SequenceTypeID, @SequenceStart, @SequenceEnd, @CodingSequenceStart, @CodingSequenceEnd
				,@LastUpdatedAt, @LastUpdateSourceID)
	END
	ELSE BEGIN
		IF (@AllowOverwrite = 1) BEGIN
			SELECT @OriginalTaxonomyID = g.TaxonomyID
					,@OriginalTaxonomy = g.Taxonomy
				FROM Gene.Gene g WHERE g.ID = @ID

			IF (@Taxonomy IS NOT NULL) AND (LTRIM(RTRIM(@Taxonomy)) <> '') AND (@OriginalTaxonomy <> @Taxonomy OR @OriginalTaxonomyID IS NULL) BEGIN
				EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT
			END
			ELSE IF (@OriginalTaxonomy = @Taxonomy) BEGIN
				SET @TaxonomyID = @OriginalTaxonomyID
			END

			-- Currently not allowing for partial updates (i.e.: ISNULL(@column, column)); whenever a gene is saved and an update is possible all
			-- values must be provided.
			UPDATE Gene.Gene
				SET SourceID = @SourceID
					,[Definition] = @Definition
					,GenBankID = @GenBankID
					,Locus = @Locus
					,Accession = @Accession
					,Organism = @Organism
					,Taxonomy = @Taxonomy
					,TaxonomyID = @TaxonomyID
					,[Description] = @Description
					,Nucleotides = @Nucleotides
					,SequenceTypeID = @SequenceTypeID
					,SequenceStart = @SequenceStart -- start:These are becoming obsolete
					,SequenceEnd = @SequenceEnd
					,CodingSequenceStart = @CodingSequenceStart
					,CodingSequenceEnd = @CodingSequenceEnd -- end:obsolete
					,LastUpdatedAt = @LastUpdatedAt
					,LastUpdateSourceID = @LastUpdateSourceID
				WHERE ID = @ID
		END
	END
END
GO

GO
-- References table for the third-party products that Pilgrimage makes use of
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Common.ThirdPartyComponentReference')) BEGIN
	CREATE TABLE Common.ThirdPartyComponentReference (
		ID int IDENTITY(1,1) NOT NULL,
		Name varchar(200) NOT NULL,
		[Version] varchar(100) NOT NULL,
		Creator varchar(200) NOT NULL,
		ProductURL varchar(2048) NOT NULL,
		LastUpdatedAt datetime2(7) NOT NULL,
		LastRetrievedAt datetime2(7) NOT NULL,
		Copyright varchar(9) NULL,
		LicenseType varchar(100) NULL,
		LicenseURL varchar(2048) NULL,
		LicenseText nvarchar(MAX) NULL,
		Modified bit NOT NULL CONSTRAINT DF_Common_ThirdPartyComponentReference_Modified DEFAULT (0),
		Logo varchar(100) NULL,
		
		CONSTRAINT PK_Common_ThirdPartyComponentReference PRIMARY KEY CLUSTERED (ID ASC)
	)
	
	DECLARE @v sql_variant 
	SET @v = N'This is the date the component itself was last updated or published.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'ThirdPartyComponentReference', N'COLUMN', N'LastUpdatedAt'
	SET @v = N'This is the date the component was downloaded by us.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'ThirdPartyComponentReference', N'COLUMN', N'LastRetrievedAt'
	SET @v = N'This is free-text for now just to make things easier, but arguably could be converted to a list of things like GNU,'
				+ N' Creative Commons, Apache, etc.  However, we''d then need to address the license''s verison number as well,'
				+ N' potentially spawning a Common.ThirdPartyComponentLicense table.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'ThirdPartyComponentReference', N'COLUMN', N'LicenseType'
	SET @v = N'Unfortunately not all products will have a specific license on them.  Some, like PAML, just have a copyright blurb, and that text'
				+ N'goes here.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'ThirdPartyComponentReference', N'COLUMN', N'LicenseText'
	SET @v = N'.NET Bio was modified to implement a configurable timeout for NCBI web services.  This kind of modification is allowable under its'
				+ N' license, but should be noted in our reference to it.  When this bit is set true, a message should appear in the UI stating'
				+ N' something to the effect of: This component has been modified by Chang Lab within the constraints of the component''s license.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'ThirdPartyComponentReference', N'COLUMN', N'Modified'
	SET @v = N'Must correspond to an entry in Pilgrimage''s global Resources.resx file.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'ThirdPartyComponentReference', N'COLUMN', N'Logo'

	INSERT INTO Common.ThirdPartyComponentReference (Name, [Version], Creator, ProductURL, LastUpdatedAt, LastRetrievedAt, Copyright, LicenseType, LicenseURL, Modified)
	VALUES ('.NET Bio', '1.1', 'The Outercurve Foundation', 'http://bio.codeplex.com/',
				'2013-07-23 3:00 AM', '2014-05-05 9:51 AM', '2011-2013', 'Apache License 2.0', 'http://bio.codeplex.com/license', 1)
	INSERT INTO Common.ThirdPartyComponentReference (Name, [Version], Creator, ProductURL, LastUpdatedAt, LastRetrievedAt, Copyright, LicenseText)
	VALUES ('Phylogenetic Analysis by Maximum Likelihood (PAML)', '4.8', 'Ziheng Yang', 'http://abacus.gene.ucl.ac.uk/software/paml.html',
				'2014-03-01 12:00 AM', '2014-07-29 2:24 PM', '1993-2008',
				'The software package is provided "as is" without warranty of any kind. In no event shall the author or his employer be held'
					+ ' responsible for any damage resulting from the use of this software, including but not limited to the frustration that you may'
					+ ' experience in using the package. The program package, including source codes, example data sets, executables, and this'
					+ ' documentation, is distributed free of charge for academic use only. Permission is granted to copy and use programs in the'
					+ ' package provided no fee is charged for it and provided that this copyright notice is not removed.')
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Common.ThirdPartyComponentReference_List')) BEGIN
	DROP PROCEDURE Common.ThirdPartyComponentReference_List
END
GO
CREATE PROCEDURE Common.ThirdPartyComponentReference_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT r.ID
			,r.Name
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
		FROM Common.ThirdPartyComponentReference r
		ORDER BY r.Name
END
GO

GO
-- Replaced ': ' with '; ' in the lineage string; a typo that was throwing off Taxonomy.Taxon_Parse
GO
ALTER PROCEDURE [Taxonomy].[Taxon_ListTreeView]
	@NameSearch varchar(200) = NULL
AS
WITH TaxonTree AS (
	SELECT t.ID
			,t.HID
			,t.Name
			,0 AS ParentID
			,hierarchyid::Parse('/') AS ParentHID
			,CAST('' AS varchar(200)) AS ParentName
			,CAST(t.Name AS varchar(MAX)) AS Lineage
	FROM Taxonomy.Taxon t
	WHERE t.HID.GetLevel() = 1
	UNION ALL
	SELECT t.ID
			,t.HID
			,t.Name
			,d.ID AS ParentID
			,d.HID AS ParentHID
			,d.Name AS ParentName
			,d.Lineage + '; ' + t.Name
	FROM Taxonomy.Taxon t
	JOIN TaxonTree d ON d.HID = t.HID.GetAncestor(1)
)

SELECT t.ID
		,t.Name
		,t.HID.ToString() AS Hierarchy
		,t.ParentID
		,t.Lineage
	FROM TaxonTree t
	WHERE (@NameSearch IS NULL OR t.Name LIKE @NameSearch) -- It's up to the caller to apply %'s
	ORDER BY t.HID
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.0.10'
	WHERE [Key] = 'DatabaseVersion'
GO