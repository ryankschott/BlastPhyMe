SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
-- Common.ApplicationProperty_List was being used as a "ping" procedure to check that the client was connected to the database, but then a parameter
-- was added to the stored procedure and in the call to it within the client.  Older versions of the database didn't yet have the parameter when they
-- were being pinged by the newer version of the client to validate connectivity before the auto-update that would've updated the stored procedure to
-- include the new parameter, and thus the procedure was being called by the newer client with a parameter when the older databases's procedure
-- didn't have it, and that'll throw an exception.
-- This procedure will serve as a ping to confirm connectivity, and should never be used for anything else.
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Common.VerifyDatabaseConnectivity')) BEGIN
	DROP PROCEDURE Common.VerifyDatabaseConnectivity
END
GO
CREATE PROCEDURE Common.VerifyDatabaseConnectivity
AS
BEGIN
	SET NOCOUNT ON

	SELECT Value AS DatabaseVersion
		FROM Common.ApplicationProperty
		WHERE [Key] = 'DatabaseVersion'
END
GO
/*
	Prior to 1.4.1.0, this procedure was at the core of the database auto-update logic in that the database version needed to be retrieved in order
	to check whether an update needed to be performed.  Any modification to this procedure is thus problematic; for example, adding a parameter will
	cause a fault when an older version of the database is connected to and the newer client calls it with a parameter that the older procedure does
	not yet have (this raises an ADO.Net SQL exception).

	As such, this procedure effectively needs to be kept as-is, until we release Pilgrimage publically.  When that happens, we can request that all
	ChangLab users update their installations, after which we can allow updates to this procedure again.  Conceivably, someone who neglected to do so
	could simply run the 1.4.1.0 installer to update their database and get through this change, and then run whatever the latest installer is.
	
	As of 1.4.1.0, Common.VerifyDatabaseConnectivity replaced this procedure for the purposes of verifying database connectivity and returning the
	database version, freeing up Common.ApplicationProperty_List to be used purely for listing application properties.
*/
ALTER PROCEDURE [Common].[ApplicationProperty_List]
AS
BEGIN
	SET NOCOUNT ON

	SELECT ap.ID
			,ap.[Key]
			,ap.Value
		FROM Common.ApplicationProperty ap
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Common.ApplicationProperty_GetByKey')) BEGIN
	DROP PROCEDURE Common.ApplicationProperty_GetByKey
END
GO
CREATE PROCEDURE Common.ApplicationProperty_GetByKey
	@Key varchar(30)
AS
BEGIN
	SET NOCOUNT ON

	SELECT ap.ID
			,ap.[Key]
			,ap.Value
		FROM Common.ApplicationProperty ap
		WHERE ap.[Key] = @Key
END
GO

-- Fix for 'Duplicates' being the first Gene Sequences subset opened for a new recordset.
GO
ALTER PROCEDURE RecordSet.RecordSet_Edit
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
		VALUES (NEWID(), @ID, 'All', @DataTypeID_GeneSequence, 1, DATEADD(SECOND, 1, @SubSetLastOpenedAt)), -- This forces 'All' to be the first subset opened.
				(NEWID(), @ID, 'Excluded', @DataTypeID_GeneSequence, 2, @SubSetLastOpenedAt),
				(NEWID(), @ID, 'Duplicates', @DataTypeID_GeneSequence, 3, @SubSetLastOpenedAt)
	
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

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.0.11'
	WHERE [Key] = 'DatabaseVersion'
GO