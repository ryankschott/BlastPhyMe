SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Convert NCBI.Request.ID from uniqueidentifier to int
BEGIN TRANSACTION
GO
BEGIN TRY
	IF (SELECT CONVERT(varchar(250), t.name) FROM sys.columns c JOIN sys.types t ON t.system_type_id = c.system_type_id WHERE c.object_id = OBJECT_ID('NCBI.Request') AND c.name = 'ID') = 'uniqueidentifier' BEGIN
		-- Drop Constraints
		ALTER TABLE NCBI.Request DROP CONSTRAINT FK_NCBI_Request_JobID
		ALTER TABLE NCBI.Request DROP CONSTRAINT DF_NCBI_Request_StartTime
		ALTER TABLE NCBI.Request DROP CONSTRAINT DF_NCBI_Request_LastUpdatedAt

		ALTER TABLE Job.Exception DROP CONSTRAINT FK_Job_Exception_RequestID
		ALTER TABLE Job.Exception DROP CONSTRAINT FK_Job_Exception_JobID
		ALTER TABLE Job.Exception DROP CONSTRAINT DF_Job_Exception_ExceptionAt

		ALTER TABLE NCBI.Gene DROP CONSTRAINT FK_NCBI_Gene_StatusID
		ALTER TABLE NCBI.Gene DROP CONSTRAINT FK_NCBI_Gene_GeneID
		ALTER TABLE NCBI.Gene DROP CONSTRAINT FK_NCBI_Gene_RequestID

		ALTER TABLE NCBI.BlastNAlignment DROP CONSTRAINT FK_NCBI_BlastNAlignment_AlignmentID
		ALTER TABLE NCBI.BlastNAlignment DROP CONSTRAINT FK_NCBI_BlastNAlignment_RequestID

		-- Create tables
		CREATE TABLE NCBI.Tmp_Request (
			ID int NOT NULL IDENTITY(1,1),
			RequestID varchar(20) NULL,
			JobID uniqueidentifier NOT NULL,
			StartTime datetime2(7) NOT NULL CONSTRAINT DF_NCBI_Request_StartTime DEFAULT (sysdatetime()),
			EndTime datetime2(7) NULL,
			LastStatus varchar(8) NULL CONSTRAINT DF_NCBI_Request_LastUpdatedAt DEFAULT (sysdatetime()),
			LastUpdatedAt datetime2(7) NOT NULL,
			StatusInformation varchar(MAX) NULL,
			TargetDatabase varchar(250) NOT NULL,
			[Algorithm] varchar(20) NOT NULL,
			OldID uniqueidentifier NOT NULL
		)

		CREATE TABLE Job.Tmp_Exception (
			ID int NOT NULL IDENTITY (1, 1),
			JobID uniqueidentifier NOT NULL,
			RequestID int NULL,
			[Message] varchar(MAX) NOT NULL,
			[Source] varchar(MAX) NULL,
			StackTrace varchar(MAX) NULL,
			ParentID int NULL,
			ExceptionAt datetime2(7) NOT NULL CONSTRAINT DF_Job_Exception_ExceptionAt DEFAULT (sysdatetime())
		)

		CREATE TABLE NCBI.Tmp_Gene (
			RequestID int NOT NULL,
			GeneID uniqueidentifier NOT NULL,
			StatusID int NOT NULL
		)

		CREATE TABLE NCBI.Tmp_BlastNAlignment (
			RequestID int NOT NULL,
			AlignmentID int NOT NULL
		)

		IF EXISTS(SELECT * FROM NCBI.Request) BEGIN
			 EXEC('INSERT INTO NCBI.Tmp_Request (RequestID, JobID, StartTime, EndTime, LastStatus, LastUpdatedAt, StatusInformation, TargetDatabase, Algorithm, OldID)
				SELECT RequestID, JobID, StartTime, EndTime, LastStatus, LastUpdatedAt, StatusInformation, TargetDatabase, Algorithm, ID 
				FROM NCBI.Request WITH (HOLDLOCK TABLOCKX)')
		END

		IF EXISTS(SELECT * FROM Job.Exception) BEGIN
			SET IDENTITY_INSERT Job.Tmp_Exception ON

			 EXEC('INSERT INTO Job.Tmp_Exception (ID, JobID, RequestID, Message, Source, StackTrace, ParentID, ExceptionAt)
				SELECT ex.ID, ex.JobID, req.ID, ex.Message, ex.Source, ex.StackTrace, ex.ParentID, ex.ExceptionAt 
				FROM Job.Exception ex WITH (HOLDLOCK TABLOCKX)
				JOIN NCBI.Tmp_Request req ON req.OldID = ex.RequestID')

			SET IDENTITY_INSERT Job.Tmp_Exception OFF
		END

		IF EXISTS(SELECT * FROM NCBI.Gene) BEGIN
			 EXEC('INSERT INTO NCBI.Tmp_Gene (RequestID, GeneID, StatusID)
				SELECT req.ID, ex.GeneID, ex.StatusID 
				FROM NCBI.Gene ex WITH (HOLDLOCK TABLOCKX)
				JOIN NCBI.Tmp_Request req ON req.OldID = ex.RequestID')
		END

		IF EXISTS(SELECT * FROM NCBI.BlastNAlignment) BEGIN
			 EXEC('INSERT INTO NCBI.Tmp_BlastNAlignment (RequestID, AlignmentID)
					SELECT req.ID, ex.AlignmentID 
					FROM NCBI.BlastNAlignment ex WITH (HOLDLOCK TABLOCKX)
					JOIN NCBI.Tmp_Request req ON req.OldID = ex.RequestID')
		END

		EXEC ('DROP TABLE NCBI.Request')
		EXECUTE sp_rename N'NCBI.Tmp_Request', N'Request', 'OBJECT' 
		EXEC ('ALTER TABLE NCBI.Request DROP COLUMN OldID')
		EXEC ('DROP TABLE Job.Exception')
		EXECUTE sp_rename N'Job.Tmp_Exception', N'Exception', 'OBJECT' 
		EXEC ('DROP TABLE NCBI.Gene')
		EXECUTE sp_rename N'NCBI.Tmp_Gene', N'Gene', 'OBJECT' 
		EXEC ('DROP TABLE NCBI.BlastNAlignment')
		EXECUTE sp_rename N'NCBI.Tmp_BlastNAlignment', N'BlastNAlignment', 'OBJECT' 

		ALTER TABLE NCBI.Request ADD CONSTRAINT PK_NCBI_Request PRIMARY KEY CLUSTERED  (ID)
		ALTER TABLE NCBI.Request ADD CONSTRAINT FK_NCBI_Request_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)

		ALTER TABLE Job.Exception ADD CONSTRAINT PK_Job_Exception PRIMARY KEY CLUSTERED (ID)
		ALTER TABLE Job.Exception ADD CONSTRAINT FK_Job_Exception_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
		ALTER TABLE Job.Exception ADD CONSTRAINT FK_Job_Exception_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)

		ALTER TABLE NCBI.Gene ADD CONSTRAINT PK_NCBI_Gene PRIMARY KEY CLUSTERED (RequestID, GeneID, StatusID)
		ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
		ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)
		ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_StatusID FOREIGN KEY (StatusID) REFERENCES Job.GeneStatus (ID)

		ALTER TABLE NCBI.BlastNAlignment ADD CONSTRAINT PK_NCBI_BlastNAlignment PRIMARY KEY CLUSTERED (RequestID, AlignmentID)
		EXEC ('CREATE NONCLUSTERED INDEX IX_NCBI_BlastNAlignment_AlignmentID ON NCBI.BlastNAlignment (AlignmentID) INCLUDE (RequestID)')
		ALTER TABLE NCBI.BlastNAlignment ADD CONSTRAINT FK_NCBI_BlastNAlignment_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
		ALTER TABLE NCBI.BlastNAlignment ADD CONSTRAINT FK_NCBI_BlastNAlignment_AlignmentID FOREIGN KEY (AlignmentID) REFERENCES BlastN.Alignment (ID)
	END

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	EXEC Common.ThrowException
END CATCH
GO
ALTER PROCEDURE [NCBI].[Request_Edit]
	@ID int = NULL OUTPUT,
	@JobID uniqueidentifier = NULL, -- One-off requests might not be associated with a job?  That might never happen.
	@TargetDatabase varchar(250) = NULL,
	@Algorithm varchar(20) = NULL,
	@RequestID varchar(20),
	@StartTime datetime2(7) = NULL,
	@EndTime datetime2(7) = NULL,
	@LastStatus varchar(8) = NULL,
	@StatusInformation varchar(MAX) = NULL,
	@LastUpdatedAt datetime2(7) = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) OR (NOT EXISTS (SELECT * FROM NCBI.Request r WHERE r.ID = @ID)) BEGIN
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

		INSERT INTO NCBI.Request (RequestID, JobID, TargetDatabase, [Algorithm], StartTime, EndTime, LastStatus, LastUpdatedAt, StatusInformation)
		VALUES (@RequestID, @JobID, 
				ISNULL(@TargetDatabase, CONVERT(varchar(250), Common.DefaultConstraintValue('NCBI.Request', 'TargetDatabase'))),
				ISNULL(@Algorithm, CONVERT(varchar(20),  Common.DefaultConstraintValue('NCBI.Request', 'Algorithm'))),
				@StartTime, @EndTime, @LastStatus, ISNULL(@LastUpdatedAt, SYSDATETIME()), @StatusInformation)

		SET @ID = @@IDENTITY
	END
	ELSE BEGIN
		UPDATE NCBI.Request
			SET RequestID = @RequestID,
				StartTime = @StartTime,
				EndTime = @EndTime,
				LastStatus = @LastStatus,
				LastUpdatedAt = ISNULL(@LastUpdatedAt, SYSDATETIME()),
				StatusInformation = @StatusInformation
			WHERE ID = @ID
	END
END
GO
ALTER PROCEDURE [NCBI].[Gene_Edit]
	@RequestID int,
	@GeneID uniqueidentifier,
	@StatusID int
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM NCBI.Gene ex
					WHERE ex.RequestID = @RequestID
						AND ex.GeneID = @GeneID) BEGIN
		INSERT INTO NCBI.Gene (RequestID, GeneID, StatusID)
		VALUES (@RequestID, @GeneID, @StatusID)
	END
	ELSE BEGIN
		UPDATE NCBI.Gene
			SET StatusID = @StatusID
			WHERE RequestID = @RequestID
				AND GeneID = @GeneID
	END
END
GO
ALTER PROCEDURE [NCBI].[Gene_EditMultiple]
	@RequestID int,
	@GeneIDs Common.ListUniqueIdentifier READONLY,
	@StatusID int
AS
BEGIN
	SET NOCOUNT ON

	UPDATE ng
		SET ng.StatusID = @StatusID
		FROM NCBI.Gene ng
		JOIN @GeneIDs g ON g.Value = ng.GeneID
		WHERE ng.RequestID = @RequestID

	INSERT INTO NCBI.Gene (RequestID, GeneID, StatusID)
	SELECT @RequestID, g.Value, @StatusID
		FROM @GeneIDs g
		WHERE NOT EXISTS (SELECT * FROM NCBI.Gene ex
							WHERE ex.RequestID = @RequestID
								AND ex.GeneID = g.Value)
END
GO
ALTER PROCEDURE [NCBI].[BlastNAlignment_Edit]
	@RequestID int,
	@AlignmentID int
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM NCBI.BlastNAlignment ex WHERE ex.RequestID = @RequestID AND ex.AlignmentID = @AlignmentID) BEGIN
		INSERT INTO NCBI.BlastNAlignment (RequestID, AlignmentID)
		VALUES (@RequestID, @AlignmentID)
	END
END
GO
ALTER PROCEDURE [Job].[Exception_Add]
	@ID int OUTPUT
	,@JobID uniqueidentifier
	,@RequestID int = NULL
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
ALTER PROCEDURE [Job].[Exception_List]
	@JobID uniqueidentifier
	,@RequestID int = NULL
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
ALTER PROCEDURE [Job].[BlastN_ListRequests]
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @History TABLE (ID int, RequestID varchar(20), StartTime datetime2(7), EndTime datetime2(7)
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
			,req.EndTime AS EndTime
			,req.LastStatus
			,ISNULL(req.StatusInformation, '')
			,req.TargetDatabase
			,req.[Algorithm]
			,rg.GeneCount
			,(0 - ROW_NUMBER() OVER (ORDER BY req.ID DESC)) AS [Rank]
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
		SELECT '0'
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
ALTER PROCEDURE [Job].[BlastN_ListQueryGenesForAlignment]
	@SubjectGeneID uniqueidentifier,
	@RequestID int,
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
ALTER PROCEDURE [RecordSet].[Import_NCBI_BlastNAlignment]
	@RequestID int
	,@AlignmentIDs Common.ListInt READONLY
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO NCBI.BlastNAlignment (RequestID, AlignmentID)
	SELECT @RequestID, ids.Value
		FROM @AlignmentIDs ids
END
GO

-- NCBI.Request.StartTime made nullable
GO
BEGIN TRANSACTION
GO
BEGIN TRY
	IF (SELECT c.is_nullable FROM sys.columns c JOIN sys.types t ON t.system_type_id = c.system_type_id WHERE c.object_id = OBJECT_ID('NCBI.Request') AND c.name = 'StartTime') = 0 BEGIN
		ALTER TABLE NCBI.Request DROP CONSTRAINT FK_NCBI_Request_JobID
		ALTER TABLE NCBI.Request DROP CONSTRAINT DF_NCBI_Request_StartTime
		ALTER TABLE NCBI.Request DROP CONSTRAINT DF_NCBI_Request_LastUpdatedAt

		CREATE TABLE NCBI.Tmp_Request (
			ID int NOT NULL IDENTITY (1, 1),
			RequestID varchar(20) NULL,
			JobID uniqueidentifier NOT NULL,
			StartTime datetime2(7) NULL,
			EndTime datetime2(7) NULL,
			LastStatus varchar(8) NULL,
			LastUpdatedAt datetime2(7) NOT NULL CONSTRAINT DF_NCBI_Request_LastUpdatedAt DEFAULT (sysdatetime()),
			StatusInformation varchar(MAX) NULL,
			TargetDatabase varchar(250) NOT NULL,
			[Algorithm] varchar(20) NOT NULL
		)

		IF EXISTS(SELECT * FROM NCBI.Request) BEGIN
			 SET IDENTITY_INSERT NCBI.Tmp_Request ON
			EXEC('INSERT INTO NCBI.Tmp_Request (ID, RequestID, JobID, StartTime, EndTime, LastStatus, LastUpdatedAt, StatusInformation, TargetDatabase, Algorithm)
					SELECT ID, RequestID, JobID, StartTime, EndTime, LastStatus, LastUpdatedAt, StatusInformation, TargetDatabase, Algorithm FROM NCBI.Request WITH (HOLDLOCK TABLOCKX)')
			SET IDENTITY_INSERT NCBI.Tmp_Request OFF
		END

		ALTER TABLE Job.Exception DROP CONSTRAINT FK_Job_Exception_RequestID
		ALTER TABLE NCBI.Gene DROP CONSTRAINT FK_NCBI_Gene_RequestID
		ALTER TABLE NCBI.BlastNAlignment DROP CONSTRAINT FK_NCBI_BlastNAlignment_RequestID

		DROP TABLE NCBI.Request
		EXECUTE sp_rename N'NCBI.Tmp_Request', N'Request', 'OBJECT' 

		ALTER TABLE NCBI.Request ADD CONSTRAINT PK_NCBI_Request PRIMARY KEY CLUSTERED (ID)
		ALTER TABLE NCBI.Request ADD CONSTRAINT FK_NCBI_Request_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
		ALTER TABLE NCBI.BlastNAlignment ADD CONSTRAINT FK_NCBI_BlastNAlignment_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
		ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
		ALTER TABLE Job.Exception ADD CONSTRAINT FK_Job_Exception_RequestID FOREIGN KEY (RequestID) REFERENCES NCBI.Request (ID)
	END
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	EXEC Common.ThrowException
END CATCH
GO

-- Adjustment to SubSet view default sorting; removed Definition as a standalone, prefering ModifiedAt DESC
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
			,sg.ModifiedAt

			,g.GenBankID
			,g.Locus
			,g.Accession
			,g.Organism
			,g.Taxonomy

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

-- Fix for top statistics in BlastN Alignment Results
GO
ALTER FUNCTION [BlastN].[Alignment_StatisticsTopForSubject] (@SubjectGeneID uniqueidentifier, @RecordSetID uniqueidentifier, @JobID uniqueidentifier)
RETURNS @Statistics TABLE ([Rank] int, MaxScore float, TotalScore float, AlignmentLength int, AlignmentPercentage float, QueryCover float)
AS
BEGIN
	INSERT INTO @Statistics
	SELECT TOP 1
			al.[Rank]
			,MAX(al_ex.BitScore) AS MaxScore
			,SUM(al_ex.BitScore)
			,SUM(al_ex.AlignmentLength)
			,Common.CalculatePercentageFromInt(SUM(al_ex.IdentitiesCount), SUM(al_ex.AlignmentLength))
			,Common.CalculatePercentageFromInt(SUM(al_ex.AlignmentLength), LEN(qry.Nucleotides))
		FROM BlastN.Alignment al
		JOIN BlastN.AlignmentExon al_ex ON al_ex.AlignmentID = al.ID
		JOIN Gene.Gene qry ON qry.ID = al.QueryID
		JOIN NCBI.BlastNAlignment n_al ON n_al.AlignmentID = al.ID
		JOIN NCBI.Request req ON req.ID = n_al.RequestID
		JOIN Job.Job j ON j.ID = req.JobID
		WHERE al.SubjectID = @SubjectGeneID
			AND j.RecordSetID = @RecordSetID
			AND (@JobID IS NULL OR j.ID = @JobID)
		GROUP BY al.ID, al.[Rank], LEN(qry.Nucleotides)
		ORDER BY al.[Rank], MaxScore DESC

	RETURN
END
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
		CROSS APPLY BlastN.Alignment_StatisticsTopForSubject(sbj.ID, @RecordSetID, @JobID) stat
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

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.3.6.0'
	WHERE [Key] = 'DatabaseVersion'
GO