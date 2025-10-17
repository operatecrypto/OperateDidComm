-- ============================================================================
-- DIDComm Infrastructure - Seed Data (Optional)
-- Company: OperateCrypto.com
-- Project: OperateDIDComm
-- Created: 2025-10-16
-- ============================================================================

USE [OperateCryptoDIDComm]
GO

-- ============================================================================
-- Sample DID for testing (based on sunstream.operatedid.com from documentation)
-- ============================================================================

PRINT 'Seeding sample data for development/testing...';
GO

-- Sample DID Keys for did:web:sunstream.operatedid.com
IF NOT EXISTS (SELECT 1 FROM [dbo].[DIDCommKeys] WHERE [Did] = 'did:web:sunstream.operatedid.com')
BEGIN
    INSERT INTO [dbo].[DIDCommKeys] ([Did], [KeyId], [KeyType], [PublicKey], [Purpose], [IsActive])
    VALUES
    (
        'did:web:sunstream.operatedid.com',
        'did:web:sunstream.operatedid.com#key-1',
        'Secp256k1',
        'MDg1MGM2RkM0QTU5YWM2ODlGOTUxNWMzQ2ZkNEQ4ZDI=', -- Sample public key from docs
        'authentication',
        1
    );

    PRINT '✓ Sample DID key created for sunstream.operatedid.com';
END
GO

-- Sample Connection
IF NOT EXISTS (SELECT 1 FROM [dbo].[Connections] WHERE [MyDid] = 'did:web:alice.operatedid.com')
BEGIN
    INSERT INTO [dbo].[Connections] ([MyDid], [TheirDid], [TheirLabel], [MyLabel], [State], [Role])
    VALUES
    (
        'did:web:alice.operatedid.com',
        'did:web:bob.operatedid.com',
        'Bob (Test User)',
        'Alice (Test User)',
        'complete',
        'inviter'
    );

    PRINT '✓ Sample connection created';
END
GO

-- Sample Message Thread
DECLARE @threadId NVARCHAR(255) = 'test-thread-' + CAST(NEWID() AS NVARCHAR(50));

IF NOT EXISTS (SELECT 1 FROM [dbo].[MessageThreads] WHERE [ThreadId] LIKE 'test-thread-%')
BEGIN
    INSERT INTO [dbo].[MessageThreads] ([ThreadId], [Participants], [Subject])
    VALUES
    (
        @threadId,
        '["did:web:alice.operatedid.com", "did:web:bob.operatedid.com"]',
        'Test Conversation'
    );

    PRINT '✓ Sample message thread created with ID: ' + @threadId;
END
GO

-- Sample Message
IF NOT EXISTS (SELECT 1 FROM [dbo].[Messages] WHERE [MessageId] LIKE 'test-msg-%')
BEGIN
    DECLARE @msgId NVARCHAR(255) = 'test-msg-' + CAST(NEWID() AS NVARCHAR(50));
    DECLARE @existingThreadId NVARCHAR(255);

    SELECT TOP 1 @existingThreadId = [ThreadId] FROM [dbo].[MessageThreads] WHERE [ThreadId] LIKE 'test-thread-%';

    INSERT INTO [dbo].[Messages]
    (
        [MessageId],
        [From],
        [To],
        [Type],
        [Body],
        [ThreadId],
        [CreatedTime],
        [Status],
        [Direction]
    )
    VALUES
    (
        @msgId,
        'did:web:alice.operatedid.com',
        'did:web:bob.operatedid.com',
        'https://didcomm.org/basicmessage/2.0/message',
        '{"content": "Hello Bob! This is a test message from the DIDComm infrastructure."}',
        @existingThreadId,
        DATEDIFF(SECOND, '1970-01-01', GETUTCDATE()) * 1000, -- Unix timestamp in milliseconds
        'sent',
        'out'
    );

    PRINT '✓ Sample message created with ID: ' + @msgId;
END
GO

PRINT '========================================';
PRINT '✓ Seed data completed successfully';
PRINT '========================================';
PRINT '';
PRINT 'Sample DIDs created:';
PRINT '  - did:web:sunstream.operatedid.com';
PRINT '  - did:web:alice.operatedid.com';
PRINT '  - did:web:bob.operatedid.com';
PRINT '';
GO
