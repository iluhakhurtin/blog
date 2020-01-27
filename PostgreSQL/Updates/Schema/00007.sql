DO $$ 
DECLARE
   article_id uuid;
   role_id uuid;
   
BEGIN 
	SELECT "Id" INTO article_id FROM "Articles" WHERE "Title" = 'Покатушки на лыжах в южном полушарии и национальный парк Костюшко';
	SELECT "Id" INTO role_id FROM "AspNetRoles" WHERE "Name" = 'PrivateReader';
   	
	INSERT INTO "ArticleRoles" 
   		("ArticleId", "RoleId")
		VALUES
		(article_id, role_id);
END $$;