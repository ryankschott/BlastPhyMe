SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
IF NOT EXISTS (SELECT * FROM Job.[Target] WHERE [Key] = 'PhyML') BEGIN
	INSERT INTO Job.[Target] (Name, [Key])
	VALUES ('PhyML', 'PhyML')
END
GO
IF NOT EXISTS (SELECT * FROM sys.tables t WHERE t.object_id = OBJECT_ID('Job.OutputText')) BEGIN
	CREATE TABLE Job.OutputText (
		JobID uniqueidentifier NOT NULL,
		OutputText nvarchar(MAX) NOT NULL,

		CONSTRAINT PK_Job_OutputText PRIMARY KEY CLUSTERED (JobID ASC)
	)

	ALTER TABLE Job.OutputText ADD CONSTRAINT FK_Job_OutputText_JobID FOREIGN KEY (JobID) REFERENCES Job.Job (ID)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Job.OutputText_Edit')) BEGIN
	DROP PROCEDURE Job.OutputText_Edit
END
GO
CREATE PROCEDURE Job.OutputText_Edit
	@JobID uniqueidentifier,
	@OutputText nvarchar(MAX)
AS
BEGIN
	SET NOCOUNT ON

	IF NOT EXISTS (SELECT * FROM Job.OutputText ot WHERE ot.JobID = @JobID) BEGIN
		INSERT INTO Job.OutputText (JobID, OutputText)
		VALUES (@JobID, @OutputText)
	END
	ELSE BEGIN
		UPDATE Job.OutputText
			SET OutputText = @OutputText
			WHERE JobID = @JobID
	END
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
			,js.Name AS JobStatusName

			,(SELECT COUNT(*)
					FROM Job.Gene g
					WHERE g.JobID = j.ID
						AND g.DirectionID = 1) AS InputGeneCount
			,ISNULL(sub.Name, '') AS InputSubSetName
			,CAST((CASE WHEN (SELECT COUNT(*) FROM Job.Exception jex WHERE jex.JobID = j.ID) > 0 THEN 1 ELSE 0 END) AS bit) AS HasDatabaseExceptions
			,ISNULL(ot.OutputText, '') AS OutputText

		FROM Job.Job j
		JOIN Job.[Target] jt ON jt.ID = j.TargetID
		JOIN Job.[Status] js ON js.ID = j.StatusID
		LEFT OUTER JOIN RecordSet.SubSet sub ON sub.ID = j.SubSetID
		LEFT OUTER JOIN Job.OutputText ot ON ot.JobID = j.ID
		WHERE j.RecordSetID = @RecordSetID
			AND j.TargetID = @TargetID
			AND j.Active = @Active
			AND ((@JobID IS NULL) OR (j.ID = @JobID))
			AND ((@StatusID IS NULL) OR (j.StatusID = @StatusID))
		ORDER BY j.StartedAt DESC
END
GO

GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.name = 'Citation' AND c.object_id = OBJECT_ID('Common.ThirdPartyComponentReference')) BEGIN
	ALTER TABLE Common.ThirdPartyComponentReference ADD Citation nvarchar(MAX) NULL
END
GO
IF NOT EXISTS (SELECT * FROM Common.ThirdPartyComponentReference ref WHERE ref.Name = 'PhyML') BEGIN
	INSERT INTO Common.ThirdPartyComponentReference
		(Name, [Version], Creator, ProductURL, LastUpdatedAt, LastRetrievedAt, Copyright, LicenseURL, LicenseText, Modified, Logo, Packaged, Citation)
	VALUES
		('PhyML', '3.1', 'CNRS - Universite Montpellier II', 'http://www.atgc-montpellier.fr/phyml/',
			'2013-03-19', '2015-05-19 09:00 AM', '1998-2008', 
			'http://www.atgc-montpellier.fr/phyml/binaries.php',
			'The software is provided "as is" without warranty of any kind. In no event shall the author be held responsible for any damage resulting from the use of this software, including but not limited to the frustration that you may experience in using the package. The program package, including source codes, example data sets, executables, and the user guide, is distributed free of charge for academic use only.',
			0, 'ComponentLogo_PhyML', 0,
			'"New Algorithms and Methods to Estimate Maximum-Likelihood Phylogenies: Assessing the Performance of PhyML 3.0." Guindon S., Dufayard J.F., Lefort V., Anisimova M., Hordijk W., Gascuel O. Systematic Biology, 59(3):307-21, 2010.')
END
GO
IF NOT EXISTS (SELECT * FROM Common.ThirdPartyComponentReference ref WHERE ref.Name = 'TreeView') BEGIN
	INSERT INTO Common.ThirdPartyComponentReference
		(Name, [Version], Creator, ProductURL, LastUpdatedAt, LastRetrievedAt, Copyright, LicenseURL, LicenseText, Modified, Logo, Packaged, Citation)
	VALUES
		('TreeView', '1.6.6', 'Roderic D. M. Page', 'http://taxonomy.zoology.gla.ac.uk/rod/treeview.html',
			'2001-09-03', '2014-08-08 09:30 AM', '2000', 
			'http://taxonomy.zoology.gla.ac.uk/rod/treeview/treeview_manual.html',
			'The software is provided "as-is" and without warranty of any kind, express, implied or otherwise, including without limitation, any warranty of merchantability or fitness for a particular purpose.  In no event shall the author, the Division of Environmental and Evolutionary Biology or the University of Glasgow be liable for any special, incidental, indirect or consequential damages of any kind, or any damages whatsoever resulting from loss of use, data or profits, whether or not advised of the possibility of damage, and on any theory of liability, arising out of or in connection with the use or performance of this software.',
			0, 'ComponentLogo_TreeView', 0,
			'Page, R. D. M. 1996. TREEVIEW: An application to display phylogenetic trees on personal computers. Computer Applications in the Biosciences 12: 357-358.')
END
GO
UPDATE ref
	SET Citation = 'Yang, Z. 2007. PAML 4: a program package for phylogenetic analysis by maximum likelihood. Molecular Biology and Evolution 24: 1586-1591'
	FROM Common.ThirdPartyComponentReference ref
	WHERE ref.Name = 'Phylogenetic Analysis by Maximum Likelihood (PAML)'
		AND ref.Citation IS NULL
GO
UPDATE ref
	SET Citation = 'Yang, Z. 2007. PAML 4: a program package for phylogenetic analysis by maximum likelihood. Molecular Biology and Evolution 24: 1586-1591'
	FROM Common.ThirdPartyComponentReference ref
	WHERE ref.Name = 'Phylogenetic Analysis by Maximum Likelihood (PAML)'
		AND ref.Citation IS NULL
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
			,r.Citation
		FROM Common.ThirdPartyComponentReference r
		ORDER BY r.Name
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.2.0'
	WHERE [Key] = 'DatabaseVersion'
GO
