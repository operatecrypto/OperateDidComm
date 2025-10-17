using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperateCrypto.DIDComm.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToDIDCommPrefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageThreads",
                schema: "dbo",
                table: "MessageThreads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                schema: "dbo",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DIDCommKeys",
                schema: "dbo",
                table: "DIDCommKeys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connections",
                schema: "dbo",
                table: "Connections");

            migrationBuilder.RenameTable(
                name: "MessageThreads",
                schema: "dbo",
                newName: "DIDComm_MessageThreads",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Messages",
                schema: "dbo",
                newName: "DIDComm_Messages",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "DIDCommKeys",
                schema: "dbo",
                newName: "DIDComm_Keys",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Connections",
                schema: "dbo",
                newName: "DIDComm_Connections",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_ThreadId",
                schema: "dbo",
                table: "DIDComm_MessageThreads",
                newName: "IX_DIDComm_Threads_ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_Parent",
                schema: "dbo",
                table: "DIDComm_MessageThreads",
                newName: "IX_DIDComm_Threads_Parent");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ThreadId",
                schema: "dbo",
                table: "DIDComm_Messages",
                newName: "IX_DIDComm_Messages_ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_Status",
                schema: "dbo",
                table: "DIDComm_Messages",
                newName: "IX_DIDComm_Messages_Status");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_MessageId",
                schema: "dbo",
                table: "DIDComm_Messages",
                newName: "IX_DIDComm_Messages_MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_FromTo",
                schema: "dbo",
                table: "DIDComm_Messages",
                newName: "IX_DIDComm_Messages_FromTo");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_CreatedAt",
                schema: "dbo",
                table: "DIDComm_Messages",
                newName: "IX_DIDComm_Messages_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Keys_Did",
                schema: "dbo",
                table: "DIDComm_Keys",
                newName: "IX_DIDComm_Keys_Did");

            migrationBuilder.RenameIndex(
                name: "IX_Keys_Active",
                schema: "dbo",
                table: "DIDComm_Keys",
                newName: "IX_DIDComm_Keys_Active");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_DIDs",
                schema: "dbo",
                table: "DIDComm_Connections",
                newName: "IX_DIDComm_Connections_DIDs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DIDComm_MessageThreads",
                schema: "dbo",
                table: "DIDComm_MessageThreads",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DIDComm_Messages",
                schema: "dbo",
                table: "DIDComm_Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DIDComm_Keys",
                schema: "dbo",
                table: "DIDComm_Keys",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DIDComm_Connections",
                schema: "dbo",
                table: "DIDComm_Connections",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DIDComm_MessageThreads",
                schema: "dbo",
                table: "DIDComm_MessageThreads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DIDComm_Messages",
                schema: "dbo",
                table: "DIDComm_Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DIDComm_Keys",
                schema: "dbo",
                table: "DIDComm_Keys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DIDComm_Connections",
                schema: "dbo",
                table: "DIDComm_Connections");

            migrationBuilder.RenameTable(
                name: "DIDComm_MessageThreads",
                schema: "dbo",
                newName: "MessageThreads",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "DIDComm_Messages",
                schema: "dbo",
                newName: "Messages",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "DIDComm_Keys",
                schema: "dbo",
                newName: "DIDCommKeys",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "DIDComm_Connections",
                schema: "dbo",
                newName: "Connections",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Threads_ThreadId",
                schema: "dbo",
                table: "MessageThreads",
                newName: "IX_Threads_ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Threads_Parent",
                schema: "dbo",
                table: "MessageThreads",
                newName: "IX_Threads_Parent");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Messages_ThreadId",
                schema: "dbo",
                table: "Messages",
                newName: "IX_Messages_ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Messages_Status",
                schema: "dbo",
                table: "Messages",
                newName: "IX_Messages_Status");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Messages_MessageId",
                schema: "dbo",
                table: "Messages",
                newName: "IX_Messages_MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Messages_FromTo",
                schema: "dbo",
                table: "Messages",
                newName: "IX_Messages_FromTo");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Messages_CreatedAt",
                schema: "dbo",
                table: "Messages",
                newName: "IX_Messages_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Keys_Did",
                schema: "dbo",
                table: "DIDCommKeys",
                newName: "IX_Keys_Did");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Keys_Active",
                schema: "dbo",
                table: "DIDCommKeys",
                newName: "IX_Keys_Active");

            migrationBuilder.RenameIndex(
                name: "IX_DIDComm_Connections_DIDs",
                schema: "dbo",
                table: "Connections",
                newName: "IX_Connections_DIDs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageThreads",
                schema: "dbo",
                table: "MessageThreads",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                schema: "dbo",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DIDCommKeys",
                schema: "dbo",
                table: "DIDCommKeys",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connections",
                schema: "dbo",
                table: "Connections",
                column: "Id");
        }
    }
}
