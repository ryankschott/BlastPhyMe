USE [LocalDB]
GO
SELECT COUNT(*)
	FROM BlastN.ResultSetData d
	WHERE d.result_set_id = (SELECT TOP 1 ID FROM BlastN.ResultSet rs ORDER BY rs.created_at DESC)

SELECT * FROM BlastN.ResultSet rs ORDER BY rs.created_at DESC

DECLARE @result_set_id int = 14
	
SELECT REPLACE(REPLACE(d.source_file, 'C:\Data\ChangLab\UTF_NCBI_GenBank\', ''), '_shortened.gb', '') AS [source]
		,REPLACE(d.subject_query_match, 'PREDICTED: ', '') AS match_header
		,CAST(
			CASE WHEN CHARINDEX('PREDICTED: ', d.subject_query_match) > 0
				 THEN 1
				 ELSE 0
				 END AS bit) AS Predicted
		,'http://www.ncbi.nlm.nih.gov/nucleotide/' + CAST(d.genbank_id AS varchar(20)) AS genbank_url
		,AVG(d.percentage_sequence_match) AS average_percentage_sequence_match
	FROM BlastN.ResultSetData d
	WHERE d.result_set_id = @result_set_id
		--AND d.source_file LIKE '%RPE65_shortened.gb'
	GROUP BY d.subject_query_match, d.genbank_id, d.source_file
	ORDER BY d.source_file, match_header