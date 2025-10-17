# 📘 Database Table Creation Guidelines

These guidelines define **standards for creating new tables** in our databases to ensure **consistency, maintainability, and data integrity** across all schemas.

---

## 1. General Conventions
- **Schema**: Use `[dbo]` unless a business-specific schema is required.  
- **Table Names**:
  - Use **PascalCase** (e.g., `BC_Blockchains`, `BC_BlockchainTypes`).  
  - Prefix tables with a **module or domain abbreviation** (`BC_` = Blockchain module).  
  - Use **plural form** for entity tables (`BC_Blockchains` not `BC_Blockchain`).  

---

## 2. Primary Keys
- Every table **must have a surrogate primary key**:
  - Name format: `<EntityName>ID` (e.g., `BlockchainID`, `BlockchainTypeID`).  
  - Type: `INT IDENTITY(1,1)` unless a natural key is strictly required.  
- Define a **clustered primary key** on the ID field.  

---

## 3. Standard System Fields
All tables must include the following fields (unless explicitly justified otherwise):

| Column Name       | Type            | Default / Rule                                   | Purpose                        |
|-------------------|----------------|-------------------------------------------------|--------------------------------|
| `Deleted`         | `BIT`          | `DEFAULT (0)`                                    | Soft delete flag               |
| `Archived`        | `BIT`          | `DEFAULT (0)`                                    | Archive indicator              |
| `LastModifiedOn`  | `DATETIME`     | `DEFAULT (GETDATE())`                            | Last update timestamp          |
| `LastModifiedBy`  | `INT`          | NULL allowed                                     | User/account who updated       |
| `CreatedOn`       | `DATETIME`     | `DEFAULT (GETDATE())`                            | Creation timestamp             |
| `CreatedBy`       | `INT`          | NULL allowed                                     | User/account who created       |
| `SourceAppID`     | `INT`          | NULL allowed                                     | Source application reference   |
| `ClientAccountID` | `INT`          | NULL allowed                                     | Multi-tenant segregation       |
| `AppDomainID`     | `INT`          | NULL allowed                                     | Domain partitioning            |
| `DataDomainID`    | `INT`          | NULL allowed                                     | Data categorization            |
| `DataSegmentID`   | `INT`          | NULL allowed                                     | Data segmentation              |
| `ResID`           | `UNIQUEIDENTIFIER` | `DEFAULT (NEWID())`                         | Globally unique record ID      |

---

## 4. Column Naming & Data Types
- **Naming**: Use `PascalCase`, no spaces, no abbreviations unless common (`URL`, `ID`).  
- **Types**:
  - Use `NVARCHAR(length)` for strings (default length **200** unless justified otherwise).  
  - Use `BIT` for booleans (`IsPreferredNetwork`, `Deleted`, etc.).  
  - Use `DATETIME` for temporal fields (`CreatedOn`, `LastModifiedOn`).  
  - Use `UNIQUEIDENTIFIER` only for globally unique references.  
- **Nullable Fields**:
  - Mark columns `NOT NULL` if values are always required.  
  - Otherwise explicitly define as `NULL`.  

---

## 5. Constraints & Defaults
- **Primary Key Constraint**:  
  ```sql
  CONSTRAINT [PK_<Module>_<Entity>] PRIMARY KEY CLUSTERED ([<EntityID>] ASC)
  ```
- **Default Constraints**:  
  - Use naming format: `DF_<Module>_<Entity>_<Column>`  
  - Example: `DF_BC_Blockchains_IsPreferredNetwork`  
- Always define default values for:
  - `Deleted` → `0`  
  - `Archived` → `0`  
  - `CreatedOn` → `GETDATE()`  
  - `LastModifiedOn` → `GETDATE()`  
  - `ResID` → `NEWID()`  

---

## 6. Indexing
- By default, rely on the **clustered PK index**.  
- Add **non-clustered indexes** only when:
  - Frequently queried on non-PK fields.  
  - Required for foreign keys or filtering.  
- Index naming convention:  
  ```
  IX_<TableName>_<ColumnName>
  ```

---

## 7. Foreign Keys
- Foreign key columns must follow the `<ReferencedEntity>ID` naming convention.  
- Define foreign keys explicitly with constraints.  
- Use **cascading deletes** only when business rules allow.  

---

## 8. Documentation
- Every new table must include:  
  - Purpose of the table (one-line description).  
  - Description of non-standard fields.  
  - Relationships to other tables.  

---

✅ **Example: New Table Structure**
```sql
CREATE TABLE [dbo].[BC_NewEntity](
    [NewEntityID] [int] IDENTITY(1,1) NOT NULL,
    [BlockchainID] [int] NOT NULL, -- FK reference
    
    -- Standard fields
    [Deleted] [bit] CONSTRAINT [DF_BC_NewEntity_Deleted] DEFAULT (0) NOT NULL,
    [Archived] [bit] CONSTRAINT [DF_BC_NewEntity_Archived] DEFAULT (0) NOT NULL,
    [LastModifiedOn] [datetime] CONSTRAINT [DF_BC_NewEntity_LastModifiedOn] DEFAULT (GETDATE()) NOT NULL,
    [LastModifiedBy] [int] NULL,
    [CreatedOn] [datetime] CONSTRAINT [DF_BC_NewEntity_CreatedOn] DEFAULT (GETDATE()) NOT NULL,
    [CreatedBy] [int] NULL,
    [SourceAppID] [int] NULL,
    [ClientAccountID] [int] NULL,
    [AppDomainID] [int] NULL,
    [DataDomainID] [int] NULL,
    [DataSegmentID] [int] NULL,
    [ResID] [uniqueidentifier] CONSTRAINT [DF_BC_NewEntity_ResID] DEFAULT (NEWID()) NOT NULL,

    CONSTRAINT [PK_BC_NewEntity] PRIMARY KEY CLUSTERED ([NewEntityID] ASC),
    CONSTRAINT [FK_BC_NewEntity_BlockchainID] FOREIGN KEY ([BlockchainID]) REFERENCES [dbo].[BC_Blockchains]([BlockchainID])
);
```
