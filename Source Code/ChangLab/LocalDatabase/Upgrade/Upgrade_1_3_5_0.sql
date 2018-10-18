SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- In moving away from uniqueidentifier IDs, a generic Common.Exception table won't work without a Table/Source column, which I'm not a fan of.
-- That being said, there are few use cases for storing exceptions.  Jobs are one, for sure, with gene sequence construction being the other, so
-- that's only two tables, which isn't so bad, considering the Gene.NucleotideSequenceException table might have some custom details relative to the
-- Job.Exception table.  For searching Ensembl we don't have the same conept of a "Request", because it's more like a GenBank Search, so it wouldn't
-- necessarily have a Job.
IF NOT EXISTS (SELECT * FROM sys.tables	t WHERE t.object_id = OBJECT_ID('Job.Exception')) BEGIN
	CREATE TABLE Job.Exception (
		ID int IDENTITY(1,1) NOT NULL
		,JobID uniqueidentifier NOT NULL
		,RequestID uniqueidentifier NULL
		,[Message] varchar(MAX) NOT NULL
		,[Source] varchar(MAX) NULL
		,StackTrace varchar(MAX) NULL
		,ParentID int NULL
		,ExceptionAt datetime2(7) NOT NULL CONSTRAINT DF_Job_Exception_ExceptionAt DEFAULT (sysdatetime())

		,CONSTRAINT PK_Job_Exception PRIMARY KEY CLUSTERED (ID ASC)
	)

	ALTER TABLE Job.Exception ADD CONSTRAINT FK_Job_Exception_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
	ALTER TABLE Job.Exception ADD CONSTRAINT FK_Job_Exception_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Exception_Add')) BEGIN
	DROP PROCEDURE Job.Exception_Add
END
GO
CREATE PROCEDURE Job.Exception_Add
	@ID int OUTPUT
	,@JobID uniqueidentifier
	,@RequestID uniqueidentifier = NULL
	,@Message varchar(MAX)
	,@Source varchar(MAX) = NULL
	,@StackTrace varchar(MAX) = NULL
	,@ParentID int = NULL
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Job.Exception (JobID, RequestID, [Message], [Source], StackTrace, ParentID)
	VALUES (@JobID, @RequestID, @Message, @Source, @StackTrace, @ParentID)

	SET @ID = @@IDENTITY
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Exception_List')) BEGIN
	DROP PROCEDURE Job.Exception_List
END
GO
CREATE PROCEDURE Job.Exception_List
	@JobID uniqueidentifier
	,@RequestID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT je.ID
			,je.[Message]
			,je.[Source]
			,je.StackTrace
			,je.ParentID
			,je.ExceptionAt
		FROM Job.Exception je
		WHERE je.JobID = @JobID
			AND (
				(@RequestID IS NULL AND je.RequestID IS NULL)
				OR
				(@RequestID IS NOT NULL AND je.RequestID = @RequestID)
			)
		ORDER BY je.ExceptionAt
END
GO

-- Merging NCBI.LocalDatabaseRequest with NCBI.Request - both have a target database!
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('NCBI.Request') AND c.name = 'TargetDatabase') BEGIN
	ALTER TABLE NCBI.Request ADD TargetDatabase varchar(250) NOT NULL CONSTRAINT DF_NCBI_Request_TargetDatabase DEFAULT ('nr')
	ALTER TABLE NCBI.Request ADD [Algorithm] varchar(20) NOT NULL CONSTRAINT DF_NCBI_Request_Algorithm DEFAULT ('blastn')
	
	ALTER TABLE NCBI.Request DROP CONSTRAINT DF_NCBI_Request_TargetDatabase
	ALTER TABLE NCBI.Request DROP CONSTRAINT DF_NCBI_Request_Algorithm
END
GO
IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('NCBI.LocalDatabaseRequest')) BEGIN
	DROP TABLE NCBI.LocalDatabaseRequest
	DROP PROCEDURE NCBI.LocalDatabaseRequest_Edit
END
GO
ALTER PROCEDURE [NCBI].[Request_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@RequestID varchar(20),
	@JobID uniqueidentifier = NULL, -- One-off requests might not be associated with a job?  That might never happen.
	@TargetDatabase varchar(250) = NULL,
	@Algorithm varchar(20) = NULL,
	@StartTime datetime2(7) = NULL,
	@EndTime datetime2(7) = NULL,
	@LastStatus varchar(8) = NULL,
	@StatusInformation varchar(MAX) = NULL,
	@LastUpdatedAt datetime2(7) = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) OR (NOT EXISTS (SELECT * FROM NCBI.Request r WHERE r.ID = @ID)) BEGIN
		SET @ID = NEWID()
		IF (@StartTime IS NULL) BEGIN
			SET @StartTime = SYSDATETIME()
		END

		IF @RequestID = 'ERROR' BEGIN
			-- This indicates a shell Request being saved to tag it with the genes that could not be submitted to NCBI and the accompanying error in
			-- Job.Exception, instead of just leaving the Exception floating without a RequestID.  There also might be something in particular about
			-- the genes for which the attempt to submit to NCBI was made, that the user can identify by examining them.
			
			-- The ERR_ prefix ensures we won't clash with an NCBI request ID that might be generated in the future.
			SET @RequestID = 'ERR_' + SUBSTRING(REPLACE(CONVERT(varchar(36), NEWID()), '-', ''), 1, 16)
			WHILE EXISTS (SELECT * FROM NCBI.Request req WHERE req.RequestID = @RequestID) BEGIN
				-- This is probably overkill, but it ensures that we generate a unique RequestID for the shell request.
				SET @RequestID = 'ERR_' + SUBSTRING(REPLACE(CONVERT(varchar(36), NEWID()), '-', ''), 1, 16)
			END
		END

		INSERT INTO NCBI.Request (ID, RequestID, JobID, TargetDatabase, [Algorithm], StartTime, EndTime, LastStatus, LastUpdatedAt, StatusInformation)
		VALUES (@ID, @RequestID, @JobID, 
				ISNULL(@TargetDatabase, CONVERT(varchar(250), Common.DefaultConstraintValue('NCBI.Request', 'TargetDatabase'))),
				ISNULL(@Algorithm, CONVERT(varchar(20),  Common.DefaultConstraintValue('NCBI.Request', 'Algorithm'))),
				@StartTime, @EndTime, @LastStatus, ISNULL(@LastUpdatedAt, SYSDATETIME()), @StatusInformation)
	END
	ELSE BEGIN
		UPDATE NCBI.Request
			SET StartTime = @StartTime,
				EndTime = @EndTime,
				LastStatus = @LastStatus,
				LastUpdatedAt = ISNULL(@LastUpdatedAt, SYSDATETIME()),
				StatusInformation = @StatusInformation
			WHERE ID = @ID
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_NCBI_Request')) BEGIN
	DROP PROCEDURE RecordSet.Import_NCBI_Request
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.BlastN_ListRequests')) BEGIN
	DROP PROCEDURE Job.BlastN_ListRequests
END
GO
CREATE PROCEDURE Job.BlastN_ListRequests
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @History TABLE (ID uniqueidentifier, RequestID varchar(20), StartTime datetime2(7), EndTime datetime2(7)
								,LastStatus varchar(8), StatusInformation varchar(MAX)
								,TargetDatabase varchar(250), [Algorithm] varchar(8), GeneCount int, [Rank] int);

	WITH RequestGenes AS (
		SELECT req.ID AS RequestID
				,COUNT(*) AS GeneCount
			FROM NCBI.Request req
			JOIN NCBI.Gene ng ON ng.RequestID = req.ID
			WHERE req.JobID = @JobID
			GROUP BY req.ID
	)

	INSERT INTO @History
	SELECT req.ID
			,req.RequestID
			,req.StartTime
			,ISNULL(req.EndTime, req.LastUpdatedAt) AS EndTime
			,req.LastStatus
			,ISNULL(req.StatusInformation, '')
			,req.TargetDatabase
			,req.[Algorithm]
			,rg.GeneCount
			,(0 - ROW_NUMBER() OVER (ORDER BY req.StartTime DESC)) AS [Rank]
		FROM NCBI.Request req
		JOIN RequestGenes rg ON rg.RequestID = req.ID
		WHERE req.JobID = @JobID;

	-- Theoretically there shouldn't be any of these, because all batches will at least get associated with a shell Request, but if some critical
	-- error failed out of the Job then there might be some that didn't get tagged.
	DECLARE @GenesWithoutARequest TABLE (GeneID uniqueidentifier)
	INSERT INTO @GenesWithoutARequest
	SELECT jg.GeneID
		FROM Job.Gene jg
		JOIN Job.GeneDirection dir ON dir.ID = jg.DirectionID
		WHERE jg.JobID = @JobID
			AND dir.[Key] = 'Input' 
			AND NOT EXISTS (SELECT *
								FROM NCBI.Request req
								JOIN NCBI.Gene ng ON ng.RequestID = req.ID
								WHERE req.JobID = @JobID
									AND ng.GeneID = jg.GeneID)
	
	IF EXISTS (SELECT * FROM @GenesWithoutARequest) BEGIN
		-- We only want to show the unsubmitted row if there were any genes not submitted.
		INSERT INTO @History
		SELECT '00000000-0000-0000-0000-000000000000'
				,'Not Processed'
				,NULL
				,NULL
				,CASE WHEN EXISTS (SELECT * FROM Job.Exception ex WHERE ex.JobID = @JobID AND ex.RequestID IS NULL)
					  THEN 'Error' -- Again, this is picking up on the idea of a critical error failing out of the job.
					  ELSE ''
					  END -- LastStatus
				,''
				,''
				,''
				,COUNT(*) -- GeneCount
				,0
			FROM @GenesWithoutARequest
	END
	
	SELECT * 
		FROM @History h
		ORDER BY h.[Rank]
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.BlastN_ListNotProcessedGenes')) BEGIN
	DROP PROCEDURE Job.BlastN_ListNotProcessedGenes
END
GO
CREATE PROCEDURE Job.BlastN_ListNotProcessedGenes
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT qry.ID
			,qry.GenBankID
			,qry.[Definition]
			,qry.Organism
			,qry.Nucleotides -- Used to calculate the sequence length
		FROM Job.Gene jg
		JOIN Job.GeneDirection dir ON dir.ID = jg.DirectionID
		JOIN Gene.Gene qry ON qry.ID = jg.GeneID
		WHERE jg.JobID = @JobID
			AND dir.[Key] = 'Input' 
			AND NOT EXISTS (SELECT *
								FROM NCBI.Gene ng
								JOIN NCBI.Request req ON req.JobID = @JobID AND req.ID = ng.RequestID
								JOIN Job.GeneStatus stat ON stat.ID = ng.StatusID
								WHERE ng.GeneID = jg.GeneID
									AND stat.[Key] IN ('Processed'))
		ORDER BY qry.Organism, qry.[Definition]
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Job.Job') AND c.name = 'Active') BEGIN
	ALTER TABLE Job.Job ADD Active bit NOT NULL CONSTRAINT DF_Job_Job_Active DEFAULT ((1))

	DECLARE @JStat_Completed varchar(5) = CAST((SELECT ID FROM Job.[Status] WHERE [Key] = 'Completed') AS varchar(5))
			,@JStat_Cancelled varchar(5) = CAST((SELECT ID FROM Job.[Status] WHERE [Key] = 'Cancelled') AS varchar(5))
	EXEC ('
	UPDATE j
		SET j.StatusID =
			CASE WHEN EXISTS (SELECT *
								FROM NCBI.Gene ng
								JOIN NCBI.Request req ON req.ID = ng.RequestID
								JOIN Job.GeneStatus stat ON stat.ID = ng.StatusID
								WHERE req.JobID = j.ID
									AND stat.[Key] IN (''NotSubmitted'', ''Submitted''))
				THEN ' + @JStat_Cancelled + '
				ELSE ' + @JStat_Completed + '
				END
			,j.Active = 0
		FROM Job.Job j
		JOIN Job.[Status] jstat ON jstat.ID = j.StatusID
		WHERE jstat.[Key] = ''Archived''
	')
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.Job_UpdateStatus')) BEGIN
	DROP PROCEDURE Job.Job_UpdateStatus
END
GO
ALTER PROCEDURE [Job].[Job_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@SubSetID uniqueidentifier = NULL, -- Not used in an UPDATE
	@TargetID int = NULL, -- Not used in an UPDATE
	@StatusID int = NULL,
	@StartedAt datetime2(7) = NULL, -- Not used in an UPDATE
	@EndedAt datetime2(7) = NULL,
	@Active bit = NULL
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @RecordSetID uniqueidentifier

	IF NOT EXISTS (SELECT * FROM Job.Job j WHERE j.ID = @ID) BEGIN
		SET @ID = NEWID()

		SELECT @RecordSetID = sub.RecordSetID
			FROM RecordSet.SubSet sub
			WHERE sub.ID = @SubSetID

		INSERT INTO Job.Job (ID, RecordSetID, SubSetID, TargetID, StartedAt, EndedAt)
		VALUES (@ID, @RecordSetID, @SubSetID, @TargetID, @StartedAt, @EndedAt)
	END
	ELSE BEGIN
		UPDATE Job.Job
			SET StatusID = ISNULL(@StatusID, StatusID)
				,EndedAt = ISNULL(@EndedAt, EndedAt)
				,Active = ISNULL(@Active, Active)
			WHERE ID = @ID
	END
END
GO
ALTER PROCEDURE [Job].[Job_List]
	@RecordSetID uniqueidentifier,
	@TargetID int,
	@StatusID int = NULL,
	@Active bit = 1
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
			,ISNULL(sub.Name, '') AS SubSetName
		FROM Job.Job j
		JOIN Job.[Target] jt ON jt.ID = j.TargetID
		JOIN Job.[Status] js ON js.ID = j.StatusID
		LEFT OUTER JOIN RecordSet.SubSet sub ON sub.ID = j.SubSetID
		WHERE j.RecordSetID = @RecordSetID
			AND j.TargetID = @TargetID
			AND j.Active = @Active
			AND ((@StatusID IS NULL) OR (j.StatusID = @StatusID))
		ORDER BY j.StartedAt DESC
END
GO
ALTER PROCEDURE [Job].[BlastN_ListAlignmentsForJob]
	@RequestID uniqueidentifier = NULL,
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
			,sbj.[Definition]
			,sbj.SequenceStart
			,sbj.SequenceEnd
			,stat.*
			,CAST(CASE WHEN EXISTS (SELECT * FROM RecordSet.Gene rs_g WHERE rs_g.RecordSetID = @RecordSetID AND rs_g.GeneID = sbj.ID)
						   THEN 1
						   ELSE 0
						   END AS bit) AS InRecordSet
		FROM Gene.Gene sbj
		JOIN @SubjectIDs id ON id.SubjectID = sbj.ID
		CROSS APPLY BlastN.Alignment_StatisticsTopForSubject(sbj.ID) stat
		ORDER BY stat.[Rank], stat.MaxScore DESC

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
ALTER PROCEDURE [Job].[BlastN_ListQueryGenesForAlignment]
	@SubjectGeneID uniqueidentifier,
	@RequestID uniqueidentifier,
	@JobID uniqueidentifier,
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF @RecordSetID IS NULL AND @JobID IS NULL AND @RequestID IS NULL BEGIN
		RAISERROR('A Request ID, Job ID, or RecordSet ID must be provided', 11, 1)
	END
		
	IF @RecordSetID IS NULL BEGIN
		IF @JobID IS NOT NULL BEGIN
			SELECT @RecordSetID = j.RecordSetID
				FROM Job.Job j
				WHERE j.ID = @JobID
		END
		ELSE BEGIN
			SELECT @RecordSetID = j.RecordSetID
					,@JobID = j.ID
				FROM NCBI.Request req
				JOIN Job.Job j ON j.ID = req.JobID
				WHERE req.ID = @RequestID
		END
	END

	IF @SubjectGeneID IS NOT NULL BEGIN
		SELECT qry.ID
				,qsrc.Name AS QuerySourceName
				,qry.GenBankID
				,qry.[Definition]
				,qry.Organism
				,qry.Taxonomy
				,stat.*
			FROM NCBI.Request r
			JOIN NCBI.BlastNAlignment nal ON nal.RequestID = r.ID
			JOIN BlastN.Alignment al ON al.ID = nal.AlignmentID
			JOIN BlastN.AlignmentExon ex ON ex.ID = BlastN.AlignmentExon_First(al.ID)
			JOIN Gene.Gene qry ON qry.ID = al.QueryID
			JOIN Gene.[Source] qsrc ON qsrc.ID = qry.SourceID
			JOIN Job.Job j ON j.ID = r.JobID
			CROSS APPLY BlastN.Alignment_Statistics(al.QueryID, al.SubjectID) stat
			WHERE al.SubjectID = @SubjectGeneID
				AND j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
			ORDER BY stat.[Rank], stat.MaxScore DESC
	END
	ELSE BEGIN
		SELECT DISTINCT
				qry.ID
				,qry.GenBankID
				,qry.[Definition]
				,qry.Organism
				,qry.Nucleotides -- Used to calculate the sequence length
			FROM NCBI.Request r
			JOIN NCBI.Gene ng ON ng.RequestID = r.ID
			JOIN Gene.Gene qry ON qry.ID = ng.GeneID
			JOIN Job.Job j ON j.ID = r.JobID
			WHERE j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
				AND (@RequestID IS NULL OR r.ID = @RequestID)
			ORDER BY qry.Organism, qry.[Definition]
	END
END
GO

-- Adding Accession to Gene.FeatureInterval
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Gene.FeatureInterval') AND c.name = 'Accession') BEGIN
	ALTER TABLE Gene.FeatureInterval ADD Accession varchar(20) NULL
END
GO
ALTER PROCEDURE [Gene].[FeatureInterval_Add]
	@ID int
	,@FeatureID int
	,@Start int
	,@End int
	,@IsComplement bit
	,@StartModifier char(1) = NULL
	,@EndModifier char(1) = NULL
	,@Accession varchar(20) = NULL
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Gene.FeatureInterval (ID, FeatureID, Start, [End], IsComplement, StartModifier, EndModifier, Accession)
	VALUES (@ID, @FeatureID, @Start, @End
			,ISNULL(@IsComplement, CONVERT(bit, Common.DefaultConstraintValue('Gene.FeatureInterval', 'IsComplement')))
			,ISNULL(@StartModifier, CONVERT(char(1), Common.DefaultConstraintValue('Gene.FeatureInterval', 'StartModifier')))
			,ISNULL(@StartModifier, CONVERT(char(1), Common.DefaultConstraintValue('Gene.FeatureInterval', 'EndModifier')))
			,@Accession)
END
GO
ALTER PROCEDURE [RecordSet].[RecordSet_Export]
	@RecordSetID uniqueidentifier
	,@JobHistory bit = 0
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @GeneIDs TABLE (GeneID uniqueidentifier)
	INSERT INTO @GeneIDs
	SELECT g.ID
		FROM Gene.Gene g
		JOIN RecordSet.SubSetGene sg ON sg.GeneID = g.ID
		JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
		JOIN RecordSet.RecordSet rs ON rs.ID = sub.RecordSetID
		WHERE rs.ID = @RecordSetID
			AND sub.Active = 1
			AND g.Active = 1
	UNION
	-- Pick up the Genes from BLASTN results that aren't already in the main Gene pool; UNION will DISTINCT this all for us
	SELECT g.ID
		FROM Gene.Gene g
		JOIN BlastN.Alignment al ON al.SubjectID = g.ID
		JOIN NCBI.BlastNAlignment n_al ON n_al.AlignmentID = al.ID
		JOIN NCBI.Request req ON req.ID = n_al.RequestID
		JOIN Job.Job j ON j.ID = req.JobID
		WHERE j.RecordSetID = @RecordSetID
			AND g.Active = 1

	IF (@JobHistory = 0) BEGIN
		SELECT (SELECT [Properties].Value AS [DatabaseVersion]
					FROM Common.ApplicationProperty [Properties]
					WHERE [Properties].[Key] = 'DatabaseVersion'
					FOR XML AUTO, ELEMENTS)
		UNION ALL
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
						,[SubSet].LastOpenedAt
						,[SubSet].[Open]
						,[SubSet-Gene].GeneID
						,[SubSet-Gene].ModifiedAt
					FROM RecordSet.RecordSet [RecordSet]
					JOIN RecordSet.SubSet [SubSet] ON [SubSet].RecordSetID = [RecordSet].ID
					JOIN RecordSet.SubSetGene [SubSet-Gene] ON [SubSet-Gene].SubSetID = [SubSet].ID
					JOIN Gene.Gene g ON g.ID = [SubSet-Gene].GeneID
					WHERE [RecordSet].ID = @RecordSetID
						AND [SubSet].Active = 1
						AND g.Active = 1
					ORDER BY [SubSet].Name
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-SubSet'))
		UNION ALL
		SELECT (SELECT [Gene].*
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
		SELECT (SELECT [Feature].*
						,[Feature-Interval].ID
						,[Feature-Interval].Start
						,[Feature-Interval].[End]
						,[Feature-Interval].IsComplement
						,[Feature-Interval].StartModifier
						,[Feature-Interval].EndModifier
						,[Feature-Interval].Accession
					FROM Gene.Feature [Feature]
					JOIN Gene.FeatureInterval [Feature-Interval] ON [Feature-Interval].FeatureID = [Feature].ID
					JOIN @GeneIDs g ON g.GeneID = [Feature].GeneID
					ORDER BY [Feature].ID, [Feature-Interval].ID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Gene-Feature'))
	END
	ELSE BEGIN
		SELECT (SELECT [Job].ID
						,[Job].TargetID
						,[Job].StartedAt
						,[Job].EndedAt
						,[Job].StatusID
						,[Job].SubSetID
						,[Job-Gene].GeneID
						,[Job-Gene].DirectionID
					FROM Job.Job [Job]
					JOIN Job.Gene [Job-Gene] ON [Job-Gene].JobID = [Job].ID
					WHERE [Job].RecordSetID = @RecordSetID
						AND EXISTS (SELECT *
										FROM RecordSet.SubSetGene sg
										JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
										WHERE sg.GeneID = [Job-Gene].GeneID
											AND sub.RecordSetID = @RecordSetID
											AND sub.Active = 1)
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Job'))
		UNION ALL
		SELECT (SELECT [Request].*
						,[Request-Gene].GeneID
						,[Request-Gene].StatusID
					FROM NCBI.Request [Request]
					JOIN Job.Job j ON j.ID = [Request].JobID
					JOIN NCBI.Gene [Request-Gene] ON [Request-Gene].RequestID = [Request].ID
					WHERE j.RecordSetID = @RecordSetID
						AND EXISTS (SELECT *
										FROM RecordSet.SubSetGene sg
										JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
										WHERE sg.GeneID = [Request-Gene].GeneID
											AND sub.RecordSetID = @RecordSetID
											AND sub.Active = 1)
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-NCBI-Request'))
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
					JOIN Job.Job j ON j.ID = req.JobID
					JOIN Gene.Gene qry ON qry.ID = Alignment.QueryID
					JOIN Gene.Gene sbj ON sbj.ID = Alignment.SubjectID
					WHERE qry.Active = 1
						AND sbj.Active = 1
						AND j.RecordSetID = @RecordSetID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Alignment'))
		UNION ALL
		SELECT (SELECT [Request].ID
						,(SELECT [Alignment].AlignmentID
								FROM NCBI.BlastNAlignment [Alignment] 
								WHERE [Alignment].RequestID = [Request].ID
								FOR XML PATH(''), TYPE) AS "Alignments"
					FROM NCBI.Request [Request]
					JOIN Job.Job j ON j.ID = [Request].JobID
					WHERE j.RecordSetID = @RecordSetID
						AND EXISTS (SELECT * FROM NCBI.BlastNAlignment ex WHERE ex.RequestID = [Request].ID)
					FOR XML PATH('Request'), ROOT ('RecordSet-Request-Alignment'))
	END
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.3.5.0'
	WHERE [Key] = 'DatabaseVersion'
GO