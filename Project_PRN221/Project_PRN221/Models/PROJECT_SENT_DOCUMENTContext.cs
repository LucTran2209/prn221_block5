using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project_PRN221.Models
{
    public partial class PROJECT_SENT_DOCUMENTContext : DbContext
    {
        public PROJECT_SENT_DOCUMENTContext()
        {
        }

        public PROJECT_SENT_DOCUMENTContext(DbContextOptions<PROJECT_SENT_DOCUMENTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agence> Agences { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<SendDocument> SendDocuments { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			if (!optionsBuilder.IsConfigured)
			{
				var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
				optionsBuilder.UseSqlServer(ConnectionString);
			}
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agence>(entity =>
            {
                entity.Property(e => e.AgenceName).HasMaxLength(100);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HumanSign).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Documents_Categories");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Documents_Users");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SendDocument>(entity =>
            {
                entity.HasKey(e => e.SendId);

                entity.ToTable("SendDocument");

                entity.Property(e => e.SendId).ValueGeneratedNever();

                entity.Property(e => e.Message).HasMaxLength(50);

                entity.Property(e => e.SentDate).HasColumnType("date");

                entity.Property(e => e.UserIdReceive).HasColumnName("UserId_Receive");

                entity.Property(e => e.UserIdSend).HasColumnName("UserId_Send");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.SendDocuments)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SendDocument_Documents");

                entity.HasOne(d => d.UserIdReceiveNavigation)
                    .WithMany(p => p.SendDocumentUserIdReceiveNavigations)
                    .HasForeignKey(d => d.UserIdReceive)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SendDocument_Users1");

                entity.HasOne(d => d.UserIdSendNavigation)
                    .WithMany(p => p.SendDocumentUserIdSendNavigations)
                    .HasForeignKey(d => d.UserIdSend)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SendDocument_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Agence)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AgenceId)
                    .HasConstraintName("FK_Users_Agences");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
