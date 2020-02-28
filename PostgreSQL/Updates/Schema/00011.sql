ALTER TABLE "Files" ADD "Timestamp" timestamp NULL;
UPDATE "Files" SET "Timestamp" = now();
ALTER TABLE "Files" ALTER COLUMN "Timestamp" SET NOT NULL;