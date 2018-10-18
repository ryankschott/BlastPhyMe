USE [LocalDB]
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('BlastN.ResultSet_Edit')) BEGIN
	DROP PROCEDURE BlastN.ResultSet_Edit
END
GO
CREATE PROCEDURE BlastN.ResultSet_Edit
	@id int = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	-- Check to see if the ID submitted is valid and would cause an update, otherwise treat it as a new record.
	IF EXISTS (SELECT id FROM BlastN.ResultSet rs WHERE rs.id = @id) BEGIN
		SELECT @id = id FROM BlastN.ResultSet rs WHERE rs.id = @id
	END

	IF (@id IS NULL) BEGIN
		INSERT INTO BlastN.ResultSet (created_at) VALUES (SYSDATETIME())

		SET @id = @@IDENTITY
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('BlastN.ResultSetData_Edit')) BEGIN
	DROP PROCEDURE BlastN.ResultSetData_Edit
END
GO
CREATE PROCEDURE BlastN.ResultSetData_Edit
	@id int = NULL OUTPUT,
	@result_set_id int,
	@input_index int,
	@original_sequence_source varchar(500),
	@contig_number varchar(100),
	@accession varchar(20),
	@original_sequence_submitted varchar(500),
	@subject_sequence_range_start int,
	@subject_sequence_range_end int,
	@subject_query_match varchar(500),
	@subject_query_match_rank int,
	@percentage_sequence_match int,
	@genbank_id varchar(20),
	@request_id varchar(12),
	@source_file varchar(250),
	@original_sequence_submitted_nucleotides varchar(MAX),
	@alignment_nucleotides varchar(MAX)
AS
BEGIN
	SET NOCOUNT ON

	-- Check to see if the ID submitted is valid and would cause an update, otherwise treat it as a new record.
	IF EXISTS (SELECT * FROM BlastN.ResultSetData d WHERE d.id = @id) BEGIN
		SELECT @id = id FROM BlastN.ResultSetData d WHERE d.id = @id
	END

	IF (@id IS NULL) BEGIN
		INSERT INTO BlastN.ResultSetData
			(result_set_id, input_index, original_sequence_source, contig_number, accession, original_sequence_submitted,
				subject_sequence_range_start, subject_sequence_range_end, subject_query_match, subject_query_match_rank, percentage_sequence_match,
				genbank_id, request_id, source_file, original_sequence_submitted_nucleotides, alignment_nucleotides)
		VALUES
			(@result_set_id, @input_index, @original_sequence_source, @contig_number, @accession, @original_sequence_submitted,
				@subject_sequence_range_start, @subject_sequence_range_end, @subject_query_match, @subject_query_match_rank, @percentage_sequence_match,
				@genbank_id, @request_id, @source_file, @original_sequence_submitted_nucleotides, @alignment_nucleotides)

		SET @id = @@IDENTITY
	END
END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('BlastN.ResultSet_List')) BEGIN
	DROP PROCEDURE BlastN.ResultSet_List
END
GO
CREATE PROCEDURE BlastN.ResultSet_List
	@result_set_id int
AS
BEGIN
	SET NOCOUNT ON

	SELECT id
			,input_index
			,original_sequence_source
			,contig_number
			,accession
			,original_sequence_submitted
			,subject_sequence_range_start
			,subject_sequence_range_end
			,subject_query_match
			,subject_query_match_rank
			,percentage_sequence_match
			,genbank_id
			,request_id
			,source_file
			,alignment_nucleotides
	FROM BlastN.ResultSetData rs
	WHERE rs.result_set_id = @result_set_id
	ORDER BY rs.input_index, rs.subject_query_match_rank

END
GO
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('BlastN.ResultSet_GroupGenBankRecords')) BEGIN
	DROP PROCEDURE BlastN.ResultSet_GroupGenBankRecords
END
GO
CREATE PROCEDURE BlastN.ResultSet_GroupGenBankRecords
	@result_set_id int
AS
BEGIN
	SET NOCOUNT ON

	SELECT REPLACE(REPLACE(d.source_file, 'C:\Data\ChangLab\UTF_NCBI_GenBank\', ''), '.gb', '') AS [source]
			,REPLACE(d.subject_query_match, 'PREDICTED: ', '') AS match_header
			,CAST(
				CASE WHEN CHARINDEX('PREDICTED: ', d.subject_query_match) > 0
					 THEN 1
					 ELSE 0
					 END AS bit) AS Predicted
			,d.genbank_id
			,'http://www.ncbi.nlm.nih.gov/nucleotide/' + CAST(d.genbank_id AS varchar(20)) AS genbank_url
			,AVG(d.percentage_sequence_match) AS average_percentage_sequence_match
		FROM BlastN.ResultSetData d
		WHERE d.result_set_id = @result_set_id
			--AND d.source_file LIKE '%RPE65_shortened.gb'
		GROUP BY d.subject_query_match, d.genbank_id, d.source_file
		ORDER BY d.source_file, match_header
END
GO
--EXEC BlastN.ResultSet_GroupGenBankRecords 14
IF EXISTS (SELECT * FROM sys.procedures p WHERE p.object_id = OBJECT_ID('GenBank.Record_Edit')) BEGIN
	DROP PROCEDURE GenBank.Record_Edit
END
GO
CREATE PROCEDURE GenBank.Record_Edit
	@ID int = NULL OUTPUT,
	@GenBankID varchar(10),
	@Locus varchar(20),
	@Definition varchar(250),
	@Accession varchar(20),
	@FullTaxonomy varchar(1000),
	@SequenceID uniqueidentifier = NULL OUTPUT,
	@Nucleotides varchar(MAX) = NULL,
	@SequenceStart int = NULL,
	@SequenceEnd int = NULL,
	@CDSStart int = NULL,
	@CDSEnd int = NULL,
	@FASTAHeader varchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @SourceID int = (SELECT ID FROM Component.SequenceSource WHERE Name = 'GenBank')

	-- If the GenBank record already exists, we'll do an update.
	IF EXISTS (SELECT * FROM GenBank.Record r 
				WHERE r.ID = @ID
					OR r.GenBankID = @GenBankID) BEGIN
		SELECT @ID = r.ID
			FROM GenBank.Record r 
			WHERE r.ID = @ID
				OR r.GenBankID = @GenBankID
	END
	ELSE BEGIN
		SET @ID = NULL
	END

	IF (@ID IS NULL) BEGIN
		INSERT INTO GenBank.Record (GenBankID, Locus, [Definition], Accession, FullTaxonomy)
		VALUES (@GenBankID, @Locus, @Definition, @Accession, @FullTaxonomy)

		SET @ID = @@IDENTITY
	END
	ELSE BEGIN
		UPDATE GenBank.Record
			SET Locus = @Locus,
				[Definition] = @Definition,
				Accession = @Accession,
				FullTaxonomy = @FullTaxonomy,
				LastUpdatedAt = SYSDATETIME()
			WHERE ID = @ID
	END

	IF (@Nucleotides IS NOT NULL) BEGIN
		-- If the Sequence record already exists, we'll do an update.
		IF (@SequenceID IS NULL) BEGIN
			SELECT @SequenceID = s.ID
				FROM Component.[Sequence] s
				WHERE s.GenBankID = @GenBankID

			IF (@SequenceID IS NULL) BEGIN
				SET @SequenceID = NEWID()
			END
		END
		ELSE BEGIN
			IF EXISTS (SELECT * FROM Component.[Sequence] s WHERE s.GenBankID = @GenBankID) BEGIN
				SELECT @SequenceID = s.ID
					FROM Component.[Sequence] s 
					WHERE s.GenBankID = @GenBankID
			END
		END

		-- SequenceIDs are generated automatically by the ChangLab.Common.Components.NucleotideSequence class, so most of the time @SequenceID will
		-- have a value by the time it reaches here.
		-- Ultimately this should execute Component.Sequence_Edit, which uses this statement
		IF NOT EXISTS (SELECT * FROM Component.[Sequence] s WHERE s.ID = @SequenceID) BEGIN
			INSERT INTO Component.[Sequence] (ID, SourceID, Nucleotides, SequenceStart, SequenceEnd, CodingSequenceStart, CodingSequenceEnd, GenBankID, FASTAHeader)
			VALUES (@SequenceID, @SourceID, @Nucleotides, @SequenceStart, @SequenceEnd, @CDSStart, @CDSEnd, @GenBankID, @FASTAHeader)
		END
		ELSE BEGIN
			UPDATE Component.[Sequence]
				SET Nucleotides = @Nucleotides,
					SequenceStart = @SequenceStart,
					SequenceEnd = @SequenceEnd,
					CodingSequenceStart = @CDSStart,
					CodingSequenceEnd = @CDSEnd,
					GenBankID = @GenBankID,
					FASTAHeader = @FASTAHeader,
					LastUpdatedAt = SYSDATETIME()
				WHERE ID = @SequenceID
		END
	END
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Component.CodingSequenceForSequence')) BEGIN
	DROP FUNCTION Component.CodingSequenceForSequence
END
GO
CREATE FUNCTION [Component].[CodingSequenceForSequence](@SequenceID uniqueidentifier)
RETURNS varchar(MAX)
AS
BEGIN

	DECLARE @SequenceStart int
			,@CDSStart int
			,@CDSEnd int
			,@Nucleotides varchar(MAX)

	SELECT @SequenceStart = s.SequenceStart
			,@CDSStart = s.CodingSequenceStart
			,@CDSEnd = s.CodingSequenceEnd
			,@Nucleotides = s.Nucleotides
		FROM Component.[Sequence] s
		WHERE s.ID = @SequenceID

	IF (@CDSStart IS NULL) OR (@CDSStart = -1)
		OR (@CDSEnd IS NULL) OR (@CDSEnd = -1)
		OR (@CDSStart >= @CDSEnd) BEGIN
		RETURN @Nucleotides
	END
	ELSE BEGIN
		RETURN SUBSTRING(@Nucleotides, (@CDSStart - @SequenceStart), (@CDSEnd - @CDSStart) + 1)
	END

	RETURN @Nucleotides
END
GO
IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = OBJECT_ID('Common.SplitString')) BEGIN
	DROP FUNCTION Common.SplitString
END
GO
CREATE FUNCTION Common.SplitString
(
   @List nvarchar(MAX),
   @Delimiter nvarchar(5)
)
RETURNS @Values TABLE (Value nvarchar(MAX), [Index] int IDENTITY(1,1))
AS
BEGIN
-- Modified, courtesy of http://sqlperformance.com/2012/07/t-sql-queries/split-strings, "Jeff Moden's splitter"

  WITH E1(N)        AS ( SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 
                         UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 
                         UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1),
       E2(N)        AS (SELECT 1 FROM E1 a, E1 b),
       E4(N)        AS (SELECT 1 FROM E2 a, E2 b),
       E42(N)       AS (SELECT 1 FROM E4 a, E2 b),
       cteTally(N)  AS (SELECT 0 UNION ALL SELECT TOP (DATALENGTH(ISNULL(@List,1))) 
                         ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM E42),
       cteStart(N1) AS (SELECT t.N+1 FROM cteTally t
                         WHERE (SUBSTRING(@List,t.N,1) = @Delimiter OR t.N = 0))

	INSERT INTO @Values
		SELECT Item = SUBSTRING(@List, s.N1, ISNULL(NULLIF(CHARINDEX(@Delimiter,@List,s.N1),0)-s.N1,8000))
			FROM cteStart s

	RETURN
END
GO