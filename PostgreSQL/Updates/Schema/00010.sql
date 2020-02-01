DO $$ 
DECLARE
   article_id uuid;
   role_id uuid;
   
BEGIN 
	SELECT "Id" INTO role_id FROM "AspNetRoles" WHERE "Name" = 'PrivateReader';

	SELECT "Id" INTO article_id FROM "Articles" WHERE "Title" = '13-17 января 2019: "Зимний" отпуск. Часть 1: поездка в Луганск';
   	
	INSERT INTO "ArticleRoles" 
   		("ArticleId", "RoleId")
		VALUES
		(article_id, role_id);

	SELECT "Id" INTO article_id FROM "Articles" WHERE "Title" = '18-25 января 2019: "Зимний" отпуск. Часть 2: Луганск';
   	
	INSERT INTO "ArticleRoles" 
   		("ArticleId", "RoleId")
		VALUES
		(article_id, role_id);

	SELECT "Id" INTO article_id FROM "Articles" WHERE "Title" = '26 января - 2 февраля 2019: "Зимний" отпуск. Часть 3: Красная Поляна';
   	
	INSERT INTO "ArticleRoles" 
   		("ArticleId", "RoleId")
		VALUES
		(article_id, role_id);
END $$;