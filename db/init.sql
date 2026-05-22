CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260521231405_InitialCreate') THEN
    CREATE TABLE "Projects" (
        "Id" uuid NOT NULL,
        "Name" character varying(200) NOT NULL,
        "Description" character varying(1000) NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL DEFAULT (NOW()),
        CONSTRAINT "PK_Projects" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260521231405_InitialCreate') THEN
    CREATE TABLE "Activities" (
        "Id" uuid NOT NULL,
        "ProjectId" uuid NOT NULL,
        "Name" character varying(200) NOT NULL,
        "BudgetedCost" numeric(18,2) NOT NULL,
        "PlannedPercentage" numeric(5,2) NOT NULL,
        "ActualPercentage" numeric(5,2) NOT NULL,
        "ActualCost" numeric(18,2) NOT NULL,
        CONSTRAINT "PK_Activities" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Activities_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260521231405_InitialCreate') THEN
    CREATE INDEX "IX_Activities_ProjectId" ON "Activities" ("ProjectId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260521231405_InitialCreate') THEN
    CREATE INDEX "IX_Projects_CreatedAt" ON "Projects" ("CreatedAt" DESC);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260521231405_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260521231405_InitialCreate', '8.0.11');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260522004315_AddRowVersionToEntities') THEN
    ALTER TABLE "Projects" ADD "RowVersion" bytea NOT NULL DEFAULT BYTEA E'\\x';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260522004315_AddRowVersionToEntities') THEN
    ALTER TABLE "Activities" ADD "RowVersion" bytea NOT NULL DEFAULT BYTEA E'\\x';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260522004315_AddRowVersionToEntities') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260522004315_AddRowVersionToEntities', '8.0.11');
    END IF;
END $EF$;
COMMIT;

