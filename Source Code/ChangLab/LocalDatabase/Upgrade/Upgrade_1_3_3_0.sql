SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Gene.Feature and Gene.FeatureInterval
BEGIN TRANSACTION
GO
BEGIN TRY
	IF (SELECT CONVERT(varchar(250), t.name) FROM sys.columns c JOIN sys.types t ON t.system_type_id = c.system_type_id WHERE c.object_id = OBJECT_ID('Gene.Feature') AND c.name = 'ID') = 'uniqueidentifier' BEGIN
		ALTER TABLE Gene.FeatureInterval DROP CONSTRAINT FK_Gene_FeatureInterval_FeatureID
		ALTER TABLE Gene.FeatureInterval DROP CONSTRAINT DF_Gene_FeatureInterval_IsComplement
		ALTER TABLE Gene.FeatureInterval DROP CONSTRAINT DF_Gene_FeatureInterval_StartModifier
		ALTER TABLE Gene.FeatureInterval DROP CONSTRAINT DF_Gene_FeatureInterval_EndModifier

		CREATE TABLE Gene.Tmp_Feature
			(
			ID int identity (1,1) NOT NULL,
			GeneID uniqueidentifier NOT NULL,
			[Rank] int NOT NULL,
			FeatureKeyID int NOT NULL,
			GeneQualifier varchar(250) NULL,
			GeneIDQualifier int NULL,
			OldID uniqueidentifier NOT NULL
			)  ON [PRIMARY]

		IF EXISTS(SELECT * FROM Gene.Feature) BEGIN
			 EXEC('INSERT INTO Gene.Tmp_Feature (GeneID, Rank, FeatureKeyID, GeneQualifier, GeneIDQualifier, OldID)
				SELECT GeneID, Rank, FeatureKeyID, GeneQualifier, GeneIDQualifier, ID FROM Gene.Feature WITH (HOLDLOCK TABLOCKX)')
		END

		CREATE TABLE Gene.Tmp_FeatureInterval (
			ID int NOT NULL,
			FeatureID int NOT NULL,
			Start int NOT NULL,
			[End] int NOT NULL,
			IsComplement bit NOT NULL CONSTRAINT DF_Gene_FeatureInterval_IsComplement DEFAULT ((0)),
			StartModifier char(1) NOT NULL CONSTRAINT DF_Gene_FeatureInterval_StartModifier DEFAULT ('='),
			EndModifier char(1) NOT NULL CONSTRAINT DF_Gene_FeatureInterval_EndModifier DEFAULT ('=')
		)

		IF EXISTS(SELECT * FROM Gene.FeatureInterval) BEGIN
			EXEC ('INSERT INTO Gene.Tmp_FeatureInterval (ID, FeatureID, Start, [End], IsComplement, StartModifier, EndModifier)
			SELECT fi.ID, f.ID, Start, [End], IsComplement, StartModifier, EndModifier 
				FROM Gene.FeatureInterval fi WITH (HOLDLOCK TABLOCKX)
				JOIN Gene.Tmp_Feature f ON f.OldID = fi.FeatureID')
		END

		EXEC ('DROP TABLE Gene.Feature')
		EXECUTE sp_rename N'Gene.Tmp_Feature', N'Feature', 'OBJECT' 
		EXEC ('DROP TABLE Gene.FeatureInterval')
		EXECUTE sp_rename N'Gene.Tmp_FeatureInterval', N'FeatureInterval', 'OBJECT' 

		ALTER TABLE Gene.Feature DROP COLUMN OldID

		ALTER TABLE Gene.Feature ADD CONSTRAINT PK_Gene_Feature PRIMARY KEY CLUSTERED (ID ASC)
		ALTER TABLE Gene.Feature ADD CONSTRAINT FK_Gene_Feature_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID)
		ALTER TABLE Gene.Feature ADD CONSTRAINT FK_Gene_Feature_FeatureKeyID FOREIGN KEY (FeatureKeyID) REFERENCES Gene.FeatureKey (ID)

		ALTER TABLE Gene.FeatureInterval ADD CONSTRAINT PK_Gene_FeatureInterval PRIMARY KEY CLUSTERED (FeatureID ASC, ID ASC)
		ALTER TABLE Gene.FeatureInterval ADD CONSTRAINT FK_Gene_FeatureInterval_FeatureID FOREIGN KEY (FeatureID) REFERENCES Gene.Feature (ID)
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
	,@ID int = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Gene.Feature (GeneID, [Rank], FeatureKeyID, GeneQualifier, GeneIDQualifier)
	VALUES (@GeneID, @Rank, @FeatureKeyID, @GeneQualifier, @GeneIDQualifier)

	SET @ID = @@IDENTITY
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
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Gene.FeatureInterval (ID, FeatureID, Start, [End], IsComplement, StartModifier, EndModifier)
	VALUES (@ID, @FeatureID, @Start, @End
			,ISNULL(@IsComplement, CONVERT(bit, Common.DefaultConstraintValue('Gene.FeatureInterval', 'IsComplement')))
			,ISNULL(@StartModifier, CONVERT(char(1), Common.DefaultConstraintValue('Gene.FeatureInterval', 'StartModifier')))
			,ISNULL(@StartModifier, CONVERT(char(1), Common.DefaultConstraintValue('Gene.FeatureInterval', 'EndModifier'))))
END
GO

-- BlastN.Alignment, BlastN.AlignmentExon, and NCBI.BlastNAlignment
BEGIN TRANSACTION
GO
BEGIN TRY
	IF (SELECT CONVERT(varchar(250), t.name) FROM sys.columns c JOIN sys.types t ON t.system_type_id = c.system_type_id WHERE c.object_id = OBJECT_ID('BlastN.Alignment') AND c.name = 'ID') = 'uniqueidentifier' BEGIN
		-- Drop constraints
		ALTER TABLE BlastN.Alignment DROP CONSTRAINT FK_BlastN_Alignment_QueryID
		ALTER TABLE BlastN.Alignment DROP CONSTRAINT FK_BlastN_Alignment_SubjectID
		ALTER TABLE BlastN.Alignment DROP CONSTRAINT DF_BlastN_Alignment_LastUpdatedAt

		ALTER TABLE BlastN.AlignmentExon DROP CONSTRAINT FK_BlastN_AlignmentExon_OrientationID
		ALTER TABLE BlastN.AlignmentExon DROP CONSTRAINT FK_BlastN_AlignmentExon_AlignmentID

		ALTER TABLE NCBI.BlastNAlignment DROP CONSTRAINT FK_NCBI_BlastNAlignment_AlignmentID
		ALTER TABLE NCBI.BlastNAlignment DROP CONSTRAINT FK_NCBI_BlastNAlignment_RequestID

		-- Create Tables
		CREATE TABLE BlastN.Tmp_Alignment (
			ID int IDENTITY(1,1) NOT NULL,
			QueryID uniqueidentifier NOT NULL,
			SubjectID uniqueidentifier NOT NULL,
			Rank int NOT NULL,
			LastUpdatedAt datetime2(7) NOT NULL CONSTRAINT DF_BlastN_Alignment_LastUpdatedAt DEFAULT (sysdatetime()),
			OldID uniqueidentifier
		)

		IF EXISTS(SELECT * FROM BlastN.Alignment) BEGIN
			 EXEC('INSERT INTO BlastN.Tmp_Alignment (QueryID, SubjectID, Rank, LastUpdatedAt, OldID)
				SELECT QueryID, SubjectID, Rank, LastUpdatedAt, ID FROM BlastN.Alignment WITH (HOLDLOCK TABLOCKX)')
		END

		CREATE TABLE BlastN.Tmp_AlignmentExon (
			ID int IDENTITY(1,1) NOT NULL,
			AlignmentID int NOT NULL,
			OrientationID int NOT NULL,
			BitScore float(53) NOT NULL,
			AlignmentLength int NOT NULL,
			IdentitiesCount int NOT NULL,
			Gaps int NOT NULL,
			QueryRangeStart int NOT NULL,
			QueryRangeEnd int NOT NULL,
			SubjectRangeStart int NOT NULL,
			SubjectRangeEnd int NOT NULL
		)

		IF EXISTS(SELECT * FROM BlastN.AlignmentExon) BEGIN
			 EXEC ('INSERT INTO BlastN.Tmp_AlignmentExon (AlignmentID, OrientationID, BitScore, AlignmentLength, IdentitiesCount, Gaps, QueryRangeStart, QueryRangeEnd, SubjectRangeStart, SubjectRangeEnd)
				SELECT al.ID, OrientationID, BitScore, AlignmentLength, IdentitiesCount, Gaps, QueryRangeStart, QueryRangeEnd, SubjectRangeStart, SubjectRangeEnd 
					FROM BlastN.AlignmentExon al_ex WITH (HOLDLOCK TABLOCKX)
					JOIN BlastN.Tmp_Alignment al ON al.OldID = al_ex.AlignmentID')
		END

		CREATE TABLE NCBI.Tmp_BlastNAlignment (
			RequestID uniqueidentifier NOT NULL,
			AlignmentID int NOT NULL
		)

		IF EXISTS(SELECT * FROM NCBI.BlastNAlignment) BEGIN
			 INSERT INTO NCBI.Tmp_BlastNAlignment (RequestID, AlignmentID)
				SELECT n_al.RequestID, al.ID
					FROM NCBI.BlastNAlignment n_al WITH (HOLDLOCK TABLOCKX)
					JOIN BlastN.Tmp_Alignment al ON al.OldID = n_al.AlignmentID
		END

		EXEC ('DROP TABLE BlastN.Alignment')
		EXECUTE sp_rename N'BlastN.Tmp_Alignment', N'Alignment', 'OBJECT' 
		EXEC ('DROP TABLE BlastN.AlignmentExon')
		EXECUTE sp_rename N'BlastN.Tmp_AlignmentExon', N'AlignmentExon', 'OBJECT' 
		EXEC ('DROP TABLE NCBI.BlastNAlignment')
		EXECUTE sp_rename N'NCBI.Tmp_BlastNAlignment', N'BlastNAlignment', 'OBJECT' 

		ALTER TABLE BlastN.Alignment DROP COLUMN OldID

		-- BlastN.Alignment
		ALTER TABLE BlastN.Alignment ADD CONSTRAINT PK_BlastN_Alignment PRIMARY KEY CLUSTERED (ID)

		CREATE UNIQUE NONCLUSTERED INDEX IX_BlastN_Alignment ON BlastN.Alignment (QueryID, SubjectID)
		CREATE NONCLUSTERED INDEX IX_BlastN_Alignment_QueryID ON BlastN.Alignment (QueryID)
		CREATE NONCLUSTERED INDEX IX_BlastN_Alignment_SubjectID ON BlastN.Alignment (SubjectID)

		ALTER TABLE BlastN.Alignment ADD CONSTRAINT FK_BlastN_Alignment_QueryID FOREIGN KEY (QueryID) REFERENCES Gene.Gene (ID)
		ALTER TABLE BlastN.Alignment ADD CONSTRAINT FK_BlastN_Alignment_SubjectID FOREIGN KEY (SubjectID) REFERENCES Gene.Gene (ID)

		-- BlastN.AlignmentExon
		ALTER TABLE BlastN.AlignmentExon ADD CONSTRAINT PK_BlastN_AlignmentExon PRIMARY KEY CLUSTERED (ID)
		CREATE NONCLUSTERED INDEX IX_BlastN_AlignmentExon_AlignmentID ON BlastN.AlignmentExon (AlignmentID)

		ALTER TABLE BlastN.AlignmentExon ADD CONSTRAINT FK_BlastN_AlignmentExon_AlignmentID FOREIGN KEY (AlignmentID) REFERENCES BlastN.Alignment (ID)
		ALTER TABLE BlastN.AlignmentExon ADD CONSTRAINT FK_BlastN_AlignmentExon_OrientationID FOREIGN KEY (OrientationID) REFERENCES Gene.ExonOrientation (ID)

		-- NCBI.BlastNAlignment
		ALTER TABLE NCBI.BlastNAlignment ADD CONSTRAINT PK_NCBI_BlastNAlignment PRIMARY KEY CLUSTERED (RequestID, AlignmentID)
		CREATE NONCLUSTERED INDEX IX_NCBI_BlastNAlignment_AlignmentID ON NCBI.BlastNAlignment (AlignmentID) INCLUDE (RequestID) 

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

ALTER PROCEDURE [NCBI].[BlastNAlignment_Edit]
	@RequestID uniqueidentifier,
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
ALTER PROCEDURE [BlastN].[AlignmentExon_Edit]
	@ID int = NULL OUTPUT,
	@AlignmentID int,
	@OrientationID int,
	@BitScore float,
	@AlignmentLength int,
	@IdentitiesCount int,
	@Gaps int,
	@QueryRangeStart int,
	@QueryRangeEnd int,
	@SubjectRangeStart int,
	@SubjectRangeEnd int
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM BlastN.AlignmentExon ae WHERE ae.ID = @ID) BEGIN
		INSERT INTO BlastN.AlignmentExon (AlignmentID, OrientationID, 
											BitScore, AlignmentLength, IdentitiesCount, Gaps, 
											QueryRangeStart, QueryRangeEnd, SubjectRangeStart, SubjectRangeEnd)
		VALUES (@AlignmentID, @OrientationID,
				@BitScore, @AlignmentLength, @IdentitiesCount, @Gaps,
				@QueryRangeStart, @QueryRangeEnd, @SubjectRangeStart, @SubjectRangeEnd)

		SET @ID = @@IDENTITY
	END
	-- I'm not sure that there's a use case for an UPDATE.
END
GO
ALTER PROCEDURE [BlastN].[Alignment_Edit]
	@ID int = NULL OUTPUT,
	@QueryID uniqueidentifier,
	@SubjectID uniqueidentifier,
	@Rank int,
	@ClearExons bit = 0
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM BlastN.Alignment ex
						WHERE ex.QueryID = @QueryID
							AND ex.SubjectID = @SubjectID) BEGIN
		INSERT INTO BlastN.Alignment (QueryID, SubjectID, [Rank])
		VALUES (@QueryID, @SubjectID, @Rank)

		SET @ID = @@IDENTITY
	END
	ELSE BEGIN
		SELECT @ID = ex.ID 
			FROM BlastN.Alignment ex
			WHERE ex.QueryID = @QueryID
				AND ex.SubjectID = @SubjectID

		UPDATE BlastN.Alignment
			SET [Rank] = @Rank
				,LastUpdatedAt = sysdatetime()
			WHERE ID = @ID
	END

	IF @ClearExons = 1 BEGIN
		DELETE FROM BlastN.AlignmentExon
			WHERE AlignmentID = @ID
	END
END
GO

GO
ALTER FUNCTION [BlastN].[Alignment_MaxAlignmentPercentage](@AlignmentID int)
RETURNS int
AS
BEGIN
	RETURN (SELECT MAX(Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength))
				FROM BlastN.AlignmentExon ex
				WHERE ex.AlignmentID = @AlignmentID)
END
GO
ALTER FUNCTION [BlastN].[Alignment_QueryCover](@AlignmentID int)
RETURNS float
AS
BEGIN

	RETURN (
		SELECT Common.CalculatePercentageFromInt(

			(SELECT SUM(ex.AlignmentLength)
				FROM BlastN.AlignmentExon ex
				WHERE ex.AlignmentID = @AlignmentID)
			,
			(SELECT LEN(g.Nucleotides)
				FROM BlastN.Alignment al
				JOIN Gene.Gene g ON g.ID = al.QueryID
				WHERE al.ID = @AlignmentID)
	))
END
GO
ALTER FUNCTION [BlastN].[AlignmentExon_First](@AlignmentID int)
RETURNS int
AS
BEGIN	
	RETURN (SELECT TOP 1 ID
				FROM BlastN.AlignmentExon ex
				WHERE ex.AlignmentID = @AlignmentID
				ORDER BY ex.BitScore DESC, Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength) DESC)
END
GO
ALTER PROCEDURE [RecordSet].[Import_BlastN_Alignment]
	@ID int = NULL OUTPUT
	,@NeedsExons bit = 0 OUTPUT
	,@QueryID uniqueidentifier
	,@SubjectID uniqueidentifier
	,@Rank int
	,@LastUpdatedAt datetime2(7)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @ExistingLastUpdatedAt datetime2(7)
			,@DeleteExons bit = 0

	IF EXISTS (SELECT * FROM BlastN.Alignment ex
					WHERE ex.QueryID = @QueryID
						AND ex.SubjectID = @SubjectID) BEGIN
		SELECT @ID = ex.ID
				,@ExistingLastUpdatedAt = ex.LastUpdatedAt
			FROM BlastN.Alignment ex
				WHERE ex.QueryID = @QueryID
					AND ex.SubjectID = @SubjectID

		IF @ExistingLastUpdatedAt < @LastUpdatedAt BEGIN
			SET @DeleteExons = 1
		END
	END

	IF @ID IS NULL OR @DeleteExons = 1 BEGIN
		SET @NeedsExons = 1
		EXEC BlastN.Alignment_Edit @ID OUTPUT, @QueryID, @SubjectID, @Rank, @DeleteExons
	END
END
GO
ALTER PROCEDURE [RecordSet].[Import_BlastN_AlignmentExon]
	@AlignmentID int
	,@OrientationID int
	,@BitScore float
	,@AlignmentLength int
	,@IdentitiesCount int
	,@Gaps int
	,@QueryRangeStart int
	,@QueryRangeEnd int
	,@SubjectRangeStart int
	,@SubjectRangeEnd int
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO BlastN.AlignmentExon (AlignmentID, OrientationID, BitScore, AlignmentLength, IdentitiesCount, Gaps, QueryRangeStart, QueryRangeEnd, SubjectRangeStart, SubjectRangeEnd)
	VALUES (@AlignmentID, @OrientationID, @BitScore, @AlignmentLength, @IdentitiesCount, @Gaps, @QueryRangeStart, @QueryRangeEnd, @SubjectRangeStart, @SubjectRangeEnd)
END
GO
ALTER PROCEDURE [RecordSet].[Import_NCBI_BlastNAlignment]
	@RequestID uniqueidentifier
	,@AlignmentIDs Common.ListInt READONLY
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO NCBI.BlastNAlignment (RequestID, AlignmentID)
	SELECT @RequestID, ids.Value
		FROM @AlignmentIDs ids
END
GO

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
						,[Request-Gene].DirectionID
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
UPDATE Common.ApplicationProperty
	SET Value = '1.3.3.0'
	WHERE [Key] = 'DatabaseVersion'
GO