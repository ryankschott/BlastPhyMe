USE vicugna_pacos_core_75_1;

SELECT g.gene_id
		,x.display_label
		,g.description
		,g.stable_id
	FROM gene g
	JOIN xref x ON x.xref_id = g.display_xref_id
	WHERE g.biotype = 'protein_coding'
		AND x.display_label LIKE '%RPE65%'
-- SELECT *
-- 	FROM external_db
-- 	WHERE external_db_id = 1100;