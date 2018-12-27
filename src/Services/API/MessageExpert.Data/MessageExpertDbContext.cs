using MessageExpert.Data.Models.Accounts;
using MessageExpert.Data.Models.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageExpert.Data
{

	public class MessageExpertDbContext : DbContext
	{
		public Guid ID
		{
			get
			{
				return Guid.NewGuid();
			}
		}

		public MessageExpertDbContext(DbContextOptions<MessageExpertDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(en => en.Id); 
				entity.Property(en => en.UserName).HasMaxLength(50).IsRequired();
				entity.Property(en => en.Password).IsRequired();
				entity.HasIndex(en => en.UserName).IsUnique();
			});

			modelBuilder.Entity<BlockedUser>(entity =>
			{
				entity.HasKey(en => en.Id);
				entity.HasOne(en => en.User).WithMany(en => en.BlockedUsers).IsRequired();
			});


			modelBuilder.Entity<UserActivityLog>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.HasOne(x => x.User).WithMany(x => x.Logs).IsRequired();
				entity.Property(x => x.LoginIsSuccess).IsRequired();
				entity.Property(x => x.LogDateTime).IsRequired();
			});

			modelBuilder.Entity<Message>(entity =>
			{
				entity.HasKey(en => en.Id);
				entity.Property(en => en.Content).HasMaxLength(200).IsRequired();
				entity.Property(en => en.SendDateTime).IsRequired();
				entity.HasOne(en => en.From).WithMany(en => en.OutgoingMessages).IsRequired().OnDelete(DeleteBehavior.Restrict);
				entity.HasOne(en => en.To).WithMany(en => en.IncomingMessages).IsRequired().OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
