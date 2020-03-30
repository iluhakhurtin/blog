CREATE TABLE "GalleryPhotos"
(
    "Id"	        		uuid NOT NULL,
    "ImageId"       		uuid NOT NULL,
	"SmallPreviewFileId"	uuid NOT NULL,
    "ArticleId"     		uuid NULL,
    "Description"   		uuid NULL,
    "Timestamp"     		timestamp NOT NULL,
    CONSTRAINT "PK_GalleryPhotos" PRIMARY KEY ("Id")
)
WITH (
	OIDS=FALSE
);

ALTER TABLE "GalleryPhotos" ADD
    CONSTRAINT "FK_GalleryPhotos_Images_ImageId" FOREIGN KEY ("ImageId")
        REFERENCES "Images" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
		
ALTER TABLE "GalleryPhotos" ADD
	CONSTRAINT "FK_GalleryPhotos_Articles_ArticleId" FOREIGN KEY ("ArticleId")
        REFERENCES "Articles" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;

CREATE INDEX "IX_GalleryPhotos_ImageId"
    ON "GalleryPhotos" USING btree ("ImageId");

CREATE INDEX "IX_GalleryPhotos_ArticleId"
    ON "GalleryPhotos" USING btree ("ArticleId");