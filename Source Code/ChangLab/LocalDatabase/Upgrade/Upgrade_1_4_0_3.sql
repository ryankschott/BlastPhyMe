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

		INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, DataTypeID, DisplayIndex, LastOpenedAt)
		VALUES (NEWID(), @ID, 'All', @DataTypeID_GeneSequence, 1, SYSDATETIME()),
				(NEWID(), @ID, 'Excluded', @DataTypeID_GeneSequence, 2, NULL),
				(NEWID(), @ID, 'Duplicates', @DataTypeID_GeneSequence, 3, NULL)
	
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
	SET Value = '1.4.0.3'
	WHERE [Key] = 'DatabaseVersion'
GO