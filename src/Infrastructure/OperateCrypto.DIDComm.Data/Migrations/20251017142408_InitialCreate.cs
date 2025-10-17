using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperateCrypto.DIDComm.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Connections",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MyDid = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TheirDid = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TheirLabel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MyLabel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConnectionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    SourceAppID = table.Column<int>(type: "int", nullable: true),
                    ClientAccountID = table.Column<int>(type: "int", nullable: true),
                    AppDomainID = table.Column<int>(type: "int", nullable: true),
                    DataDomainID = table.Column<int>(type: "int", nullable: true),
                    DataSegmentID = table.Column<int>(type: "int", nullable: true),
                    ResID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DIDCommKeys",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Did = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    KeyId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    KeyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    SourceAppID = table.Column<int>(type: "int", nullable: true),
                    ClientAccountID = table.Column<int>(type: "int", nullable: true),
                    AppDomainID = table.Column<int>(type: "int", nullable: true),
                    DataDomainID = table.Column<int>(type: "int", nullable: true),
                    DataSegmentID = table.Column<int>(type: "int", nullable: true),
                    ResID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DIDCommKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    From = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    To = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThreadId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ParentThreadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<long>(type: "bigint", nullable: false),
                    ExpiresTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    SourceAppID = table.Column<int>(type: "int", nullable: true),
                    ClientAccountID = table.Column<int>(type: "int", nullable: true),
                    AppDomainID = table.Column<int>(type: "int", nullable: true),
                    DataDomainID = table.Column<int>(type: "int", nullable: true),
                    DataSegmentID = table.Column<int>(type: "int", nullable: true),
                    ResID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageThreads",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThreadId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ParentThreadId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Participants = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    SourceAppID = table.Column<int>(type: "int", nullable: true),
                    ClientAccountID = table.Column<int>(type: "int", nullable: true),
                    AppDomainID = table.Column<int>(type: "int", nullable: true),
                    DataDomainID = table.Column<int>(type: "int", nullable: true),
                    DataSegmentID = table.Column<int>(type: "int", nullable: true),
                    ResID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageThreads", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_DIDs",
                schema: "dbo",
                table: "Connections",
                columns: new[] { "MyDid", "TheirDid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Keys_Active",
                schema: "dbo",
                table: "DIDCommKeys",
                columns: new[] { "Did", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Keys_Did",
                schema: "dbo",
                table: "DIDCommKeys",
                column: "Did");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CreatedAt",
                schema: "dbo",
                table: "Messages",
                columns: new[] { "ReceivedAt", "SentAt" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromTo",
                schema: "dbo",
                table: "Messages",
                columns: new[] { "From", "To" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageId",
                schema: "dbo",
                table: "Messages",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Status",
                schema: "dbo",
                table: "Messages",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ThreadId",
                schema: "dbo",
                table: "Messages",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_Parent",
                schema: "dbo",
                table: "MessageThreads",
                column: "ParentThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_ThreadId",
                schema: "dbo",
                table: "MessageThreads",
                column: "ThreadId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DIDCommKeys",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MessageThreads",
                schema: "dbo");
        }
    }
}
