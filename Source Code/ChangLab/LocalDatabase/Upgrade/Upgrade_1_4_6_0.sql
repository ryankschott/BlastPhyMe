BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
GO
-- More of steadily chipping away at the use of uniqueidentifier...
IF ((SELECT t.name FROM sys.columns c
		JOIN sys.types t ON t.system_type_id = c.system_type_id
		WHERE c.object_id = OBJECT_ID('RecordSet.ApplicationProperty')
			AND c.name = 'ID') = 'uniqueidentifier') BEGIN
	CREATE TABLE RecordSet.Tmp_ApplicationProperty (
		ID int NOT NULL IDENTITY (1, 1),
		RecordSetID uniqueidentifier NOT NULL,
		[Key] varchar(30) NOT NULL,
		Value varchar(MAX) NULL
	)

	IF EXISTS (SELECT * FROM RecordSet.ApplicationProperty) BEGIN
		 EXEC('INSERT INTO RecordSet.Tmp_ApplicationProperty (RecordSetID, [Key], Value)
			SELECT RecordSetID, [Key], Value FROM RecordSet.ApplicationProperty WITH (HOLDLOCK TABLOCKX)')
	END

	EXEC ('DROP TABLE RecordSet.ApplicationProperty')
	EXECUTE sp_rename N'RecordSet.Tmp_ApplicationProperty', N'ApplicationProperty', 'OBJECT' 

	ALTER TABLE RecordSet.ApplicationProperty ADD CONSTRAINT PK_RecordSet_ApplicationProperty PRIMARY KEY CLUSTERED  (ID ASC)
	CREATE UNIQUE NONCLUSTERED INDEX IX_RecordSet_ApplicationProperty_Key ON RecordSet.ApplicationProperty (RecordSetID, [Key])
END
GO
IF ((SELECT t.name FROM sys.columns c
		JOIN sys.types t ON t.system_type_id = c.system_type_id
		WHERE c.object_id = OBJECT_ID('Common.ApplicationProperty')
			AND c.name = 'ID') = 'uniqueidentifier') BEGIN
	CREATE TABLE Common.Tmp_ApplicationProperty (
		ID int NOT NULL IDENTITY (1, 1),
		[Key] varchar(30) NOT NULL,
		Value varchar(MAX) NULL
	)

	IF EXISTS (SELECT * FROM Common.ApplicationProperty) BEGIN
		 EXEC('INSERT INTO Common.Tmp_ApplicationProperty ([Key], Value)
			SELECT [Key], Value FROM Common.ApplicationProperty WITH (HOLDLOCK TABLOCKX)')
	END
	EXEC ('DROP TABLE Common.ApplicationProperty')
	EXECUTE sp_rename N'Common.Tmp_ApplicationProperty', N'ApplicationProperty', 'OBJECT' 

	ALTER TABLE Common.ApplicationProperty ADD CONSTRAINT PK_Common_ApplicationProperty PRIMARY KEY CLUSTERED (ID ASC)
	CREATE UNIQUE NONCLUSTERED INDEX IX_Common_ApplicationProperty_Key ON Common.ApplicationProperty ([Key])
END
GO
COMMIT TRANSACTION
GO
ALTER PROCEDURE [Common].[ApplicationProperty_Edit]
	@Key varchar(30)
	,@Value varchar(MAX)
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM Common.ApplicationProperty ap WHERE ap.[Key] = @Key) BEGIN
		INSERT INTO Common.ApplicationProperty ([Key], Value)
		VALUES (@Key, @Value)
	END
	ELSE BEGIN
		UPDATE Common.ApplicationProperty
			SET Value = @Value
			WHERE [Key] = @Key
	END
END
GO
ALTER PROCEDURE [RecordSet].[ApplicationProperty_Edit]
	@RecordSetID uniqueidentifier
	,@Key varchar(30)
	,@Value varchar(MAX)
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM RecordSet.ApplicationProperty ap WHERE ap.RecordSetID = @RecordSetID AND ap.[Key] = @Key) BEGIN
		INSERT INTO RecordSet.ApplicationProperty (RecordSetID, [Key], Value)
		VALUES (@RecordSetID, @Key, @Value)
	END
	ELSE BEGIN
		UPDATE RecordSet.ApplicationProperty
			SET Value = @Value
			WHERE RecordSetID = @RecordSetID
				AND [Key] = @Key
	END
END
GO

-- Trying to come up with an improved IO for Pilgrimage data files

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.types t WHERE t.is_user_defined = 1 AND t.name = 'HashtableUniqueIdentifier') BEGIN
	CREATE TYPE Common.HashtableUniqueIdentifier AS TABLE (
		[Key] uniqueidentifier NOT NULL,
		Value uniqueidentifier NOT NULL
	)
END
GO
IF NOT EXISTS (SELECT * FROM sys.types t WHERE t.is_user_defined = 1 AND t.name = 'HashtableInt') BEGIN
	CREATE TYPE Common.HashtableInt AS TABLE (
		[Key] int NOT NULL,
		Value int NOT NULL
	)
END
GO
ALTER TRIGGER [Gene].[Gene_LogHistory] ON [Gene].[Gene]
	AFTER INSERT, UPDATE
AS
BEGIN

	DECLARE @IsInsert bit = 0
			,@OriginalTaxonomyID int
			,@OriginalTaxonomy varchar(MAX)
			,@Taxonomy varchar(MAX)
			,@TaxonomyID int = NULL
			,@GeneID uniqueidentifier

	SELECT @IsInsert = CAST((CASE WHEN d.ID IS NULL THEN 1 ELSE 0 END) AS bit)
			,@OriginalTaxonomyID = d.TaxonomyID
			,@OriginalTaxonomy = d.Taxonomy
		FROM deleted d
	SELECT  @GeneID = i.ID
			,@Taxonomy = i.Taxonomy
			,@TaxonomyID = i.TaxonomyID
		FROM inserted i

	IF (@IsInsert = 1) BEGIN -- INSERT
		IF (@Taxonomy IS NOT NULL) AND (LTRIM(RTRIM(@Taxonomy)) <> '') BEGIN
			EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT
			UPDATE Gene.Gene SET TaxonomyID = @TaxonomyID WHERE ID = @GeneID
		END
	END
	ELSE BEGIN -- UPDATE
		IF (@Taxonomy IS NOT NULL) AND (LTRIM(RTRIM(@Taxonomy)) <> '') AND (@OriginalTaxonomy <> @Taxonomy OR @OriginalTaxonomyID IS NULL) BEGIN
			EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT
			UPDATE Gene.Gene SET TaxonomyID = @TaxonomyID WHERE ID = @GeneID
		END
		ELSE IF (@OriginalTaxonomy = @Taxonomy) BEGIN
			SET @TaxonomyID = @OriginalTaxonomyID
		END
	END

	INSERT INTO Gene.GeneHistory (RevisionID, ID, Name, [Definition], SourceID,
									GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID,
									Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd,
									[Description], Active, LastUpdatedAt, LastUpdateSourceID)
		SELECT ISNULL((SELECT MAX(RevisionID) FROM Gene.GeneHistory g WHERE g.ID = i.ID), 0) + 1
				,i.ID
				,i.Name
				,i.[Definition]
				,i.SourceID
				,i.GenBankID
				,i.Locus
				,i.Accession
				,i.Organism
				,i.Taxonomy
				,@TaxonomyID
				,i.Nucleotides
				,i.SequenceTypeID
				,i.SequenceStart
				,i.SequenceEnd
				,i.CodingSequenceStart
				,i.CodingSequenceEnd
				,i.[Description]
				,i.Active
				,i.LastUpdatedAt
				,i.LastUpdateSourceID
			FROM inserted i

END
GO
ALTER PROCEDURE [Gene].[Gene_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@Name varchar(100),
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
	@LastUpdatedAt datetime2(7) = NULL,
	@LastUpdateSourceID int = NULL,
	@AllowOverwrite bit = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @NowTime datetime2(7) = SYSDATETIME()

	IF @LastUpdateSourceID IS NULL BEGIN
		SET @LastUpdateSourceID = @SourceID
	END

	IF NOT EXISTS (SELECT * FROM Gene.Gene g WHERE g.ID = @ID) BEGIN
		IF (@ID IS NULL) BEGIN
			SET @ID = NEWID()
		END

		INSERT INTO Gene.Gene (ID, Name, [Definition], SourceID
								,GenBankID, Locus, Accession, Organism, Taxonomy, [Description]
								,Nucleotides, SequenceTypeID, LastUpdatedAt, LastUpdateSourceID)
		VALUES (@ID, @Name, @Definition, @SourceID
				,@GenBankID, @Locus, @Accession, @Organism, @Taxonomy, @Description
				,@Nucleotides, @SequenceTypeID
				,@LastUpdatedAt, @LastUpdateSourceID)
	END
	ELSE BEGIN
		IF (@AllowOverwrite = 1) BEGIN
			-- Currently not allowing for partial updates (i.e.: ISNULL(@column, column)); whenever a gene is saved and an update is possible all
			-- values must be provided.
			UPDATE Gene.Gene
				SET SourceID = @SourceID
					,Name = @Name
					,[Definition] = @Definition
					,GenBankID = @GenBankID
					,Locus = @Locus
					,Accession = @Accession
					,Organism = @Organism
					,Taxonomy = @Taxonomy
					,[Description] = @Description
					,Nucleotides = @Nucleotides
					,SequenceTypeID = @SequenceTypeID
					,LastUpdatedAt = @LastUpdatedAt
					,LastUpdateSourceID = @LastUpdateSourceID
				WHERE ID = @ID
		END
	END
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.ConvertToBase64')) BEGIN
	DROP FUNCTION Common.ConvertToBase64
END
GO
CREATE FUNCTION Common.ConvertToBase64 (@s nvarchar(MAX))
	RETURNS varchar(MAX)
AS
BEGIN

	DECLARE @binary varbinary(MAX) = CAST(@s AS varbinary(MAX))
	RETURN (CAST('' as xml).value('xs:base64Binary(sql:variable("@binary"))', 'varchar(max)'))

END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.ConvertFromBase64')) BEGIN
	DROP FUNCTION Common.ConvertFromBase64
END
GO
CREATE FUNCTION Common.ConvertFromBase64 (@Encoded varchar(MAX))
	RETURNS nvarchar(MAX)
AS
BEGIN

	RETURN CAST(CAST('' as xml).value('xs:base64Binary(sql:variable("@Encoded"))', 'varbinary(MAX)') AS nvarchar(MAX))

END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.ConvertXMLToBase64')) BEGIN
	DROP FUNCTION Common.ConvertXMLToBase64
END
GO
CREATE FUNCTION Common.ConvertXMLToBase64 (@x xml)
	RETURNS varchar(MAX)
AS
BEGIN

	RETURN Common.ConvertToBase64(CAST(@x AS nvarchar(MAX)))
	
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.ConvertXMLFromBase64')) BEGIN
	DROP FUNCTION Common.ConvertXMLFromBase64
END
GO
CREATE FUNCTION Common.ConvertXMLFromBase64 (@Encoded varchar(MAX))
	RETURNS xml
AS
BEGIN

	RETURN CAST(Common.ConvertFromBase64(@Encoded) AS xml)

END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('NCBI.ConcatenateBlastNAlignments')) BEGIN
	DROP FUNCTION NCBI.ConcatenateBlastNAlignments
END
GO
CREATE FUNCTION NCBI.ConcatenateBlastNAlignments (@RequestID int)
RETURNS varchar(MAX)
AS
BEGIN
	DECLARE @AlignmentIDs varchar(MAX) = ''

	SELECT @AlignmentIDs += CAST(al.AlignmentID AS varchar(10)) + ';'
		FROM NCBI.BlastNAlignment al
		WHERE al.RequestID = @RequestID

	RETURN @AlignmentIDs	
END
GO
ALTER PROCEDURE [Common].[ThrowException]
	@Rethrow bit = 1
	,@JobID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Message nvarchar(MAX)
			,@Procedure nvarchar(250)
			,@Severity int
			,@State int
			,@Line int

	SELECT @Message = ERROR_MESSAGE()
			,@Procedure = ERROR_PROCEDURE()
			,@Severity = ERROR_SEVERITY()
			,@State = ERROR_STATE()
			,@Line = ERROR_LINE()
	IF @Procedure IS NOT NULL BEGIN
		SET @Message += ' (' + ERROR_PROCEDURE() + ': line ' + CAST(ERROR_LINE() AS varchar(8)) + ')'
	END

	IF (@JobID IS NOT NULL) BEGIN
		DECLARE @ExceptionID int = NULL
		EXEC Job.Exception_Add @ExceptionID OUTPUT, @JobID, NULL, @Message, @Procedure, NULL, 'SQLException', NULL
	END

	IF (@Rethrow = 1) BEGIN
		RAISERROR (@Message, @Severity, @State)
	END
	ELSE BEGIN
		SELECT @Message AS [Message]
				,@Severity AS [Severity]
				,@State AS [State]
	END
END
GO
IF NOT EXISTS (SELECT * FROM Job.[Target] WHERE [Key] = 'Import_DataFile') BEGIN
	INSERT INTO Job.[Target] (Name, [Key])
	VALUES ('Import Data File', 'Import_DataFile')
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.ConvertXMLToHashtableUniqueIdentifier')) BEGIN
	DROP FUNCTION Common.ConvertXMLToHashtableUniqueIdentifier
END
GO
CREATE FUNCTION Common.ConvertXMLToHashtableUniqueIdentifier (@x xml)
RETURNS TABLE
AS
RETURN
	(
		SELECT id.value('(@Key)[1]', 'uniqueidentifier') AS [Key], 
				id.value('(@Value)[1]', 'uniqueidentifier')  AS [Value]
			FROM @x.nodes('(Hashtable/KV)') x(id)
	)
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.ConvertHashtableUniqueIdentifierToXML')) BEGIN
	DROP FUNCTION Common.ConvertHashtableUniqueIdentifierToXML
END
GO
CREATE FUNCTION Common.ConvertHashtableUniqueIdentifierToXML (@Table Common.HashtableUniqueIdentifier READONLY)
RETURNS xml
AS
BEGIN
	RETURN (SELECT * FROM @Table KV FOR XML AUTO, ROOT('Hashtable'))
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.ConvertXMLToHashtableInt')) BEGIN
	DROP FUNCTION Common.ConvertXMLToHashtableInt
END
GO
CREATE FUNCTION Common.ConvertXMLToHashtableInt (@x xml)
RETURNS TABLE
AS
RETURN
	(
		SELECT id.value('(@Key)[1]', 'int') AS [Key], 
				id.value('(@Value)[1]', 'int')  AS [Value]
			FROM @x.nodes('(Hashtable/KV)') x(id)
	)
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.ConvertHashtableIntToXML')) BEGIN
	DROP FUNCTION Common.ConvertHashtableIntToXML
END
GO
CREATE FUNCTION Common.ConvertHashtableIntToXML (@Table Common.HashtableInt READONLY)
RETURNS xml
AS
BEGIN
	RETURN (SELECT * FROM @Table KV FOR XML AUTO, ROOT('Hashtable'))
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.RecordSet_Export2')) BEGIN
	DROP PROCEDURE RecordSet.RecordSet_Export2
END
GO
CREATE PROCEDURE RecordSet.RecordSet_Export2
	@RecordSetID uniqueidentifier
	,@SelectedSubSetIDs Common.ListUniqueIdentifier READONLY
	,@SelectedGeneIDs Common.ListUniqueIdentifier READONLY
	,@SelectedResultIDs Common.ListInt READONLY
	,@IncludeJobHistory_TargetIDs Common.ListInt READONLY

	,@GeneOptions_IncludeAlignedSequences bit = 1
	,@GeneOptions_IncludeGeneSequenceAnnotations bit = 1
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SubSetIDs Common.ListUniqueIdentifier
	DECLARE @GeneIDs TABLE (GeneID uniqueidentifier)
	DECLARE @ResultIDs TABLE (ResultID int)
	
	IF EXISTS (SELECT * FROM @SelectedSubSetIDs) BEGIN
		INSERT INTO @SubSetIDs SELECT Value FROM @SelectedSubSetIDs

		-- To start, get all of the Gene.Gene records that are directly being exported; in other words, everything that's linked in RecordSet.SubSet_Gene
		-- for the subsets that the user selected within the current RecordSet.
		INSERT INTO @GeneIDs
		SELECT g.ID
			FROM Gene.Gene g
			JOIN RecordSet.SubSetGene sg ON sg.GeneID = g.ID
			JOIN @SubSetIDs s_id ON s_id.Value = sg.SubSetID
			WHERE g.Active = 1	
		
		-- Get all of the PAML.Result records that are directly being exported.
		INSERT INTO @ResultIDs
		SELECT r.ID
			FROM PAML.Result r
			JOIN PAML.SubSetResult sr ON sr.ResultID = r.ID
			JOIN @SubSetIDs s_id ON s_id.Value = sr.SubSetID
	END
	ELSE IF EXISTS (SELECT * FROM @SelectedGeneIDs) BEGIN
		-- The user is exporting a data file of specific gene records, not exporting a recordset's worth of data.
		INSERT INTO @GeneIDs
		SELECT sg.Value FROM @SelectedGeneIDs sg
	END
	ELSE IF EXISTS (SELECT * FROM @SelectedResultIDs) BEGIN
		-- The user is exporting a data file of specific PAML results, not exporting a recordset's worth of data.
		INSERT INTO @ResultIDs
		SELECT sr.Value FROM @SelectedResultIDs sr
	END

	IF (@GeneOptions_IncludeAlignedSequences = 1) BEGIN
		-- We also need all of the Gene.Gene records that are aligned with the directly selected sequences.	 These are not restricted by 
		-- RecordSet/SubSet because the app doesn't restrict them that way when you view aligned sequences on frmGeneDetails.
		-- At the moment we're not including Query sequences for which our directly selected sequences were aligned as Subject sequences.
		INSERT INTO @GeneIDs
		SELECT DISTINCT sg.ID
			FROM BlastN.Alignment al
			JOIN @GeneIDs g_id ON g_id.GeneID = al.QueryID
			JOIN Gene.Gene sg ON sg.ID = al.SubjectID
			WHERE sg.Active = 1
				AND NOT EXISTS (SELECT * FROM @GeneIDs ex WHERE ex.GeneID = sg.ID)
	END
	
	DECLARE @JobIDs TABLE (JobID uniqueidentifier)
	-- Build out the list of jobs to include
	IF EXISTS (SELECT * FROM @IncludeJobHistory_TargetIDs) BEGIN
		IF EXISTS (SELECT *
						FROM @IncludeJobHistory_TargetIDs t_id
						JOIN Job.[Target] t ON t.ID = t_id.Value
						WHERE t.[Key] IN ('BLASTN_NCBI', 'PRANK', 'MUSCLE', 'PhyML')) BEGIN
			INSERT INTO @JobIDs
			SELECT j.ID
				FROM Job.Job j
				JOIN @IncludeJobHistory_TargetIDs t ON t.Value = j.TargetID
				WHERE j.Active = 1
					AND EXISTS (SELECT *
									FROM Job.Gene jg
									JOIN @GeneIDs g_id ON g_id.GeneID = jg.GeneID
									WHERE jg.JobID = j.ID
										AND jg.DirectionID = 1)
		END

		IF EXISTS (SELECT *
						FROM @IncludeJobHistory_TargetIDs t_id
						JOIN Job.[Target] t ON t.ID = t_id.Value
						WHERE t.[Key] IN ('CodeML')) BEGIN
			INSERT INTO @JobIDs
			SELECT j.ID
				FROM Job.Job j
				JOIN @IncludeJobHistory_TargetIDs t ON t.Value = j.TargetID
				WHERE j.Active = 1
					AND EXISTS (SELECT *
									FROM PAML.SubSetResult sr
									JOIN @ResultIDs r_id ON r_id.ResultID = sr.ResultID
									JOIN PAML.Result r ON r.ID = r_id.ResultID
									JOIN PAML.Tree t ON t.ID = r.TreeID
									WHERE t.JobID = j.ID
										AND r.Active = 1)
		END
	END

	DECLARE @Output TABLE (ID int identity(1,1), Data xml)
	
	-- Export some properties of the source database; this becomes the "header" for the data file.
	INSERT INTO @Output (Data)
	SELECT (SELECT [Properties].Value AS [DatabaseVersion]
					FROM Common.ApplicationProperty [Properties]
					WHERE [Properties].[Key] = 'DatabaseVersion'
					FOR XML AUTO, ELEMENTS)
	
	IF EXISTS (SELECT * FROM @SelectedSubSetIDs) BEGIN
		-- This is only necessary if the user is exporting a recordset.
		INSERT INTO @Output (Data)
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
						,[SubSet].DisplayIndex
					FROM RecordSet.RecordSet [RecordSet]
					JOIN RecordSet.SubSet [SubSet] ON [SubSet].RecordSetID = [RecordSet].ID
					JOIN @SubSetIDs s_id ON s_id.Value = [SubSet].ID
					ORDER BY [SubSet].Name
					FOR XML AUTO, ROOT ('RecordSet-SubSet'))
	END

	-- Export the Genes, nucleotide sequence data and annotations, and their SubSet assignments
	INSERT INTO @Output (Data)
	SELECT (SELECT [Gene].ID
					,[Gene].Name
					,[Gene].[Definition]
					,[Gene].SourceID
					,[Gene].GenBankID
					,[Gene].Locus
					,[Gene].Accession
					,[Gene].Organism
					,[Gene].Taxonomy
					,[Gene].Nucleotides
					,[Gene].SequenceTypeID
					,[Gene].[Description]
					,[Gene].LastUpdatedAt
					,[Gene].LastUpdateSourceID
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
	SELECT (SELECT [SubSet].ID
					,[Gene].GeneID
					,[Gene].ModifiedAt
				FROM RecordSet.SubSet [SubSet]
				JOIN RecordSet.SubSetGene [Gene] ON [Gene].SubSetID = [SubSet].ID
				JOIN @SubSetIDs s_id ON s_id.Value = [SubSet].ID
				JOIN @GeneIDs g ON g.GeneID = [Gene].GeneID
				ORDER BY [SubSet].ID
				FOR XML AUTO, ROOT ('RecordSet-SubSet-Gene'))
	
	IF (@GeneOptions_IncludeGeneSequenceAnnotations = 1) BEGIN
		INSERT INTO @Output (Data)
		SELECT (SELECT [Gene].GeneID
						,[Feature].ID
						,[Feature].[Rank]
						,[Feature].FeatureKeyID
						,[Feature].GeneQualifier
						,[Feature].GeneIDQualifier
						,[Feature-Interval].ID
						,[Feature-Interval].Start
						,[Feature-Interval].[End]
						,[Feature-Interval].IsComplement
						,[Feature-Interval].StartModifier
						,[Feature-Interval].EndModifier
						,[Feature-Interval].Accession
					FROM Gene.Feature [Feature]
					JOIN Gene.FeatureInterval [Feature-Interval] ON [Feature-Interval].FeatureID = [Feature].ID
					JOIN @GeneIDs [Gene] ON [Gene].GeneID = [Feature].GeneID
					ORDER BY [Gene].GeneID, [Feature].ID, [Feature-Interval].ID
					FOR XML AUTO, ROOT ('RecordSet-Gene-Feature'))
	END

	IF EXISTS (SELECT * FROM @IncludeJobHistory_TargetIDs) BEGIN
		INSERT INTO @Output (Data)
		SELECT (SELECT [Job].ID
						,[Job].TargetID
						,[Job].StartedAt
						,[Job].EndedAt
						,[Job].StatusID
						,[Job].SubSetID
						,[Job].Title
						,CAST('<Encoded>' + Common.ConvertXMLToBase64([Job].AdditionalProperties) + '</Encoded>' AS xml) AS AdditionalProperties
						,CAST(('<Output>' + Common.ConvertToBase64(CONVERT(nvarchar(MAX), [Output].OutputText)) + '</Output>') AS xml) AS OutputText
					FROM Job.Job [Job]
					LEFT OUTER JOIN Job.OutputText [Output] ON [Output].JobID = [Job].ID
					JOIN @JobIDs j_id ON j_id.JobID = [Job].ID
					ORDER BY [Job].ID
					FOR XML AUTO, ROOT ('RecordSet-Job'))
		UNION ALL
		SELECT (SELECT [Job].ID
						,[Exception].ID
						,[Exception].RequestID
						,[Exception].ParentID
						,[Exception].ExceptionAt
						,[Exception].ExceptionType
						,Common.ConvertToBase64(CONVERT(nvarchar(MAX), [Exception].[Message])) AS [Message]
						,Common.ConvertToBase64(CONVERT(nvarchar(MAX), [Exception].[Source])) AS [Source]
						,Common.ConvertToBase64(CONVERT(nvarchar(MAX), [Exception].StackTrace)) AS StackTrace
					FROM Job.Job [Job]
					JOIN Job.Exception [Exception] ON [Exception].JobID = [Job].ID
					JOIN @JobIDs j_id ON j_id.JobID = [Job].ID
					ORDER BY [Job].ID, [Exception].ID
					FOR XML AUTO, ROOT ('RecordSet-Job-Exception'))
		UNION ALL
		SELECT (SELECT [Job].ID
						,[Gene].GeneID
						,[Gene].DirectionID
					FROM Job.Gene [Gene]
					JOIN Job.Job [Job] ON [Job].ID = [Gene].JobID
					JOIN @JobIDs j_id ON j_id.JobID = [Job].ID
					ORDER BY [Job].ID, [Gene].GeneID
					FOR XML AUTO, ROOT ('RecordSet-Job-Gene'))
		UNION ALL
		SELECT (SELECT [Request].ID
						,[Request].RequestID
						,[Request].JobID
						,[Request].StartTime
						,[Request].EndTime
						,[Request].LastStatus
						,[Request].LastUpdatedAt
						,[Request].TargetDatabase
						,[Request].[Algorithm]
						,CASE WHEN ISNULL([Request].StatusInformation, '') = '' THEN NULL
							ELSE CAST(('<Text>' + [Request].StatusInformation + '</Text>') AS xml) 
							END AS StatusInformation
						,CAST(('<IDs>' + NCBI.ConcatenateBlastNAlignments([Request].ID) + '</IDs>') AS xml) AS BlastNAlignments
						,[Request-Gene].GeneID
						,[Request-Gene].StatusID
					FROM NCBI.Request [Request]
					JOIN @JobIDs j_id ON j_id.JobID = [Request].JobID
					JOIN NCBI.Gene [Request-Gene] ON [Request-Gene].RequestID = [Request].ID
					ORDER BY [Request].ID, [Request-Gene].GeneID
					FOR XML AUTO, ROOT ('RecordSet-NCBI-Request'))
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
					JOIN @JobIDs j_id ON j_id.JobID = req.JobID
					JOIN Gene.Gene qry ON qry.ID = Alignment.QueryID
					JOIN Gene.Gene sbj ON sbj.ID = Alignment.SubjectID
					WHERE qry.Active = 1
						AND sbj.Active = 1
					ORDER BY [Alignment].ID
					FOR XML AUTO, ROOT ('RecordSet-BLASTN-Alignment'))
		--UNION ALL
		--SELECT (SELECT [Request].ID
		--				,[Alignment].AlignmentID
		--				--,(SELECT [Alignment].AlignmentID
		--				--		FROM NCBI.BlastNAlignment [Alignment] 
		--				--		WHERE [Alignment].RequestID = [Request].ID
		--				--		FOR XML PATH(''), TYPE) AS "Alignments"
		--			FROM NCBI.Request [Request]
		--			JOIN NCBI.BlastNAlignment [Alignment] ON [Alignment].RequestID = [Request].ID
		--			JOIN @JobIDs j_id ON j_id.JobID = [Request].JobID
		--			WHERE EXISTS (SELECT * FROM NCBI.BlastNAlignment ex WHERE ex.RequestID = [Request].ID)
		--			FOR XML AUTO, ROOT ('RecordSet-Request-Alignment'))
		UNION ALL
		SELECT (SELECT [Tree].ID
						,[Tree].JobID
						,[Tree].TreeFilePath
						,[Tree].SequencesFilePath
						,[Tree].[Rank]
						,[Tree].StatusID
						,[Tree].Title
						,[Tree].SequenceCount
						,[Tree].SequenceLength
						,CAST('<Encoded>' + Common.ConvertXMLToBase64([Tree].ControlConfiguration) + '</Encoded>' AS xml) AS ControlConfiguration

						,[Config].ID
						,[Config].Model
						,[Config].NCatG
						,CONVERT(varchar(10), [Config].KStart) + '|' + CONVERT(varchar(10), ISNULL([Config].KEnd, [Config].KStart)) + '|' + CONVERT(varchar(10), [Config].KInterval) + '|' + CONVERT(varchar(1), [Config].KFixed) AS K
						,CONVERT(varchar(10), [Config].WStart) + '|' + CONVERT(varchar(10), ISNULL([Config].WEnd, [Config].WStart)) + '|' + CONVERT(varchar(10), [Config].WInterval) + '|' + CONVERT(varchar(1), [Config].WFixed) AS W
						,[Config].[Rank]
						,[Config].StatusID
						,[Config].ModelPresetID
						,PAML.GetNSSitesListForAnalysisConfiguration([Config].ID) AS NSSites
					FROM PAML.Tree [Tree]
					JOIN PAML.AnalysisConfiguration [Config] ON [Config].TreeID = [Tree].ID
					JOIN @JobIDs j_id ON j_id.JobID = [Tree].JobID
					ORDER BY [Tree].ID, [Config].ID
					FOR XML AUTO, ROOT ('RecordSet-PAML-Tree'))
		UNION ALL
		SELECT (SELECT [Result].ID
						,[Result].TreeID
						,[Result].AnalysisConfigurationID
						,[Result].NSSite
						,[Result].Kappa
						,[Result].Omega
						,[Result].np
						,[Result].lnL
						,[Result].k
						,[Result].Duration
						,[Result].CompletedAt
						,CONVERT(xml,
							(SELECT ssr.SubSetID AS ID
								FROM PAML.SubSetResult ssr
								WHERE ssr.ResultID = [Result].ID
								FOR XML RAW)
							) AS SubSetIDs
						,[Value].ID
						,[Value].SiteClass
						,[Value].ValueTypeID
						,[Value].[Rank]
						,[Value].Value
					FROM PAML.Result [Result]
					JOIN PAML.ResultdNdSValue [Value] ON [Value].ResultID = [Result].ID
					JOIN PAML.SubSetResult [SubSetResult] ON [SubSetResult].ResultID = [Result].ID
					JOIN PAML.Tree [Tree] ON [Tree].ID = [Result].TreeID
					JOIN @ResultIDs r_id ON r_id.ResultID = [Result].ID
					WHERE [Result].Active = 1
					ORDER BY [Result].ID, [Value].ID
					FOR XML AUTO, ROOT ('RecordSet-PAML-Result'))
		UNION ALL
		SELECT (SELECT [Output].ID
						,[Output].TreeID
						,[Output].AnalysisConfigurationID
						,[Output].Kappa
						,[Output].Omega
						,[Output].StatusID
						,[Output].ProcessDirectory
						,Common.ConvertToBase64(CONVERT(nvarchar(MAX), [Output].OutputData)) AS OutputData
						,Common.ConvertToBase64(CONVERT(nvarchar(MAX), [Output].ErrorData)) AS ErrorData
					FROM PAML.ProcessOutput [Output]
					JOIN PAML.Tree [Tree] ON [Tree].ID = [Output].TreeID
					JOIN @JobIDs j_id ON j_id.JobID = [Tree].JobID
					ORDER BY [Output].ID
					FOR XML AUTO, ROOT ('RecordSet-PAML-Process'))
		UNION ALL
		SELECT (SELECT [Exception].ExceptionID
						,[Exception].ProcessOutputID
					FROM PAML.ProcessException [Exception]
					JOIN PAML.ProcessOutput o ON o.ID = [Exception].ProcessOutputID
					JOIN PAML.Tree t ON t.ID = o.TreeID
					JOIN @JobIDs j_id ON j_id.JobID = t.JobID
					FOR XML AUTO, ROOT ('RecordSet-PAML-Exception'))
	END

	DECLARE @final xml = '<Pilgrimage />'
			,@fragment xml
	DECLARE @count int = (SELECT COUNT(*) FROM @Output)
	DECLARE @current int = 1

	WHILE @current <= @count BEGIN
		SELECT @fragment = Data
			FROM @Output
			WHERE ID = @current

		SET @final.modify('insert sql:variable("@fragment") into (/Pilgrimage)[1] ')           
	
		SET @current += 1;
	END 
	
	SELECT @final;

	/*
	SELECT *
		FROM @Output
		ORDER BY ID
	*/
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.Import_DataFile_Progress')) BEGIN
	CREATE TABLE Job.Import_DataFile_Progress (
		JobID uniqueidentifier NOT NULL
		,LastStep int NOT NULL
		,[Message] varchar(1000) NOT NULL

		CONSTRAINT PK_Job_Import_DataFile_Progress PRIMARY KEY CLUSTERED (JobID ASC, LastStep ASC)
	)

	ALTER TABLE Job.Import_DataFile_Progress ADD CONSTRAINT FK_Job_Import_DataFile_Progress_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Import_DataFile_Progress_Add')) BEGIN
	DROP PROCEDURE Job.Import_DataFile_Progress_Add
END
GO
CREATE PROCEDURE Job.Import_DataFile_Progress_Add
	@JobID uniqueidentifier
	,@Message varchar(1000)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @LastStep int = ISNULL((SELECT MAX(LastStep) FROM Job.Import_DataFile_Progress WHERE JobID = @JobID), 0) + 1

	INSERT INTO Job.Import_DataFile_Progress (JobID, LastStep, [Message])
	VALUES (@JobID, @LastStep, @Message)

	UPDATE ot
		SET ot.OutputText += '\r\n' + @Message
		FROM Job.OutputText ot
		WHERE ot.JobID = @JobID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Import_DataFile_Progress_List')) BEGIN
	DROP PROCEDURE Job.Import_DataFile_Progress_List
END
GO
CREATE PROCEDURE Job.Import_DataFile_Progress_List
	@JobID uniqueidentifier
	,@LastStep int = 0
AS
BEGIN
	SET NOCOUNT ON

	SELECT p.LastStep
			,p.[Message]
		FROM Job.Import_DataFile_Progress p
		WHERE p.JobID = @JobID
			AND ((@LastStep = 0) OR (p.LastStep > @LastStep))
		ORDER BY p.LastStep
END
GO

GO
ALTER PROCEDURE [RecordSet].[Import_Rollback]
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRANSACTION
	BEGIN TRY
		DELETE sg
			FROM RecordSet.SubSetGene sg
			JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
			WHERE sub.RecordSetID = @RecordSetID
		DELETE sub
			FROM RecordSet.SubSet sub
			WHERE sub.RecordSetID = @RecordSetID

		-- Capture the genes that belong to this recordset; if they don't exist in any other recordset they can be deleted from Gene.Gene.
		DECLARE @Genes TABLE (ID uniqueidentifier)
		INSERT INTO @Genes
		SELECT g.GeneID
			FROM RecordSet.Gene g
			WHERE g.RecordSetID = @RecordSetID
		DELETE g
			FROM RecordSet.Gene g
			WHERE g.RecordSetID = @RecordSetID

		DELETE ap
			FROM RecordSet.ApplicationProperty ap
			WHERE ap.RecordSetID = @RecordSetID
		DELETE rs
			FROM RecordSet.RecordSet rs
			WHERE rs.ID = @RecordSetID

		-- Same idea as with Genes
		DECLARE @Alignments TABLE (ID int)
		INSERT INTO @Alignments
		SELECT DISTINCT n_al.AlignmentID
			FROM NCBI.BlastNAlignment n_al
			JOIN NCBI.Request req ON req.ID = n_al.RequestID
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE j.RecordSetID = @RecordSetID
		-- Pick up the subject genes
		INSERT INTO @Genes
		SELECT DISTINCT al.SubjectID
			FROM BlastN.Alignment al 
			JOIN NCBI.BlastNAlignment n_al ON n_al.AlignmentID = al.ID
			JOIN NCBI.Request req ON req.ID = n_al.RequestID
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE j.RecordSetID = @RecordSetID

		DELETE n_al
			FROM NCBI.BlastNAlignment n_al
			JOIN @Alignments al ON al.ID = n_al.AlignmentID
		DELETE ex
			FROM BlastN.AlignmentExon ex
			JOIN BlastN.Alignment al ON al.ID = ex.AlignmentID
			JOIN @Alignments n_al ON n_al.ID = al.ID
			WHERE NOT EXISTS (SELECT * -- Don't delete if it's aligned via a different recordset's request.
								FROM NCBI.BlastNAlignment existing
								WHERE existing.AlignmentID = al.ID)
		DELETE al
			FROM BlastN.Alignment al 
			JOIN @Alignments n_al ON n_al.ID = al.ID
			WHERE NOT EXISTS (SELECT *
								FROM NCBI.BlastNAlignment existing
								WHERE existing.AlignmentID = al.ID)
		DELETE g
			FROM NCBI.Gene g
			JOIN NCBI.Request req ON req.ID = g.RequestID
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE req
			FROM NCBI.Request req
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE g
			FROM Job.Gene g
			JOIN Job.Job j ON j.ID = g.JobID
			WHERE j.RecordSetID = @RecordSetID

		DELETE pe
			FROM PAML.ProcessException pe
			JOIN PAML.ProcessOutput po ON po.ID = pe.ProcessOutputID
			JOIN PAML.Tree t ON t.ID = po.TreeID
			JOIN Job.Job j ON j.ID = t.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE po
			FROM PAML.ProcessOutput po
			JOIN PAML.Tree t ON t.ID = po.TreeID
			JOIN Job.Job j ON j.ID = t.JobID
			WHERE j.RecordSetID = @RecordSetID

		DELETE sr
			FROM PAML.SubSetResult sr
			JOIN PAML.Result r ON r.ID = sr.ResultID
			JOIN PAML.Tree t ON t.ID = r.TreeID
			JOIN Job.Job j ON j.ID = t.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE v
			FROM PAML.ResultdNdSValue v
			JOIN PAML.Result r ON r.ID = v.ResultID
			JOIN PAML.Tree t ON t.ID = r.TreeID
			JOIN Job.Job j ON j.ID = t.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE r
			FROM PAML.Result r
			JOIN PAML.Tree t ON t.ID = r.TreeID
			JOIN Job.Job j ON j.ID = t.JobID
			WHERE j.RecordSetID = @RecordSetID

		DELETE ns
			FROM PAML.AnalysisConfigurationNSSite ns
			JOIN PAML.AnalysisConfiguration c ON c.ID = ns.AnalysisConfigurationID
			JOIN PAML.Tree t ON t.ID = c.TreeID
			JOIN Job.Job j ON j.ID = t.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE c
			FROM PAML.AnalysisConfiguration c
			JOIN PAML.Tree t ON t.ID = c.TreeID
			JOIN Job.Job j ON j.ID = t.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE t
			FROM PAML.Tree t
			JOIN Job.Job j ON j.ID = t.JobID
			WHERE j.RecordSetID = @RecordSetID

		DELETE ot
			FROM Job.OutputText ot
			JOIN Job.Job j ON j.ID = ot.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE je
			FROM Job.Exception je
			JOIN Job.Job j ON j.ID = je.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE j
			FROM Job.Job j
			WHERE j.RecordSetID = @RecordSetID

		-- Narrow @Genes down to just the orphaned gene sequences
		DELETE id
			FROM @Genes id
			WHERE EXISTS (SELECT * FROM RecordSet.Gene rs_g WHERE rs_g.GeneID = id.ID)
				OR EXISTS (SELECT * FROM BlastN.Alignment al WHERE (al.SubjectID = id.ID OR al.QueryID = id.ID))

		-- Delete the orphaned records
		DELETE fi
			FROM Gene.FeatureInterval fi
			JOIN Gene.Feature f ON f.ID = fi.FeatureID
			JOIN @Genes id ON id.ID = f.GeneID
		DELETE f
			FROM Gene.Feature f
			JOIN @Genes id ON id.ID = f.GeneID
		DELETE seq
			FROM Gene.NucleotideSequence seq
			JOIN @Genes id ON id.ID = seq.GeneID
		DELETE g -- Remove any orphaned gene sequences
			FROM Gene.GeneHistory g
			JOIN @Genes id ON id.ID = g.ID
		DELETE g
			FROM Gene.Gene g
			JOIN @Genes id ON id.ID = g.ID

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		
		EXEC Common.ThrowException
	END CATCH
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_RecordSet')) BEGIN
	DROP PROCEDURE RecordSet.Import_RecordSet
END
GO
CREATE PROCEDURE RecordSet.Import_RecordSet
	@JobID uniqueidentifier
	,@x xml
	,@RecordSetName varchar(200)
	,@RecordSetID uniqueidentifier OUTPUT
	,@SubSetsXML xml OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @ProgressMessage varchar(1000)

	SET @RecordSetID = NEWID()
	INSERT INTO RecordSet.RecordSet (ID, Name, CreatedAt, LastOpenedAt, ModifiedAt, Active)
	SELECT @RecordSetID
			,@RecordSetName
			,SYSDATETIME()
			,rs.value('(LastOpenedAt)[1]', 'datetime2(7)')
			,rs.value('(ModifiedAt)[1]', 'datetime2(7)')
			,1
		FROM @x.nodes('(/Pilgrimage/RecordSet)') AS RecordSet(rs)

	INSERT INTO RecordSet.ApplicationProperty (RecordSetID, [Key], Value)
	SELECT @RecordSetID
			,kv.value('(Key)[1]', 'varchar(30)')
			,kv.value('(Value)[1]', 'varchar(MAX)')
		FROM @x.nodes('(/Pilgrimage/RecordSet/Properties)') AS Properties(KV)

	SET @ProgressMessage = 'Created recordset ' + @RecordSetName
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	DECLARE @SubSetIDs Common.HashtableUniqueIdentifier
	INSERT INTO @SubSetIDs
	SELECT sub.value('(@ID)[1]', 'uniqueidentifier'), NEWID()
		FROM @x.nodes('(/Pilgrimage/RecordSet-SubSet/SubSet)') AS SubSets(Sub)
	SET @SubSetsXML = Common.ConvertHashtableUniqueIdentifierToXML(@SubSetIDs)

	INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, [Open], DisplayIndex, DataTypeID)
	SELECT id.Value
			,@RecordSetID
			,sub.value('(@Name)[1]', 'varchar(100)')
			,sub.value('(@Open)[1]', 'bit')
			,sub.value('(@DisplayIndex)[1]', 'int')
			,sub.value('(@DataTypeID)[1]', 'int')
		FROM @x.nodes('(/Pilgrimage/RecordSet-SubSet/SubSet)') AS SubSets(Sub)
		JOIN @SubSetIDs id ON id.[Key] = sub.value('(@ID)[1]', 'uniqueidentifier')

	SET @ProgressMessage = 'Created ' + CAST((SELECT COUNT(*) FROM @SubSetIDs) AS varchar(10)) + ' subsets'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_Genes')) BEGIN
	DROP PROCEDURE RecordSet.Import_Genes
END
GO
CREATE PROCEDURE RecordSet.Import_Genes
	@JobID uniqueidentifier
	,@x xml
	,@RecordSetID uniqueidentifier
	,@SubSetIDs Common.HashtableUniqueIdentifier READONLY
	,@GenesXML xml OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @GeneIDs Common.HashtableUniqueIdentifier
	DECLARE @GeneFeatureIDs Common.HashtableInt
	DECLARE @ProgressMessage varchar(1000)
	
	INSERT INTO @GeneIDs
	SELECT g.value('(ID)[1]', 'uniqueidentifier'), NEWID()
		FROM @x.nodes('(/Pilgrimage/RecordSet-Gene/Gene)') AS Genes(g)
	SET @GenesXML = Common.ConvertHashtableUniqueIdentifierToXML(@GeneIDs)

	INSERT INTO Gene.Gene (ID, Name, [Definition], SourceID, GenBankID, Locus, Accession, Organism, Taxonomy, Nucleotides, SequenceTypeID, [Description], LastUpdatedAt,  LastUpdateSourceID)
	SELECT id.Value
			,g.value('(Name)[1]', 'varchar(100)')
			,g.value('(Definition)[1]', 'varchar(1000)')
			,g.value('(SourceID)[1]', 'int')
			,g.value('(GenBankID)[1]', 'int')
			,g.value('(Locus)[1]', 'varchar(100)')
			,g.value('(Accession)[1]', 'varchar(20)')
			,g.value('(Organism)[1]', 'varchar(250)')
			,g.value('(Taxonomy)[1]', 'varchar(4000)')
			,g.value('(Nucleotides)[1]', 'varchar(MAX)')
			,g.value('(SequenceTypeID)[1]', 'int')
			,g.value('(Description)[1]', 'varchar(MAX)')
			,g.value('(LastUpdatedAt)[1]', 'datetime2(7)')
			,g.value('(LastUpdateSourceID)[1]', 'int')
		FROM @x.nodes('(/Pilgrimage/RecordSet-Gene/Gene)') AS Genes(g)
		JOIN @GeneIDs id ON id.[Key] = g.value('(ID)[1]', 'uniqueidentifier')

	INSERT INTO Gene.NucleotideSequence (GeneID, Nucleotides, Start, [End])
	SELECT id.Value
			,s.value('(Nucleotides)[1]', 'varchar(MAX)')
			,s.value('(Start)[1]', 'int')
			,s.value('(End)[1]', 'int')
		FROM @x.nodes('(/Pilgrimage/RecordSet-Gene-Sequence/Sequence)') AS Seq(s)
		JOIN @GeneIDs id ON id.[Key] = s.value('(GeneID)[1]', 'uniqueidentifier')
		
	INSERT INTO RecordSet.SubSetGene
	SELECT subset_id.Value
			,gene_id.Value
			,g.value('(@ModifiedAt)[1]', 'datetime2(7)')
		FROM @x.nodes('(/Pilgrimage/RecordSet-SubSet-Gene/SubSet)') AS SubSet(s)
		CROSS APPLY s.nodes('(/Gene)') AS Gene(g)
		JOIN @SubSetIDs subset_id ON subset_id.[Key] = s.value('(@ID)[1]', 'uniqueidentifier')
		JOIN @GeneIDs gene_id ON gene_id.[Key] = g.value('(@GeneID)[1]', 'uniqueidentifier')

	INSERT INTO RecordSet.Gene
	SELECT @RecordSetID
			,gene_id.Value
			,MAX(g.value('(@ModifiedAt)[1]', 'datetime2(7)'))
		FROM @x.nodes('(/Pilgrimage/RecordSet-SubSet-Gene/SubSet)') AS SubSet(s)
		CROSS APPLY s.nodes('(/Gene)') AS Gene(g)
		JOIN @SubSetIDs subset_id ON subset_id.[Key] = s.value('(@ID)[1]', 'uniqueidentifier')
		JOIN @GeneIDs gene_id ON gene_id.[Key] = g.value('(@GeneID)[1]', 'uniqueidentifier')
		GROUP BY gene_id.Value

	SET @ProgressMessage = 'Imported ' + CAST((SELECT COUNT(*) FROM @GeneIDs) AS varchar(10)) + ' gene sequence records'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage
	
	MERGE INTO Gene.Feature
	USING (SELECT gene_id.Value AS ReplacementGeneID
					,f.value('(@ID)[1]', 'int') AS OriginalFeatureID
					,f.value('(@Rank)[1]', 'int') AS [Rank]
					,f.value('(@FeatureKeyID)[1]', 'int') AS FeatureKeyID
					,f.value('(@GeneQualifier)[1]', 'varchar(250)') AS GeneQualifier
					,f.value('(@GeneIDQualifier)[1]', 'int') AS GeneIDQualifier
				FROM @x.nodes('(Pilgrimage/RecordSet-Gene-Feature/Gene)') AS Gene(g)
				CROSS APPLY g.nodes('Feature') AS Feature(f)
				JOIN @GeneIDs gene_id ON gene_id.[Key] = g.value('(@GeneID)[1]', 'uniqueidentifier')) AS f
	-- This looks weird, but in order to get the OriginalID and the new identity value for the ID column we need the OUTPUT clause, and OUTPUT on INSERT
	-- won't let you get at values in any table other than the INSERTED pseudotable, whereas MERGE is quite happy to involve other tables.
		ON 1 = 0 
	WHEN NOT MATCHED THEN
		INSERT (GeneID, [Rank], FeatureKeyID, GeneQualifier, GeneIDQualifier)
		VALUES (f.ReplacementGeneID, f.[Rank], f.FeatureKeyID, f.GeneQualifier, f.GeneIDQualifier)
		OUTPUT f.OriginalFeatureID, inserted.ID /* Replacement Feature ID */ INTO @GeneFeatureIDs;

	INSERT INTO Gene.FeatureInterval (ID, FeatureID, Start, [End], IsComplement, StartModifier, EndModifier, Accession)
	SELECT fi.value('(@ID)[1]', 'int')
			,feature_id.Value
			,fi.value('(@Start)[1]', 'int')
			,fi.value('(@End)[1]', 'int')
			,fi.value('(@IsComplement)[1]', 'bit')
			,fi.value('(@StartModifier)[1]', 'char(1)')
			,fi.value('(@EndModifier)[1]', 'char(1)')
			,fi.value('(@Accession)[1]', 'varchar(20)')
		FROM @x.nodes('(Pilgrimage/RecordSet-Gene-Feature/Gene/Feature)') AS Feature(f)
		CROSS APPLY f.nodes('(Feature-Interval)') AS FeatureInterval(fi)
		JOIN @GeneFeatureIDs feature_id ON feature_id.[Key] = f.value('(@ID)[1]', 'int')
	
	SET @ProgressMessage = 'Imported ' + CAST((SELECT COUNT(*) FROM @GeneFeatureIDs) AS varchar(10)) + ' annotations for gene sequence records'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_Jobs')) BEGIN
	DROP PROCEDURE RecordSet.Import_Jobs
END
GO
CREATE PROCEDURE RecordSet.Import_Jobs
	@JobID uniqueidentifier
	,@RecordSetID uniqueidentifier
	,@x xml
	,@GeneIDs Common.HashtableUniqueIdentifier READONLY
	,@JobsXML xml OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @ProgressMessage varchar(1000)

	-- Job.Job
	DECLARE @JobIDs Common.HashtableUniqueIdentifier
	INSERT INTO @JobIDs
	SELECT j.value('(@ID)[1]', 'uniqueidentifier'), NEWID()
		FROM @x.nodes('(Pilgrimage/RecordSet-Job/Job)') AS Jobs(j)
	SET @JobsXML = Common.ConvertHashtableUniqueIdentifierToXML(@JobIDs)

	INSERT INTO Job.Job (ID, RecordSetID, TargetID, StartedAt, EndedAt, StatusID, SubSetID, Title, AdditionalProperties)
	SELECT j_id.Value
			,@RecordSetID
			,j.value('(@TargetID)[1]', 'int')
			,j.value('(@StartedAt)[1]', 'datetime2(7)')
			,j.value('(@EndedAt)[1]', 'datetime2(7)')
			,j.value('(@StatusID)[1]', 'int')
			,j.value('(@SubSetID)[1]', 'uniqueidentifier')
			,j.value('(@Title)[1]', 'varchar(200)')
			,Job.ConvertXMLFromBase64(j.value('(AdditionalProperties/Encoded)[1]', 'varchar(MAX)'))
		FROM @x.nodes('(Pilgrimage/RecordSet-Job/Job)') AS Jobs(j)
		JOIN @JobIDs j_id ON j_id.[Key] = j.value('(@ID)[1]', 'uniqueidentifier')

	INSERT INTO Job.OutputText (JobID, OutputText)
	SELECT j_id.Value
			,CONVERT(varchar(MAX), Common.ConvertFromBase64(j.value('(OutputText/Output)[1]', 'varchar(MAX)')))
		FROM @x.nodes('(Pilgrimage/RecordSet-Job/Job)') AS Jobs(j)
		JOIN @JobIDs j_id ON j_id.[Key] = j.value('(@ID)[1]', 'uniqueidentifier')
		WHERE j.value('(OutputText/Output)[1]', 'varchar(MAX)') IS NOT NULL

	SET @ProgressMessage = 'Imported ' + CAST((SELECT COUNT(*) FROM @JobIDs) AS varchar(10)) + ' job histories'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	-- Job.Gene
	INSERT INTO Job.Gene (JobID, GeneID, DirectionID)
	SELECT j_id.Value
			,g_id.Value
			,jg.value('(@DirectionID)[1]', 'int')
		FROM @x.nodes('(Pilgrimage/RecordSet-Job-Gene/Job)') AS Jobs(j)
		CROSS APPLY j.nodes('(Gene)') AS JobGenes(jg)
		JOIN @JobIDs j_id ON j_id.[Key] = j.value('(@ID)[1]', 'uniqueidentifier')
		JOIN @GeneIDs g_id ON g_id.[Key] = jg.value('(@GeneID)[1]', 'uniqueidentifier')

	SET @ProgressMessage = 'Imported ' + CAST(@@ROWCOUNT AS varchar(10)) + ' job gene associations'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_BLASTNHistory')) BEGIN
	DROP PROCEDURE RecordSet.Import_BLASTNHistory
END
GO
CREATE PROCEDURE RecordSet.Import_BLASTNHistory
	@JobID uniqueidentifier
	,@RecordSetID uniqueidentifier
	,@x xml
	,@GeneIDs Common.HashtableUniqueIdentifier READONLY
	,@JobIDs Common.HashtableUniqueIdentifier READONLY
	,@JobExceptionsXML xml OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @ProgressMessage varchar(1000)

	DECLARE @RequestIDs Common.HashtableInt
	MERGE INTO NCBI.Request
	USING (SELECT job_id.Value AS ReplacementJobID
					,r.value('(@ID)[1]', 'int') AS OriginalRequestID
					,r.value('(@RequestID)[1]', 'varchar(20)') AS RequestID
					,r.value('(@StartTime)[1]', 'datetime2(7)') AS StartTime
					,r.value('(@EndTime)[1]', 'datetime2(7)') AS EndTime
					,r.value('(@LastStatus)[1]', 'varchar(8)') AS LastStatus
					,r.value('(@LastUpdatedAt)[1]', 'datetime2(7)') AS LastUpdatedAt
					,r.value('(StatusInformation/Text)[1]', 'varchar(MAX)') AS StatusInformation
					,r.value('(@TargetDatabase)[1]', 'varchar(250)') AS TargetDatabase
					,r.value('(@Algorithm)[1]', 'varchar(20)') AS [Algorithm]
				FROM @x.nodes('(Pilgrimage/RecordSet-NCBI-Request/Request)') AS Request(r)
				JOIN @JobIDs job_id ON job_id.[Key] = r.value('(@JobID)[1]', 'uniqueidentifier')) AS r
	-- This looks weird, but in order to get the OriginalID and the new identity value for the ID column we need the OUTPUT clause, and OUTPUT on INSERT
	-- won't let you get at values in any table other than the INSERTED pseudotable, whereas MERGE is quite happy to involve other tables.
		ON 1 = 0 
	WHEN NOT MATCHED THEN
		INSERT (RequestID, JobID, StartTime, EndTime, LastStatus, LastUpdatedAt, StatusInformation, TargetDatabase, [Algorithm])
		VALUES (r.RequestID, r.ReplacementJobID, r.StartTime, r.EndTime, r.LastStatus, r.LastUpdatedAt, r.StatusInformation, r.TargetDatabase, r.[Algorithm])
		OUTPUT r.OriginalRequestID, inserted.ID /* Replacement Request ID (identity, not the RequestID from NCBI) */ INTO @RequestIDs;

	SET @ProgressMessage = 'Imported ' + CAST(@@ROWCOUNT AS varchar(10)) + ' NCBI requests'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	INSERT INTO NCBI.Gene (RequestID, GeneID, StatusID)
	SELECT request_id.Value
			,rg.value('(@GeneID)[1]', 'uniqueidentifier')
			,rg.value('(@StatusID)[1]', 'int')
		FROM @x.nodes('(Pilgrimage/RecordSet-NCBI-Request/Request)') AS Request(r)
		CROSS APPLY r.nodes('(Request-Gene)') AS RequestGene(rg)
		JOIN @RequestIDs request_id ON request_id.[Key] = r.value('(@ID)[1]', 'int')

	SET @ProgressMessage = 'Imported ' + CAST(@@ROWCOUNT AS varchar(10)) + ' NCBI request gene sequence record associations'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage
	
	DECLARE @AlignmentIDs Common.HashtableInt
	MERGE INTO BlastN.Alignment
	USING (SELECT a.value('(@ID)[1]', 'int') AS OriginalAlignmentID
					,query_id.Value AS QueryID
					,subject_id.Value AS SubjectID
					,a.value('(@Rank)[1]', 'varchar(20)') AS [Rank]
					,a.value('(@LastUpdatedAt)[1]', 'datetime2(7)') AS LastUpdatedAt
				FROM @x.nodes('(Pilgrimage/RecordSet-BLASTN-Alignment/Alignment)') AS Alignment(a)
				JOIN @GeneIDs query_id ON query_id.[Key] = a.value('(@QueryID)[1]', 'uniqueidentifier')
				JOIN @GeneIDs subject_id ON subject_id.[Key] = a.value('(@SubjectID)[1]', 'uniqueidentifier')) AS a
		ON 1 = 0 
	WHEN NOT MATCHED THEN
		INSERT (QueryID, SubjectID, [Rank], LastUpdatedAt)
		VALUES (a.QueryID, a.SubjectID, a.[Rank], a.LastUpdatedAt)
		OUTPUT a.OriginalAlignmentID, inserted.ID /* Replacement Alignment ID */ INTO @AlignmentIDs;

	SET @ProgressMessage = 'Imported ' + CAST((SELECT COUNT(*) FROM @AlignmentIDs) AS varchar(10)) + ' NCBI BLASTN alignments'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	INSERT INTO BlastN.AlignmentExon (AlignmentID, OrientationID, BitScore, AlignmentLength, IdentitiesCount, Gaps, QueryRangeStart, QueryRangeEnd, SubjectRangeStart, SubjectRangeEnd)
	SELECT alignment_id.Value
			,ae.value('(@OrientationID)[1]', 'int')
			,ae.value('(@BitScore)[1]', 'float')
			,ae.value('(@AlignmentLength)[1]', 'int')
			,ae.value('(@IdentitiesCount)[1]', 'int')
			,ae.value('(@Gaps)[1]', 'int')
			,ae.value('(@QueryRangeStart)[1]', 'int')
			,ae.value('(@QueryRangeEnd)[1]', 'int')
			,ae.value('(@SubjectRangeStart)[1]', 'int')
			,ae.value('(@SubjectRangeEnd)[1]', 'int')
		FROM @x.nodes('(Pilgrimage/RecordSet-BLASTN-Alignment/Alignment)') AS Alignment(a)
		CROSS APPLY a.nodes('(Alignment-Exon)') AS AlignmentExon(ae)
		JOIN @AlignmentIDs alignment_id ON alignment_id.[Key] = a.value('(@ID)[1]', 'int')

	SET @ProgressMessage = 'Imported ' + CAST(@@ROWCOUNT AS varchar(10)) + ' NCBI BLASTN alignment exon statistics'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	INSERT INTO NCBI.BlastNAlignment (RequestID, AlignmentID)
	SELECT request_id.Value
			,alignment_id.Value
		FROM @x.nodes('(Pilgrimage/RecordSet-NCBI-Request/Request)') AS Request(r)
		JOIN @RequestIDs request_id ON request_id.[Key] = r.value('(@ID)[1]', 'int')
		CROSS APPLY Common.SplitString(r.value('(BlastNAlignments/IDs)[1]', 'varchar(MAX)'), ';') id
		JOIN @AlignmentIDs alignment_id ON alignment_id.[Key] = CAST(id.Value AS int)

	SET @ProgressMessage = 'Cross-referenced ' + CAST(@@ROWCOUNT AS varchar(10)) + ' NCBI requests with alignment results'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	-- This looks a little out of place, but unfortunately Job.Exception links directly to NCBI.Request via its RequestID column, unlike for PAML
	-- jobs where PAML.ProcessException acts as a linking table.  So we can't import rows into Job.Exception until we've first imported rows into 
	-- NCBI.Request so that we've got new RequestID values.
	DECLARE @JobExceptionIDs Common.HashtableInt
	MERGE Job.Exception
	USING (SELECT e.value('(@ID)[1]', 'int') AS OriginalExceptionID
					,j_id.Value AS ReplacementJobID
					,r_id.Value AS ReplacementRequestID
					,CONVERT(varchar(MAX), Common.ConvertFromBase64(e.value('(@Message)[1]', 'varchar(MAX)'))) AS [Message]
					,CONVERT(varchar(MAX), Common.ConvertFromBase64(e.value('(@Source)[1]', 'varchar(MAX)'))) AS [Source]
					,CONVERT(varchar(MAX), Common.ConvertFromBase64(e.value('(@StackTrace)[1]', 'varchar(MAX)'))) AS [StackTrace]
					,e.value('(@ParentID)[1]', 'int') AS ParentID
					,e.value('(@ExceptionAt)[1]', 'datetime2(7)') AS ExceptionAt
					,e.value('(@ExceptionType)[1]', 'varchar(250)') AS ExceptionType
				FROM @x.nodes('(Pilgrimage/RecordSet-Job-Exception/Job)') AS Job(j)
				CROSS APPLY j.nodes('(Exception)') AS Exception(e)
				JOIN @JobIDs j_id ON j_id.[Key] = j.value('(@ID)[1]', 'uniqueidentifier')
				LEFT OUTER JOIN @RequestIDs r_id ON r_id.[Key] = e.value('(@RequestID)[1]', 'int')) AS e
		ON 1 = 0
	WHEN NOT MATCHED THEN
	INSERT (JobID, RequestID, [Message], [Source], StackTrace, ParentID, ExceptionAt, ExceptionType)
	VALUES (ReplacementJobID, ReplacementRequestID, [Message], [Source], StackTrace, ParentID, ExceptionAt, ExceptionType)
	OUTPUT e.OriginalExceptionID, inserted.ID INTO @JobExceptionIDs;

	-- Re-connect the child exceptions with their parents
	UPDATE e
		SET ParentID = p_id.Value
		FROM Job.Exception e
		JOIN @JobExceptionIDs e_id ON e_id.Value = e.ID
		JOIN @JobExceptionIDs p_id ON p_id.[Key] = e.ParentID
	
	SET @JobExceptionsXML = Common.ConvertHashtableIntToXML(@JobExceptionIDs)
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('PAML.AnalysisConfiguration_KWFromString')) BEGIN
	DROP FUNCTION PAML.AnalysisConfiguration_KWFromString
END
GO
CREATE FUNCTION PAML.AnalysisConfiguration_KWFromString (@String varchar(35))
RETURNS TABLE
AS
	RETURN (SELECT 
				CONVERT(decimal(9,3), pvt.Start) AS Start
				,CONVERT(decimal(9,3), pvt.[End]) AS [End]
				,CONVERT(decimal(9,3), pvt.Interval) AS Interval
				,CONVERT(bit, pvt.Fixed) AS Fixed
			FROM (SELECT CASE k.[Index]
							WHEN 1 THEN 'Start'
							WHEN 2 THEN 'End'
							WHEN 3 THEN 'Interval'
							WHEN 4 THEN 'Fixed'
							END AS Component
						 ,k.Value
					FROM Common.SplitString(@String, '|') k) k
			PIVOT (MAX(k.Value) FOR [Component] IN (Start, [End], Interval, Fixed)) pvt
		)
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_PAML')) BEGIN
	DROP PROCEDURE RecordSet.Import_PAML
END
GO
CREATE PROCEDURE RecordSet.Import_PAML
	@JobID uniqueidentifier
	,@RecordSetID uniqueidentifier
	,@x xml
	,@SubSetIDs Common.HashtableUniqueIdentifier READONLY
	,@JobIDs Common.HashtableUniqueIdentifier READONLY
	,@JobExceptionIDs Common.HashtableInt READONLY
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @ProgressMessage varchar(1000)

	DECLARE @TreeIDs Common.HashtableInt
	MERGE INTO PAML.Tree
	USING (SELECT job_id.Value AS ReplacementJobID
					,t.value('(@ID)[1]', 'int') AS OriginalTreeID
					,t.value('(@TreeFilePath)[1]', 'varchar(250)') AS TreeFilePath
					,t.value('(@SequencesFilePath)[1]', 'varchar(250)') AS SequencesFilePath
					,t.value('(@Rank)[1]', 'int') AS [Rank]
					,t.value('(@StatusID)[1]', 'int') AS StatusID
					,t.value('(@Title)[1]', 'varchar(250)') AS Title
					,t.value('(@SequenceCount)[1]', 'int') AS SequenceCount
					,t.value('(@SequenceLength)[1]', 'int') AS SequenceLength
					,Job.ConvertXMLFromBase64(t.value('(ControlConfiguration/Encoded)[1]', 'varchar(MAX)')) AS ControlConfiguration
				FROM @x.nodes('(Pilgrimage/RecordSet-PAML-Tree/Tree)') AS Tree(t)
				JOIN @JobIDs job_id ON job_id.[Key] = t.value('(@JobID)[1]', 'uniqueidentifier')) AS t
	-- This looks weird, but in order to get the OriginalID and the new identity value for the ID column we need the OUTPUT clause, and OUTPUT on INSERT
	-- won't let you get at values in any table other than the INSERTED pseudotable, whereas MERGE is quite happy to involve other tables.
		ON 1 = 0 
	WHEN NOT MATCHED THEN
		INSERT (JobID, TreeFilePath, SequencesFilePath, [Rank], StatusID, Title, SequenceCount, SequenceLength, ControlConfiguration)
		VALUES (t.ReplacementJobID, t.TreeFilePath, t.SequencesFilePath, t.[Rank], t.StatusID, t.Title, t.SequenceCount, t.SequenceLength, t.ControlConfiguration)
		OUTPUT t.OriginalTreeID, inserted.ID /* Replacement Tree ID (identity) */ INTO @TreeIDs;

	SET @ProgressMessage = 'Imported ' + CAST(@@ROWCOUNT AS varchar(10)) + ' PAML tree data file configurations'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	DECLARE @AnalysisConfigurationIDs Common.HashtableInt
	MERGE INTO PAML.AnalysisConfiguration
	USING (SELECT tree_id.Value AS ReplacementTreeID
					,c.value('(@ID)[1]', 'int') AS OriginalConfigID
					,c.value('(@Model)[1]', 'int') AS Model
					,c.value('(@NCatG)[1]', 'int') AS NCatG
					,k.Start AS KStart
					,CASE WHEN k.[End] = k.Start THEN NULL ELSE k.[End] END AS KEnd
					,k.Interval AS KInterval
					,k.Fixed AS KFixed
					,w.Start AS WStart
					,CASE WHEN w.[End] = w.Start THEN NULL ELSE w.[End] END AS WEnd
					,w.Interval AS WInterval
					,w.Fixed AS WFixed
					,c.value('(@Rank)[1]', 'int') AS [Rank]
					,c.value('(@StatusID)[1]', 'int') AS StatusID
					,c.value('(@ModelPresetID)[1]', 'int') AS ModelPresetID

				FROM @x.nodes('(Pilgrimage/RecordSet-PAML-Tree/Tree)') AS Tree(t)
				CROSS APPLY t.nodes('(Config)') AS Config(c)
				CROSS APPLY PAML.AnalysisConfiguration_KWFromString(c.value('(@K)[1]', 'varchar(35)')) k
				CROSS APPLY PAML.AnalysisConfiguration_KWFromString(c.value('(@W)[1]', 'varchar(35)')) w
				JOIN @TreeIDs tree_id ON tree_id.[Key] = t.value('(@ID)[1]', 'int')) AS c
		ON 1 = 0
	WHEN NOT MATCHED THEN
		INSERT (TreeID, Model, NCatG, KStart, KEnd, KInterval, KFixed, WStart, WEnd, WInterval, WFixed, [Rank], StatusID, ModelPresetID)
		VALUES (c.ReplacementTreeID, c.Model, c.NCatG, c.KStart, c.KEnd, c.KInterval, c.KFixed, c.WStart, c.WEnd, c.WInterval, c.WFixed, c.[Rank], c.StatusID, c.ModelPresetID)
		OUTPUT c.OriginalConfigID, inserted.ID INTO @AnalysisConfigurationIDs;

	SET @ProgressMessage = 'Imported ' + CAST(@@ROWCOUNT AS varchar(10)) + ' PAML analysis configurations'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	INSERT INTO PAML.AnalysisConfigurationNSSite (AnalysisConfigurationID, NSSite)
	SELECT config_id.Value, ns.Value
		FROM @x.nodes('(Pilgrimage/RecordSet-PAML-Tree/Tree/Config)') AS Config(c)
		CROSS APPLY Common.SplitString(c.value('(@NSSites)[1]', 'varchar(250)'), ',') ns
		JOIN @AnalysisConfigurationIDs config_id ON config_id.[Key] = c.value('(@ID)[1]', 'int')
	-- Not bothering with a progress message for these.

	DECLARE @ResultIDs Common.HashtableInt
	MERGE INTO PAML.Result
	USING (SELECT r.value('(@ID)[1]', 'int') AS OriginalResultID
					,tree_id.Value AS TreeID
					,config_id.Value AS AnalysisConfigurationID
					,r.value('(@NSSite)[1]', 'int') AS NSSite
					,r.value('(@Kappa)[1]', 'decimal(9,3)') AS Kappa
					,r.value('(@Omega)[1]', 'decimal(9,3)') AS Omega
					,r.value('(@np)[1]', 'int') AS np
					,r.value('(@lnL)[1]', 'decimal(19,8)') AS lnL
					,r.value('(@k)[1]', 'decimal(9,6)') AS k
					,r.value('(@Duration)[1]', 'time(7)') AS Duration
					,r.value('(@CompletedAt)[1]', 'datetime2(7)') AS CompletedAt
				FROM @x.nodes('(Pilgrimage/RecordSet-PAML-Result/Result)') AS Result(r)
				JOIN @TreeIDs tree_id ON tree_id.[Key] = r.value('(@TreeID)[1]', 'int')
				JOIN @AnalysisConfigurationIDs config_id ON config_id.[Key] = r.value('(@AnalysisConfigurationID)[1]', 'int')) AS r
	-- This looks weird, but in order to get the OriginalID and the new identity value for the ID column we need the OUTPUT clause, and OUTPUT on INSERT
	-- won't let you get at values in any table other than the INSERTED pseudotable, whereas MERGE is quite happy to involve other tables.
		ON 1 = 0 
	WHEN NOT MATCHED THEN
		INSERT (TreeID, AnalysisConfigurationID, NSSite, Kappa, Omega, np, lnL, k, Duration, CompletedAt)
		VALUES (TreeID, AnalysisConfigurationID, NSSite, Kappa, Omega, np, lnL, k, Duration, CompletedAt)
		OUTPUT r.OriginalResultID, inserted.ID /* Replacement Result ID (identity) */ INTO @ResultIDs;

	SET @ProgressMessage = 'Imported ' + CAST(@@ROWCOUNT AS varchar(10)) + ' PAML results'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	INSERT INTO PAML.ResultdNdSValue (ResultID, SiteClass, ValueTypeID, [Rank], Value)
	SELECT result_id.Value
			,v.value('(@SiteClass)[1]', 'varchar(2)')
			,v.value('(@ValueTypeID)[1]', 'int')
			,v.value('(@Rank)[1]', 'int')
			,v.value('(@Value)[1]', 'decimal(9,6)')
		FROM @x.nodes('(Pilgrimage/RecordSet-PAML-Result/Result)') AS Result(r)
		CROSS APPLY r.nodes('(Value)') AS Value(v)
		JOIN @ResultIDs result_id ON result_id.[Key] = r.value('(@ID)[1]', 'int')
		
	SET @ProgressMessage = 'Imported ' + CAST(@@ROWCOUNT AS varchar(10)) + ' PAML result dN/dS values'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	INSERT INTO PAML.SubSetResult (SubSetID, ResultID)
	SELECT subset_id.Value
			,result_id.Value
		FROM @x.nodes('(Pilgrimage/RecordSet-PAML-Result/Result)') AS Result(r)
		CROSS APPLY r.nodes('(SubSetIDs/row)') AS Value(v)
		JOIN @ResultIDs result_id ON result_id.[Key] = r.value('(@ID)[1]', 'int')
		JOIN @SubSetIDs subset_id ON subset_id.[Key] = v.value('(@ID)[1]', 'uniqueidentifier')

	SET @ProgressMessage = 'Assigned ' + CAST(@@ROWCOUNT AS varchar(10)) + ' PAML results to subsets'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	DECLARE @ProcessOutputIDs Common.HashtableInt
	MERGE INTO PAML.ProcessOutput
	USING (SELECT p.value('(@ID)[1]', 'int') AS OriginalOutputID
					,tree_id.Value AS TreeID
					,config_id.Value AS AnalysisConfigurationID
					,p.value('(@Kappa)[1]', 'decimal(9,3)') AS Kappa
					,p.value('(@Omega)[1]', 'decimal(9,3)') AS Omega
					,p.value('(@StatusID)[1]', 'int') AS StatusID
					,p.value('(@ProcessDirectory)[1]', 'varchar(250)') AS ProcessDirectory
					,CONVERT(varchar(MAX), Common.ConvertFromBase64(p.value('(@OutputData)[1]', 'varchar(MAX)'))) AS OutputData
					,CONVERT(varchar(MAX), Common.ConvertFromBase64(p.value('(@ErrorData)[1]', 'varchar(MAX)'))) AS ErrorData
				FROM @x.nodes('(Pilgrimage/RecordSet-PAML-Process/Output)') AS ProcessOutput(p)
				JOIN @TreeIDs tree_id ON tree_id.[Key] = p.value('(@TreeID)[1]', 'int')
				JOIN @AnalysisConfigurationIDs config_id ON config_id.[Key] = p.value('(@AnalysisConfigurationID)[1]', 'int')) AS p
		ON 1 = 0 
	WHEN NOT MATCHED THEN
		INSERT (TreeID, AnalysisConfigurationID, Kappa, Omega, StatusID, ProcessDirectory, OutputData, ErrorData)
		VALUES (TreeID, AnalysisConfigurationID, Kappa, Omega, StatusID, ProcessDirectory, OutputData, ErrorData)
		OUTPUT p.OriginalOutputID, inserted.ID /* Replacement ProcessOutput ID (identity) */ INTO @ProcessOutputIDs;
	
	SET @ProgressMessage = 'Imported ' + CAST(@@ROWCOUNT AS varchar(10)) + ' PAML process histories'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	INSERT INTO PAML.ProcessException (ExceptionID, ProcessOutputID)
	SELECT ex_id.Value
			,po_id.Value
		FROM @x.nodes('(Pilgrimage/RecordSet-PAML-Exception/Exception)') AS Exception(e)
		JOIN @JobExceptionIDs ex_id ON ex_id.[Key] = e.value('(@ExceptionID)[1]', 'int')
		JOIN @ProcessOutputIDs po_id ON po_id.[Key] = e.value('(@ProcessOutputID)[1]', 'int')
	-- Not bothering with a progress message for these...
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_DataFile')) BEGIN
	DROP PROCEDURE RecordSet.Import_DataFile
END
GO
CREATE PROCEDURE RecordSet.Import_DataFile
	@x xml
	,@JobRecordSetID uniqueidentifier
	,@RecordSetName varchar(200)
	,@NewRecordSetID uniqueidentifier OUTPUT
	,@JobID uniqueidentifier OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @JobTargetID int = (SELECT ID FROM Job.[Target] WHERE [Key] = 'Import_DataFile')
			,@JobTitle varchar(250) = ('Importing ' + @x.value('(/Pilgrimage/RecordSet/Name)[1]', 'varchar(200)'))
			,@JobStatusID int = (SELECT ID FROM Job.[Status] WHERE [Key] = 'Running')
			,@JobStartedAt datetime2(7) = SYSDATETIME()
			,@JobEndedAt datetime2(7)
			,@JobOutputText varchar(MAX);

	EXEC Job.Job_Edit
		@ID = @JobID OUTPUT, 
		@RecordSetID = @JobRecordSetID,
		@TargetID = @JobTargetID,
		@Title = @JobTitle,
		@StatusID = @JobStatusID,
		@StartedAt = @JobStartedAt;

	SET @JobOutputText = 'Job started at ' + CAST(@JobStartedAt as varchar(50))
	EXEC Job.OutputText_Edit @JobID, @JobOutputText

	BEGIN TRANSACTION
	BEGIN TRY
		DECLARE @SubSetIDs Common.HashtableUniqueIdentifier -- [Key] = Original ID, [Value] = Replacement ID
				,@SubSetsXML xml
				,@GeneIDs Common.HashtableUniqueIdentifier -- [Key] = Original ID, [Value] = Replacement ID
				,@GenesXML xml
				,@JobIDs Common.HashtableUniqueIdentifier -- [Key] = Original ID, [Value] = Replacement ID
				,@JobsXML xml
				,@JobExceptionsIDs Common.HashtableInt -- [Key] = Original ID, [Value] = Replacement ID
				,@JobExceptionssXML xml
	
		-- Import top-level recordset details and create the subsets
		EXEC RecordSet.Import_RecordSet @JobID, @x, @RecordSetName, @NewRecordSetID OUTPUT, @SubSetsXML OUTPUT
		INSERT INTO @SubSetIDs SELECT * FROM Common.ConvertXMLToHashtableUniqueIdentifier(@SubSetsXML)
	
		-- Import Gene.Gene records, nucleotide sequences, and recordset/subset assignments
		EXEC RecordSet.Import_Genes @JobID, @x, @NewRecordSetID, @SubSetIDs, @GenesXML OUTPUT
		INSERT INTO @GeneIDs SELECT * FROM Common.ConvertXMLToHashtableUniqueIdentifier(@GenesXML)

		-- Import into Job, Job.OutputText, and Job.Genes
		-- PRANK, MUSCLE, and PHYML jobs do not touch any other tables, and thus have no specific stored procedure for importing additional data.
		EXEC RecordSet.Import_Jobs @JobID, @NewRecordSetID, @x, @GeneIDs, @JobsXML OUTPUT
		INSERT INTO @JobIDs SELECT * FROM Common.ConvertXMLToHashtableUniqueIdentifier(@JobsXML)
	
		-- Import BLASTN job history
		EXEC RecordSet.Import_BLASTNHistory @JobID, @NewRecordSetID, @x, @GeneIDs, @JobIDs, @JobExceptionssXML OUTPUT
		INSERT INTO @JobExceptionsIDs SELECT * FROM Common.ConvertXMLToHashtableInt(@JobExceptionssXML)
	
		-- Import PAML job history
		EXEC RecordSet.Import_PAML @JobID, @NewRecordSetID, @x, @SubSetIDs, @JobIDs, @JobExceptionsIDs
		
		SET @JobStatusID = (SELECT ID FROM Job.[Status] WHERE [Key] = 'Completed')
		SET @JobEndedAt = SYSDATETIME()
		EXEC Job.Job_Edit @ID = @JobID, @StatusID = @JobStatusID, @EndedAt = @JobEndedAt

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION

		SET @JobOutputText = 'Import failed: ' + ERROR_MESSAGE()
		EXEC Job.Import_DataFile_Progress_Add @JobID, @JobOutputText

		SET @JobStatusID = (SELECT ID FROM Job.[Status] WHERE [Key] = 'Failed')
		SET @JobEndedAt = SYSDATETIME()
		EXEC Job.Job_Edit @ID = @JobID, @StatusID = @JobStatusID, @EndedAt = @JobEndedAt
		
		EXEC Common.ThrowException 0, @JobID
	END CATCH

	---- Check job status for cancellation.
	--IF (SELECT s.[Key] FROM Job.Job j JOIN Job.[Status] s ON s.ID = j.StatusID WHERE j.ID = @JobID) = 'Cancelled' BEGIN
	--	GOTO cancelled;
	--END

	--cancelled:
	--IF (SELECT s.[Key] FROM Job.Job j JOIN Job.[Status] s ON s.ID = j.StatusID WHERE j.ID = @JobID) = 'Cancelled' BEGIN
	--	SET @JobOutputText = 'Job cancelled at ' + CAST(SYSDATETIME() as varchar(50))
	--	EXEC Job.Import_DataFile_Progress_Add @JobID, @JobOutputText
	--END
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.6.0'
	WHERE [Key] = 'DatabaseVersion'
GO