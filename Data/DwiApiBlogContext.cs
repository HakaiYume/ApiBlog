using System;
using System.Collections.Generic;
using ApiBlog.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBlog.Data;

public partial class DwiApiBlogContext : DbContext
{
    public DwiApiBlogContext()
    {
    }

    public DwiApiBlogContext(DbContextOptions<DwiApiBlogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__comments__3213E83F8DF669C9");

            entity.ToTable("comments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author).HasColumnName("author");
            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Post).HasColumnName("post");

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("FK__comments__author__3F466844");

            entity.HasOne(d => d.PostNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Post)
                .HasConstraintName("FK__comments__create__3E52440B");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__posts__3213E83FA7AEA3B3");

            entity.ToTable("posts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author).HasColumnName("author");
            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("FK__posts__author__3B75D760");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F71B019A8");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E616445DBC0DB").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC572C28B7C36").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__user_pro__B9BE370F7C7D8377");

            entity.ToTable("user_profiles");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Bio)
                .IsUnicode(false)
                .HasColumnName("bio");
            entity.Property(e => e.ProfileImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("profile_image_url");

            entity.HasOne(d => d.User).WithOne(p => p.UserProfile)
                .HasForeignKey<UserProfile>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_prof__user___4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
