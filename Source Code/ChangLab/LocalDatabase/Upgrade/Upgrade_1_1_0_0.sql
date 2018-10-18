SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('[Common].[SplitString]')) BEGIN
	DROP FUNCTION [Common].[SplitString]
END
GO
CREATE FUNCTION [Common].[SplitString]
(
   @List nvarchar(MAX),
   @Delimiter nvarchar(5)
)
RETURNS @Values TABLE (Value nvarchar(MAX), [Index] int IDENTITY(1,1))
AS
BEGIN
-- Modified, courtesy of http://sqlperformance.com/2012/07/t-sql-queries/split-strings, "Jeff Moden's splitter"

  WITH E1(N)        AS ( SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 
                         UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 
                         UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1),
       E2(N)        AS (SELECT 1 FROM E1 a, E1 b),
       E4(N)        AS (SELECT 1 FROM E2 a, E2 b),
       E42(N)       AS (SELECT 1 FROM E4 a, E2 b),
       cteTally(N)  AS (SELECT 0 UNION ALL SELECT TOP (DATALENGTH(ISNULL(@List,1))) 
                         ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM E42),
       cteStart(N1) AS (SELECT t.N+1 FROM cteTally t
                         WHERE (SUBSTRING(@List,t.N,1) = @Delimiter OR t.N = 0))

	INSERT INTO @Values
		SELECT Item = SUBSTRING(@List, s.N1, ISNULL(NULLIF(CHARINDEX(@Delimiter,@List,s.N1),0)-s.N1,8000))
			FROM cteStart s

	RETURN
END
GO
ALTER PROCEDURE [Common].[ApplicationProperty_Edit]
	@Key varchar(30)
	,@Value varchar(MAX)
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM Common.ApplicationProperty ap WHERE ap.[Key] = @Key) BEGIN
		INSERT INTO Common.ApplicationProperty (ID, [Key], Value)
		VALUES (newid(), @Key, @Value)
	END
	ELSE BEGIN
		UPDATE Common.ApplicationProperty
			SET Value = @Value
			WHERE [Key] = @Key
	END
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Taxonomy.Taxon')) BEGIN
	CREATE TABLE Taxonomy.Taxon
	(
		ID int NOT NULL IDENTITY(1,1),
		HID hierarchyid NOT NULL,
		Name varchar(30) NOT NULL,

		CONSTRAINT PK_Taxonomy_Taxon PRIMARY KEY (ID ASC)
	)
	CREATE UNIQUE INDEX IX_Taxonomy_Taxon_HID ON Taxonomy.Taxon (HID ASC)
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Gene.Gene') AND c.name = 'TaxonomyID') BEGIN
	ALTER TABLE Gene.Gene ADD TaxonomyID int NULL
	ALTER TABLE Gene.GeneHistory ADD TaxonomyID int NULL
	ALTER TABLE Gene.Gene ADD CONSTRAINT FK_Gene_Gene_TaxonomyID FOREIGN KEY (TaxonomyID) REFERENCES Taxonomy.Taxon (ID)
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_TaxonomyID FOREIGN KEY (TaxonomyID) REFERENCES Taxonomy.Taxon (ID)
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Gene.GeneHistory') AND c.name = 'RevisionAt') BEGIN
	ALTER TABLE Gene.GeneHistory ADD RevisionAt datetime2(7) CONSTRAINT DF_Gene_GeneHistory_RevisionAt DEFAULT (sysdatetime()) NOT NULL
END
GO
ALTER TRIGGER [Gene].[Gene_LogHistory] ON [Gene].[Gene]
AFTER INSERT, UPDATE
AS
BEGIN

	INSERT INTO Gene.GeneHistory (RevisionID, ID, SourceID,
									GenBankID, Locus, [Definition], Accession, Organism, Taxonomy,
									Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd,
									LastUpdatedAt, Active, TaxonomyID)
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
				,i.TaxonomyID
			FROM inserted i

END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('RecordSet.SubSet')) BEGIN
	CREATE TABLE RecordSet.SubSet
	(
		ID uniqueidentifier NOT NULL,
		RecordSetID uniqueidentifier NOT NULL,
		Name varchar(30) NOT NULL,
		LastOpenedAt datetime2(7) NULL,
		[Open] bit CONSTRAINT DF_RecordSet_SubSet_Open DEFAULT (1) NOT NULL,
		Active bit CONSTRAINT DF_RecordSet_SubSet_Active DEFAULT (1) NOT NULL,

		CONSTRAINT PK_RecordSet_SubSet PRIMARY KEY (ID ASC)
	)

	ALTER TABLE RecordSet.SubSet ADD CONSTRAINT FK_RecordSet_SubSet_RecordSetID FOREIGN KEY (RecordSetID) REFERENCES RecordSet.RecordSet (ID)
	CREATE UNIQUE INDEX IX_RecordSet_SubSet_Name ON RecordSet.SubSet (RecordSetID ASC, Name ASC)
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('RecordSet.SubSetGene')) BEGIN
	CREATE TABLE RecordSet.SubSetGene
	(
		SubSetID uniqueidentifier NOT NULL,
		GeneID uniqueidentifier NOT NULL,
		
		CONSTRAINT PK_RecordSet_SubSetGene PRIMARY KEY (SubSetID ASC, GeneID ASC)
	)

	ALTER TABLE RecordSet.SubSetGene ADD CONSTRAINT FK_RecordSet_SubSetGene_SubSetID FOREIGN KEY (SubSetID) REFERENCES RecordSet.SubSet (ID)
	ALTER TABLE RecordSet.SubSetGene ADD CONSTRAINT FK_RecordSet_SubSetGene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)
END
GO
IF EXISTS (SELECT * FROM RecordSet.Gene g WHERE NOT EXISTS (SELECT * FROM RecordSet.SubSetGene sg WHERE sg.GeneID = g.GeneID)) BEGIN
	DECLARE @sql nvarchar(MAX) =
	'INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name)
	SELECT newid(), s.RecordSetID, s.Name
		FROM (SELECT DISTINCT s.Name, g.RecordSetID
				FROM RecordSet.Gene g
				JOIN RecordSet.GeneStatus s ON s.ID = g.GeneStatusID
				WHERE NOT EXISTS (SELECT * FROM RecordSet.SubSet sub WHERE sub.Name = s.Name)) s'
	EXEC (@sql)

	SET @sql =
	'INSERT INTO RecordSet.SubSetGene (SubSetID, GeneID)
	SELECT sub.ID, g.GeneID
		FROM RecordSet.Gene g
		JOIN RecordSet.GeneStatus s ON s.ID = g.GeneStatusID
		JOIN RecordSet.SubSet sub ON sub.Name = s.Name AND sub.RecordSetID = g.RecordSetID
		WHERE NOT EXISTS (SELECT * FROM RecordSet.SubSetGene sg WHERE sg.GeneID = g.GeneID)'
	EXEC (@sql)
END
GO
IF EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('RecordSet.Gene') AND c.name = 'GeneStatusID') BEGIN
	ALTER TABLE RecordSet.Gene DROP CONSTRAINT PK_RecordSet_Gene
	ALTER TABLE RecordSet.Gene DROP CONSTRAINT FK_RecordSet_Gene_GeneStatusID
	ALTER TABLE RecordSet.Gene DROP COLUMN GeneStatusID

	ALTER TABLE RecordSet.Gene ADD CONSTRAINT PK_RecordSet_Gene PRIMARY KEY (RecordSetID ASC, GeneID ASC)
	--ALTER TABLE RecordSet.Gene ADD GeneStatusID int NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Taxonomy.ParseError')) BEGIN
	CREATE TABLE Taxonomy.ParseError
	(
		ID int IDENTITY(1,1) NOT NULL,
		[Message] varchar(MAX),
		Taxonomy varchar(MAX),
		ParentID hierarchyid,
		ParentValue varchar(30),
		ParentIndex int,
		NewValue varchar(30),
		NewIndex int
	)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Taxonomy.Taxon_ListTreeView')) BEGIN
	DROP PROCEDURE Taxonomy.Taxon_ListTreeView
END
GO
CREATE PROCEDURE Taxonomy.Taxon_ListTreeView
AS
WITH TaxonTree AS (
	SELECT t.ID
			,t.HID
			,t.Name
			,0 AS ParentID
			,hierarchyid::Parse('/') AS ParentHID
			,CAST('' AS varchar(30)) AS ParentName
	FROM Taxonomy.Taxon t
	WHERE t.HID.GetLevel() = 1
	UNION ALL
	SELECT t.ID
			,t.HID
			,t.Name
			,d.ID AS ParentID
			,d.HID AS ParentHID
			,d.Name AS ParentName
	FROM Taxonomy.Taxon t
	JOIN TaxonTree d ON d.HID = t.HID.GetAncestor(1)
)

SELECT t.ID
		,t.Name
		,t.HID.ToString() AS Hierarchy
		,t.ParentID
	FROM TaxonTree t
	ORDER BY t.HID
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Taxonomy.Taxon_ListTreeView_ForRecordSet')) BEGIN
	DROP PROCEDURE Taxonomy.Taxon_ListTreeView_ForRecordSet
END
GO
CREATE PROCEDURE Taxonomy.Taxon_ListTreeView_ForRecordSet
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
				,CAST('' AS varchar(30)) AS ParentName
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
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Taxonomy.Taxon_Parse')) BEGIN
	DROP PROCEDURE Taxonomy.Taxon_Parse
END
GO
CREATE PROCEDURE Taxonomy.Taxon_Parse
	@Taxonomy varchar(MAX),
	@TaxonomyID int OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Value varchar(30)
			,@Index int
			,@ParentValue varchar(30)
			,@ParentIndex int
			,@ParentID hierarchyid
			,@NewID hierarchyid
	SET @Taxonomy = REPLACE(@Taxonomy, ' ', '')

	DECLARE cTax CURSOR FOR 
	SELECT t1.Value
			,t1.[Index]
			,t2.Value AS ParentValue
			,t2.[Index] AS ParentIndex
		FROM Common.SplitString(@taxonomy, ';') t1
		LEFT OUTER JOIN Common.SplitString(@taxonomy, ';') t2 ON t2.[Index] = t1.[Index] - 1

	OPEN cTax
	FETCH NEXT FROM cTax INTO @Value, @Index, @ParentValue, @ParentIndex
	WHILE @@FETCH_STATUS = 0 BEGIN
		BEGIN TRY
			IF NOT EXISTS (SELECT *
								FROM Taxonomy.Taxon t
								WHERE t.Name = @Value
								AND t.HID.GetLevel() = @Index) BEGIN
				SET @NewID = NULL

				SELECT @ParentID = t.HID
					FROM Taxonomy.Taxon t
					WHERE t.Name = @ParentValue
						AND t.HID.GetLevel() = @ParentIndex

				IF (@ParentID IS NULL AND @ParentIndex = 1) BEGIN
					INSERT INTO Taxonomy.Taxon (HID, Name)
					VALUES ('/1/', @ParentValue)

					SELECT @ParentID = t.HID
						FROM Taxonomy.Taxon t
						WHERE t.Name = @ParentValue
							AND t.HID.GetLevel() = @ParentIndex
				END

				IF (@ParentID IS NULL) BEGIN
					-- This would be odd because we're inserting into Taxonomy.Taxon from the top-down
					DECLARE @ErrorMessage nvarchar(MAX) = 'Parent taxon [' + @ParentValue + '] not found.';
					RAISERROR (@ErrorMessage, 16, 1);
				END
				ELSE BEGIN
					SELECT @NewID = @ParentID.GetDescendant(t.HID, NULL)
						FROM Taxonomy.Taxon t
						WHERE t.HID = (SELECT MAX(HID) FROM Taxonomy.Taxon t 
											WHERE t.HID.GetLevel() = @Index
											AND t.HID.GetAncestor(1) = @ParentID)

					IF (@NewID IS NULL) BEGIN
						SET @NewID = @ParentID.GetDescendant(NULL, NULL)
					END

					INSERT INTO Taxonomy.Taxon (HID, Name)
					VALUES (@NewID, @Value)
				END
			END
		END TRY
		BEGIN CATCH
			SELECT @ErrorMessage = ERROR_MESSAGE()

			INSERT INTO Taxonomy.ParseError ([Message], Taxonomy, ParentID, ParentValue, ParentIndex, NewValue, NewIndex)
			VALUES (@ErrorMessage, @Taxonomy, @ParentID, @ParentValue, @ParentIndex, @Value, @Index)
		END CATCH

		FETCH NEXT FROM cTax INTO @Value, @Index, @ParentValue, @ParentIndex
	END
	CLOSE cTax
	DEALLOCATE cTax

	SELECT @TaxonomyID = t.ID
		FROM Taxonomy.Taxon t
		WHERE t.HID = (SELECT MAX(t.HID)
						FROM Taxonomy.Taxon t
						JOIN Common.SplitString(@Taxonomy, ';') t1 ON t1.Value = t.Name AND t1.[Index] = t.HID.GetLevel())
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
			,@OriginalTaxonomyID int
			,@OriginalTaxonomy varchar(MAX)
			,@TaxonomyID int = NULL

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
								,GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID
								,Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd
								,LastUpdatedAt)
		VALUES (@ID, @Definition, @SourceID
				,@GenBankID, @Locus, @Accession, @Organism, @Taxonomy, @TaxonomyID
				,@Nucleotides, @SequenceTypeID, @SequenceStart, @SequenceEnd, @CodingSequenceStart, @CodingSequenceEnd
				,@LastUpdatedAt)
	END
	ELSE BEGIN
		IF (@Overwrite = 1) BEGIN
			SELECT @OriginalTaxonomyID = g.TaxonomyID
					,@OriginalTaxonomy = g.Taxonomy
				FROM Gene.Gene g WHERE g.ID = @ID

			IF (@Taxonomy IS NOT NULL) AND (LTRIM(RTRIM(@Taxonomy)) <> '') AND (@OriginalTaxonomy <> @Taxonomy OR @OriginalTaxonomyID IS NULL) BEGIN
				EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT
			END

			UPDATE Gene.Gene
				SET SourceID = ISNULL(@SourceID, SourceID)
					,[Definition] = ISNULL(@Definition, [Definition])
					,Locus = ISNULL(@Locus, Locus)
					,Accession = ISNULL(@Accession, Accession)
					,Organism = ISNULL(@Organism, Organism)
					,Taxonomy = ISNULL(@Taxonomy, Taxonomy)
					,TaxonomyID = ISNULL(@TaxonomyID, TaxonomyID)
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
ALTER PROCEDURE [Job].[BlastN_ListAlignmentsForJob]
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @RecordSetID uniqueidentifier
	SELECT @RecordSetID = j.RecordSetID
		FROM Job.Job j
		WHERE j.ID = @JobID

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
			AND NOT EXISTS (SELECT *
								FROM RecordSet.Gene rs_g
								JOIN Gene.Gene g ON g.ID = rs_g.GeneID
								WHERE rs_g.RecordSetID = @RecordSetID
									AND g.GenBankID = sbj.GenBankID)
		GROUP BY sbj.ID, sbj.SourceID, sbj.GenBankID, sbj.[Definition], sbj.SequenceStart, sbj.SequenceEnd
		ORDER BY AlignmentPercentage DESC, [Rank]
END
GO

BEGIN TRANSACTION
GO
IF EXISTS (SELECT * FROM Gene.Gene g WHERE g.Taxonomy IS NOT NULL AND g.TaxonomyID IS NULL) BEGIN
	DECLARE @GeneID uniqueidentifier
			,@Taxonomy varchar(MAX)
			,@TaxonomyID int

	DECLARE cGene CURSOR FOR SELECT g.ID, g.Taxonomy FROM Gene.Gene g WHERE g.Taxonomy IS NOT NULL AND g.TaxonomyID IS NULL
	OPEN cGene
	FETCH NEXT FROM cGene INTO @GeneID, @Taxonomy
	WHILE @@FETCH_STATUS = 0 BEGIN
		EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT

		UPDATE Gene.Gene
			SET TaxonomyID = @TaxonomyID
			WHERE ID = @GeneID 

		FETCH NEXT FROM cGene INTO @GeneID, @Taxonomy
	END
	CLOSE cGene
	DEALLOCATE cGene
END
GO
/*
SELECT g.ID
		,g.GenBankID
		,g.[Definition]
		,g.Organism
		,g.Taxonomy
		,t.HID.ToString()
		,t.Name AS LastTaxon
	FROM Gene.Gene g
	JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
*/
/*
SELECT t.ID, t.HID.ToString(), t.Name
	FROM Taxonomy.Taxon t
	WHERE t.HID = hierarchyid::Parse('/1/1/1/1/1/1/1/')
		OR t.HID.GetAncestor(1) = hierarchyid::Parse('/1/1/1/1/1/1/1/')

SELECT t.ID, t.HID.ToString(), t.Name
	FROM Taxonomy.Taxon t
	WHERE t.HID.GetLevel() = 8

SELECT REPLACE(e.[Message], 'A .NET Framework error occurred during execution of user-defined routine or aggregate "hierarchyid": ', '') AS [Message]
		,e.Taxonomy
		,e.ParentID.ToString()
		,e.ParentValue
		,e.ParentIndex
		,e.NewValue
		,e.NewIndex
	FROM Taxonomy.ParseError e
	ORDER BY e.ParentID
*/
GO
COMMIT TRANSACTION
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.SubSet_Edit')) BEGIN
	DROP PROCEDURE RecordSet.SubSet_Edit
END
GO
CREATE PROCEDURE RecordSet.SubSet_Edit
	@ID uniqueidentifier = NULL OUTPUT,
	@RecordSetID uniqueidentifier = NULL,
	@Name varchar(200),
	@Active bit = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		SET @ID = NEWID()
	END

	IF NOT EXISTS (SELECT * FROM RecordSet.SubSet WHERE ID = @ID) BEGIN
		INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name)
		VALUES (@ID, @RecordSetID, @Name)
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
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.SubSet_Toggle')) BEGIN
	DROP PROCEDURE RecordSet.SubSet_Toggle
END
GO
CREATE PROCEDURE RecordSet.SubSet_Toggle
	@ID uniqueidentifier,
	@Open bit = NULL,
	@Active bit = NULL
AS
BEGIN
	SET NOCOUNT ON

	UPDATE RecordSet.SubSet
		SET [Open] = ISNULL(@Open, [Open]),
			Active = ISNULL(@Active, Active)
		WHERE ID = @ID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.SubSet_Opened')) BEGIN
	DROP PROCEDURE RecordSet.SubSet_Opened
END
GO
CREATE PROCEDURE RecordSet.SubSet_Opened
	@ID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	UPDATE RecordSet.SubSet
		SET LastOpenedAt = SYSDATETIME()
		WHERE ID = @ID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.SubSet_List')) BEGIN
	DROP PROCEDURE RecordSet.SubSet_List
END
GO
CREATE PROCEDURE RecordSet.SubSet_List
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT sub.ID
			,sub.Name
			,sub.LastOpenedAt
			,sub.[Open]
			,(SELECT COUNT(*)
					FROM RecordSet.SubSetGene sg
					WHERE sg.SubSetID = sub.ID) AS GeneCount
		FROM RecordSet.SubSet sub
		WHERE sub.RecordSetID = @RecordSetID
			AND sub.Active = 1
		ORDER BY sub.LastOpenedAt, sub.Name
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.SubSetGene_Edit')) BEGIN
	DROP PROCEDURE RecordSet.SubSetGene_Edit
END
GO
CREATE PROCEDURE RecordSet.SubSetGene_Edit
	@SubSetID uniqueidentifier,
	@GeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM RecordSet.SubSetGene WHERE SubSetID = @SubSetID AND GeneID = @GeneID) BEGIN
		INSERT INTO RecordSet.SubSetGene (SubSetID, GeneID)
		VALUES (@SubSetID, @GeneID)
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.SubSetGene_List')) BEGIN
	DROP PROCEDURE RecordSet.SubSetGene_List
END
GO
CREATE PROCEDURE RecordSet.SubSetGene_List
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.ID
			,g.[Definition]
			,g.SourceID
			,g.LastUpdatedAt
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

			,CASE WHEN (t.ID IS NOT NULL)
				  THEN t.HID.ToString()
				  ELSE ''
				  END AS TaxonomyHierarchy

		FROM RecordSet.SubSetGene sg
		JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
		JOIN RecordSet.Gene rs_g ON rs_g.GeneID = sg.GeneID AND rs_g.RecordSetID = sub.RecordSetID
		JOIN Gene.Gene g ON g.ID = rs_g.GeneID
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		
		WHERE sg.SubSetID = @SubSetID
			AND g.Active = 1
		ORDER BY rs_g.ModifiedAt DESC, g.Organism, g.Accession, g.GenBankID
END
GO
ALTER PROCEDURE [RecordSet].[Gene_Edit]
	@RecordSetID uniqueidentifier,
	@GeneID uniqueidentifier,
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM RecordSet.Gene WHERE RecordSetID = @RecordSetID AND GeneID = @GeneID) BEGIN
		INSERT INTO RecordSet.Gene (RecordSetID, GeneID)
		VALUES (@RecordSetID, @GeneID)
	END
	IF NOT EXISTS (SELECT * FROM RecordSet.SubSetGene WHERE GeneID = @GeneID AND SubSetID = @SubSetID) BEGIN
		INSERT INTO RecordSet.SubSetGene (SubSetID, GeneID)
		VALUES (@SubSetID, @GeneID)
	END
END
GO
ALTER PROCEDURE [RecordSet].[Gene_Delete]
	@RecordSetID uniqueidentifier,
	@GeneID uniqueidentifier,
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM RecordSet.SubSetGene
		WHERE SubSetID = @SubSetID
			AND GeneID = @GeneID

	IF NOT EXISTS (SELECT * 
						FROM RecordSet.SubSetGene sg
						JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
						WHERE sub.RecordSetID = @RecordSetID
							AND sg.GeneID = @GeneID) BEGIN
		-- The gene is considered orphaned if it is not associated with any subsets.
		DELETE FROM RecordSet.Gene
			WHERE RecordSetID = @RecordSetID
				AND GeneID = @GeneID
	END
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
ALTER PROCEDURE [RecordSet].[RecordSet_List]
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
				WHERE g.RecordSetID = rs.ID) AS GeneCountFinal
		FROM RecordSet.RecordSet rs
		WHERE (@Active IS NULL OR rs.Active = @Active)
		ORDER BY rs.LastOpenedAt DESC, rs.ModifiedAt DESC
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

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('RecordSet.ApplicationProperty')) BEGIN
	CREATE TABLE RecordSet.ApplicationProperty
	(
		ID uniqueidentifier NOT NULL,
		RecordSetID uniqueidentifier NOT NULL,
		[Key] varchar(30) NOT NULL,
		Value varchar(MAX) NULL,

		CONSTRAINT PK_RecordSet_ApplicationProperty PRIMARY KEY (ID ASC)
	)

	CREATE UNIQUE INDEX IX_RecordSet_ApplicationProperty_Key ON RecordSet.ApplicationProperty (RecordSetID ASC, [Key] ASC)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.ApplicationProperty_Edit')) BEGIN
	DROP PROCEDURE RecordSet.ApplicationProperty_Edit
END
GO
CREATE PROCEDURE RecordSet.ApplicationProperty_Edit
	@RecordSetID uniqueidentifier
	,@Key varchar(30)
	,@Value varchar(MAX)
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM RecordSet.ApplicationProperty ap WHERE ap.RecordSetID = @RecordSetID AND ap.[Key] = @Key) BEGIN
		INSERT INTO RecordSet.ApplicationProperty (ID, RecordSetID, [Key], Value)
		VALUES (newid(), @RecordSetID, @Key, @Value)
	END
	ELSE BEGIN
		UPDATE RecordSet.ApplicationProperty
			SET Value = @Value
			WHERE RecordSetID = @RecordSetID
				AND [Key] = @Key
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.ApplicationProperty_List')) BEGIN
	DROP PROCEDURE RecordSet.ApplicationProperty_List
END
GO
CREATE PROCEDURE RecordSet.ApplicationProperty_List
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT ap.ID
			,ap.[Key]
			,ap.Value
		FROM RecordSet.ApplicationProperty ap
		WHERE ap.RecordSetID = @RecordSetID
END
GO

UPDATE Common.ApplicationProperty
	SET Value = '1.1.0.0'
	WHERE [Key] = 'DatabaseVersion'
GO