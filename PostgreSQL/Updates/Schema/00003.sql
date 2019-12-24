DO $$ 
DECLARE
   category_id uuid := md5(random()::text || clock_timestamp()::text)::uuid;
   new_category_id uuid := md5(random()::text || clock_timestamp()::text)::uuid;
   
BEGIN 
   	INSERT INTO "Categories" 
   		("Id", "Name")
		VALUES
		(category_id, 'Австралия');
	
	INSERT INTO "Categories" 
   		("Id", "Name", "ParentId")
		VALUES
		(new_category_id, 'Garie Beach', category_id);
		
	category_id = md5(random()::text || clock_timestamp()::text)::uuid;
	
	INSERT INTO "Categories" 
   		("Id", "Name")
		VALUES
		(category_id, 'Подводная охота');
END $$;