-- ============================================================================
-- DIDComm Infrastructure - Database Indexes
-- Company: OperateCrypto.com
-- Project: OperateDIDComm
-- Created: 2025-10-16
-- ============================================================================

USE [OperateCryptoDIDComm]
GO

-- ============================================================================
-- Messages Table Indexes
-- ============================================================================

-- Index on MessageId (unique)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Messages_MessageId' AND object_id = OBJECT_ID('[dbo].[Messages]'))
BEGIN
    CREATE UNIQUE INDEX [IX_Messages_MessageId] ON [dbo].[Messages]([MessageId])
    WHERE [Deleted] = 0;
    PRINT '✓ Index [IX_Messages_MessageId] created';
END
GO

-- Index on From and To for querying conversations
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Messages_FromTo' AND object_id = OBJECT_ID('[dbo].[Messages]'))
BEGIN
    CREATE INDEX [IX_Messages_FromTo] ON [dbo].[Messages]([From], [To])
    INCLUDE ([Status], [Direction])
    WHERE [Deleted] = 0;
    PRINT '✓ Index [IX_Messages_FromTo] created';
END
GO

-- Index on ThreadId for conversation threading
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Messages_ThreadId' AND object_id = OBJECT_ID('[dbo].[Messages]'))
BEGIN
    CREATE INDEX [IX_Messages_ThreadId] ON [dbo].[Messages]([ThreadId])
    WHERE [Deleted] = 0 AND [ThreadId] IS NOT NULL;
    PRINT '✓ Index [IX_Messages_ThreadId] created';
END
GO

-- Index on Status for filtering
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Messages_Status' AND object_id = OBJECT_ID('[dbo].[Messages]'))
BEGIN
    CREATE INDEX [IX_Messages_Status] ON [dbo].[Messages]([Status])
    WHERE [Deleted] = 0;
    PRINT '✓ Index [IX_Messages_Status] created';
END
GO

-- Index on timestamps for sorting
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Messages_Timestamps' AND object_id = OBJECT_ID('[dbo].[Messages]'))
BEGIN
    CREATE INDEX [IX_Messages_Timestamps] ON [dbo].[Messages]([ReceivedAt] DESC, [SentAt] DESC)
    INCLUDE ([From], [To], [Type])
    WHERE [Deleted] = 0;
    PRINT '✓ Index [IX_Messages_Timestamps] created';
END
GO

-- ============================================================================
-- Connections Table Indexes
-- ============================================================================

-- Unique index on MyDid and TheirDid
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Connections_DIDs' AND object_id = OBJECT_ID('[dbo].[Connections]'))
BEGIN
    CREATE UNIQUE INDEX [IX_Connections_DIDs] ON [dbo].[Connections]([MyDid], [TheirDid])
    WHERE [Deleted] = 0;
    PRINT '✓ Index [IX_Connections_DIDs] created';
END
GO

-- Index on State for filtering
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Connections_State' AND object_id = OBJECT_ID('[dbo].[Connections]'))
BEGIN
    CREATE INDEX [IX_Connections_State] ON [dbo].[Connections]([State])
    WHERE [Deleted] = 0;
    PRINT '✓ Index [IX_Connections_State] created';
END
GO

-- ============================================================================
-- DIDCommKeys Table Indexes
-- ============================================================================

-- Index on Did
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Keys_Did' AND object_id = OBJECT_ID('[dbo].[DIDCommKeys]'))
BEGIN
    CREATE INDEX [IX_Keys_Did] ON [dbo].[DIDCommKeys]([Did])
    WHERE [Deleted] = 0;
    PRINT '✓ Index [IX_Keys_Did] created';
END
GO

-- Index on Did and IsActive
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Keys_Active' AND object_id = OBJECT_ID('[dbo].[DIDCommKeys]'))
BEGIN
    CREATE INDEX [IX_Keys_Active] ON [dbo].[DIDCommKeys]([Did], [IsActive])
    INCLUDE ([KeyId], [KeyType], [Purpose])
    WHERE [Deleted] = 0 AND [IsActive] = 1;
    PRINT '✓ Index [IX_Keys_Active] created';
END
GO

-- Index on KeyId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Keys_KeyId' AND object_id = OBJECT_ID('[dbo].[DIDCommKeys]'))
BEGIN
    CREATE INDEX [IX_Keys_KeyId] ON [dbo].[DIDCommKeys]([KeyId])
    WHERE [Deleted] = 0;
    PRINT '✓ Index [IX_Keys_KeyId] created';
END
GO

-- ============================================================================
-- MessageThreads Table Indexes
-- ============================================================================

-- Unique index on ThreadId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Threads_ThreadId' AND object_id = OBJECT_ID('[dbo].[MessageThreads]'))
BEGIN
    CREATE UNIQUE INDEX [IX_Threads_ThreadId] ON [dbo].[MessageThreads]([ThreadId])
    WHERE [Deleted] = 0;
    PRINT '✓ Index [IX_Threads_ThreadId] created';
END
GO

-- Index on ParentThreadId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Threads_Parent' AND object_id = OBJECT_ID('[dbo].[MessageThreads]'))
BEGIN
    CREATE INDEX [IX_Threads_Parent] ON [dbo].[MessageThreads]([ParentThreadId])
    WHERE [Deleted] = 0 AND [ParentThreadId] IS NOT NULL;
    PRINT '✓ Index [IX_Threads_Parent] created';
END
GO

PRINT '✓ All indexes created successfully';
GO
