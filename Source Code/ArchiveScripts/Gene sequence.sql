USE canis_familiaris_core_75_31;

SELECT g.gene_id
		,x.display_label
		,g.description
		,g.stable_id
		,sr.name AS sequence
		,cs.version AS coord_system_version
		,g.canonical_transcript_id
		,t.seq_region_strand
	FROM gene g
	JOIN xref x ON x.xref_id = g.display_xref_id
	JOIN seq_region sr ON sr.seq_region_id = g.seq_region_id
	JOIN coord_system cs ON cs.coord_system_id = sr.coord_system_id
	JOIN transcript t ON t.transcript_id = g.canonical_transcript_id
	WHERE g.biotype = 'protein_coding'
		AND x.display_label LIKE '%RPE65%'

SELECT *
	FROM transcript t
	WHERE t.gene_id = 17054

SELECT * FROM gene g WHERE g.stable_id = 'ENSCAFG00000020431' 
-- transcript_id = 20238
-- seq_region_id = 39

SELECT *
	FROM sdsd
	WHERE sf.seq_region_id = 39

SELECT *
	FROM exon e
	WHERE e.stable_id = 'ENSACAT00000011562'

SELECT e.exon_id
		,e.seq_region_start
		,e.seq_region_end
		,e.stable_id
	FROM exon e
	JOIN exon_transcript et ON et.exon_id = e.exon_id
	WHERE et.transcript_id = 20238
	ORDER BY et.rank

SELECT *
	FROM seq_region sr
	WHERE EXISTS (SELECT d.seq_region_id FROM dna d WHERE d.seq_region_id = sr.seq_region_id)
		AND EXISTS (SELECT g.seq_region_id FROM gene g WHERE g.seq_region_id = sr.seq_region_id)
	LIMIT 10

SELECT *
	FROM dna d
	WHERE d.seq_region_id = 6

SELECT *
	FROM seq_region sr
	JOIN coord_system cs ON cs.coord_system_id = sr.coord_system_id
	WHERE sr.seq_region_id = 39