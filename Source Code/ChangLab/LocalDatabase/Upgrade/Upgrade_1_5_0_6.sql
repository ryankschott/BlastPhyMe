SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO


GO

UPDATE Common.ApplicationProperty
	SET Value = '1.5.0.6'
	WHERE [Key] = 'DatabaseVersion'
GO