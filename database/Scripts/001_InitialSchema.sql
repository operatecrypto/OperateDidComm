-- ============================================================================
-- DIDComm Infrastructure - Initial Database Schema
-- Company: OperateCrypto.com
-- Project: OperateDIDComm
-- Created: 2025-10-16
-- ============================================================================

USE [OperateCryptoDIDComm]
GO

-- ============================================================================
-- 1. Messages Table
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Messages]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Messages](
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [MessageId] NVARCHAR(255) NOT NULL UNIQUE,
        [From] NVARCHAR(500) NOT NULL,
        [To] NVARCHAR(500) NOT NULL,
        [Type] NVARCHAR(500) NOT NULL,
        [Body] NVARCHAR(MAX), -- JSON content
        [Attachments] NVARCHAR(MAX), -- JSON array
        [ThreadId] NVARCHAR(255),
        [ParentThreadId] NVARCHAR(255),
        [CreatedTime] BIGINT,
        [ExpiresTime] DATETIME2,
        [SentAt] DATETIME2,
        [ReceivedAt] DATETIME2,
        [Status] NVARCHAR(50) NOT NULL DEFAULT 'pending', -- sent, received, failed, read
        [Direction] NVARCHAR(10), -- in, out
        [ErrorMessage] NVARCHAR(MAX),

        -- Standard system fields
        [Deleted] BIT CONSTRAINT [DF_Messages_Deleted] DEFAULT (0) NOT NULL,
        [Archived] BIT CONSTRAINT [DF_Messages_Archived] DEFAULT (0) NOT NULL,
        [LastModifiedOn] DATETIME CONSTRAINT [DF_Messages_LastModifiedOn] DEFAULT (GETUTCDATE()) NOT NULL,
        [LastModifiedBy] INT NULL,
        [CreatedOn] DATETIME CONSTRAINT [DF_Messages_CreatedOn] DEFAULT (GETUTCDATE()) NOT NULL,
        [CreatedBy] INT NULL,
        [SourceAppID] INT NULL,
        [ClientAccountID] INT NULL,
        [AppDomainID] INT NULL,
        [DataDomainID] INT NULL,
        [DataSegmentID] INT NULL,
        [ResID] UNIQUEIDENTIFIER CONSTRAINT [DF_Messages_ResID] DEFAULT (NEWID()) NOT NULL
    );

    PRINT 'Table [dbo].[Messages] created successfully';
END
GO

-- ============================================================================
-- 2. Connections Table
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Connections]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Connections](
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [MyDid] NVARCHAR(500) NOT NULL,
        [TheirDid] NVARCHAR(500) NOT NULL,
        [TheirLabel] NVARCHAR(255),
        [MyLabel] NVARCHAR(255),
        [State] NVARCHAR(50) NOT NULL DEFAULT 'invited', -- invited, requested, responded, complete
        [Role] NVARCHAR(50), -- inviter, invitee
        [ConnectionData] NVARCHAR(MAX), -- JSON with additional data
        [CreatedAt] DATETIME2 DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 DEFAULT GETUTCDATE(),

        -- Standard system fields
        [Deleted] BIT CONSTRAINT [DF_Connections_Deleted] DEFAULT (0) NOT NULL,
        [Archived] BIT CONSTRAINT [DF_Connections_Archived] DEFAULT (0) NOT NULL,
        [LastModifiedOn] DATETIME CONSTRAINT [DF_Connections_LastModifiedOn] DEFAULT (GETUTCDATE()) NOT NULL,
        [LastModifiedBy] INT NULL,
        [CreatedOn] DATETIME CONSTRAINT [DF_Connections_CreatedOn] DEFAULT (GETUTCDATE()) NOT NULL,
        [CreatedBy] INT NULL,
        [SourceAppID] INT NULL,
        [ClientAccountID] INT NULL,
        [AppDomainID] INT NULL,
        [DataDomainID] INT NULL,
        [DataSegmentID] INT NULL,
        [ResID] UNIQUEIDENTIFIER CONSTRAINT [DF_Connections_ResID] DEFAULT (NEWID()) NOT NULL
    );

    PRINT 'Table [dbo].[Connections] created successfully';
END
GO

-- ============================================================================
-- 3. DIDCommKeys Table
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DIDCommKeys]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DIDCommKeys](
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [Did] NVARCHAR(500) NOT NULL,
        [KeyId] NVARCHAR(500) NOT NULL,
        [KeyType] NVARCHAR(50) NOT NULL, -- Ed25519, X25519, Secp256k1
        [PublicKey] NVARCHAR(MAX) NOT NULL,
        [PrivateKey] NVARCHAR(MAX), -- Encrypted with master key
        [Purpose] NVARCHAR(50), -- signing, keyAgreement, authentication
        [IsActive] BIT DEFAULT 1,
        [CreatedAt] DATETIME2 DEFAULT GETUTCDATE(),
        [RevokedAt] DATETIME2,

        -- Standard system fields
        [Deleted] BIT CONSTRAINT [DF_DIDCommKeys_Deleted] DEFAULT (0) NOT NULL,
        [Archived] BIT CONSTRAINT [DF_DIDCommKeys_Archived] DEFAULT (0) NOT NULL,
        [LastModifiedOn] DATETIME CONSTRAINT [DF_DIDCommKeys_LastModifiedOn] DEFAULT (GETUTCDATE()) NOT NULL,
        [LastModifiedBy] INT NULL,
        [CreatedOn] DATETIME CONSTRAINT [DF_DIDCommKeys_CreatedOn] DEFAULT (GETUTCDATE()) NOT NULL,
        [CreatedBy] INT NULL,
        [SourceAppID] INT NULL,
        [ClientAccountID] INT NULL,
        [AppDomainID] INT NULL,
        [DataDomainID] INT NULL,
        [DataSegmentID] INT NULL,
        [ResID] UNIQUEIDENTIFIER CONSTRAINT [DF_DIDCommKeys_ResID] DEFAULT (NEWID()) NOT NULL
    );

    PRINT 'Table [dbo].[DIDCommKeys] created successfully';
END
GO

-- ============================================================================
-- 4. MessageThreads Table
-- ============================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MessageThreads]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[MessageThreads](
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [ThreadId] NVARCHAR(255) NOT NULL UNIQUE,
        [ParentThreadId] NVARCHAR(255),
        [Participants] NVARCHAR(MAX), -- JSON array of DIDs
        [Subject] NVARCHAR(500),
        [Context] NVARCHAR(MAX), -- JSON metadata
        [CreatedAt] DATETIME2 DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 DEFAULT GETUTCDATE(),

        -- Standard system fields
        [Deleted] BIT CONSTRAINT [DF_MessageThreads_Deleted] DEFAULT (0) NOT NULL,
        [Archived] BIT CONSTRAINT [DF_MessageThreads_Archived] DEFAULT (0) NOT NULL,
        [LastModifiedOn] DATETIME CONSTRAINT [DF_MessageThreads_LastModifiedOn] DEFAULT (GETUTCDATE()) NOT NULL,
        [LastModifiedBy] INT NULL,
        [CreatedOn] DATETIME CONSTRAINT [DF_MessageThreads_CreatedOn] DEFAULT (GETUTCDATE()) NOT NULL,
        [CreatedBy] INT NULL,
        [SourceAppID] INT NULL,
        [ClientAccountID] INT NULL,
        [AppDomainID] INT NULL,
        [DataDomainID] INT NULL,
        [DataSegmentID] INT NULL,
        [ResID] UNIQUEIDENTIFIER CONSTRAINT [DF_MessageThreads_ResID] DEFAULT (NEWID()) NOT NULL
    );

    PRINT 'Table [dbo].[MessageThreads] created successfully';
END
GO

PRINT '✓ Initial schema created successfully';
GO
