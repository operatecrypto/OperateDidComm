# DIDComm Database Scripts

This directory contains SQL Server database scripts for the OperateDIDComm infrastructure.

## Prerequisites

- SQL Server 2019 or later
- Database: `OperateCryptoDIDComm`

## Script Execution Order

Execute the scripts in the following order:

1. **001_InitialSchema.sql** - Creates the core database tables
   - Messages
   - Connections
   - DIDCommKeys
   - MessageThreads

2. **002_AddIndexes.sql** - Creates indexes for optimal query performance
   - Indexes on Messages table for FromTo, ThreadId, Status, and Timestamps
   - Indexes on Connections table for DIDs and State
   - Indexes on DIDCommKeys table for Did, Active keys, and KeyId
   - Indexes on MessageThreads table for ThreadId and Parent

3. **003_SeedData.sql** (Optional) - Seeds sample data for development/testing
   - Sample DID keys for sunstream.operatedid.com
   - Sample connection between Alice and Bob
   - Sample message thread and message

## Manual Execution

```sql
-- 1. Create the database
CREATE DATABASE [OperateCryptoDIDComm];
GO

-- 2. Run the scripts in order
:r 001_InitialSchema.sql
:r 002_AddIndexes.sql
:r 003_SeedData.sql  -- Optional for dev/test
```

## Entity Framework Core Migrations

Alternatively, you can use EF Core migrations from the Data project:

```bash
# Create initial migration
dotnet ef migrations add InitialCreate --project src/Infrastructure/OperateCrypto.DIDComm.Data --startup-project src/API/OperateCrypto.DIDComm.Api

# Update database
dotnet ef database update --project src/Infrastructure/OperateCrypto.DIDComm.Data --startup-project src/API/OperateCrypto.DIDComm.Api
```

## Connection String

Add this to your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OperateCryptoDIDComm;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

## Table Guidelines

All tables follow the OperateCrypto database guidelines:
- PascalCase naming convention
- GUID primary keys
- Standard system fields (Deleted, Archived, LastModifiedOn, etc.)
- Proper indexing strategy
- Soft delete support

## Security Notes

- Private keys in DIDCommKeys table are encrypted with a master key
- Ensure proper encryption at rest (TDE) for production
- Implement column-level encryption for sensitive data
- Use SSL/TLS for database connections

## Maintenance

### Cleanup Deleted Records
```sql
-- Permanently delete soft-deleted records older than 30 days
DELETE FROM Messages WHERE Deleted = 1 AND LastModifiedOn < DATEADD(day, -30, GETUTCDATE());
DELETE FROM Connections WHERE Deleted = 1 AND LastModifiedOn < DATEADD(day, -30, GETUTCDATE());
DELETE FROM DIDCommKeys WHERE Deleted = 1 AND LastModifiedOn < DATEADD(day, -30, GETUTCDATE());
DELETE FROM MessageThreads WHERE Deleted = 1 AND LastModifiedOn < DATEADD(day, -30, GETUTCDATE());
```

### Archive Old Messages
```sql
-- Archive messages older than 90 days
UPDATE Messages
SET Archived = 1
WHERE CreatedOn < DATEADD(day, -90, GETUTCDATE()) AND Archived = 0;
```
