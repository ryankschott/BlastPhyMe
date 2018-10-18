SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.RecordSet_Export2')) BEGIN
	DROP PROCEDURE RecordSet.RecordSet_Export2
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.RecordSet_Export')) BEGIN
	DROP PROCEDURE RecordSet.RecordSet_Export
END
GO

-- Add capacity to import selected Genes or Results into an existing subset, instead of just creating a whole new recordset
GO
CREATE PROCEDURE RecordSet.RecordSet_Export
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
				,@JobExceptionssXML xml
	
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
	SET Value = '1.4.6.1'
	WHERE [Key] = 'DatabaseVersion'
GO