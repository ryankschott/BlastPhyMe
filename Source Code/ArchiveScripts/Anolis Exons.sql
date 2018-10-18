USE anolis_carolinensis_core_75_2;

SELECT g.gene_id
		,g.seq_region_id
		,x.display_label
		,g.description
		,g.stable_id
	FROM gene g
	JOIN xref x ON x.xref_id = g.display_xref_id
	WHERE g.biotype = 'protein_coding'
		AND x.display_label LIKE '%RPE65%'

SELECT * FROM gene g WHERE g.gene_id = 1841
SELECT * FROM gene g WHERE g.seq_region_id = 29793

SELECT *
	FROM transcript t
	WHERE t.stable_id = 'ENSACAT00000011562'

SELECT *
	FROM seq_region sr
	WHERE EXISTS (SELECT d.seq_region_id FROM dna d WHERE d.seq_region_id = sr.seq_region_id)
		AND EXISTS (SELECT g.seq_region_id FROM gene g WHERE g.seq_region_id = sr.seq_region_id AND g.biotype = 'protein_coding')
	LIMIT 10

SELECT e.*
	FROM exon e
	JOIN exon_transcript et ON et.exon_id = e.exon_id
	WHERE et.transcript_id = 1908

SELECT *
	FROM seq_region sr
	WHERE sr.seq_region

SELECT SUBSTRING(d.sequence, 41279, 41335-41278)
	FROM dna d
	WHERE d.seq_region_id = 29793 -- 

SELECT LENGTH(d.sequence)
	FROM dna d
	WHERE d.seq_region_id = 29579
	AND d.sequence LIKE '%AGCTACCGTTTGTTTTTGACCTACGACATACACGGGTGAAAAAGGCCTTGGTAAAATAGG%'

SELECT REVERSE('GGATAAAATGGTTCCGGAAAAAGTGGGCACATACAGCATCCAGTTTTTGTTTGCCATCGA')

SELECT *
	FROM translation t
	WHERE t.transcript_id = 1908

