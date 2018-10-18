SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
GO
BEGIN TRANSACTION
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Gene.Gene') AND c.name = 'Name') BEGIN
	ALTER TABLE Gene.Gene DROP CONSTRAINT FK_Gene_Gene_TaxonomyID
	ALTER TABLE Gene.Gene DROP CONSTRAINT FK_Gene_Gene_SequenceTypeID
	ALTER TABLE Gene.Gene DROP CONSTRAINT FK_Gene_Gene_SourceID
	ALTER TABLE Gene.Gene DROP CONSTRAINT FK_Gene_Gene_LastUpdateSourceID
	ALTER TABLE Gene.Gene DROP CONSTRAINT DF_Gene_Gene_Active

	CREATE TABLE Gene.Tmp_Gene
		(ID uniqueidentifier NOT NULL, Name varchar(100) NULL, Definition varchar(1000) NOT NULL, SourceID int NOT NULL, GenBankID int NULL, 
			Locus varchar(100) NULL, Accession varchar(20) NULL, Organism varchar(250) NULL, Taxonomy varchar(4000) NULL, TaxonomyID int NULL,
			Nucleotides varchar(MAX) NULL, SequenceTypeID int NULL, SequenceStart int NULL, SequenceEnd int NULL, CodingSequenceStart int NULL, CodingSequenceEnd int NULL, 
			Description varchar(MAX) NULL, Active bit NOT NULL, LastUpdatedAt datetime2(7) NULL, LastUpdateSourceID int NOT NULL
		)  ON [PRIMARY]
		 TEXTIMAGE_ON [PRIMARY]
	
	ALTER TABLE Gene.Tmp_Gene ADD CONSTRAINT DF_Gene_Gene_Active DEFAULT ((1)) FOR Active
	
	IF EXISTS(SELECT * FROM Gene.Gene) BEGIN
		 EXEC('INSERT INTO Gene.Tmp_Gene (ID, Definition, SourceID, GenBankID, Locus, Accession, Organism, Taxonomy, Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd, LastUpdatedAt, Active, TaxonomyID, LastUpdateSourceID, Description)
				SELECT ID, Definition, SourceID, GenBankID, Locus, Accession, Organism, Taxonomy, Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd, LastUpdatedAt, Active, TaxonomyID, LastUpdateSourceID, Description FROM Gene.Gene WITH (HOLDLOCK TABLOCKX)')
	END

	ALTER TABLE Gene.Feature DROP CONSTRAINT FK_Gene_Feature_GeneID
	ALTER TABLE RecordSet.SubSetGene DROP CONSTRAINT FK_RecordSet_SubSetGene_GeneID
	ALTER TABLE Gene.GeneHistory DROP CONSTRAINT FK_Gene_GeneHistory_ID
	ALTER TABLE Gene.NucleotideSequence DROP CONSTRAINT FK_Gene_NucleotideSequence_GeneID
	ALTER TABLE Gene.Exception DROP CONSTRAINT FK_Gene_Exception_GeneID
	ALTER TABLE BlastN.Alignment DROP CONSTRAINT FK_BlastN_Alignment_QueryID
	ALTER TABLE BlastN.Alignment DROP CONSTRAINT FK_BlastN_Alignment_SubjectID
	ALTER TABLE RecordSet.Gene DROP CONSTRAINT FK_RecordSet_Gene_GeneID
	ALTER TABLE PRANK.GeneSource DROP CONSTRAINT FK_PRANK_GeneSource_InputGeneID
	ALTER TABLE PRANK.GeneSource DROP CONSTRAINT FK_PRANK_GeneSource_OutputGeneID
	ALTER TABLE Job.Gene DROP CONSTRAINT FK_Job_Gene_GeneID
	ALTER TABLE NCBI.Gene DROP CONSTRAINT FK_NCBI_Gene_GeneID

	EXEC ('DROP TABLE Gene.Gene')
	EXECUTE sp_rename N'Gene.Tmp_Gene', N'Gene', 'OBJECT' 

	ALTER TABLE Gene.Gene ADD CONSTRAINT PK_Gene_Gene PRIMARY KEY CLUSTERED 
		(ID) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	ALTER TABLE Gene.Gene ADD CONSTRAINT FK_Gene_Gene_SourceID FOREIGN KEY (SourceID) REFERENCES Gene.Source (ID) ON UPDATE  NO ACTION ON DELETE  NO ACTION 
	ALTER TABLE Gene.Gene ADD CONSTRAINT FK_Gene_Gene_SequenceTypeID FOREIGN KEY (SequenceTypeID) REFERENCES Gene.SequenceType (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Gene.Gene ADD CONSTRAINT FK_Gene_Gene_LastUpdateSourceID FOREIGN KEY (LastUpdateSourceID) REFERENCES Gene.Source (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Gene.Gene ADD CONSTRAINT FK_Gene_Gene_TaxonomyID FOREIGN KEY (TaxonomyID) REFERENCES Taxonomy.Taxon (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE NCBI.Gene ADD CONSTRAINT FK_NCBI_Gene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Job.Gene ADD CONSTRAINT FK_Job_Gene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE PRANK.GeneSource ADD CONSTRAINT FK_PRANK_GeneSource_InputGeneID FOREIGN KEY (InputGeneID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE PRANK.GeneSource ADD CONSTRAINT FK_PRANK_GeneSource_OutputGeneID FOREIGN KEY (OutputGeneID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE RecordSet.Gene ADD CONSTRAINT FK_RecordSet_Gene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE BlastN.Alignment ADD CONSTRAINT FK_BlastN_Alignment_QueryID FOREIGN KEY (QueryID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE BlastN.Alignment ADD CONSTRAINT FK_BlastN_Alignment_SubjectID FOREIGN KEY (SubjectID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Gene.Exception ADD CONSTRAINT FK_Gene_Exception_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Gene.NucleotideSequence ADD CONSTRAINT FK_Gene_NucleotideSequence_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_ID FOREIGN KEY (ID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE RecordSet.SubSetGene ADD CONSTRAINT FK_RecordSet_SubSetGene_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Gene.Feature ADD CONSTRAINT FK_Gene_Feature_GeneID FOREIGN KEY (GeneID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
END
GO
COMMIT TRANSACTION
GO
BEGIN TRANSACTION
GO
IF NOT EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = OBJECT_ID('Gene.GeneHistory') AND c.name = 'Name') BEGIN
	ALTER TABLE Gene.GeneHistory DROP CONSTRAINT FK_Gene_GeneHistory_TaxonomyID
	ALTER TABLE Gene.GeneHistory DROP CONSTRAINT FK_Gene_GeneHistory_ID
	ALTER TABLE Gene.GeneHistory DROP CONSTRAINT FK_Gene_GeneHistory_SequenceTypeID
	ALTER TABLE Gene.GeneHistory DROP CONSTRAINT FK_Gene_GeneHistory_SourceID
	ALTER TABLE Gene.GeneHistory DROP CONSTRAINT DF_Gene_GeneHistory_LastUpdateSourceID
	ALTER TABLE Gene.GeneHistory DROP CONSTRAINT DF_Gene_GeneHistory_RevisionAt
	
	CREATE TABLE Gene.Tmp_GeneHistory 
		(RevisionID int NOT NULL, ID uniqueidentifier NOT NULL, Name varchar(100) NULL, Definition varchar(1000) NULL, SourceID int NOT NULL,
		GenBankID int NULL, Locus varchar(100) NULL, Accession varchar(20) NULL, Organism varchar(250) NULL, Taxonomy varchar(4000) NULL, TaxonomyID int NULL,
		Nucleotides varchar(MAX) NULL, SequenceTypeID int NULL, SequenceStart int NULL, SequenceEnd int NULL, CodingSequenceStart int NULL, CodingSequenceEnd int NULL,
		Description varchar(MAX) NULL, Active bit NOT NULL, LastUpdatedAt datetime2(7) NULL, LastUpdateSourceID int NOT NULL, RevisionAt datetime2(7) NOT NULL
		)  ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	
	ALTER TABLE Gene.Tmp_GeneHistory ADD CONSTRAINT DF_Gene_GeneHistory_LastUpdateSourceID DEFAULT ((1)) FOR LastUpdateSourceID
	ALTER TABLE Gene.Tmp_GeneHistory ADD CONSTRAINT DF_Gene_GeneHistory_RevisionAt DEFAULT (sysdatetime()) FOR RevisionAt
	
	IF EXISTS (SELECT * FROM Gene.GeneHistory) BEGIN
		 EXEC('INSERT INTO Gene.Tmp_GeneHistory (RevisionID, ID, Definition, SourceID, GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID, Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd, Description, Active, LastUpdatedAt, LastUpdateSourceID, RevisionAt)
			SELECT RevisionID, ID, Definition, SourceID, GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID, Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd, Description, Active, LastUpdatedAt, LastUpdateSourceID, RevisionAt FROM Gene.GeneHistory WITH (HOLDLOCK TABLOCKX)')
	END

	EXEC ('DROP TABLE Gene.GeneHistory')
	EXECUTE sp_rename N'Gene.Tmp_GeneHistory', N'GeneHistory', 'OBJECT' 
	
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT PK_Gene_GeneHistory PRIMARY KEY CLUSTERED (RevisionID, ID) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_SourceID FOREIGN KEY (SourceID) REFERENCES Gene.Source (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_SequenceTypeID FOREIGN KEY (SequenceTypeID) REFERENCES Gene.SequenceType (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_ID FOREIGN KEY (ID) REFERENCES Gene.Gene (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
	ALTER TABLE Gene.GeneHistory ADD CONSTRAINT FK_Gene_GeneHistory_TaxonomyID FOREIGN KEY (TaxonomyID) REFERENCES Taxonomy.Taxon (ID) ON UPDATE  NO ACTION  ON DELETE  NO ACTION 
END	
GO
COMMIT TRANSACTION
GO
IF NOT EXISTS (SELECT * FROM Gene.Gene g WHERE g.Name IS NOT NULL) BEGIN
	UPDATE g
		SET Name = n.GeneQualifier
		FROM Gene.Gene g
		JOIN (SELECT g.GeneID, g.GeneQualifier
				FROM (SELECT g.GeneID, g.GeneQualifier, ROW_NUMBER() OVER (PARTITION BY g.GeneID ORDER BY g.QualifierCount DESC) AS GeneNameRank
						FROM (SELECT f.GeneID, f.GeneQualifier, COUNT(*) AS QualifierCount
								FROM Gene.Feature f
								WHERE f.GeneQualifier IS NOT NULL
									AND f.GeneQualifier <> ''
								GROUP BY f.GeneID, f.GeneQualifier) g) g
				WHERE g.GeneNameRank = 1) n ON n.GeneID = g.ID

	UPDATE h
		SET Name = g.Name
		FROM Gene.GeneHistory h
		JOIN Gene.Gene g ON g.ID = h.ID
			WHERE g.Name IS NOT NULL
END
GO
IF EXISTS (SELECT * FROM sys.triggers t WHERE t.object_id = OBJECT_ID('Gene.Gene_LogHistory')) BEGIN
	DROP TRIGGER Gene.Gene_LogHistory
END
GO
CREATE TRIGGER [Gene].[Gene_LogHistory] ON Gene.Gene
	AFTER INSERT, UPDATE
AS
BEGIN

	INSERT INTO Gene.GeneHistory (RevisionID, ID, Name, [Definition], SourceID,
									GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID,
									Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd,
									[Description], Active, LastUpdatedAt, LastUpdateSourceID)
		SELECT ISNULL((SELECT MAX(RevisionID) FROM Gene.GeneHistory g WHERE g.ID = i.ID), 0) + 1
				,i.ID
				,i.Name
				,i.[Definition]
				,i.SourceID
				,i.GenBankID
				,i.Locus
				,i.Accession
				,i.Organism
				,i.Taxonomy
				,i.TaxonomyID
				,i.Nucleotides
				,i.SequenceTypeID
				,i.SequenceStart
				,i.SequenceEnd
				,i.CodingSequenceStart
				,i.CodingSequenceEnd
				,i.[Description]
				,i.Active
				,i.LastUpdatedAt
				,i.LastUpdateSourceID
			FROM inserted i

END
GO
ALTER PROCEDURE [Gene].[Gene_Edit]
	@ID uniqueidentifier = NULL OUTPUT,
	@Name varchar(100),
	@Definition varchar(1000),
	@SourceID int,
	@GenBankID int,
	@Locus varchar(100),
	@Accession varchar(20),
	@Organism varchar(250),
	@Taxonomy varchar(4000),
	@Description varchar(MAX),
	@Nucleotides varchar(MAX),
	@SequenceTypeID int,
	@SequenceStart int = NULL,
	@SequenceEnd int = NULL,
	@CodingSequenceStart int = NULL,
	@CodingSequenceEnd int = NULL,
	@LastUpdatedAt datetime2(7) = NULL,
	@LastUpdateSourceID int = NULL,
	@AllowOverwrite bit = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @NowTime datetime2(7) = SYSDATETIME()
			,@OriginalTaxonomyID int
			,@OriginalTaxonomy varchar(MAX)
			,@TaxonomyID int = NULL

	IF @LastUpdateSourceID IS NULL BEGIN
		SET @LastUpdateSourceID = @SourceID
	END

	IF NOT EXISTS (SELECT * FROM Gene.Gene g WHERE g.ID = @ID) BEGIN
		IF (@ID IS NULL) BEGIN
			SET @ID = NEWID()
		END

		IF (@Taxonomy IS NOT NULL) AND (LTRIM(RTRIM(@Taxonomy)) <> '') BEGIN
			EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT
		END

		INSERT INTO Gene.Gene (ID, Name, [Definition], SourceID
								,GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID, [Description]
								,Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd
								,LastUpdatedAt, LastUpdateSourceID)
		VALUES (@ID, @Name, @Definition, @SourceID
				,@GenBankID, @Locus, @Accession, @Organism, @Taxonomy, @TaxonomyID, @Description
				,@Nucleotides, @SequenceTypeID, @SequenceStart, @SequenceEnd, @CodingSequenceStart, @CodingSequenceEnd
				,@LastUpdatedAt, @LastUpdateSourceID)
	END
	ELSE BEGIN
		IF (@AllowOverwrite = 1) BEGIN
			SELECT @OriginalTaxonomyID = g.TaxonomyID
					,@OriginalTaxonomy = g.Taxonomy
				FROM Gene.Gene g WHERE g.ID = @ID

			IF (@Taxonomy IS NOT NULL) AND (LTRIM(RTRIM(@Taxonomy)) <> '') AND (@OriginalTaxonomy <> @Taxonomy OR @OriginalTaxonomyID IS NULL) BEGIN
				EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT
			END
			ELSE IF (@OriginalTaxonomy = @Taxonomy) BEGIN
				SET @TaxonomyID = @OriginalTaxonomyID
			END

			-- Currently not allowing for partial updates (i.e.: ISNULL(@column, column)); whenever a gene is saved and an update is possible all
			-- values must be provided.
			UPDATE Gene.Gene
				SET SourceID = @SourceID
					,Name = @Name
					,[Definition] = @Definition
					,GenBankID = @GenBankID
					,Locus = @Locus
					,Accession = @Accession
					,Organism = @Organism
					,Taxonomy = @Taxonomy
					,TaxonomyID = @TaxonomyID
					,[Description] = @Description
					,Nucleotides = @Nucleotides
					,SequenceTypeID = @SequenceTypeID
					,SequenceStart = @SequenceStart -- start:These are becoming obsolete
					,SequenceEnd = @SequenceEnd
					,CodingSequenceStart = @CodingSequenceStart
					,CodingSequenceEnd = @CodingSequenceEnd -- end:obsolete
					,LastUpdatedAt = @LastUpdatedAt
					,LastUpdateSourceID = @LastUpdateSourceID
				WHERE ID = @ID
		END
	END
END
GO
ALTER PROCEDURE [Gene].[Gene_ForExport]
	@GeneIDs Common.ListUniqueIdentifier READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.Name
			,g.[Definition]
			,g.Organism
			,g.Taxonomy
			,g.Locus
			,g.Accession
			,g.GenBankID
			,src.Name AS [Source]
			,LEN(ISNULL(g.Nucleotides, '')) AS [Length]
			,g.Nucleotides
		FROM Gene.Gene g
		JOIN Gene.[Source] src ON src.ID = g.SourceID
		JOIN @GeneIDs ids ON ids.Value = g.ID
		ORDER BY g.Organism, LEN(ISNULL(g.Nucleotides, '')), g.[Definition]
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
			,sbj.Name
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
ALTER PROCEDURE [Job].[BlastN_ListNotProcessedGenes]
	@JobID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT qry.ID
			,qry.GenBankID
			,qry.Name
			,qry.[Definition]
			,qry.Organism
			,qry.Nucleotides -- Used to calculate the sequence length
		FROM Job.Gene jg
		JOIN Job.GeneDirection dir ON dir.ID = jg.DirectionID
		JOIN Gene.Gene qry ON qry.ID = jg.GeneID
		WHERE jg.JobID = @JobID
			AND dir.[Key] = 'Input' 
			AND NOT EXISTS (SELECT *
								FROM NCBI.Gene ng
								JOIN NCBI.Request req ON req.JobID = @JobID AND req.ID = ng.RequestID
								JOIN Job.GeneStatus stat ON stat.ID = ng.StatusID
								WHERE ng.GeneID = jg.GeneID
									AND stat.[Key] IN ('Processed'))
		ORDER BY qry.Organism, qry.[Definition]
END
GO
ALTER PROCEDURE [Job].[BlastN_ListQueryGenesForAlignment]
	@SubjectGeneID uniqueidentifier,
	@RequestID int,
	@JobID uniqueidentifier,
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF @RecordSetID IS NULL AND @JobID IS NULL AND @RequestID IS NULL BEGIN
		RAISERROR('A Request ID, Job ID, or RecordSet ID must be provided', 11, 1)
	END
		
	IF @RecordSetID IS NULL BEGIN
		IF @JobID IS NOT NULL BEGIN
			SELECT @RecordSetID = j.RecordSetID
				FROM Job.Job j
				WHERE j.ID = @JobID
		END
		ELSE BEGIN
			SELECT @RecordSetID = j.RecordSetID
					,@JobID = j.ID
				FROM NCBI.Request req
				JOIN Job.Job j ON j.ID = req.JobID
				WHERE req.ID = @RequestID
		END
	END

	IF @SubjectGeneID IS NOT NULL BEGIN
		SELECT qry.ID
				,qsrc.Name AS QuerySourceName
				,qry.GenBankID
				,qry.Name
				,qry.[Definition]
				,qry.Organism
				,qry.Taxonomy
				,stat.*
			FROM NCBI.Request r
			JOIN NCBI.BlastNAlignment nal ON nal.RequestID = r.ID
			JOIN BlastN.Alignment al ON al.ID = nal.AlignmentID
			JOIN BlastN.AlignmentExon ex ON ex.ID = BlastN.AlignmentExon_First(al.ID)
			JOIN Gene.Gene qry ON qry.ID = al.QueryID
			JOIN Gene.[Source] qsrc ON qsrc.ID = qry.SourceID
			JOIN Job.Job j ON j.ID = r.JobID
			CROSS APPLY BlastN.Alignment_Statistics(al.QueryID, al.SubjectID) stat
			WHERE al.SubjectID = @SubjectGeneID
				AND j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
			ORDER BY stat.[Rank], stat.MaxScore DESC
	END
	ELSE BEGIN
		SELECT DISTINCT
				qry.ID
				,qry.GenBankID
				,qry.Name
				,qry.[Definition]
				,qry.Organism
				,qry.Nucleotides -- Used to calculate the sequence length
			FROM NCBI.Request r
			JOIN NCBI.Gene ng ON ng.RequestID = r.ID
			JOIN Gene.Gene qry ON qry.ID = ng.GeneID
			JOIN Job.Job j ON j.ID = r.JobID
			WHERE j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
				AND (@RequestID IS NULL OR r.ID = @RequestID)
			ORDER BY qry.Organism, qry.[Definition]
	END
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
			,sbj.Name
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
IF EXISTS (SELECT * FROM sys.views v WHERE v.object_id = OBJECT_ID('Gene.Gene_ListView')) BEGIN
	DROP VIEW Gene.Gene_ListView
END
GO
CREATE VIEW Gene.Gene_ListView
AS
	SELECT g.ID
			,g.Name
			,g.[Definition]
			,g.SourceID
			
			,g.GenBankID
			,g.Locus
			,g.Accession
			,g.Organism
			,g.Taxonomy
			,g.TaxonomyID

			,g.Nucleotides
			,g.SequenceTypeID
			,g.SequenceStart
			,g.SequenceEnd
			,g.CodingSequenceStart
			,g.CodingSequenceEnd
			
			,g.[Description]
			,g.Active
			,g.LastUpdatedAt
			,g.LastUpdateSourceID

			,CASE WHEN (t.ID IS NOT NULL)
				  THEN t.HID.ToString()
				  ELSE ''
				  END AS TaxonomyHierarchy
		FROM Gene.Gene g
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
GO
ALTER PROCEDURE Gene.Gene_Get
	@GeneID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.*
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.QueryID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedSubjectSequences
			,CAST(CASE WHEN EXISTS (SELECT *
									FROM BlastN.Alignment a
									WHERE a.SubjectID = g.ID)
				  THEN 1
				  ELSE 0
				  END AS bit) AS HasAlignedQuerySequences

		FROM Gene.Gene_ListView g
		
		WHERE g.ID = @GeneID
		ORDER BY g.Organism, g.Accession, g.GenBankID
END
GO
ALTER PROCEDURE [RecordSet].[Gene_List]
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	SELECT g.*
			,rs_g.ModifiedAt

		FROM RecordSet.Gene rs_g
		JOIN Gene.Gene_ListView g ON g.ID = rs_g.GeneID
		
		WHERE rs_g.RecordSetID = @RecordSetID
			AND g.Active = 1
		ORDER BY rs_g.ModifiedAt DESC, g.Organism, g.Accession, g.GenBankID
END
GO
ALTER PROCEDURE [RecordSet].[SubSetGene_List]
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @GeneStatusID_Processed int = (SELECT s.ID FROM Job.GeneStatus s WHERE s.[Key] = 'Processed')

	SELECT g.*
			,sg.ModifiedAt

			,CAST(CASE WHEN EXISTS (SELECT * 
										FROM NCBI.Gene ng
										WHERE ng.GeneID = sg.GeneID
											AND ng.StatusID = @GeneStatusID_Processed)
				-- We're not doing a LEFT OUTER JOIN for this because a gene could be submitted multiple times, thus multiple requests
						THEN 1
						ELSE 0
						END AS bit) AS ProcessedThroughBLASTNAtNCBI
			
		FROM RecordSet.SubSetGene sg
		JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
		JOIN RecordSet.Gene rs_g ON rs_g.GeneID = sg.GeneID AND rs_g.RecordSetID = sub.RecordSetID
		JOIN Gene.Gene_ListView g ON g.ID = rs_g.GeneID
		LEFT OUTER JOIN Taxonomy.Taxon t ON t.ID = g.TaxonomyID
		
		WHERE sg.SubSetID = @SubSetID
			AND g.Active = 1
		ORDER BY ISNULL(g.Organism, g.[Definition]), LEN(ISNULL(g.Nucleotides, '')), rs_g.ModifiedAt DESC
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
			,Gene.IsInAnySubSet(g.ID, @RecordSetID) AS InRecordSet
		FROM Job.Gene jg
		JOIN Gene.Gene_ListView g ON g.ID = jg.GeneID
		WHERE jg.JobID = @JobID
			AND jg.DirectionID = @GeneDirectionID
			AND g.Active = 1
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_EditName_Multiple')) BEGIN
	DROP PROCEDURE Gene.Gene_EditName_Multiple
END
GO
CREATE PROCEDURE Gene.Gene_EditName_Multiple
	@GeneIDs Common.ListUniqueIdentifier READONLY,
	@Name varchar(100),
	@LastUpdateSourceID int
AS
BEGIN
	SET NOCOUNT ON

	UPDATE g
		SET g.Name = @Name
			,g.LastUpdatedAt = SYSDATETIME()
			,g.LastUpdateSourceID = @LastUpdateSourceID
		FROM Gene.Gene g
		JOIN @GeneIDs id ON id.Value = g.ID
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Gene.Gene_ListAllOrganismNames')) BEGIN
	DROP PROCEDURE Gene.Gene_ListAllOrganismNames
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.SubSetGene_ListReferenceNames')) BEGIN
	DROP PROCEDURE RecordSet.SubSetGene_ListReferenceNames
END
GO
CREATE PROCEDURE RecordSet.SubSetGene_ListReferenceNames
	@SubSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	-- Organism names
	SELECT DISTINCT g.Organism
		FROM RecordSet.SubSetGene sg
		JOIN Gene.Gene g ON g.ID = sg.GeneID
		WHERE sg.SubSetID = @SubSetID
			AND g.Active = 1
			AND g.Organism IS NOT NULL
		ORDER BY g.Organism

	-- Gene names
	SELECT DISTINCT g.Name AS GeneName
		-- If I decide this should be case-sensitive: SELECT DISTINCT g.Name COLLATE SQL_Latin1_General_CP1_CS_AS AS GeneName
		FROM RecordSet.SubSetGene sg
		JOIN Gene.Gene g ON g.ID = sg.GeneID
		WHERE sg.SubSetID = @SubSetID
			AND g.Active = 1
			AND g.Name IS NOT NULL AND g.Name <> ''
		ORDER BY GeneName
END
GO

GO
UPDATE Common.ApplicationProperty
	SET Value = '1.4.1.2'
	WHERE [Key] = 'DatabaseVersion'
GO
