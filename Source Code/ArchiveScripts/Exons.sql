USE anolis_carolinensis_core_75_2;

SELECT e.exon_id
		,e.seq_region_start
		,e.seq_region_end
		,e.stable_id
	FROM exon e
	JOIN exon_transcript et ON et.exon_id = e.exon_id
	WHERE et.transcript_id = 1908
	ORDER BY et.rank