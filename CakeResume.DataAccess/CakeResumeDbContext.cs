using CakeResume.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.DataAccess
{
	public class CakeResumeDbContext : DbContext
	{
		public CakeResumeDbContext(DbContextOptions<CakeResumeDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<ItemImage> ItemImage { get; set; }
		public DbSet<TodoItem> TodoItems { get; set; }
		public DbSet<SearchResult> SearchResults { get; set; }
		public DbSet<EventLog> EventLogs { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(e => e.Email);

				entity.Property(e => e.Email)
					.IsRequired()
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.Property(e => e.UserName)
					.HasMaxLength(128)
					.IsUnicode(false);
				
				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(128);
			});

			modelBuilder.Entity<Item>(entity =>
			{
				entity.HasKey(e => e.ItemId);

				entity.HasIndex(e => e.Email);

				entity.Property(e => e.ItemId)
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.Property(e => e.ItemJson)
					.IsRequired()
					.HasColumnType("NTEXT");

				entity.Property(e => e.SendEmails)
					.HasMaxLength(512)
					.IsUnicode(false);

				entity.Property(e => e.Email)
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.HasOne(d => d.User)
					.WithMany(p => p.Items)
					.HasForeignKey(d => d.Email);
			});

			modelBuilder.Entity<ItemImage>(entity =>
			{
				entity.HasIndex(e => e.ItemId);

				entity.Property(e => e.ItemImageId)
					.HasValueGenerator<SequentialGuidValueGenerator>();

				entity.Property(e => e.ItemId)
					.HasMaxLength(128)
					.IsUnicode(false);

				entity.Property(e => e.OriginalUrl)
					.IsRequired()
					.HasMaxLength(1024)
					.IsUnicode(false);

				entity.Property(e => e.SaveUrl)
					.IsRequired()
					.HasMaxLength(1024)
					.IsUnicode(false);

				entity.HasOne(d => d.Item)
					.WithMany(p => p.Images)
					.HasForeignKey(d => d.ItemId);
			});

			modelBuilder.Entity<TodoItem>(entity =>
			{
				entity.HasKey(e => e.TodoItemId);

				entity.HasIndex(e => e.ItemId);

				entity.Property(e => e.ItemId)
					.IsRequired()
					.HasMaxLength(128)
					.IsUnicode(false);
			});

			modelBuilder.Entity<SearchResult>(entity =>
			{
				entity.HasKey(e => e.SearchKey);

				entity.Property(e => e.SearchKey)
					.IsRequired()
					.HasMaxLength(1024)
					.IsUnicode(false);
			});

			modelBuilder.Entity<EventLog>(entity =>
			{
				entity.HasKey(e => e.EventLogId);

				entity.Property(e => e.Message)
					.HasMaxLength(2048);

				entity.Property(e => e.Exception)
					.HasColumnType("NTEXT");
			});
		}

	}
}
