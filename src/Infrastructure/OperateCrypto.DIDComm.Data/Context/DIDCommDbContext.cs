using Microsoft.EntityFrameworkCore;
using OperateCrypto.DIDComm.Data.Entities;

namespace OperateCrypto.DIDComm.Data.Context;

/// <summary>
/// Entity Framework DbContext for DIDComm infrastructure
/// </summary>
public class DIDCommDbContext : DbContext
{
    public DIDCommDbContext(DbContextOptions<DIDCommDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<MessageRecord> Messages { get; set; }
    public DbSet<Connection> Connections { get; set; }
    public DbSet<DIDCommKey> DIDCommKeys { get; set; }
    public DbSet<MessageThread> MessageThreads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure MessageRecord entity
        modelBuilder.Entity<MessageRecord>(entity =>
        {
            entity.ToTable("Messages", "dbo");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MessageId)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.MessageId)
                .IsUnique()
                .HasDatabaseName("IX_Messages_MessageId");

            entity.Property(e => e.From)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.To)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasIndex(e => new { e.From, e.To })
                .HasDatabaseName("IX_Messages_FromTo");

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.ThreadId)
                .HasMaxLength(255);

            entity.HasIndex(e => e.ThreadId)
                .HasDatabaseName("IX_Messages_ThreadId");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.Status)
                .HasDatabaseName("IX_Messages_Status");

            entity.HasIndex(e => new { e.ReceivedAt, e.SentAt })
                .HasDatabaseName("IX_Messages_CreatedAt")
                .IsDescending();

            // Standard fields
            entity.Property(e => e.Deleted).HasDefaultValue(false);
            entity.Property(e => e.Archived).HasDefaultValue(false);
            entity.Property(e => e.LastModifiedOn).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.ResID).HasDefaultValueSql("NEWID()");
        });

        // Configure Connection entity
        modelBuilder.Entity<Connection>(entity =>
        {
            entity.ToTable("Connections", "dbo");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MyDid)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.TheirDid)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasIndex(e => new { e.MyDid, e.TheirDid })
                .IsUnique()
                .HasDatabaseName("IX_Connections_DIDs");

            entity.Property(e => e.TheirLabel).HasMaxLength(255);
            entity.Property(e => e.MyLabel).HasMaxLength(255);

            entity.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Role).HasMaxLength(50);

            // Standard fields
            entity.Property(e => e.Deleted).HasDefaultValue(false);
            entity.Property(e => e.Archived).HasDefaultValue(false);
            entity.Property(e => e.LastModifiedOn).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.ResID).HasDefaultValueSql("NEWID()");
        });

        // Configure DIDCommKey entity
        modelBuilder.Entity<DIDCommKey>(entity =>
        {
            entity.ToTable("DIDCommKeys", "dbo");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Did)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasIndex(e => e.Did)
                .HasDatabaseName("IX_Keys_Did");

            entity.Property(e => e.KeyId)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.KeyType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Purpose).HasMaxLength(50);

            entity.HasIndex(e => new { e.Did, e.IsActive })
                .HasDatabaseName("IX_Keys_Active");

            // Standard fields
            entity.Property(e => e.Deleted).HasDefaultValue(false);
            entity.Property(e => e.Archived).HasDefaultValue(false);
            entity.Property(e => e.LastModifiedOn).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.ResID).HasDefaultValueSql("NEWID()");
        });

        // Configure MessageThread entity
        modelBuilder.Entity<MessageThread>(entity =>
        {
            entity.ToTable("MessageThreads", "dbo");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ThreadId)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.ThreadId)
                .IsUnique()
                .HasDatabaseName("IX_Threads_ThreadId");

            entity.Property(e => e.ParentThreadId).HasMaxLength(255);

            entity.HasIndex(e => e.ParentThreadId)
                .HasDatabaseName("IX_Threads_Parent");

            entity.Property(e => e.Subject).HasMaxLength(500);

            // Standard fields
            entity.Property(e => e.Deleted).HasDefaultValue(false);
            entity.Property(e => e.Archived).HasDefaultValue(false);
            entity.Property(e => e.LastModifiedOn).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.ResID).HasDefaultValueSql("NEWID()");
        });
    }
}
