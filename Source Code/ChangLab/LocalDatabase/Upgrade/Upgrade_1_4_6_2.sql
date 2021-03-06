SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

		DECLARE @DataTypeID_GeneSequence int = (SELECT dt.ID FROM RecordSet.DataType dt WHERE dt.[Key] = 'GeneSequence')
				,@DataTypeID_CodeMLResult int = (SELECT dt.ID FROM RecordSet.DataType dt WHERE dt.[Key] = 'CodeMLResult')

		DECLARE @SubSetLastOpenedAt datetime2(7) = SYSDATETIME()

		INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DataTypeID, DisplayIndex, LastOpenedAt)
			VALUES (NEWID(), @ID, 'All', @DataTypeID_GeneSequence, 1, @SubSetLastOpenedAt)
	
		INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DataTypeID, DisplayIndex, LastOpenedAt)
			VALUES (NEWID(), @ID, 'All', @DataTypeID_CodeMLResult, 1, SYSDATETIME())
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
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_BlastN_Alignment')) BEGIN
	DROP PROCEDURE RecordSet.Import_BlastN_Alignment
END
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_BlastN_AlignmentExon')) BEGIN
	DROP PROCEDURE RecordSet.Import_BlastN_AlignmentExon
END
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_Job')) BEGIN
	DROP PROCEDURE RecordSet.Import_Job
END
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_Job_Gene')) BEGIN
	DROP PROCEDURE RecordSet.Import_Job_Gene
END
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_NCBI_BlastNAlignment')) BEGIN
	DROP PROCEDURE RecordSet.Import_NCBI_BlastNAlignment
END
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_RecordSet_Gene')) BEGIN
	DROP PROCEDURE RecordSet.Import_RecordSet_Gene
END
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_RecordSet_Property')) BEGIN
	DROP PROCEDURE RecordSet.Import_RecordSet_Property
END
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_SubSet')) BEGIN
	DROP PROCEDURE RecordSet.Import_SubSet
END
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_SubSet_Gene')) BEGIN
	DROP PROCEDURE RecordSet.Import_SubSet_Gene
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_Rollback')) BEGIN
	DROP PROCEDURE RecordSet.Import_Rollback
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.RecordSet_Destroy')) BEGIN
	DROP PROCEDURE RecordSet.RecordSet_Destroy
END
GO
CREATE PROCEDURE RecordSet.RecordSet_Destroy
	@RecordSetID uniqueidentifier
AS
BEGIN
	-- This procedure is used for debug purposes only; deleting a recordset in the app simply sets the Active flag to 0.
	SET NOCOUNT ON

	BEGIN TRANSACTION
	BEGIN TRY
		DELETE sg
			FROM RecordSet.SubSetGene sg
			JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
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
		
		DELETE sub
			FROM RecordSet.SubSet sub
			WHERE sub.RecordSetID = @RecordSetID
		DELETE ap
			FROM RecordSet.ApplicationProperty ap
			WHERE ap.RecordSetID = @RecordSetID
		DELETE rs
			FROM RecordSet.RecordSet rs
			WHERE rs.ID = @RecordSetID

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
			,@OriginalTaxonomyID int
			,@OriginalTaxonomy varchar(MAX)
			,@TaxonomyID int = NULL

	IF @LastUpdateSourceID IS NULL BEGIN
		SET @LastUpdateSourceID = @SourceID
	END

	IF @GenBankID <> 0
		AND (@SourceID IN (SELECT s.ID FROM Gene.[Source] s WHERE s.[Key] IN ('BLASTN_NCBI', 'GenBank')))
		AND EXISTS (SELECT * 
						FROM Gene.Gene g 
						JOIN Gene.[Source] s ON s.ID = g.SourceID
						WHERE g.GenBankID = @GenBankID 
							AND s.[Key] IN ('BLASTN_NCBI', 'GenBank')
							AND (@ID IS NULL OR g.ID <> @ID))
		BEGIN
		-- We need to maintain 1:1 with GenBank-sourced records so that the aggregation by GenBankID aspect of BLASTN alignments actually works.
		-- GenBankID is thus treated as unique for NCBI and GenBank sourced records.
		SELECT @ID = g.ID
			FROM Gene.Gene g
			JOIN Gene.[Source] s ON s.ID = g.SourceID
			WHERE g.GenBankID = @GenBankID
				AND s.[Key] IN ('BLASTN_NCBI', 'GenBank')
	END

	IF NOT EXISTS (SELECT * FROM Gene.Gene g WHERE g.ID = @ID) BEGIN
		IF (@ID IS NULL) BEGIN
			SET @ID = NEWID()
		END

		IF (@Taxonomy IS NOT NULL) AND (LTRIM(RTRIM(@Taxonomy)) <> '') BEGIN
			EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT
		END

		INSERT INTO Gene.Gene (ID, Name, [Definition], SourceID
								,GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID, [Description]
								,Nucleotides, SequenceTypeID, LastUpdatedAt, LastUpdateSourceID)
		VALUES (@ID, @Name, @Definition, @SourceID
				,@GenBankID, @Locus, @Accession, @Organism, @Taxonomy, @TaxonomyID, @Description
				,@Nucleotides, @SequenceTypeID
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
					,Name = @Name
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
					,LastUpdatedAt = @LastUpdatedAt
					,LastUpdateSourceID = @LastUpdateSourceID
				WHERE ID = @ID
		END
	END
END
GO
ALTER PROCEDURE [Taxonomy].[Taxon_Parse]
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
			,@ErrorMessage nvarchar(MAX) 
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
					SET @ErrorMessage = 'Parent taxon [' + @ParentValue + '] not found.';
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
ALTER TRIGGER [Gene].[Gene_LogHistory] ON [Gene].[Gene]
	AFTER INSERT, UPDATE
AS
BEGIN

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
				,i.TaxonomyID
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
ALTER PROCEDURE [RecordSet].[RecordSet_Export]
	@RecordSetID uniqueidentifier = NULL
	,@SelectedSubSetIDs Common.ListUniqueIdentifier READONLY
	,@SelectedGeneIDs Common.ListUniqueIdentifier READONLY
	,@SelectedResultIDs Common.ListInt READONLY
	,@SourceSubSetID_ForSelectedRecords uniqueidentifier = NULL
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
		SELECT DISTINCT g.ID
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

		-- We use this as a convenience for the Import logic; the RecordSet-SubSet-Gene block will be generated for just this one subset and the
		-- import logic can leverage it to assign the Gene.Gene records it just created to the target subset ID specified by the user.
		INSERT INTO @SubSetIDs
		SELECT @SourceSubSetID_ForSelectedRecords
	END
	ELSE IF EXISTS (SELECT * FROM @SelectedResultIDs) BEGIN
		-- The user is exporting a data file of specific PAML results, not exporting a recordset's worth of data.
		INSERT INTO @ResultIDs
		SELECT sr.Value FROM @SelectedResultIDs sr

		-- We use this as a convenience for the Import logic; the RecordSet-PAML-Result/SubSetIDs block will be generated for just this one subset
		-- and theimport logic can leverage it to assign the PAML.Result records it just created to the target subset ID specified by the user.
		INSERT INTO @SubSetIDs
		SELECT @SourceSubSetID_ForSelectedRecords
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
								JOIN @SubSetIDs sub ON sub.Value = ssr.SubSetID
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
ALTER PROCEDURE [RecordSet].[Import_DataFile]
	@x xml
	,@RecordSetName varchar(200) = NULL
	,@TargetSubSetID uniqueidentifier = NULL

	,@JobRecordSetID uniqueidentifier
	,@NewRecordSetID uniqueidentifier OUTPUT
	,@JobID uniqueidentifier OUTPUT
	,@ValidateOnly bit = 0
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @JobTargetID int = (SELECT ID FROM Job.[Target] WHERE [Key] = 'Import_DataFile')
			,@JobTitle varchar(250)
			,@JobStatusID int = (SELECT ID FROM Job.[Status] WHERE [Key] = 'Running')
			,@JobStartedAt datetime2(7) = SYSDATETIME()
			,@JobEndedAt datetime2(7)
			,@JobOutputText varchar(MAX);

	IF (@RecordSetName IS NOT NULL) BEGIN
		SET @JobTitle = 'Importing ' + @x.value('(/Pilgrimage/RecordSet/Name)[1]', 'varchar(200)') + ' into ' + @RecordSetName;
	END
	ELSE IF (@TargetSubSetID IS NOT NULL) BEGIN
		SELECT @JobTitle = 'Importing into ' + sub.Name + ' in ' + rs.Name
				,@NewRecordSetID = sub.RecordSetID
			FROM RecordSet.SubSet sub
			JOIN RecordSet.RecordSet rs ON rs.ID = sub.RecordSetID
			WHERE sub.ID = @TargetSubSetID;
	END
	ELSE BEGIN
		RAISERROR ('A new recordset or an existing subset must be specified for importing from a data file.', 18, 1);
	END

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
				,@JobExceptionsXML xml
	
		IF (@TargetSubSetID IS NULL) BEGIN
			-- Import top-level recordset details and create the subsets
			EXEC RecordSet.Import_RecordSet @JobID, @x, @RecordSetName, @NewRecordSetID OUTPUT, @SubSetsXML OUTPUT
			INSERT INTO @SubSetIDs SELECT * FROM Common.ConvertXMLToHashtableUniqueIdentifier(@SubSetsXML)
		END
		ELSE BEGIN
			INSERT INTO @SubSetIDs
			SELECT TOP 1 s.value('(@ID)[1]', 'uniqueidentifier'), @TargetSubSetID
				FROM @x.nodes('(/Pilgrimage/RecordSet-SubSet-Gene/SubSet)') AS SubSet(s)
			UNION
			SELECT TOP 1 s.value('(@ID)[1]', 'uniqueidentifier'), @TargetSubSetID
				FROM @x.nodes('(Pilgrimage/RecordSet-PAML-Result/Result/SubSetIDs/row)') AS SubSet(s)

			-- This is an import of a data file created from exporting specific records, not a recordset, so we should never have both PAML and Gene records
			-- in the same data file.  Just in case, though...
			
			IF (SELECT COUNT(*) FROM @SubSetIDs) > 1 BEGIN
				RAISERROR ('Data file contains records from multiple subsets and cannot be imported into a single subset.', 18, 1);
			END
		END
	
		-- Import Gene.Gene records, nucleotide sequences, and recordset/subset assignments
		EXEC RecordSet.Import_Genes @JobID, @x, @NewRecordSetID, @SubSetIDs, @GenesXML OUTPUT
		INSERT INTO @GeneIDs SELECT * FROM Common.ConvertXMLToHashtableUniqueIdentifier(@GenesXML)

		-- Import into Job, Job.OutputText, and Job.Genes
		-- PRANK, MUSCLE, and PHYML jobs do not touch any other tables, and thus have no specific stored procedure for importing additional data.
		EXEC RecordSet.Import_Jobs @JobID, @NewRecordSetID, @x, @GeneIDs, @JobsXML OUTPUT
		INSERT INTO @JobIDs SELECT * FROM Common.ConvertXMLToHashtableUniqueIdentifier(@JobsXML)
	
		-- Import BLASTN job history
		EXEC RecordSet.Import_BLASTNHistory @JobID, @NewRecordSetID, @x, @GeneIDs, @JobIDs, @JobExceptionsXML OUTPUT
		INSERT INTO @JobExceptionsIDs SELECT * FROM Common.ConvertXMLToHashtableInt(@JobExceptionsXML)
	
		-- Import PAML job history
		EXEC RecordSet.Import_PAML @JobID, @NewRecordSetID, @x, @SubSetIDs, @JobIDs, @JobExceptionsIDs
		
		SET @JobStatusID = (SELECT ID FROM Job.[Status] WHERE [Key] = 'Completed')
		SET @JobEndedAt = SYSDATETIME()
		EXEC Job.Job_Edit @ID = @JobID, @StatusID = @JobStatusID, @EndedAt = @JobEndedAt

		IF (@ValidateOnly = 0) BEGIN
			COMMIT TRANSACTION
		END
		ELSE BEGIN
			ROLLBACK TRANSACTION
		END
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
ALTER PROCEDURE [RecordSet].[Import_Genes]
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

	DECLARE @GeneTaxonomies TABLE ([Index] int IDENTITY(1,1), GeneID uniqueidentifier, Taxonomy varchar(4000), TaxonomyID int)
	INSERT INTO @GeneTaxonomies (GeneID, Taxonomy)
	SELECT g.value('(ID)[1]', 'uniqueidentifier')
			,g.value('(Taxonomy)[1]', 'varchar(4000)') AS Taxonomy
		FROM @x.nodes('(/Pilgrimage/RecordSet-Gene/Gene)') AS Genes(g)

	DECLARE @TaxonomyIterator int = 0
			,@TaxonomyCount int = (SELECT COUNT(*) FROM @GeneTaxonomies)
			,@Taxonomy varchar(4000)
			,@GeneID uniqueidentifier
			,@TaxonomyID int

	WHILE (@TaxonomyIterator < @TaxonomyCount) BEGIN
		SELECT @GeneID = t.GeneID
				,@Taxonomy = t.Taxonomy
			FROM @GeneTaxonomies t
			WHERE [Index] = (@TaxonomyIterator + 1)

		IF (@Taxonomy IS NOT NULL AND LTRIM(RTRIM(@Taxonomy)) <> SPACE(0)) BEGIN
			EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT

			UPDATE @GeneTaxonomies
				SET TaxonomyID = @TaxonomyID
				WHERE GeneID = @GeneID
		END

		SET @TaxonomyIterator = (@TaxonomyIterator + 1)
	END

	DECLARE @NCBI_SourceIDs TABLE (SourceID int)
	INSERT INTO @NCBI_SourceIDs SELECT ID FROM Gene.[Source] WHERE [Key] IN ('GenBank', 'BLASTN_NCBI')
	
	MERGE Gene.Gene t
	USING (SELECT g.value('(ID)[1]', 'uniqueidentifier') AS OriginalGeneID 
				,NEWID() AS NewGeneID
				,g.value('(Name)[1]', 'varchar(100)') AS Name
				,g.value('(Definition)[1]', 'varchar(1000)') AS [Definition]
				,g.value('(SourceID)[1]', 'int') AS SourceID
				,src.[Key] AS SourceKey
				,g.value('(GenBankID)[1]', 'int') AS GenBankID
				,g.value('(Locus)[1]', 'varchar(100)') AS Locus
				,g.value('(Accession)[1]', 'varchar(20)') AS Accession
				,g.value('(Organism)[1]', 'varchar(250)') AS Organism
				,g.value('(Taxonomy)[1]', 'varchar(4000)') AS Taxonomy
				,tax.TaxonomyID
				,g.value('(Nucleotides)[1]', 'varchar(MAX)') AS Nucleotides
				,g.value('(SequenceTypeID)[1]', 'int') AS SequenceTypeID
				,g.value('(Description)[1]', 'varchar(MAX)') AS [Description]
				,g.value('(LastUpdatedAt)[1]', 'datetime2(7)') AS LastUpdatedAt
				,g.value('(LastUpdateSourceID)[1]', 'int') AS LastUpdateSourceID
			FROM @x.nodes('(/Pilgrimage/RecordSet-Gene/Gene)') AS Genes(g)
			JOIN @GeneTaxonomies tax ON tax.GeneID = g.value('(ID)[1]', 'uniqueidentifier')
			JOIN Gene.[Source] src ON src.ID = g.value('(SourceID)[1]', 'int')) s
	ON (t.GenBankID = s.GenBankID
			AND (
					t.SourceID IN (2, 3) --SELECT src_ncbi.SourceID FROM @NCBI_SourceIDs src_ncbi)
					AND s.SourceID IN (2, 3)
			)
	)
	WHEN MATCHED AND s.LastUpdatedAt > t.LastUpdatedAt THEN
		UPDATE
			SET Name = s.Name
				,[Definition] = s.[Definition]
				,Locus = s.Locus
				,Accession = s.Accession
				,Organism = s.Organism
				,Taxonomy = s.Taxonomy
				,TaxonomyID = s.TaxonomyID
				,Nucleotides = s.Nucleotides
				,[Description] = s.[Description]
				,LastUpdatedAt = s.LastUpdatedAt
				,LastUpdateSourceID = s.LastUpdateSourceID
	WHEN NOT MATCHED THEN
		INSERT (ID, Name, [Definition], SourceID, GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID, Nucleotides, SequenceTypeID, [Description], LastUpdatedAt, LastUpdateSourceID)
		VALUES (s.NewGeneID, s.Name, s.[Definition], s.SourceID, s.GenBankID, s.Locus, s.Accession, s.Organism, s.Taxonomy, s.TaxonomyID, s.Nucleotides, s.SequenceTypeID, s.[Description], s.LastUpdatedAt, s.LastUpdateSourceID)
	OUTPUT s.OriginalGeneID, inserted.ID INTO @GeneIDs;
		
	MERGE Gene.NucleotideSequence t
	USING (SELECT id.Value AS GeneID
				,s.value('(Nucleotides)[1]', 'varchar(MAX)') AS Nucleotides
				,s.value('(Start)[1]', 'int') AS Start
				,s.value('(End)[1]', 'int') AS [End]
			FROM @x.nodes('(/Pilgrimage/RecordSet-Gene-Sequence/Sequence)') AS Seq(s)
			JOIN @GeneIDs id ON id.[Key] = s.value('(GeneID)[1]', 'uniqueidentifier')) s
	ON (t.GeneID = s.GeneID)
	WHEN MATCHED THEN
		UPDATE SET Nucleotides = s.Nucleotides, Start = s.Start, [End] = s.[End]
	WHEN NOT MATCHED THEN
		INSERT (GeneID, Nucleotides, Start, [End])
		VALUES (s.GeneID, s.Nucleotides, s.Start, s.[End]);
		
	SET @ProgressMessage = 'Imported ' + CAST((SELECT COUNT(*) FROM @GeneTaxonomies) AS varchar(10)) + ' gene sequence records'
	EXEC Job.Import_DataFile_Progress_Add @JobID, @ProgressMessage

	-- This DELETE only happens for NCBI-sourced records that were updated in the above MERGE on Gene.Gene.
	-- Deleting the existing features makes things easier on us for merging into Gene.Feature, instead of setting up an UPDATE.
	DELETE fi
		FROM Gene.FeatureInterval fi
		JOIN Gene.Feature f ON f.ID = fi.FeatureID
		JOIN @GeneIDs id ON id.[Key] = f.GeneID
		WHERE id.[Key] = id.Value
	DELETE f
		FROM Gene.Feature f
		JOIN @GeneIDs id ON id.[Key] = f.GeneID
		WHERE id.[Key] = id.Value
	
	MERGE INTO Gene.Feature
	USING (SELECT gene_id.Value AS ReplacementGeneID
					,f.value('(@ID)[1]', 'int') AS OriginalFeatureID
					,f.value('(@Rank)[1]', 'int') AS [Rank]
					,f.value('(@FeatureKeyID)[1]', 'int') AS FeatureKeyID
					,f.value('(@GeneQualifier)[1]', 'varchar(250)') AS GeneQualifier
					,f.value('(@GeneIDQualifier)[1]', 'int') AS GeneIDQualifier
				FROM @x.nodes('(Pilgrimage/RecordSet-Gene-Feature/Gene)') AS Gene(g)
				CROSS APPLY g.nodes('(Feature)') AS Feature(f)
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

	-- Pick up any NCBI-sourced genes that were not updated in the MERGE on Gene.Gene so that we can pop them into the RecordSet and SubSet.
	INSERT INTO @GeneIDs
	SELECT g.value('(ID)[1]', 'uniqueidentifier'), g.value('(ID)[1]', 'uniqueidentifier')
		FROM @x.nodes('(/Pilgrimage/RecordSet-Gene/Gene)') AS Genes(g)
		WHERE NOT EXISTS (SELECT * FROM @GeneIDs ex WHERE ex.[Key] = g.value('(ID)[1]', 'uniqueidentifier'))

	-- Assign the gene records to the recordset and subsets
	INSERT INTO RecordSet.SubSetGene
	SELECT subset_id.Value
			,gene_id.Value
			,g.value('(@ModifiedAt)[1]', 'datetime2(7)')
		FROM @x.nodes('(/Pilgrimage/RecordSet-SubSet-Gene/SubSet)') AS SubSet(s)
		CROSS APPLY s.nodes('(Gene)') AS Gene(g)
		JOIN @SubSetIDs subset_id ON subset_id.[Key] = s.value('(@ID)[1]', 'uniqueidentifier')
		JOIN @GeneIDs gene_id ON gene_id.[Key] = g.value('(@GeneID)[1]', 'uniqueidentifier')

	INSERT INTO RecordSet.Gene
	SELECT @RecordSetID
			,gene_id.Value
			,MAX(g.value('(@ModifiedAt)[1]', 'datetime2(7)'))
		FROM @x.nodes('(/Pilgrimage/RecordSet-SubSet-Gene/SubSet)') AS SubSet(s)
		CROSS APPLY s.nodes('(Gene)') AS Gene(g)
		JOIN @SubSetIDs subset_id ON subset_id.[Key] = s.value('(@ID)[1]', 'uniqueidentifier')
		JOIN @GeneIDs gene_id ON gene_id.[Key] = g.value('(@GeneID)[1]', 'uniqueidentifier')
		GROUP BY gene_id.Value

	DELETE FROM @GeneIDs WHERE [Key] = Value
	SET @GenesXML = Common.ConvertHashtableUniqueIdentifierToXML(@GeneIDs)
END
GO
ALTER PROCEDURE [RecordSet].[Import_Jobs]
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
			,Common.ConvertXMLFromBase64(j.value('(AdditionalProperties/Encoded)[1]', 'varchar(MAX)'))
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
ALTER PROCEDURE [RecordSet].[Import_PAML]
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
					,Common.ConvertXMLFromBase64(t.value('(ControlConfiguration/Encoded)[1]', 'varchar(MAX)')) AS ControlConfiguration
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

GO
ALTER PROCEDURE [Job].[BlastN_ListAlignmentsForJob]
	@RequestID int = NULL,
	@JobID uniqueidentifier,
	@RecordSetID uniqueidentifier,
	@FilterByID int = NULL
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @JobStatusID_Archived int

	IF @RecordSetID IS NULL AND @JobID IS NULL BEGIN
		RAISERROR('A Job ID or a RecordSet ID must be provided', 11, 1)
	END

	IF @RecordSetID IS NULL BEGIN
		SELECT @RecordSetID = j.RecordSetID
			FROM Job.Job j
			WHERE j.ID = @JobID
	END

	SELECT @JobStatusID_Archived = ID 
		FROM Job.[Status] 
		WHERE [Key] = 'Archived';

	DECLARE @SubjectIDs TABLE (SubjectID uniqueidentifier)

	INSERT INTO @SubjectIDs
	SELECT DISTINCT sbj.ID
		FROM BlastN.Alignment al
		JOIN Gene.Gene qry ON qry.ID = al.QueryID
		JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
		JOIN NCBI.BlastNAlignment nal ON nal.AlignmentID = al.ID
		JOIN NCBI.Request req ON req.ID = nal.RequestID
		JOIN Job.Job j ON j.ID = req.JobID
		WHERE j.RecordSetID = @RecordSetID
			AND 
			(
				(@JobID IS NOT NULL AND req.JobID = @JobID)
				OR
				(@JobID IS NULL AND j.StatusID <> @JobStatusID_Archived)
			)
			AND
			(
				(@RequestID IS NULL OR req.ID = @RequestID)
			)
			AND (
					(@FilterByID = 1)
					OR
					(@FilterByID = 2 AND NOT EXISTS (SELECT *
														FROM RecordSet.Gene rs_g
														JOIN Gene.Gene g ON g.ID = rs_g.GeneID
														WHERE rs_g.RecordSetID = @RecordSetID
															AND g.GenBankID = sbj.GenBankID))
					OR
					(@FilterByID = 3 AND NOT EXISTS (SELECT *
														FROM RecordSet.Gene rs_g
														JOIN Gene.Gene g ON g.ID = rs_g.GeneID
														WHERE rs_g.RecordSetID = @RecordSetID
															AND g.Organism = sbj.Organism))
			)

	SELECT sbj.ID
			,sbj.SourceID
			,sbj.GenBankID
			,sbj.Name
			,sbj.[Definition]
			,sbj.Nucleotides
			,stat.*
			,Gene.IsInRecordSet(sbj.ID, @RecordSetID) AS InRecordSet
		FROM Gene.Gene sbj
		JOIN @SubjectIDs id ON id.SubjectID = sbj.ID
		CROSS APPLY BlastN.Alignment_StatisticsTopForSubject(sbj.ID, @RecordSetID, @JobID) stat
		ORDER BY stat.MaxScore DESC

/*
	WITH Alignments AS (
		SELECT al.ID AS AlignmentID
				,al.SubjectID
				,al.[Rank] AS [Rank]
				,LEN(qry.Nucleotides) AS QuerySequenceLength
			FROM BlastN.Alignment al
			JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
			JOIN Gene.Gene qry ON qry.ID = al.QueryID
			JOIN NCBI.BlastNAlignment nal ON nal.AlignmentID = al.ID
			JOIN NCBI.Request req ON req.ID = nal.RequestID
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE
				j.RecordSetID = @RecordSetID
				AND 
				(
					(@JobID IS NOT NULL AND req.JobID = @JobID)
					OR
					(@JobID IS NULL AND j.StatusID <> @JobStatusID_Archived)
				)
				AND (
						(@FilterByID = 1)
						OR
						(@FilterByID = 2 AND NOT EXISTS (SELECT *
															FROM RecordSet.Gene rs_g
															JOIN Gene.Gene g ON g.ID = rs_g.GeneID
															WHERE rs_g.RecordSetID = @RecordSetID
																AND g.GenBankID = sbj.GenBankID))
						OR
						(@FilterByID = 3 AND NOT EXISTS (SELECT *
															FROM RecordSet.Gene rs_g
															JOIN Gene.Gene g ON g.ID = rs_g.GeneID
															WHERE rs_g.RecordSetID = @RecordSetID
																AND g.Organism = sbj.Organism))
				)
	), AlignmentExons AS (
		SELECT al_ex.AlignmentID
				,al_ex.IdentitiesCount
				,al_ex.AlignmentLength
			FROM BlastN.AlignmentExon al_ex
			JOIN Alignments al ON al.AlignmentID = al_ex.AlignmentID
	), AlignmentPercentage AS (
		SELECT al_ex.AlignmentID
				,MAX(Common.CalculatePercentageFromInt(al_ex.IdentitiesCount, al_ex.AlignmentLength)) AS AlignmentPercentage
			FROM AlignmentExons al_ex
			GROUP BY al_ex.AlignmentID
	), QueryCover AS (
		SELECT al_ex.AlignmentID
				,Common.CalculatePercentageFromInt(SUM(al_ex.AlignmentLength), al.QuerySequenceLength) AS QueryCover
			FROM AlignmentExons al_ex
			JOIN Alignments al ON al.AlignmentID = al_ex.AlignmentID
			GROUP BY al_ex.AlignmentID, al.QuerySequenceLength
	)

	SELECT sbj.ID
			,sbj.SourceID
			,sbj.GenBankID
			,sbj.[Definition]
			,sbj.SequenceStart
			,sbj.SequenceEnd
			,MAX(al.[Rank]) AS [Rank]
			,Common.MinimumMaximumFloat(MAX(perc.AlignmentPercentage), 1.0) AS AlignmentPercentage
			,Common.MinimumMaximumFloat(MAX(qcover.QueryCover), 1.0) AS QueryCover
			,CAST(CASE WHEN EXISTS (SELECT * FROM RecordSet.Gene rs_g WHERE rs_g.GeneID = sbj.ID)
					   THEN 1
					   ELSE 0
					   END AS bit) AS InRecordSet
		FROM Alignments al
		JOIN AlignmentPercentage perc ON perc.AlignmentID = al.AlignmentID
		JOIN QueryCover qcover ON qcover.AlignmentID = al.AlignmentID
		JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
		GROUP BY sbj.ID, sbj.SourceID, sbj.GenBankID, sbj.[Definition], sbj.SequenceStart, sbj.SequenceEnd
		ORDER BY [Rank], AlignmentPercentage DESC
*/
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.6.2'
	WHERE [Key] = 'DatabaseVersion'
GO