BEGIN TRANSACTION;

ALTER TABLE "DbSettings"
	ADD "SchemaVersion" integer NULL;
	
ALTER TABLE "DbSettings"
	ADD "DataVersion" integer NULL;
	
UPDATE "DbSettings" SET
	"SchemaVersion"="dbs"."DbVersion",
	"DataVersion"=0
	FROM (SELECT "DbVersion" FROM "DbSettings") AS "dbs";
	
ALTER TABLE "DbSettings"
	DROP "DbVersion";
	
--ROLLBACK;
COMMIT;