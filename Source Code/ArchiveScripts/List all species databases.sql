USE ensembl_production_75;
-- List all databases
SELECT d.db_id, d.species_id, 
		s.web_name,
		s.scientific_name,
		s.url_name,
		CAST(concat(concat_ws('_',
                         s.db_name,
                         d.db_type,
                         d.db_release,
                         d.db_assembly), d.db_suffix) AS char) AS full_database_name
		 ,s.*
	FROM db d
	JOIN species s ON s.species_id = d.species_id
	WHERE d.is_current = 1
		AND d.db_type = 'core'
	GROUP BY d.db_id, d.species_id