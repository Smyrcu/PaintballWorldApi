using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO;
using Org.BouncyCastle.Math.EC.Rfc7748;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Infrastructure;

public partial class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<EmailInbox> EmailInboxes { get; set; }

    public virtual DbSet<EmailOutbox> EmailOutboxes { get; set; }

    public virtual DbSet<EntityType> EntityTypes { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Field> Fields { get; set; }

    public virtual DbSet<FieldRating> FieldRatings { get; set; }

    public virtual DbSet<FieldSchedule> FieldSchedules { get; set; }

    public virtual DbSet<FieldType> FieldTypes { get; set; }

    public virtual DbSet<Newsletter> Newsletters { get; set; }

    public virtual DbSet<NewsletterSub> NewsletterSubs { get; set; }

    public virtual DbSet<OsmCity> OsmCities { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    public virtual DbSet<UserRating> UserRatings { get; set; }

    public virtual DbSet<UsersToEvent> UsersToEvents { get; set; }

    public virtual DbSet<ApiKey> ApiKeys { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

#if DEBUG
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PaintballWorldApp2;Integrated Security=true;",
            provider =>
            {
                provider.UseNetTopologySuite();
            });
#else
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     => optionsBuilder.UseSqlServer(
         //"Server=127.0.0.1,9210;User Id=sa;Password=JakiesLosoweHaslo123;Database=PaintballWorldApp2;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=false",
                     "Server=192.168.1.191,1433;User Id=sa;Password=JakiesLosoweHaslo123;Database=PaintballWorldApp2;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=false",
         providerOptions =>
         {
             providerOptions.EnableRetryOnFailure(
                 maxRetryCount: 5,
                 maxRetryDelay: TimeSpan.FromSeconds(30),
                 errorNumbersToAdd: null);
             providerOptions.UseNetTopologySuite();
         }
         );
#endif

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Address__3214EC0730CE6642");

            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new AddressId(value))
                .ValueGeneratedOnAdd();

            entity.ToTable("Address");

            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.HouseNo).HasMaxLength(30);
            entity.Property(e => e.PhoneNo).HasMaxLength(50);
            entity.Property(e => e.PostalNumber).HasMaxLength(50);
            entity.Property(e => e.Street).HasMaxLength(255);
            entity.Property(e => e.Location).HasColumnType("geography");
        });

        modelBuilder.Entity<ApiKey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ApiKey__20240206CE");

            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new ApiKeyId(value))
                .ValueGeneratedOnAdd();

            entity.ToTable("ApiKey");

            entity.Property(e => e.Key).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();

        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attachme__3214EC077EC533FA");

            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new AttachmentId(value)).ValueGeneratedOnAdd();

            entity.Property(e => e.Path).HasMaxLength(255);

            entity.Property(e => e.EmailOutboxId).IsRequired(false);
            entity.Property(e => e.EmailInboxId).IsRequired(false);
            entity.ToTable("Attachments");

            entity.HasCheckConstraint("CK_Attachment_EmailInboxId_EmailOutboxId", "([EmailInboxId] IS NOT NULL AND [EmailOutboxId] IS NULL) OR ([EmailInboxId] IS NULL AND [EmailOutboxId] IS NOT NULL)");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contact__3214EC0B1234DA2");
            entity.Property(e => e.Id).HasConversion(id => id.Value, value => new ContactId(value))
                .ValueGeneratedOnAdd();
            entity.ToTable("Contact");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Topic).HasMaxLength(255);
            entity.Property(e => e.Content).HasMaxLength(4000);

        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company__3214EC07B2730D43");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new CompanyId(value)).ValueGeneratedOnAdd();

            entity.ToTable("Company");

            entity.Property(e => e.CompanyName).HasMaxLength(400);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.TaxId).HasMaxLength(255);

            entity.HasOne(e => e.Address)
                .WithOne()
                .HasForeignKey<Company>(e => e.AddressId)
                .IsRequired();
        });

        modelBuilder.Entity<EmailInbox>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmailInb__3214EC07CB025E82");

            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new EmailInboxId(value)).ValueGeneratedOnAdd();
            entity.ToTable("EmailInbox");

            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.MessageId).HasMaxLength(255);
            entity.Property(e => e.ReceivedTime).HasColumnType("datetime");
            entity.Property(e => e.Sender).HasMaxLength(255);
            entity.Property(e => e.Subject).HasMaxLength(255);
            entity.HasMany(e => e.Attachments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EmailOutbox>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EmailOut__3214EC070557AC54");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new EmailOutboxId(value)).ValueGeneratedOnAdd();
            entity.ToTable("EmailOutbox");

            entity.Property(e => e.IsSent).HasDefaultValue(false);
            entity.Property(e => e.MessageId).HasMaxLength(255);
            entity.Property(e => e.Recipient).HasMaxLength(255);
            entity.Property(e => e.SentTime).HasColumnType("datetime");
            entity.Property(e => e.Subject).HasMaxLength(255);
            entity.HasMany(e => e.Attachments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EntityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EntityTy__3214EC071CCCEF86");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new EntityTypeId(value)).ValueGeneratedOnAdd();
            entity.ToTable("EntityType");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events__3214EC0735E05FB8");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new EventId(value))
                .ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedOnUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.LastUpdatedUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
           entity.Property(x => x.Name).HasMaxLength(500);

           entity.Property(e => e.FieldScheduleId).IsRequired(false);
             /*  .HasConversion(
                   id => id.Value,
                   value => new FieldScheduleId(value.Value))
               .IsRequired(false);*/

            entity.HasOne<IdentityUser>(e => e.CreatedByUser)
                .WithMany() 
                .HasForeignKey(e => e.CreatedBy)
                .IsRequired(true) 
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.FieldSchedule)
                .WithOne(f => f.Event)
                .IsRequired(false)
                .HasForeignKey<FieldSchedule>(e => e.EventId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.Photos)
                .WithOne() 
                .HasForeignKey(p => p.EventId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Field)
                .WithMany(f => f.Events)
                .HasForeignKey(e => e.FieldId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Field__3214EC07274DB93D");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new FieldId(value)).ValueGeneratedOnAdd();
            entity.ToTable("Field");

            entity.Property(e => e.CreatedOnUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.LastUpdatedUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Regulations).HasMaxLength(255);

            entity.HasOne(f => f.FieldType)
                .WithMany(f => f.Fields) 
                .HasForeignKey(f => f.FieldTypeId);

            entity.HasOne(f => f.Address)
                .WithOne() 
                .HasForeignKey<Field>(f => f.AddressId);

            entity.HasOne(f => f.Owner)
                .WithMany(f => f.Fields)
                .HasForeignKey(f => f.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(f => f.Sets)
                .WithOne(f => f.Field)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(f => f.Photos)
                .WithOne() 
                .HasForeignKey(p => p.FieldId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.Cascade); 
            
            entity.HasMany(f => f.Events)
                .WithOne(e => e.Field) 
                .HasForeignKey(f => f.FieldId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(f => f.MainPhotoId).IsRequired(false);

            entity.HasOne(x => x.MainPhoto)
                .WithOne()
                .HasForeignKey<Field>(f => f.MainPhotoId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });

        modelBuilder.Entity<FieldRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FieldRat__3214EC079B566052");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new FieldRatingId(value)).ValueGeneratedOnAdd();
            entity.ToTable("FieldRating");

            entity.Property(e => e.Content).HasMaxLength(400);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedOnUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(fr => fr.Field)
                .WithMany(f => f.FieldRatings)
                .HasForeignKey(fr => fr.FieldId);
        });

        modelBuilder.Entity<FieldSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FieldSch__3214EC07A2B31AFC");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new FieldScheduleId(value)).ValueGeneratedOnAdd();
            entity.ToTable("FieldSchedule");

            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.EventId)
                .HasConversion(
                    id => id.Value,
                    value => new EventId(value))
                .HasColumnType("uniqueidentifier");

            entity.Property(x => x.MaxPlayers);
            entity.Property(x => x.MaxPlaytime);

            entity.HasOne(fs => fs.Field) 
                .WithMany(f => f.FieldSchedules)
                .HasForeignKey(fs => fs.FieldId);

            entity.HasOne(e => e.Event)
                .WithOne(f => f.FieldSchedule)
                .HasForeignKey<Event>(e => e.FieldScheduleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

        });

        modelBuilder.Entity<FieldType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FieldTyp__3214EC0794D0A617");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new FieldTypeId(value)).ValueGeneratedOnAdd();
            entity.ToTable("FieldTypeName");

            entity.Property(e => e.CreatedOnUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FieldTypeName)
                .HasMaxLength(50)
                .HasColumnName("FieldTypeName");


            entity.HasMany(ft => ft.Fields) 
                .WithOne(f => f.FieldType) 
                .HasForeignKey(f => f.FieldTypeId);
        });

        modelBuilder.Entity<Newsletter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Newslett__3214EC071D8386E7");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new NewsletterId(value)).ValueGeneratedOnAdd();
            entity.ToTable("Newsletter");

            entity.Property(e => e.Name).HasMaxLength(400);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<NewsletterSub>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Newslett__3214EC07D5F303C4");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new NewsletterSubId(value)).ValueGeneratedOnAdd();
            entity.Property(e => e.Email).HasMaxLength(255);
        });

        modelBuilder.Entity<OsmCity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OsmCitie__3214EC07F146088D");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new OsmCityId(value)).ValueGeneratedOnAdd();
            entity.Property(e => e.County).HasMaxLength(255);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 7)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 7)");
            entity.Property(e => e.Municipality).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Province).HasMaxLength(255);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Owners__3214EC07F0523BFF");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new OwnerId(value)).ValueGeneratedOnAdd();
            entity.Property(e => e.CompanyId);
            entity.Property(e => e.CreatedOnUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photo__3214EC078A632B4C");
            entity.Property(x => x.Id)
                .HasConversion(id => id.Value, value => new PhotoId(value))
                .ValueGeneratedOnAdd();
            entity.ToTable("Photo");

            entity.Property(e => e.CreatedOnUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Path).HasMaxLength(255);

            entity.Property(p => p.FieldId).IsRequired(false);
            entity.Property(p => p.EventId).IsRequired(false);
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sets__3214EC07C4C3B70D");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new SetId(value)).ValueGeneratedOnAdd();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.HasOne(e => e.Field)
                .WithMany(e => e.Sets)
                .HasForeignKey(e => e.FieldId);
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity
                .HasKey(e => e.UserId).HasName("PK__UserInfo__JFREW67FI4F3E4RW");
            
            entity.ToTable("UserInfo");

                entity.HasOne(ui => ui.User)
                    .WithOne() 
                    .HasForeignKey<UserInfo>(ui => ui.UserId);

            entity.Property(e => e.DateOfBirth).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.PhoneNo).HasMaxLength(50);

            entity.HasOne(e => e.ProfileImage)
                .WithOne()
                .HasForeignKey<UserInfo>(e => e.ProfileImageId);


        });

        modelBuilder.Entity<UserRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRati__3214EC071D3FF75F");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new UserRatingId(value)).ValueGeneratedOnAdd();
            entity.ToTable("UserRating");

            entity.Property(e => e.Content).HasMaxLength(400);
            entity.Property(e => e.CreatedOnUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.HasOne(e => e.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(e => e.UserId);

            entity.HasOne(e => e.Creator)
                .WithMany() 
                .HasForeignKey(e => e.CreatorId)
                .IsRequired(false);
        });

        modelBuilder.Entity<UsersToEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersToE__3214EC07A6DB8A30");
            entity.Property(x => x.Id).HasConversion(id => id.Value, value => new UsersToEventId(value)).ValueGeneratedOnAdd();
            entity.Property(e => e.JoinedOnUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.HasOne(ute => ute.Event)
                .WithMany(e => e.UsersToEvents)
                .HasForeignKey(ute => ute.EventId)
                .IsRequired();

            entity.HasOne(ute => ute.set)
                    .WithMany(s => s.UsersToEvents) 
                    .HasForeignKey(ute => ute.SetId)
                    .IsRequired(false);

            entity.HasOne(ute => ute.User)
                .WithMany()
                .HasForeignKey(ute => ute.UserId)
                .IsRequired(); 
        });

        OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
