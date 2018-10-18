SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [NCBI].[ESearchHistory_List]
	@RecordSetID uniqueidentifier
	,@TargetDatabase varchar(20) = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT TOP 10 h.ID
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

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.0.6'
	WHERE [Key] = 'DatabaseVersion'
GO