CREATE TABLE "Gallery"
(
    "Id"	        		uuid NOT NULL,
    "ImageId"       		uuid NOT NULL,
	"SmallPreviewFileId"	uuid NOT NULL,
    "ArticleId"     		uuid NULL,
    "Description"   		text NULL,
    "Timestamp"     		timestamp NOT NULL,
    CONSTRAINT "PK_Gallery" PRIMARY KEY ("Id")
)
WITH (
	OIDS=FALSE
);

ALTER TABLE "Gallery" ADD
    CONSTRAINT "FK_Gallery_Images_ImageId" FOREIGN KEY ("ImageId")
        REFERENCES "Images" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
		
ALTER TABLE "Gallery" ADD
	CONSTRAINT "FK_Gallery_Articles_ArticleId" FOREIGN KEY ("ArticleId")
        REFERENCES "Articles" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;

CREATE INDEX "IX_Gallery_ImageId"
    ON "Gallery" USING btree ("ImageId");

CREATE INDEX "IX_Gallery_ArticleId"
    ON "Gallery" USING btree ("ArticleId");