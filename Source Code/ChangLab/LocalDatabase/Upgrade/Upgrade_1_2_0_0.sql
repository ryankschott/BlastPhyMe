SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('Common.ThrowException')) BEGIN
	DROP PROCEDURE Common.ThrowException
END
GO
CREATE PROCEDURE Common.ThrowException
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Message nvarchar(MAX)
			,@Procedure nvarchar(250)
			,@Severity int
			,@State int
			,@Line int

	SELECT @Message = ERROR_MESSAGE()
			,@Procedure = ERROR_PROCEDURE()
			,@Severity = ERROR_SEVERITY()
			,@State = ERROR_STATE()
			,@Line = ERROR_LINE()
	IF @Procedure IS NOT NULL BEGIN
		SET @Message += ' (' + ERROR_PROCEDURE() + ': line ' + CAST(ERROR_LINE() AS varchar(8)) + ')'
	END

	RAISERROR (@Message, @Severity, @State)
END
GO
IF NOT EXISTS (SELECT * FROM sys.table_types WHERE user_type_id = TYPE_ID('Common.ListUniqueIdentifier')) BEGIN
	CREATE TYPE [Common].[ListUniqueIdentifier] AS TABLE(
		[Value] uniqueidentifier NOT NULL
	)
END
GO

ALTER PROCEDURE Job.BlastN_ListQueryGenesForAlignment
	@SubjectGeneID uniqueidentifier,
	@JobID uniqueidentifier,
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	IF @RecordSetID IS NULL AND @JobID IS NULL BEGIN
		RAISERROR('A Job ID or a RecordSet ID must be provided', 11, 1)
	END
		
	IF @RecordSetID IS NULL BEGIN
		SELECT @RecordSetID = j.RecordSetID
			FROM Job.Job j
			WHERE j.ID = @JobID
	END

	IF @SubjectGeneID IS NOT NULL BEGIN
		SELECT qry.ID
				,qsrc.Name AS QuerySourceName
				,qry.GenBankID
				,qry.[Definition]
				,qry.Organism
				,qry.Taxonomy
				,al.[Rank]
				,ex.AlignmentLength
				,ex.BitScore
				,CAST(Common.CalculatePercentageFromInt(ex.IdentitiesCount, ex.AlignmentLength) AS int) AS AlignmentPercentage
				,BlastN.Alignment_QueryCover(al.ID) AS QueryCover
				,ex.Gaps
			FROM NCBI.Request r
			JOIN NCBI.BlastNAlignment nal ON nal.RequestID = r.ID
			JOIN BlastN.Alignment al ON al.ID = nal.AlignmentID
			JOIN BlastN.AlignmentExon ex ON ex.ID = BlastN.AlignmentExon_First(al.ID)
			JOIN Gene.Gene qry ON qry.ID = al.QueryID
			JOIN Gene.[Source] qsrc ON qsrc.ID = qry.SourceID
			JOIN Job.Job j ON j.ID = r.JobID
			WHERE al.SubjectID = @SubjectGeneID
				AND j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
			ORDER BY AlignmentPercentage DESC, BitScore DESC
	END
	ELSE BEGIN
		SELECT DISTINCT
				qry.ID
				,qry.GenBankID
				,qry.[Definition]
				,qry.Organism
				,qry.CodingSequenceStart
				,qry.CodingSequenceEnd
			FROM NCBI.Request r
			JOIN NCBI.Gene ng ON ng.RequestID = r.ID
			JOIN Gene.Gene qry ON qry.ID = ng.GeneID
			JOIN Job.Job j ON j.ID = r.JobID
			WHERE j.RecordSetID = @RecordSetID
				AND (@JobID IS NULL OR r.JobID = @JobID)
				AND ng.DirectionID = 1
			ORDER BY qry.Organism, qry.[Definition]
	END
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.RecordSet_Export')) BEGIN
	DROP PROCEDURE RecordSet.RecordSet_Export
END
GO
CREATE PROCEDURE RecordSet.RecordSet_Export
	@RecordSetID uniqueidentifier
	,@JobHistory bit = 0
AS
BEGIN
	SET NOCOUNT ON

	IF (@JobHistory = 0) BEGIN
		SELECT (SELECT [Properties].Value AS [DatabaseVersion]
					FROM Common.ApplicationProperty [Properties]
					WHERE [Properties].[Key] = 'DatabaseVersion'
					FOR XML AUTO, ELEMENTS)
		UNION ALL
		SELECT (SELECT [RecordSet].*
						,[Properties].[Key]
						,[Properties].Value
					FROM RecordSet.RecordSet [RecordSet]
					JOIN RecordSet.ApplicationProperty [Properties] ON [Properties].RecordSetID = [RecordSet].ID
					WHERE [RecordSet].ID = @RecordSetID
					FOR XML AUTO, ELEMENTS)
		UNION ALL 
		SELECT (SELECT [SubSet].ID
						,[SubSet].Name
						,[SubSet].LastOpenedAt
						,[SubSet].[Open]
						,[SubSet-Gene].GeneID
						,[SubSet-Gene].ModifiedAt
					FROM RecordSet.RecordSet [RecordSet]
					JOIN RecordSet.SubSet [SubSet] ON [SubSet].RecordSetID = [RecordSet].ID
					JOIN RecordSet.SubSetGene [SubSet-Gene] ON [SubSet-Gene].SubSetID = [SubSet].ID
					JOIN Gene.Gene g ON g.ID = [SubSet-Gene].GeneID
					WHERE [RecordSet].ID = @RecordSetID
						AND [SubSet].Active = 1
						AND g.Active = 1
					ORDER BY [SubSet].Name
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-SubSet'))
		UNION ALL
		SELECT (SELECT [Gene].*
					FROM (
						SELECT DISTINCT [Gene].*
							FROM RecordSet.RecordSet rs
							JOIN RecordSet.SubSet sub ON sub.RecordSetID = rs.ID
							JOIN RecordSet.SubSetGene sg ON sg.SubSetID = sub.ID
							JOIN Gene.Gene [Gene] ON [Gene].ID = sg.GeneID
							WHERE rs.ID = @RecordSetID
								AND sub.Active = 1
								AND [Gene].Active = 1
					) [Gene]
					ORDER BY ID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Gene'))
	END
	ELSE BEGIN
		SELECT (SELECT [Job].ID
						,[Job].TargetID
						,[Job].StartedAt
						,[Job].EndedAt
						,[Job].StatusID
						,[Job].SubSetID
						,[Job-Gene].GeneID
						,[Job-Gene].DirectionID
					FROM Job.Job [Job]
					JOIN Job.Gene [Job-Gene] ON [Job-Gene].JobID = [Job].ID
					WHERE [Job].RecordSetID = @RecordSetID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Job'))
		UNION ALL
		SELECT (SELECT [Request].*
						,[Request-Gene].GeneID
						,[Request-Gene].DirectionID
					FROM NCBI.Request [Request]
					JOIN Job.Job j ON j.ID = [Request].JobID
					JOIN NCBI.Gene [Request-Gene] ON [Request-Gene].RequestID = [Request].ID
					WHERE j.RecordSetID = @RecordSetID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-NCBI-Request'))
		UNION ALL
		SELECT (SELECT [Gene].*
					FROM (
						SELECT DISTINCT [Gene].* -- Pick up the Genes from BLASTN results
							FROM Job.Job j
							JOIN NCBI.Request req ON req.JobID = j.ID
							JOIN NCBI.BlastNAlignment n_al ON n_al.RequestID = req.ID
							JOIN BlastN.Alignment al ON al.ID = n_al.AlignmentID
							JOIN Gene.Gene [Gene] ON [Gene].ID = al.SubjectID
							WHERE j.RecordSetID = @RecordSetID
								AND [Gene].Active = 1
								AND NOT EXISTS (SELECT * -- This will gather the genes that weren't added to a subset
													FROM RecordSet.Gene g
													WHERE g.GeneID = [Gene].ID
														AND g.RecordSetID = @RecordSetID)
					) [Gene]
					ORDER BY ID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Subject-Gene'))
		UNION ALL
		SELECT (SELECT DISTINCT [Alignment].*
						,[Alignment-Exon].OrientationID
						,[Alignment-Exon].BitScore
						,[Alignment-Exon].AlignmentLength
						,[Alignment-Exon].IdentitiesCount
						,[Alignment-Exon].Gaps
						,[Alignment-Exon].QueryRangeStart
						,[Alignment-Exon].QueryRangeEnd
						,[Alignment-Exon].SubjectRangeStart
						,[Alignment-Exon].SubjectRangeEnd
					FROM BlastN.Alignment [Alignment]
					JOIN BlastN.AlignmentExon [Alignment-Exon] ON [Alignment-Exon].AlignmentID = [Alignment].ID
					JOIN NCBI.BlastNAlignment n_al ON n_al.AlignmentID = [Alignment].ID
					JOIN NCBI.Request req ON req.ID = n_al.RequestID
					JOIN Job.Job j ON j.ID = req.JobID
					JOIN Gene.Gene qry ON qry.ID = Alignment.QueryID
					JOIN Gene.Gene sbj ON sbj.ID = Alignment.SubjectID
					WHERE qry.Active = 1
						AND sbj.Active = 1
						AND j.RecordSetID = @RecordSetID
					FOR XML AUTO, ELEMENTS, ROOT ('RecordSet-Alignment'))
		UNION ALL
		SELECT (SELECT [Request].ID
						,(SELECT [Alignment].AlignmentID
								FROM NCBI.BlastNAlignment [Alignment] 
								WHERE [Alignment].RequestID = [Request].ID
								FOR XML PATH(''), TYPE) AS "Alignments"
					FROM NCBI.Request [Request]
					JOIN Job.Job j ON j.ID = [Request].JobID
					WHERE j.RecordSetID = @RecordSetID
						AND EXISTS (SELECT * FROM NCBI.BlastNAlignment ex WHERE ex.RequestID = [Request].ID)
					FOR XML PATH('Request'), ROOT ('RecordSet-Request-Alignment'))
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_RecordSet')) BEGIN
	DROP PROCEDURE RecordSet.Import_RecordSet
END
GO
CREATE PROCEDURE RecordSet.Import_RecordSet
	@RecordSetID uniqueidentifier = NULL OUTPUT
	,@Name varchar(200)
	,@CreatedAt datetime2(7)
	,@LastOpenedAt datetime2(7)
AS
BEGIN
	SET NOCOUNT ON

	IF @RecordSetID IS NULL BEGIN
		SET @RecordSetID = NEWID()
	END

	INSERT INTO RecordSet.RecordSet (ID, Name, CreatedAt, LastOpenedAt)
	VALUES (@RecordSetID, @Name, @CreatedAt, @LastOpenedAt)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_RecordSet_Property')) BEGIN
	DROP PROCEDURE RecordSet.Import_RecordSet_Property
END
GO
CREATE PROCEDURE RecordSet.Import_RecordSet_Property
	@RecordSetID uniqueidentifier
	,@Key varchar(30)
	,@Value varchar(MAX)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO RecordSet.ApplicationProperty (ID, RecordSetID, [Key], Value)
	VALUES (NEWID(), @RecordSetID, @Key, @Value)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_RecordSet_Gene')) BEGIN
	DROP PROCEDURE RecordSet.Import_RecordSet_Gene
END
GO
CREATE PROCEDURE RecordSet.Import_RecordSet_Gene
	@RecordSetID uniqueidentifier = NULL
	,@ID uniqueidentifier = NULL OUTPUT
	,@Definition varchar(1000)
	,@SourceID int
	,@GenBankID int = NULL
	,@Locus varchar(100) = NULL
	,@Accession varchar(20) = NULL
	,@Organism varchar(250) = NULL
	,@Taxonomy varchar(4000) = NULL
	,@Nucleotides varchar(MAX) = NULL
	,@SequenceTypeID int = NULL
	,@SequenceStart int = NULL
	,@SequenceEnd int = NULL
	,@CodingSequenceStart int = NULL
	,@CodingSequenceEnd int = NULL
	,@LastUpdatedAt datetime2(7) = NULL
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @TaxonomyID int
			,@ExistingLastUpdatedAt datetime2(7)
			,@Overwrite bit = 0

	IF (@Taxonomy IS NOT NULL) AND (LTRIM(RTRIM(@Taxonomy)) <> '') BEGIN
		EXEC Taxonomy.Taxon_Parse @Taxonomy, @TaxonomyID OUTPUT
	END

	IF @GenBankID <> 0 AND EXISTS (SELECT * FROM Gene.Gene g WHERE g.GenBankID = @GenBankID)
	BEGIN
		SELECT @ID = g.ID FROM Gene.Gene g WHERE g.GenBankID = @GenBankID
		SELECT @ExistingLastUpdatedAt FROM Gene.Gene g WHERE g.GenBankID = @GenBankID

		IF @ExistingLastUpdatedAt < @LastUpdatedAt BEGIN
			-- If the existing record is newer then we won't overwrite it; if it's the same date we assume it has the same data.
			SET @Overwrite = 1
		END
	END

	IF @ID IS NULL BEGIN
		SET @ID = NEWID()

		INSERT INTO Gene.Gene (ID, [Definition], SourceID
								,GenBankID, Locus, Accession, Organism, Taxonomy, TaxonomyID
								,Nucleotides, SequenceTypeID, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd
								,LastUpdatedAt)
		VALUES (@ID, @Definition, @SourceID
				,@GenBankID, @Locus, @Accession, @Organism, @Taxonomy, @TaxonomyID
				,@Nucleotides, @SequenceTypeID, @SequenceStart, @SequenceEnd, @CodingSequenceStart, @CodingSequenceEnd
				,@LastUpdatedAt)
	END
	ELSE IF @Overwrite = 1 BEGIN
		UPDATE Gene.Gene
			SET SourceID = ISNULL(@SourceID, SourceID)
				,[Definition] = ISNULL(@Definition, [Definition])
				,Locus = ISNULL(@Locus, Locus)
				,Accession = ISNULL(@Accession, Accession)
				,Organism = ISNULL(@Organism, Organism)
				,Taxonomy = ISNULL(@Taxonomy, Taxonomy)
				,TaxonomyID = ISNULL(@TaxonomyID, TaxonomyID)
				,Nucleotides = ISNULL(@Nucleotides, Nucleotides)
				,SequenceTypeID = ISNULL(@SequenceTypeID, SequenceTypeID)
				,SequenceStart = ISNULL(@SequenceStart, SequenceStart)
				,SequenceEnd = ISNULL(@SequenceEnd, SequenceEnd)
				,CodingSequenceStart = ISNULL(@CodingSequenceStart, CodingSequenceStart)
				,CodingSequenceEnd = ISNULL(@CodingSequenceEnd, CodingSequenceEnd)
				,LastUpdatedAt = ISNULL(@LastUpdatedAt, LastUpdatedAt)
			WHERE ID = @ID
	END

	IF (@RecordSetID IS NOT NULL) BEGIN
		-- Finally, tag the gene against the recordset.
		INSERT INTO RecordSet.Gene (RecordSetID, GeneID)
		VALUES (@RecordSetID, @ID)
		-- If we're importing subject sequences from BLASTN results, we won't have a recordset ID.
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_SubSet')) BEGIN
	DROP PROCEDURE RecordSet.Import_SubSet
END
GO
CREATE PROCEDURE RecordSet.Import_SubSet
	@RecordSetID uniqueidentifier
	,@ID uniqueidentifier = NULL OUTPUT
	,@Name varchar(100)
	,@LastOpenedAt datetime2(7) = NULL
	,@Open bit
AS
BEGIN
	SET NOCOUNT ON
	SET @ID = NEWID()

	INSERT INTO RecordSet.SubSet (ID, RecordSetID, Name, LastOpenedAt, [Open])
	VALUES (@ID, @RecordSetID, @Name, @LastOpenedAt, @Open)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_SubSet_Gene')) BEGIN
	DROP PROCEDURE RecordSet.Import_SubSet_Gene
END
GO
CREATE PROCEDURE RecordSet.Import_SubSet_Gene
	@SubSetID uniqueidentifier
	,@GeneID uniqueidentifier
	,@ModifiedAt datetime2(7)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO RecordSet.SubSetGene (SubSetID, GeneID, ModifiedAt)
	VALUES (@SubSetID, @GeneID, @ModifiedAt)
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_Job')) BEGIN
	DROP PROCEDURE RecordSet.Import_Job
END
GO
CREATE PROCEDURE RecordSet.Import_Job
	@RecordSetID uniqueidentifier
	,@ID uniqueidentifier = NULL OUTPUT
	,@SubSetID uniqueidentifier
	,@TargetID int
	,@StatusID int
	,@StartedAt datetime2(7) = NULL
	,@EndedAt datetime2(7) = NULL
AS
BEGIN
	SET NOCOUNT ON
	SET @ID = NEWID()

	INSERT INTO Job.Job (ID, RecordSetID, SubSetID, TargetID, StatusID, StartedAt, EndedAt)
	VALUES (@ID, @RecordSetID, @SubSetID, @TargetID, @StatusID, @StartedAt, @EndedAt)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_Job_Gene')) BEGIN
	DROP PROCEDURE RecordSet.Import_Job_Gene
END
GO
CREATE PROCEDURE RecordSet.Import_Job_Gene
	@JobID uniqueidentifier
	,@GeneID uniqueidentifier
	,@DirectionID int
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Job.Gene (JobID, GeneID, DirectionID)
	VALUES (@JobID, @GeneID, @DirectionID)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_NCBI_Request')) BEGIN
	DROP PROCEDURE RecordSet.Import_NCBI_Request
END
GO
CREATE PROCEDURE RecordSet.Import_NCBI_Request
	@JobID uniqueidentifier
	,@ID uniqueidentifier = NULL OUTPUT
	,@RequestID varchar(20)
	,@StartTime datetime2(7)
	,@EndTime datetime2(7) = NULL
	,@LastStatus varchar(8) = NULL
	,@LastUpdatedAt datetime2(7)
	,@StatusInformation varchar(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON
	SET @ID = NEWID()

	INSERT INTO NCBI.Request (ID, RequestID, JobID, StartTime, EndTime, LastStatus, LastUpdatedAt, StatusInformation)
	VALUES (@ID, @RequestID, @JobID, @StartTime, @EndTime, @LastStatus, @LastUpdatedAt, @StatusInformation)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_NCBI_Gene')) BEGIN
	DROP PROCEDURE RecordSet.Import_NCBI_Gene
END
GO
CREATE PROCEDURE RecordSet.Import_NCBI_Gene
	@RequestID uniqueidentifier
	,@GeneID uniqueidentifier
	,@DirectionID int
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO NCBI.Gene (RequestID, GeneID, DirectionID)
	VALUES (@RequestID, @GeneID, @DirectionID)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_BlastN_Alignment')) BEGIN
	DROP PROCEDURE RecordSet.Import_BlastN_Alignment
END
GO
CREATE PROCEDURE RecordSet.Import_BlastN_Alignment
	@ID uniqueidentifier = NULL OUTPUT
	,@QueryID uniqueidentifier
	,@SubjectID uniqueidentifier
	,@Rank int
AS
BEGIN
	SET NOCOUNT ON
	SET @ID = NEWID()

	INSERT INTO BlastN.Alignment (ID, QueryID, SubjectID, [Rank])
	VALUES (@ID, @QueryID, @SubjectID, @Rank)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_BlastN_AlignmentExon')) BEGIN
	DROP PROCEDURE RecordSet.Import_BlastN_AlignmentExon
END
GO
CREATE PROCEDURE RecordSet.Import_BlastN_AlignmentExon
	@AlignmentID uniqueidentifier
	,@OrientationID int
	,@BitScore float
	,@AlignmentLength int
	,@IdentitiesCount int
	,@Gaps int
	,@QueryRangeStart int
	,@QueryRangeEnd int
	,@SubjectRangeStart int
	,@SubjectRangeEnd int
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @ID uniqueidentifier = NEWID()

	INSERT INTO BlastN.AlignmentExon (ID, AlignmentID, OrientationID, BitScore, AlignmentLength, IdentitiesCount, Gaps, QueryRangeStart, QueryRangeEnd, SubjectRangeStart, SubjectRangeEnd)
	VALUES (@ID, @AlignmentID, @OrientationID, @BitScore, @AlignmentLength, @IdentitiesCount, @Gaps, @QueryRangeStart, @QueryRangeEnd, @SubjectRangeStart, @SubjectRangeEnd)
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_NCBI_BlastNAlignment')) BEGIN
	DROP PROCEDURE RecordSet.Import_NCBI_BlastNAlignment
END
GO
CREATE PROCEDURE RecordSet.Import_NCBI_BlastNAlignment
	@RequestID uniqueidentifier
	,@AlignmentIDs Common.ListUniqueIdentifier READONLY
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO NCBI.BlastNAlignment (RequestID, AlignmentID)
	SELECT @RequestID, ids.Value
		FROM @AlignmentIDs ids
END
GO

IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('RecordSet.Import_Rollback')) BEGIN
	DROP PROCEDURE RecordSet.Import_Rollback
END
GO
CREATE PROCEDURE RecordSet.Import_Rollback
	@RecordSetID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRANSACTION
	BEGIN TRY
		DELETE sg
			FROM RecordSet.SubSetGene sg
			JOIN RecordSet.SubSet sub ON sub.ID = sg.SubSetID
			WHERE sub.RecordSetID = @RecordSetID
		DELETE sub
			FROM RecordSet.SubSet sub
			WHERE sub.RecordSetID = @RecordSetID

		-- Capture the genes that belong to this recordset; if they don't exist in any other recordset they can be deleted from Gene.Gene.
		DECLARE @Genes TABLE (ID uniqueidentifier)
		INSERT INTO @Genes
		SELECT g.GeneID
			FROM RecordSet.Gene g
			WHERE g.RecordSetID = @RecordSetID
		DELETE g
			FROM RecordSet.Gene g
			WHERE g.RecordSetID = @RecordSetID

		DELETE ap
			FROM RecordSet.ApplicationProperty ap
			WHERE ap.RecordSetID = @RecordSetID
		DELETE rs
			FROM RecordSet.RecordSet rs
			WHERE rs.ID = @RecordSetID

		-- Same idea as with Genes
		DECLARE @Alignments TABLE (ID uniqueidentifier)
		INSERT INTO @Alignments
		SELECT DISTINCT n_al.AlignmentID
			FROM NCBI.BlastNAlignment n_al
			JOIN NCBI.Request req ON req.ID = n_al.RequestID
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE j.RecordSetID = @RecordSetID
		-- Pick up the subject genes
		INSERT INTO @Genes
		SELECT DISTINCT al.SubjectID
			FROM BlastN.Alignment al 
			JOIN NCBI.BlastNAlignment n_al ON n_al.AlignmentID = al.ID
			JOIN NCBI.Request req ON req.ID = n_al.RequestID
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE j.RecordSetID = @RecordSetID

		DELETE n_al
			FROM NCBI.BlastNAlignment n_al
			JOIN @Alignments al ON al.ID = n_al.AlignmentID
		DELETE ex
			FROM BlastN.AlignmentExon ex
			JOIN BlastN.Alignment al ON al.ID = ex.AlignmentID
			JOIN @Alignments n_al ON n_al.ID = al.ID
			WHERE NOT EXISTS (SELECT * -- Don't delete if it's aligned via a different recordset's request.
								FROM NCBI.BlastNAlignment existing
								WHERE existing.AlignmentID = al.ID)
		DELETE al
			FROM BlastN.Alignment al 
			JOIN @Alignments n_al ON n_al.ID = al.ID
			WHERE NOT EXISTS (SELECT *
								FROM NCBI.BlastNAlignment existing
								WHERE existing.AlignmentID = al.ID)
		DELETE g
			FROM NCBI.Gene g
			JOIN NCBI.Request req ON req.ID = g.RequestID
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE req
			FROM NCBI.Request req
			JOIN Job.Job j ON j.ID = req.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE g
			FROM Job.Gene g
			JOIN Job.Job j ON j.ID = g.JobID
			WHERE j.RecordSetID = @RecordSetID
		DELETE j
			FROM Job.Job j
			WHERE j.RecordSetID = @RecordSetID

		DELETE g -- Remove any orphaned gene sequences
			FROM Gene.GeneHistory g
			JOIN @Genes id ON id.ID = g.ID
			WHERE NOT EXISTS (SELECT *
								FROM RecordSet.Gene rs_g
								WHERE rs_g.GeneID = g.ID)
			AND NOT EXISTS (SELECT *
								FROM BlastN.Alignment al
								WHERE (al.SubjectID = g.ID OR al.QueryID = g.ID))
		DELETE g
			FROM Gene.Gene g
			JOIN @Genes id ON id.ID = g.ID
			WHERE NOT EXISTS (SELECT *
								FROM RecordSet.Gene rs_g
								WHERE rs_g.GeneID = g.ID)
			AND NOT EXISTS (SELECT *
								FROM BlastN.Alignment al
								WHERE (al.SubjectID = g.ID OR al.QueryID = g.ID))

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		
		EXEC Common.ThrowException
	END CATCH
END
GO

UPDATE Common.ApplicationProperty
	SET Value = '1.2.0.0'
	WHERE [Key] = 'DatabaseVersion'
GO