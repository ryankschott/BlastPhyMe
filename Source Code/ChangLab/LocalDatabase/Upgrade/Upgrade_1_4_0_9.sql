SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.table_types t WHERE t.user_type_id = TYPE_ID('Common.ListVarCharIdentifier')) BEGIN
	CREATE TYPE [Common].[ListVarCharIdentifier] AS TABLE ([Value] varchar(36) NOT NULL)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_FilterBy')) BEGIN
	DROP PROCEDURE Gene.Gene_FilterBy
END
GO
CREATE PROCEDURE Gene.Gene_FilterBy
	@GeneIDs Common.ListVarCharIdentifier READONLY
	,@DuplicateByOrganismName bit
	,@DuplicateWholeSequences bit
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @FilteredGeneIDsByOrganismName TABLE (GeneID uniqueidentifier)
	DECLARE @FilteredGeneIDsByWholeSequence TABLE (GeneID uniqueidentifier, DuplicateWholeSequenceGroupNumber int)
	DECLARE @FilteredGeneIDs TABLE (GeneID uniqueidentifier, DuplicateWholeSequenceGroupNumber int)
	
	IF @DuplicateByOrganismName = 1 BEGIN
		WITH DistinctOrganismNames AS (
			SELECT g.Organism, COUNT(*) AS Duplicates
				FROM Gene.Gene g
				JOIN @GeneIDs ids ON CAST(ids.Value AS uniqueidentifier) = g.ID
				GROUP BY g.Organism
		)

		INSERT INTO @FilteredGeneIDsByOrganismName
		SELECT g.ID
			FROM Gene.Gene g
			JOIN @GeneIDs ids ON CAST(ids.Value AS uniqueidentifier) = g.ID
			JOIN DistinctOrganismNames org ON org.Organism = g.Organism
			WHERE org.Duplicates > 1
	END
	ELSE BEGIN
		INSERT INTO @FilteredGeneIDsByOrganismName
		SELECT g.Value FROM @GeneIDs g
	END
	
	IF @DuplicateWholeSequences = 1 BEGIN
		WITH WholeSequenceGroups AS (
			SELECT g.ID
					,DENSE_RANK() OVER (ORDER BY seq.Nucleotides) AS DuplicateWholeSequenceGroupNumber
				FROM Gene.Gene g
				JOIN @GeneIDs ids ON CAST(ids.Value AS uniqueidentifier) = g.ID
				JOIN Gene.NucleotideSequence seq ON seq.GeneID = g.ID
		),
		DuplicateSequenceGroups AS (
			SELECT g.DuplicateWholeSequenceGroupNumber
				FROM WholeSequenceGroups g
				GROUP BY g.DuplicateWholeSequenceGroupNumber
				HAVING COUNT(*) > 1
		)

		INSERT INTO @FilteredGeneIDsByWholeSequence
		SELECT grp.ID
				,grp.DuplicateWholeSequenceGroupNumber
			FROM WholeSequenceGroups grp
			JOIN DuplicateSequenceGroups dup ON dup.DuplicateWholeSequenceGroupNumber = grp.DuplicateWholeSequenceGroupNumber
	END
	ELSE BEGIN
		INSERT INTO @FilteredGeneIDsByWholeSequence
		SELECT g.Value, 0 FROM @GeneIDs g
	END

	SELECT g.Value AS GeneID, f_seq.DuplicateWholeSequenceGroupNumber
		FROM @GeneIDs g
		JOIN @FilteredGeneIDsByOrganismName f_org ON f_org.GeneID = g.Value
		JOIN @FilteredGeneIDsByWholeSequence f_seq ON f_seq.GeneID = g.Value
	ORDER BY f_seq.DuplicateWholeSequenceGroupNumber
END
GO
ALTER PROCEDURE [Common].[ApplicationProperty_List]
	@Key varchar(30) = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT ap.ID
			,ap.[Key]
			,ap.Value
		FROM Common.ApplicationProperty ap
		WHERE ((@Key IS NULL) OR (@Key IS NOT NULL AND ap.[Key] = @Key))
END
GO

GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Gene.Gene') AND c.name = 'LastUpdateSourceID') BEGIN
	ALTER TABLE Gene.Gene ADD LastUpdateSourceID int NOT NULL 
		CONSTRAINT DF_Gene_Gene_LastUpdateSourceID DEFAULT (1)
		CONSTRAINT FK_Gene_Gene_LastUpdateSourceID FOREIGN KEY (LastUpdateSourceID) REFERENCES Gene.[Source] (ID)

	ALTER TABLE Gene.GeneHistory ADD LastUpdateSourceID int NOT NULL 
		CONSTRAINT DF_Gene_GeneHistory_LastUpdateSourceID DEFAULT (1)
		CONSTRAINT FK_Gene_GeneHistory_LastUpdateSourceID FOREIGN KEY (LastUpdateSourceID) REFERENCES Gene.[Source] (ID)

	EXEC('DISABLE TRIGGER Gene.Gene_LogHistory ON Gene.Gene')
	EXEC('UPDATE Gene.Gene SET LastUpdateSourceID = SourceID')
	EXEC('UPDATE Gene.GeneHistory SET LastUpdateSourceID = SourceID')
	EXEC('ENABLE TRIGGER Gene.Gene_LogHistory ON Gene.Gene')

	ALTER TABLE Gene.Gene DROP CONSTRAINT DF_Gene_Gene_LastUpdateSourceID
	ALTER TABLE Gene.GeneHistory DROP CONSTRAINT FK_Gene_GeneHistory_LastUpdateSourceID

	INSERT INTO Gene.[Source] (Name, [Key]) VALUES ('User', 'User')
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Gene.Gene') AND c.name = 'Description') BEGIN
	ALTER TABLE Gene.Gene ADD [Description] varchar(MAX)
	ALTER TABLE Gene.GeneHistory ADD [Description] varchar(MAX)
END
GO
ALTER TRIGGER [Gene].[Gene_LogHistory] ON [Gene].[Gene]
AFTER INSERT, UPDATE
AS
BEGIN

	INSERT INTO Gene.GeneHistory (RevisionID, ID, SourceID,
									GenBankID, Locus, [Definition], Accession, Organism, Taxonomy, [Description],
									Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd,
									LastUpdatedAt, Active, TaxonomyID, LastUpdateSourceID)
		SELECT ISNULL((SELECT MAX(RevisionID) FROM Gene.GeneHistory g WHERE g.ID = i.ID), 0) + 1
				,i.ID
				,i.SourceID
				,i.GenBankID
				,i.Locus
				,i.[Definition]
				,i.Accession
				,i.Organism
				,i.Taxonomy
				,i.[Description]
				,i.Nucleotides
				,i.SequenceTypeID
				,i.SequenceStart
				,i.SequenceEnd
				,i.CodingSequenceStart
				,i.CodingSequenceEnd
				,i.LastUpdatedAt
				,i.Active
				,i.TaxonomyID
				,i.LastUpdateSourceID
			FROM inserted i

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
			,@Overwrite bit = 1
			,@OriginalTaxonomyID int
			,@OriginalTaxonomy varchar(MAX)
			,@TaxonomyID int = NULL

	IF @LastUpdateSourceID IS NULL BEGIN
		SET @LastUpdateSourceID = @SourceID
	END

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
		IF (@Overwrite = 1) BEGIN
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
					,SequenceStart = @SequenceStart -- These are becoming obsolete
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
ALTER PROCEDURE [Gene].[Gene_Get]
	@GeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.ID
			,g.[Definition]
			,g.SourceID
			,g.LastUpdatedAt
			,g.LastUpdateSourceID

			,g.GenBankID
			,g.Locus
			,g.Accession
			,g.Organism
			,g.Taxonomy
			,g.[Description]

			,g.Nucleotides
			,g.SequenceTypeID
			,g.SequenceStart
			,g.SequenceEnd
			,g.CodingSequenceStart
			,g.CodingSequenceEnd

			,CASE WHEN (t.ID IS NOT NULL)
				  THEN t.HID.ToString()
				  ELSE ''
				  END AS TaxonomyHierarchy
			
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.QueryID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedSubjectSequences
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.SubjectID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedQuerySequences

		FROM Gene.Gene g
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		
		WHERE g.ID = @GeneID
		ORDER BY g.Organism, g.Accession, g.GenBankID
END
GO
ALTER PROCEDURE [RecordSet].[Gene_List]
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.ID
			,g.[Definition]
			,g.SourceID
			,g.LastUpdatedAt
			,g.LastUpdateSourceID
			,rs_g.ModifiedAt

			,g.GenBankID
			,g.Locus
			,g.Accession
			,g.Organism
			,g.Taxonomy
			,g.[Description]

			,g.Nucleotides
			,g.SequenceTypeID
			,g.SequenceStart
			,g.SequenceEnd
			,g.CodingSequenceStart
			,g.CodingSequenceEnd

			,CASE WHEN (t.ID IS NOT NULL)
				  THEN t.HID.ToString()
				  ELSE ''
				  END AS TaxonomyHierarchy

		FROM RecordSet.Gene rs_g
		JOIN Gene.Gene g ON g.ID = rs_g.GeneID
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		
		WHERE rs_g.RecordSetID = @RecordSetID
			AND g.Active = 1
		ORDER BY rs_g.ModifiedAt DESC, g.Organism, g.Accession, g.GenBankID
END
GO
ALTER PROCEDURE [RecordSet].[SubSetGene_List]
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @GeneStatusID_Processed int = (SELECT s.ID FROM Job.GeneStatus s WHERE s.[Key] = 'Processed')

	SELECT g.ID
			,g.[Definition]
			,g.SourceID
			,g.LastUpdatedAt
			,g.LastUpdateSourceID
			,sg.ModifiedAt

			,g.GenBankID
			,g.Locus
			,g.Accession
			,g.Organism
			,g.Taxonomy
			,g.[Description]

			,g.Nucleotides
			,g.SequenceTypeID

			,CASE WHEN (t.ID IS NOT NULL)
				  THEN t.HID.ToString()
				  ELSE ''
				  END AS TaxonomyHierarchy
			,CAST(CASE WHEN EXISTS (SELECT * 
										FROM NCBI.Gene ng
										WHERE ng.GeneID = sg.GeneID
											AND ng.StatusID = @GeneStatusID_Processed)
				-- We're not doing a LEFT OUTER JOIN for this because a gene could be submitted multiple times, thus multiple requests
						THEN 1
						ELSE 0
						END AS bit) AS ProcessedThroughBLASTNAtNCBI
			
		FROM RecordSet.SubSetGene sg
		JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
		JOIN RecordSet.Gene rs_g ON rs_g.GeneID = sg.GeneID AND rs_g.RecordSetID = sub.RecordSetID
		JOIN Gene.Gene g ON g.ID = rs_g.GeneID
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		
		WHERE sg.SubSetID = @SubSetID
			AND g.Active = 1
		ORDER BY ISNULL(g.Organism, g.[Definition])
				,LEN(ISNULL(g.Nucleotides, ''))
				,rs_g.ModifiedAt DESC
END
GO

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
IF (SELECT c.max_length FROM sys.columns c WHERE c.object_id = OBJECT_ID('Taxonomy.Taxon') AND c.name = 'Name') <> 200 BEGIN
	CREATE TABLE Taxonomy.Tmp_Taxon (
		ID int NOT NULL IDENTITY (1, 1),
		HID hierarchyid NOT NULL,
		Name varchar(200) NOT NULL
	)  ON [PRIMARY]

	SET IDENTITY_INSERT Taxonomy.Tmp_Taxon ON
	IF EXISTS(SELECT * FROM Taxonomy.Taxon) BEGIN
		 EXEC('INSERT INTO Taxonomy.Tmp_Taxon (ID, HID, Name) SELECT ID, HID, Name FROM Taxonomy.Taxon WITH (HOLDLOCK TABLOCKX)')
	END
	SET IDENTITY_INSERT Taxonomy.Tmp_Taxon OFF

	IF EXISTS (SELECT * FROM sys.foreign_keys fk WHERE fk.parent_object_id = OBJECT_ID('Gene.Gene') AND fk.name = 'FK_Gene_Gene_TaxonomyID') BEGIN
		EXEC ('ALTER TABLE Gene.Gene DROP CONSTRAINT FK_Gene_Gene_TaxonomyID')
	END
	IF EXISTS (SELECT * FROM sys.foreign_keys fk WHERE fk.parent_object_id = OBJECT_ID('Gene.GeneHistory') AND fk.name = 'FK_Gene_GeneHistory_TaxonomyID') BEGIN
		EXEC ('ALTER TABLE Gene.GeneHistory DROP CONSTRAINT FK_Gene_GeneHistory_TaxonomyID')
	END
	
	DROP TABLE Taxonomy.Taxon
	EXECUTE sp_rename N'Taxonomy.Tmp_Taxon', N'Taxon', 'OBJECT' 
	ALTER TABLE Taxonomy.Taxon ADD CONSTRAINT PK_Taxonomy_Taxon PRIMARY KEY CLUSTERED (ID) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	CREATE UNIQUE NONCLUSTERED INDEX IX_Taxonomy_Taxon_HID ON Taxonomy.Taxon (HID) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_TaxonomyID FOREIGN KEY (TaxonomyID) REFERENCES Taxonomy.Taxon (ID) ON UPDATE NO ACTION ON DELETE NO ACTION 
	ALTER TABLE Gene.Gene ADD CONSTRAINT FK_Gene_Gene_TaxonomyID FOREIGN KEY (TaxonomyID) REFERENCES Taxonomy.Taxon (ID) ON UPDATE NO ACTION ON DELETE NO ACTION
END
GO
COMMIT TRANSACTION
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
			,d.Lineage + ': ' + t.Name
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
ALTER PROCEDURE [Taxonomy].[Taxon_ListTreeView_ForRecordSet]
	@RecordSetID uniqueidentifier,
	@SubSetID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON;

	WITH SelectedTaxa AS (
		SELECT DISTINCT t.ID
			FROM Gene.Gene g
			JOIN RecordSet.SubSetGene sg ON sg.GeneID = g.ID
			JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
			JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
			WHERE sub.RecordSetID = @RecordSetID
				AND ((@SubSetID IS NULL) OR (sg.SubSetID = @SubSetID))
				AND g.Active = 1
	), RecurseUp AS (
		SELECT t.ID
				,t.HID
				,t.Name
				,p.ID AS ParentID
				,p.HID AS ParentHID
				,p.Name AS ParentName
			FROM Taxonomy.Taxon t
			JOIN Taxonomy.Taxon p ON p.HID = t.HID.GetAncestor(1)
			JOIN SelectedTaxa s ON s.ID = t.ID
		UNION ALL
		SELECT t.ParentID AS ID
				,t.ParentHID AS HID
				,t.ParentName AS Name
				,p.ID AS ParentID
				,p.HID AS ParentHID
				,p.Name AS ParentName
			FROM RecurseUp t 
			JOIN Taxonomy.Taxon p ON p.HID = t.ParentHID.GetAncestor(1)
	), TaxonTree AS (
		SELECT DISTINCT
				t.ID
				,t.HID
				,t.Name
				,0 AS ParentID
				,hierarchyid::Parse('/') AS ParentHID
				,CAST('' AS varchar(200)) AS ParentName
			FROM Taxonomy.Taxon t
			JOIN RecurseUp r ON r.ParentHID = t.HID
			WHERE r.HID.GetLevel() = 2
			-- Level 1 won't be included in the recurse because it doesn't have a parent node.  If I ever add a 0x parent node, this will break.
		UNION ALL
		SELECT DISTINCT
				t.*
			FROM RecurseUp t
	)

	SELECT t.ID
			,t.Name
			,t.HID.ToString() AS Hierarchy
			,t.ParentID
		FROM TaxonTree t
		ORDER BY t.HID
END
GO

GO
IF NOT EXISTS (SELECT * FROM Common.ApplicationProperty prop WHERE prop.[Key] = 'LastImportableDataFileVersion') BEGIN
	INSERT INTO Common.ApplicationProperty VALUES (NEWID(), 'LastImportableDataFileVersion', '1.4.0.6')
END
GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.0.9'
	WHERE [Key] = 'DatabaseVersion'
GO