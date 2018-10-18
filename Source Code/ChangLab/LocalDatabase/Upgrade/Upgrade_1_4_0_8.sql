SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Job.Job') AND c.name = 'AdditionalProperties') BEGIN
	ALTER TABLE Job.Job ADD AdditionalProperties xml NULL
END
GO
ALTER PROCEDURE [Job].[Job_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@RecordSetID uniqueidentifier = NULL, -- Not used in an UPDATE
	@SubSetID uniqueidentifier = NULL, -- Not used in an UPDATE
	@TargetID int = NULL, -- Not used in an UPDATE
	@Title varchar(250) = NULL, -- Not used in an UPDATE
	@StatusID int = NULL,
	@StartedAt datetime2(7) = NULL, -- Not used in an UPDATE
	@EndedAt datetime2(7) = NULL,
	@AdditionalProperties xml = NULL,
	@Active bit = NULL
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM Job.Job j WHERE j.ID = @ID) BEGIN
		IF (@RecordSetID IS NULL AND @SubSetID IS NULL) BEGIN
			RAISERROR('A RecordSet ID or a SubSet ID must be provided', 11, 1)
		END

		SET @ID = NEWID()

		IF (@RecordSetID IS NULL) BEGIN
			SELECT @RecordSetID = sub.RecordSetID
				FROM RecordSet.SubSet sub
				WHERE sub.ID = @SubSetID
		END

		INSERT INTO Job.Job (ID, RecordSetID, SubSetID, TargetID, Title, StartedAt, EndedAt, AdditionalProperties)
		VALUES (@ID, @RecordSetID, @SubSetID, @TargetID, @Title, @StartedAt, @EndedAt, @AdditionalProperties)
	END
	ELSE BEGIN
		UPDATE Job.Job
			SET StatusID = ISNULL(@StatusID, StatusID)
				,EndedAt = ISNULL(@EndedAt, EndedAt)
				,AdditionalProperties = ISNULL(@AdditionalProperties, AdditionalProperties)
				,Active = ISNULL(@Active, Active)
			WHERE ID = @ID
	END
END
GO
ALTER PROCEDURE [Job].[Job_List]
	@RecordSetID uniqueidentifier = NULL,
	@TargetID int = NULL,
	@JobID uniqueidentifier = NULL,
	@StatusID int = NULL,
	@Active bit = 1
AS
BEGIN
	SET NOCOUNT ON

	IF (@JobID IS NULL AND @RecordSetID IS NULL AND @TargetID IS NULL) BEGIN
		RAISERROR('No search criteria were provided', 11, 1)
	END
	ELSE IF (@JobID IS NULL AND (@RecordSetID IS NULL OR @TargetID IS NULL)) BEGIN
		RAISERROR('Insufficient search criteria were provided', 11, 1)
	END

	IF (@JobID IS NOT NULL) BEGIN
		SELECT @RecordSetID = j.RecordSetID
				,@TargetID = j.TargetID
			FROM Job.Job j
			WHERE j.ID = @JobID
	END

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
			AND ((@JobID IS NULL) OR (j.ID = @JobID))
			AND ((@StatusID IS NULL) OR (j.StatusID = @StatusID))
		ORDER BY j.StartedAt DESC
END
GO
SET NOCOUNT ON

UPDATE PAML.ModelPreset SET Name = 'Random Sites (M0, M1a, M2a, M3, M7, M8)', ShortName = 'Random Sites' WHERE [Key] = 'Model0'
UPDATE PAML.ModelPreset SET Name = 'Random Sites (M2a_rel)', ShortName = 'Random Sites (M2a_rel)' WHERE [Key] = 'Model2a'
UPDATE PAML.ModelPreset SET Name = 'Random Sites (M8a)', ShortName = 'Random Sites (M8a)' WHERE [Key] = 'Model8a'
UPDATE PAML.ModelPreset SET Name = 'Branch Model (Alt)', ShortName = 'Branch Model (Alt)' WHERE [Key] = 'Branch'
UPDATE PAML.ModelPreset SET Name = 'Branch Model (Null)', ShortName = 'Branch Model (Null)' WHERE [Key] = 'BranchNull'
UPDATE PAML.ModelPreset SET Name = 'Branch-Site Model (Alt)', ShortName = 'Branch-Site Model (Alt)' WHERE [Key] = 'BranchSite'
UPDATE PAML.ModelPreset SET Name = 'Branch-Site Model (Null)', ShortName = 'Branch-Site Model (Null)' WHERE [Key] = 'BranchSiteNull'
UPDATE PAML.ModelPreset SET Name = 'Clade Model C (Alt)', ShortName = 'Clade Model C (Alt)' WHERE [Key] = 'CmC'
UPDATE PAML.ModelPreset SET Name = 'Clade Model C (Null)', ShortName = 'Clade Model C (Null)' WHERE [Key] = 'CmcNull'

SET NOCOUNT OFF
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.0.8'
	WHERE [Key] = 'DatabaseVersion'
GO