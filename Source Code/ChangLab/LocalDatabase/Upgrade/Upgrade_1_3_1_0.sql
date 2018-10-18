SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.DefaultConstraintValue')) BEGIN
	DROP FUNCTION Common.DefaultConstraintValue
END
GO
CREATE FUNCTION Common.DefaultConstraintValue (@TableFullName varchar(250), @ColumnName varchar(100))
RETURNS sql_variant
AS
BEGIN
	DECLARE @value sql_variant
			,@constraint nvarchar(MAX)
			,@typename varchar(100)

	SELECT @constraint = REPLACE(REPLACE(df.definition, '(', ''), ')', '')
			,@typename = typ.name
		FROM sys.default_constraints df
		JOIN sys.columns col ON col.object_id = df.parent_object_id AND col.column_id = df.parent_column_id
		JOIN sys.types typ ON typ.system_type_id = col.system_type_id
		WHERE df.parent_object_id = OBJECT_ID(@TableFullName)
			AND col.name = @ColumnName

	-- For whatever reason a CASE statement didn't work here; it was trying all of the conversions instead of just the one corresponding to @typename
	IF (@typename = 'bit') BEGIN
		SET @value = CAST(@constraint AS bit)
	END
	ELSE IF (@typename = 'int') BEGIN
		SET @value = CAST(@constraint AS int)
	END
	ELSE IF (@typename = 'char') BEGIN
		SET @value = CAST(REPLACE(@constraint, CHAR(39), '') AS char(1))
	END

	RETURN @value
END
GO

-- CR-008
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.FeatureKey')) BEGIN
	CREATE TABLE Gene.FeatureKey (
		ID int IDENTITY(1,1) NOT NULL,
		Name varchar(10) NOT NULL,
		[Key] varchar(10) NOT NULL,
		[Rank] int NOT NULL

		CONSTRAINT PK_Gene_FeatureKey PRIMARY KEY CLUSTERED (ID ASC)
	)

	INSERT INTO Gene.FeatureKey (Name, [Key], [Rank])
	VALUES ('Gene', 'gene', 4), ('mRNA', 'mRNA', 3), ('Exon', 'exon', 2), ('CDS', 'CDS', 1),
			-- We'd not want to use these to form a sequence
			('STS', 'STS', 10), ('Source', 'source', 100), ('Misc RNA', 'misc_RNA', 11)

	--INSERT INTO Gene.FeatureKey (Name, [Key], [Rank])
	--VALUES 
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Gene.FeatureKey_List')) BEGIN
	DROP PROCEDURE Gene.FeatureKey_List
END
GO
CREATE PROCEDURE Gene.FeatureKey_List
AS
BEGIN
	SET NOCOUNT ON

	SELECT fk.ID
			,fk.Name
			,fk.[Key]
			,fk.[Rank]
		FROM Gene.FeatureKey fk
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.Feature')) BEGIN
	CREATE TABLE Gene.Feature (
		GeneID uniqueidentifier NOT NULL,
		ID int NOT NULL, -- This is purposefully not an identity, so that it's sequential from 1 per Gene
		FeatureKeyID int NOT NULL,
		Start int NOT NULL,
		[End] int NOT NULL,
		IsComplement bit NOT NULL CONSTRAINT DF_Gene_Feature_IsComplement DEFAULT (0),

		StartModifier char(1) NOT NULL CONSTRAINT DF_Gene_Feature_StartModifier DEFAULT ('='),
		EndModifier char(1) NOT NULL CONSTRAINT DF_Gene_Feature_EndModifier DEFAULT ('='),
		
		CONSTRAINT PK_Gene_Feature PRIMARY KEY CLUSTERED (GeneID ASC, ID ASC)
	)
	ALTER TABLE Gene.Feature ADD CONSTRAINT FK_Gene_Feature_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)
	ALTER TABLE Gene.Feature ADD CONSTRAINT FK_Gene_Feature_FeatureKeyID FOREIGN KEY (FeatureKeyID) REFERENCES Gene.FeatureKey (ID)
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Gene.Feature_Add')) BEGIN
	DROP PROCEDURE Gene.Feature_Add
END
GO
CREATE PROCEDURE Gene.Feature_Add
	@GeneID uniqueidentifier
	,@ID int
	,@FeatureKeyID int
	,@Start int
	,@End int
	,@IsComplement bit = NULL
	,@StartModifier char(1) = NULL
	,@EndModifier char(1) = NULL
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Gene.Feature (GeneID, ID, FeatureKeyID, Start, [End], IsComplement, StartModifier, EndModifier)
	VALUES (@GeneID, @ID, @FeatureKeyID, @Start, @End
			,ISNULL(@IsComplement, CONVERT(bit, Common.DefaultConstraintValue('Gene.Feature', 'IsComplement')))
			,ISNULL(@StartModifier, CONVERT(char(1), Common.DefaultConstraintValue('Gene.Feature', 'StartModifier')))
			,ISNULL(@StartModifier, CONVERT(char(1), Common.DefaultConstraintValue('Gene.Feature', 'EndModifier'))))
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Gene.Feature_Delete')) BEGIN
	DROP PROCEDURE Gene.Feature_Delete
END
GO
CREATE PROCEDURE Gene.Feature_Delete
	@GeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM Gene.Feature
		WHERE GeneID = @GeneID
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.NucleotideSequence')) BEGIN
	CREATE TABLE Gene.NucleotideSequence (
		GeneID uniqueidentifier NOT NULL,
		Nucleotides varchar(MAX) NOT NULL,
		Start int NOT NULL,
		[End] int NOT NULL,

		CONSTRAINT PK_Gene_NucleotideSequence PRIMARY KEY CLUSTERED (GeneID ASC)
	)
	ALTER TABLE Gene.NucleotideSequence ADD CONSTRAINT FK_Gene_NucleotideSequence_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)

	INSERT INTO Gene.NucleotideSequence (GeneID, Nucleotides, Start, [End])
	SELECT g.ID, g.Nucleotides, ISNULL(g.CodingSequenceStart, g.SequenceStart), ISNULL(g.CodingSequenceEnd, g.SequenceEnd)
		FROM Gene.Gene g
		WHERE g.Nucleotides IS NOT NULL

	DECLARE @key_CDS int, @key_Gene int
	SELECT @key_CDS = ID FROM Gene.FeatureKey k WHERE k.[Key] = 'CDS'
	SELECT @key_Gene = ID FROM Gene.FeatureKey k WHERE k.[Key] = 'gene'

	INSERT INTO Gene.Feature (GeneID, ID, FeatureKeyID, Start, [End])
	SELECT g.ID
			,1
			,@key_Gene
			,g.SequenceStart, g.SequenceEnd
		FROM Gene.Gene g
		WHERE g.Nucleotides IS NOT NULL
			AND g.SequenceStart IS NOT NULL

	INSERT INTO Gene.Feature (GeneID, ID, FeatureKeyID, Start, [End])
	SELECT g.ID
			,(CASE WHEN EXISTS (SELECT * FROM Gene.Feature ex WHERE ex.GeneID = g.ID) THEN 2 ELSE 1 END)
			,@key_CDS
			,g.CodingSequenceStart, g.CodingSequenceEnd
		FROM Gene.Gene g
		WHERE g.Nucleotides IS NOT NULL
			AND g.CodingSequenceStart IS NOT NULL

	--ALTER TABLE Gene.Gene DROP COLUMN SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Gene.NucleotideSequence_Edit')) BEGIN
	DROP PROCEDURE Gene.NucleotideSequence_Edit
END
GO
CREATE PROCEDURE Gene.NucleotideSequence_Edit
	@GeneID uniqueidentifier
	,@Nucleotides varchar(MAX)
	,@Start int
	,@End int
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM Gene.NucleotideSequence seq WHERE seq.GeneID = @GeneID) BEGIN
		INSERT INTO Gene.NucleotideSequence (GeneID, Nucleotides, Start, [End])
		VALUES (@GeneID, @Nucleotides, @Start, @End)
	END
	ELSE BEGIN
		UPDATE Gene.NucleotideSequence
			SET Nucleotides = @Nucleotides
				,Start = @Start
				,[End] = @End
			WHERE GeneID = @GeneID
	END
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Gene.BuildSequence')) BEGIN
	DROP FUNCTION Gene.BuildSequence
END
GO
CREATE FUNCTION Gene.BuildSequence (@GeneID uniqueidentifier)
RETURNS varchar(MAX)
AS
BEGIN

	-- NOTE: This function is not finished.  IsComplement isn't being handled, but more importantly there's the issue with indexing nucleotides 
	-- such that the source start might not be set to 1.  ChangLab.Genes.NucleotideSequence currently handles that, and a number table could
	-- potentially do so in SQL server as well if not embedding a CLR assembly.
	
	DECLARE @seq varchar(MAX) = (SELECT g.Nucleotides FROM Gene.Gene g WHERE g.ID = @GeneID)
			,@Sequence varchar(MAX)= ''

	IF (@seq IS NOT NULL) BEGIN
		DECLARE @Features TABLE (Start int, [End] int, IsComplement bit)
		INSERT INTO @Features
		SELECT f.Start, f.[End], f.IsComplement
			FROM (SELECT f.ID
						,f.Start, f.[End]
						,f.IsComplement
						,DENSE_RANK() OVER (ORDER BY k.[Rank]) AS [Rank]
					FROM Gene.Feature f
					JOIN Gene.FeatureKey k ON k.ID = f.FeatureKeyID
					WHERE f.GeneID = @GeneID) f
			WHERE f.[Rank] = 1
			ORDER BY f.Start

		SELECT @Sequence += SUBSTRING(@seq, f.Start, f.[End])
			FROM @Features f
	END

	RETURN @Sequence

END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Gene.Gene_GetSequenceData')) BEGIN
	DROP PROCEDURE Gene.Gene_GetSequenceData
END
GO
CREATE PROCEDURE Gene.Gene_GetSequenceData
	@GeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT f.ID
			,f.FeatureKeyID
			,f.Start
			,f.StartModifier
			,f.[End]
			,f.EndModifier
			,f.IsComplement
		FROM Gene.Feature f
		JOIN Gene.FeatureKey k ON k.ID = f.FeatureKeyID
		WHERE f.GeneID = @GeneID
		ORDER BY k.[Rank], f.ID

	SELECT seq.Nucleotides
			,seq.Start
			,seq.[End]
		FROM Gene.NucleotideSequence seq
		WHERE seq.GeneID = @GeneID
END
GO

-- Performance tuning
GO
IF NOT EXISTS (SELECT * FROM sys.indexes i WHERE i.object_id = OBJECT_ID('BlastN.AlignmentExon') AND i.name = 'IX_BlastN_AlignmentExon_AlignmentID') BEGIN
	CREATE NONCLUSTERED INDEX IX_BlastN_AlignmentExon_AlignmentID ON BlastN.AlignmentExon (AlignmentID ASC)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes i WHERE i.object_id = OBJECT_ID('BlastN.Alignment') AND i.name = 'IX_BlastN_Alignment_QueryID') BEGIN
	CREATE NONCLUSTERED INDEX IX_BlastN_Alignment_QueryID ON BlastN.Alignment (QueryID ASC)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes i WHERE i.object_id = OBJECT_ID('BlastN.Alignment') AND i.name = 'IX_BlastN_Alignment_SubjectID') BEGIN
	CREATE NONCLUSTERED INDEX IX_BlastN_Alignment_SubjectID ON BlastN.Alignment (SubjectID ASC)
END
GO

GO
-- Fix for Filter SubSet interface error on null Organism name
GO
ALTER PROCEDURE [Gene].[Gene_ListAllOrganismNames]
	@RecordSetID uniqueidentifier = NULL
	,@SubSetID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON

	-- This is a little clunky (on account of being repetitive), but saves us doing OUTER JOINs.
	IF @SubSetID IS NOT NULL BEGIN
		SELECT DISTINCT g.Organism
			FROM RecordSet.SubSetGene sg
			JOIN Gene.Gene g ON g.ID = sg.GeneID
			WHERE sg.SubSetID = @SubSetID
				AND g.Active = 1
				AND g.Organism IS NOT NULL
			ORDER BY Organism
	END
	ELSE IF @RecordSetID IS NOT NULL BEGIN
		SELECT DISTINCT g.Organism
			FROM RecordSet.Gene rg
			JOIN Gene.Gene g ON g.ID = rg.GeneID
			WHERE rg.RecordSetID = @RecordSetID
				AND g.Active = 1
				AND g.Organism IS NOT NULL
			ORDER BY Organism
	END
	ELSE BEGIN
		SELECT DISTINCT g.Organism
			FROM Gene.Gene g
			WHERE g.Active = 1
				AND g.Organism IS NOT NULL
			ORDER BY Organism
	END
END
GO

-- Fix for >100% query cover
GO
ALTER PROCEDURE [Job].[BlastN_ListQueryGenesForAlignment]
	@SubjectGeneID uniqueidentifier,
	@JobID uniqueidentifier,
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF @RecordSetID IS NULL AND @JobID IS NULL BEGIN
		RAISERROR('A Job ID or a RecordSet ID must be provided', 11, 1)
	END
		
	IF @RecordSetID IS NULL BEGIN
		SELECT @RecordSetID = j.RecordSetID
			FROM Job.Job j
			WHERE j.ID = @JobID
	END

	IF @SubjectGeneID IS NOT NULL BEGIN
		SELECT qry.ID
				,qsrc.Name AS QuerySourceName
				,qry.GenBankID
				,qry.[Definition]
				,qry.Organism
				,qry.Taxonomy
				,al.[Rank]
				,ex.AlignmentLength
				,ex.BitScore
				,Common.MinimumMaximumFloat(Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength), 1.0) AS AlignmentPercentage
				,Common.MinimumMaximumFloat(BlastN.Alignment_QueryCover(al.ID), 1.0) AS QueryCover
				,ex.Gaps
			FROM NCBI.Request r
			JOIN NCBI.BlastNAlignment nal ON nal.RequestID = r.ID
			JOIN BlastN.Alignment al ON al.ID = nal.AlignmentID
			JOIN BlastN.AlignmentExon ex ON ex.ID = BlastN.AlignmentExon_First(al.ID)
			JOIN Gene.Gene qry ON qry.ID = al.QueryID
			JOIN Gene.[Source] qsrc ON qsrc.ID = qry.SourceID
			JOIN Job.Job j ON j.ID = r.JobID
			WHERE al.SubjectID = @SubjectGeneID
				AND j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
			ORDER BY AlignmentPercentage DESC, BitScore DESC
	END
	ELSE BEGIN
		SELECT DISTINCT
				qry.ID
				,qry.GenBankID
				,qry.[Definition]
				,qry.Organism
				,qry.CodingSequenceStart
				,qry.CodingSequenceEnd
			FROM NCBI.Request r
			JOIN NCBI.Gene ng ON ng.RequestID = r.ID
			JOIN Gene.Gene qry ON qry.ID = ng.GeneID
			JOIN Job.Job j ON j.ID = r.JobID
			WHERE j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
				AND ng.DirectionID = 1
			ORDER BY qry.Organism, qry.[Definition]
	END
END
GO

-- Default subsets
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

		INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name)
		VALUES (NEWID(), @ID, 'All'), (NEWID(), @ID, 'Excluded'), (NEWID(), @ID, 'Duplicates')
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

IF NOT EXISTS (SELECT * FROM Job.[Status] WHERE [Key] = 'Failed') BEGIN
	INSERT INTO Job.[Status] (Name, [Key]) VALUES ('Failed', 'Failed')
END
GO

UPDATE Common.ApplicationProperty
	SET Value = '1.3.1.0'
	WHERE [Key] = 'DatabaseVersion'
GO