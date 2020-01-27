CREATE TABLE "ArticleRoles"
(
	"ArticleId"		uuid,
  	"RoleId"		text
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "ArticleRoles" ADD 
	CONSTRAINT "FK_ArticleRoles_Articles_ArticleId" FOREIGN KEY ("ArticleId")
        REFERENCES "Articles" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;

ALTER TABLE "ArticleRoles" ADD 
	CONSTRAINT "FK_ArticleRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId")
        REFERENCES "AspNetRoles" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;

CREATE INDEX "IX_ArticleRoles_ArticleId"
    ON "ArticleRoles" USING btree ("ArticleId");

CREATE INDEX "IX_ArticleRoles_RoleId"
    ON "ArticleRoles" USING btree ("RoleId");