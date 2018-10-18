SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Finally making use of the generic exception table
IF (SELECT c.is_nullable FROM sys.columns c WHERE c.object_id = OBJECT_ID('Common.Exception') AND c.name = 'ParentID') = 1 BEGIN
	DELETE FROM Common.Exception
	ALTER TABLE Common.Exception DROP COLUMN ParentID
	ALTER TABLE Common.Exception ADD ParentID uniqueidentifier NOT NULL
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Common.Exception') AND c.name = 'Message') BEGIN
	ALTER TABLE Common.Exception ADD ObjectID uniqueidentifier NOT NULL
END
GO

IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.Exception_Add')) BEGIN
	DROP PROCEDURE Common.Exception_Add
END
GO
CREATE PROCEDURE Common.Exception_Add
	@ID uniqueidentifier OUTPUT
	,@Message varchar(MAX)
	,@Source varchar(MAX)
	,@StackTrace varchar(MAX)
	,@ParentID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		SET @ID = NEWID()
	END
	
	INSERT INTO Common.Exception (ID, [Message], [Source], StackTrace, ParentID)
	VALUES (@ID, @Message, @Source, @StackTrace, @ParentID)
END
GO

-- Performance improvements for BLASTN alignment results
GO
IF NOT EXISTS (SELECT * FROM sys.indexes i WHERE i.object_id = OBJECT_ID('NCBI.BlastNAlignment') AND i.name = 'IX_NCBI_BlastNAlignment_AlignmentID') BEGIN
	CREATE NONCLUSTERED INDEX IX_NCBI_BlastNAlignment_AlignmentID ON [NCBI].[BlastNAlignment] ([AlignmentID]) INCLUDE ([RequestID])
END
GO
ALTER PROCEDURE [Job].[BlastN_ListAlignmentsForJob]
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
END
GO

GO
BEGIN TRANSACTION
GO

-- Split Gene.Feature into .Feature and .FeatureInterval to preserve the separation of features.
BEGIN TRY
	IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Gene.Feature') AND c.name = 'Rank') BEGIN
		CREATE TABLE Gene.Tmp_Feature (
			ID uniqueidentifier NOT NULL,
			GeneID uniqueidentifier NOT NULL,
			[Rank] int NOT NULL,
			FeatureKeyID int NOT NULL,
			GeneQualifier varchar(250) NULL,
			GeneIDQualifier int NULL
		)

		CREATE TABLE Gene.FeatureInterval (
			FeatureID uniqueidentifier NOT NULL,
			ID int NOT NULL,
			Start int NOT NULL,
			[End] int NOT NULL,
			IsComplement bit NOT NULL CONSTRAINT DF_Gene_FeatureInterval_IsComplement DEFAULT (0),
			StartModifier char(1) NOT NULL CONSTRAINT DF_Gene_FeatureInterval_StartModifier DEFAULT ('='),
			EndModifier char(1) NOT NULL CONSTRAINT DF_Gene_FeatureInterval_EndModifier DEFAULT ('=')

			CONSTRAINT PK_Gene_FeatureInterval PRIMARY KEY CLUSTERED (FeatureID ASC, ID ASC)
		)
	END

	IF NOT EXISTS (SELECT * FROM Gene.FeatureInterval) BEGIN
		SET NOCOUNT ON

		DECLARE @GeneID uniqueidentifier,
				@LastGeneID uniqueidentifier = '00000000-0000-0000-0000-000000000000',
				@ID int,
				@Rank int,
				@FeatureKeyID int, 
				@LastFeatureKeyID int = 0,
				@FeatureID uniqueidentifier

		DECLARE cF CURSOR FOR
			SELECT f.GeneID, f.ID, f.FeatureKeyID
				FROM Gene.Feature f
		OPEN cF
			FETCH NEXT FROM cF INTO @GeneID, @ID, @FeatureKeyID
			WHILE @@FETCH_STATUS = 0 BEGIN
				IF (@GeneID <> @LastGeneID) BEGIN
					SET @LastGeneID = @GeneID
					SET @LastFeatureKeyID = 0
					SET @Rank = 0
				END
				IF (@FeatureKeyID <> @LastFeatureKeyID) BEGIN
					-- We're treating a change in feature key as a new feature block in the GenBank record, since that's generally how that worked
					-- STSs were often listed consecutively, and others may have as well, but this still gets us closest to separating by feature
					SET @LastFeatureKeyID = @FeatureKeyID
					SET @FeatureID = NEWID()

					SET @Rank += 1

					INSERT INTO Gene.Tmp_Feature (ID, GeneID, [Rank], FeatureKeyID)
					SELECT @FeatureID, @GeneID, @Rank, @FeatureKeyID
				END

				DECLARE @sql nvarchar(MAX) = 
				'INSERT INTO Gene.FeatureInterval (FeatureID, ID, Start, [End], IsComplement, StartModifier, EndModifier)
				SELECT @FeatureID
						,ISNULL((SELECT MAX(ID) FROM Gene.FeatureInterval fi WHERE fi.FeatureID = @FeatureID), 0) + 1
						,f.Start
						,f.[End]
						,f.IsComplement
						,f.StartModifier
						,f.EndModifier
					FROM Gene.Feature f
					WHERE f.GeneID = @GeneID
						AND f.ID = @ID'
						,@params nvarchar(MAX) = '@FeatureID uniqueidentifier, @GeneID uniqueidentifier, @ID int'
				EXEC sp_executesql @sql, @params, @FeatureID = @FeatureID, @GeneID = @GeneID, @ID = @ID
				
				FETCH NEXT FROM cF INTO @GeneID, @ID, @FeatureKeyID
			END
		CLOSE cF
		DEALLOCATE cF
	END

	IF EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.Tmp_Feature')) BEGIN
		EXEC ('DROP TABLE Gene.Feature')
		EXECUTE sp_rename N'Gene.Tmp_Feature', N'Feature', 'OBJECT'

		EXEC ('ALTER TABLE Gene.Feature ADD CONSTRAINT PK_Gene_Feature PRIMARY KEY CLUSTERED (ID ASC)')
		EXEC ('ALTER TABLE Gene.Feature ADD CONSTRAINT FK_Gene_Feature_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)')
		EXEC ('ALTER TABLE Gene.Feature ADD CONSTRAINT FK_Gene_Feature_FeatureKeyID FOREIGN KEY (FeatureKeyID) REFERENCES Gene.FeatureKey (ID)')
		EXEC ('ALTER TABLE Gene.FeatureInterval ADD CONSTRAINT FK_Gene_FeatureInterval_FeatureID FOREIGN KEY (FeatureID) REFERENCES Gene.Feature (ID)')
	END

	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	EXEC Common.ThrowException
END CATCH
GO
ALTER PROCEDURE [Gene].[Feature_Add]
	@GeneID uniqueidentifier
	,@Rank int
	,@FeatureKeyID int
	,@GeneQualifier varchar(250) = NULL
	,@GeneIDQualifier int = NULL
	,@ID uniqueidentifier = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	SET @ID = NEWID()

	INSERT INTO Gene.Feature (ID, GeneID, [Rank], FeatureKeyID, GeneQualifier, GeneIDQualifier)
	VALUES (@ID, @GeneID, @Rank, @FeatureKeyID, @GeneQualifier, @GeneIDQualifier)
END
GO
ALTER PROCEDURE [Gene].[Feature_Delete]
	@GeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	DELETE fi
		FROM Gene.FeatureInterval fi
		JOIN Gene.Feature f ON f.ID = fi.FeatureID
		WHERE f.GeneID = @GeneID

	DELETE FROM Gene.Feature
		WHERE GeneID = @GeneID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.FeatureInterval_Add')) BEGIN
	DROP PROCEDURE Gene.FeatureInterval_Add
END
GO
CREATE PROCEDURE Gene.FeatureInterval_Add
	@FeatureID uniqueidentifier
	,@ID int
	,@Start int
	,@End int
	,@IsComplement bit
	,@StartModifier char(1) = NULL
	,@EndModifier char(1) = NULL
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Gene.FeatureInterval (FeatureID, ID, Start, [End], IsComplement, StartModifier, EndModifier)
	VALUES (@FeatureID, @ID, @Start, @End
			,ISNULL(@IsComplement, CONVERT(bit, Common.DefaultConstraintValue('Gene.FeatureInterval', 'IsComplement')))
			,ISNULL(@StartModifier, CONVERT(char(1), Common.DefaultConstraintValue('Gene.FeatureInterval', 'StartModifier')))
			,ISNULL(@StartModifier, CONVERT(char(1), Common.DefaultConstraintValue('Gene.FeatureInterval', 'EndModifier'))))
END
GO
ALTER PROCEDURE [Gene].[Gene_GetSequenceData]
	@GeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT f.ID AS FeatureID
			,f.FeatureKeyID
			,f.[Rank]
			,f.GeneQualifier
			,f.GeneIDQualifier

			,fi.ID
			,fi.Start
			,fi.StartModifier
			,fi.[End]
			,fi.EndModifier
			,fi.IsComplement
		FROM Gene.FeatureInterval fi
		JOIN Gene.Feature f ON f.ID = fi.FeatureID
		WHERE f.GeneID = @GeneID
		ORDER BY f.[Rank], fi.ID

	--SELECT f.ID
	--		,f.FeatureKeyID
	--		,f.Start
	--		,f.StartModifier
	--		,f.[End]
	--		,f.EndModifier
	--		,f.IsComplement
	--	FROM Gene.Feature f
	--	JOIN Gene.FeatureKey k ON k.ID = f.FeatureKeyID
	--	WHERE f.GeneID = @GeneID
	--	ORDER BY k.[Rank], f.[Rank]

	SELECT seq.Nucleotides
			,seq.Start
			,seq.[End]
		FROM Gene.NucleotideSequence seq
		WHERE seq.GeneID = @GeneID
END
GO

GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Gene.FeatureKeySurvey')) BEGIN
	CREATE TABLE Gene.FeatureKeySurvey (
		GenBankID int NOT NULL,
		FeatureKey varchar(250) NOT NULL
	)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.FeatureKeySurvey_Add')) BEGIN
	DROP PROCEDURE Gene.FeatureKeySurvey_Add
END
GO
CREATE PROCEDURE Gene.FeatureKeySurvey_Add
	@GenBankID int,
	@FeatureKey varchar(250)
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM Gene.FeatureKeySurvey s WHERE s.GenBankID = @GenBankID AND s.FeatureKey = @FeatureKey) BEGIN
		INSERT INTO Gene.FeatureKeySurvey (GenBankID, FeatureKey)
		VALUES (@GenBankID, @FeatureKey)
	END
END
GO
BEGIN TRANSACTION
GO
IF (SELECT c.max_length FROM sys.columns c WHERE c.object_id = OBJECT_ID('Gene.FeatureKey') AND c.name = 'Name') = 10 BEGIN
	CREATE TABLE Gene.Tmp_FeatureKey (
		ID int NOT NULL IDENTITY (1, 1),
		Name varchar(25) NOT NULL,
		[Key] varchar(25) NOT NULL,
		[Rank] int NOT NULL)

		SET IDENTITY_INSERT Gene.Tmp_FeatureKey ON
		IF EXISTS(SELECT * FROM Gene.FeatureKey) BEGIN
			 EXEC('INSERT INTO Gene.Tmp_FeatureKey (ID, Name, [Key], Rank)
				SELECT ID, Name, [Key], Rank FROM Gene.FeatureKey WITH (HOLDLOCK TABLOCKX)')
		END
		SET IDENTITY_INSERT Gene.Tmp_FeatureKey OFF

		ALTER TABLE Gene.Feature DROP CONSTRAINT FK_Gene_Feature_FeatureKeyID
		EXEC ('DROP TABLE Gene.FeatureKey')
		EXECUTE sp_rename N'Gene.Tmp_FeatureKey', N'FeatureKey', 'OBJECT' 
		ALTER TABLE Gene.FeatureKey ADD CONSTRAINT PK_Gene_FeatureKey PRIMARY KEY CLUSTERED (ID)
		ALTER TABLE Gene.Feature ADD CONSTRAINT FK_Gene_Feature_FeatureKeyID FOREIGN KEY (FeatureKeyID) REFERENCES Gene.FeatureKey (ID)
END
GO
COMMIT TRANSACTION
GO
IF NOT EXISTS (SELECT * FROM Gene.FeatureKey fk WHERE fk.[Key] = 'misc_feature') BEGIN
	INSERT INTO Gene.FeatureKey (Name, [Key], [Rank])
	VALUES ('Misc feature', 'misc_feature', 15)
			,('Poly-A site', 'polyA_site', 12)
			,('3'' UTR', 'UTR_3', 13)
			,('5'' UTR', 'UTR_5', 14)
			,('Unsure', 'unsure', 16)
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.3.2.0'
	WHERE [Key] = 'DatabaseVersion'
GO