-- Create OperateCryptoDIDComm database with files in tracked directory
-- This ensures all database changes are tracked in git

USE master;
GO

-- Drop database if it exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'OperateCryptoDIDComm')
BEGIN
    ALTER DATABASE OperateCryptoDIDComm SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE OperateCryptoDIDComm;
END
GO

-- Create database with explicit file paths
CREATE DATABASE OperateCryptoDIDComm
ON PRIMARY
(
    NAME = N'OperateCryptoDIDComm',
    FILENAME = N'C:\OperateCrypto\GitHub\OperateDIDComm\database\data\OperateCryptoDIDComm.mdf',
    SIZE = 8MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 64MB
)
LOG ON
(
    NAME = N'OperateCryptoDIDComm_log',
    FILENAME = N'C:\OperateCrypto\GitHub\OperateDIDComm\database\data\OperateCryptoDIDComm_log.ldf',
    SIZE = 8MB,
    MAXSIZE = 2GB,
    FILEGROWTH = 64MB
);
GO

USE OperateCryptoDIDComm;
GO

PRINT 'Database created successfully with files in C:\OperateCrypto\GitHub\OperateDIDComm\database\data\';
GO
