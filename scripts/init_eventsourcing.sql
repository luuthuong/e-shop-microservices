-- Events Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Events' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE [dbo].[Events] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [AggregateId] UNIQUEIDENTIFIER NOT NULL,
    [Type] NVARCHAR(500) NOT NULL,
    [Version] INT NOT NULL,
    [Data] NVARCHAR(MAX) NOT NULL,
    [Timestamp] DATETIME2 NOT NULL,
    CONSTRAINT [IX_Events_AggregateId_Version] UNIQUE ([AggregateId], [Version])
    );
END

-- Index on Events.Timestamp
IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'IX_Events_Timestamp' AND object_id = OBJECT_ID('dbo.Events')
)
BEGIN
CREATE INDEX [IX_Events_Timestamp] ON [dbo].[Events] ([Timestamp]);
END


-- OutboxEvents Table

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OutboxEvents' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE [dbo].[OutboxEvents] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [EventName] NVARCHAR(500) NOT NULL,
    [JsonPayload] NVARCHAR(MAX) NOT NULL
);
END

-- configure CDC
EXEC sys.sp_cdc_enable_db;

IF NOT EXISTS (SELECT 1 FROM cdc.change_tables WHERE source_object_id = OBJECT_ID('dbo.OutboxEvents'))
BEGIN
EXEC sys.sp_cdc_enable_table @source_schema = N'dbo',  
                             @source_name = N'OutboxEvents',  
                             @role_name = NULL
END