SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
IF NOT EXISTS (SELECT * FROM Job.[Target] WHERE [Key] = 'PRANK') BEGIN
	INSERT INTO Job.[Target] (Name, [Key]) VALUES ('PRANK', 'PRANK')
END
GO
IF NOT EXISTS (SELECT * FROM Gene.[Source] WHERE [Key] = 'PRANK') BEGIN
	INSERT INTO Gene.[Source] (Name, [Key]) VALUES ('PRANK', 'PRANK')
END
GO
IF NOT EXISTS (SELECT * FROM sys.schemas s WHERE s.name = 'PRANK') BEGIN
	EXEC ('CREATE SCHEMA [PRANK]')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('PRANK.GeneSource')) BEGIN
	CREATE TABLE PRANK.GeneSource (
		JobID uniqueidentifier NOT NULL,
		InputGeneID uniqueidentifier NOT NULL,
		OutputGeneID uniqueidentifier NOT NULL,

		CONSTRAINT PK_PRANK_GeneSource PRIMARY KEY CLUSTERED (JobID ASC, InputGeneID ASC, OutputGeneID ASC)
	)

	ALTER TABLE PRANK.GeneSource ADD CONSTRAINT FK_PRANK_GeneSource_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
	ALTER TABLE PRANK.GeneSource ADD CONSTRAINT FK_PRANK_GeneSource_InputGeneID FOREIGN KEY (InputGeneID) REFERENCES Gene.Gene (ID)
	ALTER TABLE PRANK.GeneSource ADD CONSTRAINT FK_PRANK_GeneSource_OutputGeneID FOREIGN KEY (OutputGeneID) REFERENCES Gene.Gene (ID)

	DECLARE @v sql_variant 
	SET @v = N'Stores a link between the gene that''s sequence was aligned via PRANK and the output gene created with the new nucleotide sequence. '
			+ N'This provides a way for us to offer the user the option to overwrite the original record with the new nucleotide sequence instead '
			+ N'of creating new Gene.Gene records.  That''s slighlty complicated by the fact that the Gene.Gene records already exist in the '
			+ N'by the point at which they''ve been offered the opportunity, and we have to go back and clean-up the unnecessary records after the '
			+ N'fact, but doing it this way gives us the capacity to allow the user to close and re-open Pilgrimage and all that good stuff before '
			+ N'they''ve dealt with the results of the PRANK alignment.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'PRANK', N'TABLE', N'GeneSource', NULL, NULL
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PRANK.GeneSource_Edit')) BEGIN
	DROP PROCEDURE PRANK.GeneSource_Edit
END
GO
CREATE PROCEDURE PRANK.GeneSource_Edit
	@JobID uniqueidentifier
	,@InputGeneID uniqueidentifier
	,@OutputGeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO PRANK.GeneSource (JobID, InputGeneID, OutputGeneID)
	VALUES (@JobID, @InputGeneID, @OutputGeneID)
END
GO
ALTER PROCEDURE [Job].[Job_List]
	@TargetID int = NULL,
	@RecordSetID uniqueidentifier = NULL,
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
			,CAST((CASE WHEN (SELECT COUNT(*) FROM Job.Exception jex WHERE jex.JobID = j.ID) > 0 THEN 1 ELSE 0 END) AS bit) AS HasDatabaseExceptions
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
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('PRANK.Job_List')) BEGIN
	DROP PROCEDURE PRANK.Job_List
END
GO
CREATE PROCEDURE PRANK.Job_List
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @InputDirectionID int = (SELECT ID FROM Job.GeneDirection gd WHERE gd.[Key] = 'Input')
			,@TargetID int = (SELECT ID FROM Job.[Target] WHERE [Key] = 'PRANK')

	SELECT j.ID
			,j.StartedAt
			,j.EndedAt
			,j.StatusID
			,js.Name AS JobStatusName

			,sub.Name AS InputSubSetName
			,(SELECT COUNT(*)
				FROM Job.Gene jg
				WHERE jg.JobID = j.ID
					AND jg.DirectionID = @InputDirectionID) AS InputGeneCount
			,j.AdditionalProperties

		FROM Job.Job j
		JOIN Job.[Status] js ON js.ID = j.StatusID
		JOIN RecordSet.SubSet sub ON sub.ID = j.SubSetID
		WHERE j.RecordSetID = @RecordSetID
			AND j.TargetID = @TargetID
			AND j.Active = 1
		ORDER BY j.StartedAt DESC
END
GO
ALTER PROCEDURE [PAML].[Job_List]
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @TargetID int = (SELECT ID FROM Job.[Target] WHERE [Key] = 'CodeML')

	SELECT j.ID
			,j.Title
			,j.StartedAt
			,j.EndedAt
			,j.StatusID
			,js.Name AS JobStatusName

			,Common.FileNameFromPath(t.TreeFilePath) AS TreeFileName
			,Common.FileNameFromPath(t.SequencesFilePath) AS SequencesFileName
			,t.Title AS TreeTitle
			,(SELECT COUNT(*)
				FROM PAML.Tree t
				WHERE t.JobID = j.ID) AS TreeFileCount
		
		FROM Job.Job j
		JOIN Job.[Target] jt ON jt.ID = j.TargetID
		JOIN Job.[Status] js ON js.ID = j.StatusID
		JOIN PAML.Tree t ON t.JobID = j.ID
		WHERE j.RecordSetID = @RecordSetID
			AND j.TargetID = @TargetID
			AND t.[Rank] = 1
			AND j.Active = 1
		ORDER BY j.StartedAt DESC
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Taxonomy.Taxon_ListTreeView_ForTaxa')) BEGIN
	DROP PROCEDURE Taxonomy.Taxon_ListTreeView_ForTaxa
END
GO
CREATE PROCEDURE Taxonomy.Taxon_ListTreeView_ForTaxa
	@TaxaIDs Common.ListInt READONLY
AS
BEGIN
	SET NOCOUNT ON;

	WITH SelectedTaxa AS (
		SELECT DISTINCT t.Value AS ID
			FROM @TaxaIDs t
	), RecurseUp AS (
		SELECT t.ID
				,t.HID
				,t.Name
				,p.ID AS ParentID
				,p.HID AS ParentHID
				,p.Name AS ParentName
			FROM Taxonomy.Taxon t
			JOIN Taxonomy.Taxon p ON p.HID = t.HID.GetAncestor(1)
			JOIN SelectedTaxa s ON s.ID = t.ID
		UNION ALL
		SELECT t.ParentID AS ID
				,t.ParentHID AS HID
				,t.ParentName AS Name
				,p.ID AS ParentID
				,p.HID AS ParentHID
				,p.Name AS ParentName
			FROM RecurseUp t 
			JOIN Taxonomy.Taxon p ON p.HID = t.ParentHID.GetAncestor(1)
	), TaxonTree AS (
		SELECT DISTINCT
				t.ID
				,t.HID
				,t.Name
				,0 AS ParentID
				,hierarchyid::Parse('/') AS ParentHID
				,CAST('' AS varchar(200)) AS ParentName
			FROM Taxonomy.Taxon t
			JOIN RecurseUp r ON r.ParentHID = t.HID
			WHERE r.HID.GetLevel() = 2
			-- Level 1 won't be included in the recurse because it doesn't have a parent node.  If I ever add a 0x parent node, this will break.
		UNION ALL
		SELECT DISTINCT
				t.*
			FROM RecurseUp t
	)

	SELECT t.ID
			,t.Name
			,t.HID.ToString() AS Hierarchy
			,t.ParentID
		FROM TaxonTree t
		ORDER BY t.HID
END
GO
ALTER PROCEDURE Taxonomy.Taxon_ListTreeView_ForRecordSet
	@RecordSetID uniqueidentifier,
	@SubSetID uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TaxaIDs Common.ListInt

	INSERT INTO @TaxaIDs
	SELECT DISTINCT t.ID
			FROM Gene.Gene g
			JOIN RecordSet.SubSetGene sg ON sg.GeneID = g.ID
			JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
			JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
			WHERE sub.RecordSetID = @RecordSetID
				AND ((@SubSetID IS NULL) OR (sg.SubSetID = @SubSetID))
				AND g.Active = 1

	EXEC Taxonomy.Taxon_ListTreeView_ForTaxa @TaxaIDs
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Taxonomy.Taxon_ListTreeView_ForGenes')) BEGIN
	DROP PROCEDURE Taxonomy.Taxon_ListTreeView_ForGenes
END
GO
CREATE PROCEDURE Taxonomy.Taxon_ListTreeView_ForGenes
	@GeneIDs Common.ListUniqueIdentifier READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TaxaIDs Common.ListInt

	INSERT INTO @TaxaIDs
	SELECT DISTINCT t.ID
			FROM Gene.Gene g
			JOIN @GeneIDs id ON id.Value = g.ID
			JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
			
	EXEC Taxonomy.Taxon_ListTreeView_ForTaxa @TaxaIDs
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Gene.IsInRecordSet')) BEGIN
	DROP FUNCTION Gene.IsInRecordSet
END
GO
CREATE FUNCTION Gene.IsInRecordSet(@GeneID uniqueidentifier, @RecordSetID uniqueidentifier)
	RETURNS bit
AS
BEGIN

	RETURN CAST(CASE WHEN EXISTS (SELECT * FROM RecordSet.Gene rs_g WHERE rs_g.RecordSetID = @RecordSetID AND rs_g.GeneID = @GeneID) THEN 1 ELSE 0 END AS bit)
	
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
			,Gene.IsInRecordSet(g.ID, @RecordSetID) AS InRecordSet
		FROM Job.Gene jg
		JOIN Gene.Gene g ON g.ID = jg.GeneID
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		WHERE jg.JobID = @JobID
			AND jg.DirectionID = @GeneDirectionID
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
			,Gene.IsInRecordSet(sbj.ID, @RecordSetID) AS InRecordSet
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
ALTER PROCEDURE [Job].[BlastN_ListSubjectGenesForQueryGene]
	@QueryGeneID uniqueidentifier
	,@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON
		
	SELECT sbj.ID
			,ssrc.Name AS QuerySourceName
			,sbj.GenBankID
			,sbj.[Definition]
			,sbj.Organism
			,sbj.Taxonomy
			,stat.*
			,Gene.IsInRecordSet(sbj.ID, @RecordSetID) AS InRecordSet
		FROM BlastN.Alignment al
		JOIN Gene.Gene sbj ON sbj.ID = al.SubjectID
		JOIN Gene.[Source] ssrc ON ssrc.ID = sbj.SourceID
		CROSS APPLY BlastN.Alignment_Statistics(al.QueryID, al.SubjectID) stat
		WHERE al.QueryID = @QueryGeneID
		ORDER BY stat.[Rank], stat.MaxScore DESC
END
GO

GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Common.ThirdPartyComponentReference') AND c.name = 'Packaged') BEGIN
	ALTER TABLE Common.ThirdPartyComponentReference ADD Packaged bit NOT NULL CONSTRAINT DF_Common_ThirdPartyComponentReference_Packaged DEFAULT (0)

	DECLARE @v sql_variant = N'Indicates that the component is included in the Pilgrimage installation package, and does not need to be seaprately installed.'
	EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Common', N'TABLE', N'ThirdPartyComponentReference', N'COLUMN', N'Packaged'
END
GO
IF NOT EXISTS (SELECT * FROM Common.ThirdPartyComponentReference ref WHERE ref.Name = 'PRANK') BEGIN
	INSERT INTO Common.ThirdPartyComponentReference
	(Name, [Version], Creator, ProductURL, LastUpdatedAt, LastRetrievedAt, Copyright, LicenseType, LicenseURL, Modified, Logo, Packaged)
	VALUES
	('PRANK', '140603', 'Löytynoja Lab', 'http://wasabiapp.org/software/prank/',
		'2014-06-23', '2015-05-04 2:46 PM',
		'2013', 'GNU General Public License v3', 'http://www.gnu.org/licenses/gpl.html',
		0, 'ComponentLogo_PRANK', 0)
END
GO
ALTER PROCEDURE [Common].[ThirdPartyComponentReference_List]
AS
BEGIN
	SET NOCOUNT ON

	SELECT r.ID
			,r.Name
			,r.[Version]
			,r.Creator
			,r.ProductURL
			,r.LastUpdatedAt
			,r.LastRetrievedAt 
			,r.Copyright
			,r.LicenseType
			,r.LicenseURL
			,r.LicenseText
			,r.Modified
			,r.Logo
			,r.Packaged
		FROM Common.ThirdPartyComponentReference r
		ORDER BY r.Name
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.1.0'
	WHERE [Key] = 'DatabaseVersion'
GO