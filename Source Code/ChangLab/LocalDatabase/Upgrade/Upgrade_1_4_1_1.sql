SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Gene.IsInAnySubSet')) BEGIN
	DROP FUNCTION Gene.IsInAnySubSet
END
GO
CREATE FUNCTION Gene.IsInAnySubSet(@GeneID uniqueidentifier, @RecordSetID uniqueidentifier)
	RETURNS bit
AS
BEGIN

	RETURN CAST(CASE WHEN EXISTS (SELECT * 
									FROM RecordSet.SubSetGene ss_g
									JOIN RecordSet.SubSet sub ON sub.ID = ss_g.SubSetID
									WHERE ss_g.GeneID = @GeneID
										AND sub.RecordSetID = @RecordSetID
										AND sub.Active = 1)
					 THEN 1 ELSE 0 END AS bit)
	
END
GO
ALTER PROCEDURE [Job].[Gene_List]
	@JobID uniqueidentifier,
	@GeneDirectionID int
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @RecordSetID uniqueidentifier = (SELECT RecordSetID FROM Job.Job j WHERE j.ID = @JobID)

	SELECT g.*
			,t.HID AS TaxonomyHierarchy
			,Gene.IsInAnySubSet(g.ID, @RecordSetID) AS InRecordSet
		FROM Job.Gene jg
		JOIN Gene.Gene g ON g.ID = jg.GeneID
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		WHERE jg.JobID = @JobID
			AND jg.DirectionID = @GeneDirectionID
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.1.1'
	WHERE [Key] = 'DatabaseVersion'
GO