-- create empty database Blog first

CREATE TABLE "Categories"
(
	"Id"		uuid,
  	"Name"		text,
	"ParentId"	uuid,
	CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "Categories" ADD 
	CONSTRAINT "FK_Categories_Categories_ParentId" FOREIGN KEY ("ParentId")
        REFERENCES "Categories" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;

CREATE INDEX "IX_Categories_ParentId"
    ON "Categories" USING btree ("ParentId");

CREATE INDEX "IX_Categories_Name"
    ON "Categories"("Name");

CREATE TABLE "Articles"
(
	"Id"		uuid NOT NULL,
	"Title"		text,
	"Body"		text,
	"Timestamp"	timestamp,
	CONSTRAINT "PK_Articles" PRIMARY KEY ("Id")
)
WITH (
	OIDS=FALSE
);

CREATE INDEX "IX_Articles_Title"
    ON "Articles"("Title");

CREATE TABLE "ArticleCategories"
(
    "ArticleId"		uuid NOT NULL,
    "CategoryId" 	uuid NOT NULL,
    CONSTRAINT "PK_ArticleCategories" PRIMARY KEY ("ArticleId", "CategoryId")
)
WITH (
    OIDS = FALSE
);

ALTER TABLE "ArticleCategories" ADD
    CONSTRAINT "FK_ArticleCategories_Articles_ArticleId" FOREIGN KEY ("ArticleId")
        REFERENCES "Articles" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
		
ALTER TABLE "ArticleCategories" ADD
	CONSTRAINT "FK_ArticleCategories_Categories_CategoryId" FOREIGN KEY ("CategoryId")
        REFERENCES "Categories" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;

CREATE TABLE "Files"
(
    "Id" 		uuid NOT NULL,
    "Name" 		text NOT NULL,
	"Extension"	text,
    "Data" 		bytea NOT NULL,
    CONSTRAINT "PK_File" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
);

CREATE TABLE "Images"
(
    "Id"				uuid NOT NULL,
    "PreviewFileId"		uuid NULL,
    "OriginalFileId"	uuid NULL,
    CONSTRAINT "PK_Images" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
);

ALTER TABLE "Images" ADD
    CONSTRAINT "FK_Images_Files_PreviewFileId" FOREIGN KEY ("PreviewFileId")
        REFERENCES "Files" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
		
ALTER TABLE "Images" ADD
	CONSTRAINT "FK_Images_Files_OriginalFileId" FOREIGN KEY ("OriginalFileId")
        REFERENCES "Files" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;

CREATE INDEX "IX_Images_OriginalFileId"
    ON "Images" USING btree ("OriginalFileId");

CREATE INDEX "IX_Images_PreviewFileId"
    ON "Images" USING btree ("PreviewFileId");