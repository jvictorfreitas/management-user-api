START TRANSACTION;

CREATE TABLE "OutboxMessages" (
    "Id" uuid NOT NULL,
    "Type" text NOT NULL,
    "Payload" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ProcessedAt" timestamp with time zone,
    CONSTRAINT "PK_OutboxMessages" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260309035212_AddOutboxTable', '9.0.0');

COMMIT;

