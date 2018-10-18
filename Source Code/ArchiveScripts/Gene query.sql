USE ailuropoda_melanoleuca_core_75_1;

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

SELECT g.*
	FROM gene g
	WHERE g.stable_id = 'ENSMUSG00000028174'

SELECT atr.code, atr.name, atr.description, ga.value
	FROM gene g
	JOIN gene_attrib ga ON ga.gene_id = g.gene_id
	JOIN attrib_type atr ON atr.attrib_type_id = ga.attrib_type_id
	WHERE g.stable_id = 'ENSAMXG00000000001'

SELECT *
	FROM transcript t
	WHERE t.gene_id = 216702

SELECT *
	FROM xref x
	JOIN external_db ex_db ON ex_db.external_db_id = x.external_db_id
	WHERE x.xref_id = 109879

SELECT g.*, x.*
	FROM gene g
	JOIN xref x ON x.xref_id = g.display_xref_id
	WHERE g.biotype = 'protein_coding'
		AND x.display_label LIKE 'RPE65'

SELECT DISTINCT biotype
	FROM gene

SELECT *
	FROM gene g
	WHERE g.display_xref_id = 25682

