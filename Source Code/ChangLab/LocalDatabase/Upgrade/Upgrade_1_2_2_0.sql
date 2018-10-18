SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('NCBI.ESearchHistory')) BEGIN
	CREATE TABLE NCBI.ESearchHistory (
		ID uniqueidentifier NOT NULL
		,RecordSetID uniqueidentifier NOT NULL
		,TargetDatabase varchar(20) NOT NULL
		,Term varchar(2000) NOT NULL
		,WebEnvironment varchar(200) NOT NULL
		,QueryKey varchar(10) NOT NULL
		,ResultCount int NOT NULL
		,ReturnMaximum int NOT NULL
		,QueryAt datetime2(7) NOT NULL CONSTRAINT NCBI_ESearchHistory_QueryAt DEFAULT (sysdatetime())
	)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('NCBI.ESearchHistory_Edit')) BEGIN
	DROP PROCEDURE NCBI.ESearchHistory_Edit
END
GO
CREATE PROCEDURE NCBI.ESearchHistory_Edit
	@RecordSetID uniqueidentifier
	,@TargetDatabase varchar(20)
	,@Term varchar(2000)
	,@WebEnvironment varchar(200)
	,@QueryKey varchar(10)
	,@ResultCount int
	,@ReturnMaximum int
	,@ID uniqueidentifier = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	IF (@ID IS NULL) BEGIN
		SELECT @ID = h.ID
			FROM NCBI.ESearchHistory h
			WHERE h.RecordSetID = @RecordSetID
				AND h.TargetDatabase = @TargetDatabase
				AND h.Term = @Term
	END

	IF (@ID IS NULL) OR (NOT EXISTS (SELECT * FROM NCBI.ESearchHistory h WHERE H.ID = @ID)) BEGIN
		SET @ID = NEWID()

		INSERT INTO NCBI.ESearchHistory (ID, RecordSetID, TargetDatabase, Term, WebEnvironment, QueryKey, ResultCount, ReturnMaximum)
		VALUES (@ID, @RecordSetID, @TargetDatabase, @Term, @WebEnvironment, @QueryKey, @ResultCount, @ReturnMaximum)
	END
	ELSE BEGIN
		UPDATE NCBI.ESearchHistory
			SET Term = ISNULL(@Term, Term)
				,WebEnvironment = ISNULL(@WebEnvironment, WebEnvironment)
				,QueryKey = ISNULL(@QueryKey, QueryKey)
				,ResultCount = ISNULL(@ResultCount, ResultCount)
				,ReturnMaximum = ISNULL(@ReturnMaximum, ReturnMaximum)
				,QueryAt = SYSDATETIME()
			WHERE ID = @ID
	END

	-- We only keep the top 10 recent results.
	IF (SELECT COUNT(*) FROM NCBI.ESearchHistory h WHERE h.RecordSetID = @RecordSetID AND h.TargetDatabase = @TargetDatabase) > 10 BEGIN
		DELETE h
			FROM NCBI.ESearchHistory h
			WHERE h.RecordSetID = @RecordSetID
				AND h.TargetDatabase = @TargetDatabase
				AND h.ID NOT IN (
					SELECT TOP 10 recent.ID
						FROM NCBI.ESearchHistory recent
						WHERE recent.RecordSetID = @RecordSetID
							AND recent.TargetDatabase = @TargetDatabase
						ORDER BY recent.QueryAt DESC
				)
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('NCBI.ESearchHistory_List')) BEGIN
	DROP PROCEDURE NCBI.ESearchHistory_List
END
GO
CREATE PROCEDURE NCBI.ESearchHistory_List
	@RecordSetID uniqueidentifier
	,@TargetDatabase varchar(20) = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT h.ID
			,h.TargetDatabase
			,h.Term
			,h.WebEnvironment
			,h.QueryKey
			,h.ResultCount
			,h.ReturnMaximum
			,h.QueryAt
		FROM NCBI.ESearchHistory h
		WHERE h.RecordSetID = @RecordSetID
			AND (@TargetDatabase IS NULL OR h.TargetDatabase = @TargetDatabase)
		ORDER BY h.QueryAt DESC
END
GO

UPDATE Common.ApplicationProperty
	SET Value = '1.2.2.0'
	WHERE [Key] = 'DatabaseVersion'
GO