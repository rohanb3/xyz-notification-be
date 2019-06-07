﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xyzies.Notification.Data;

namespace Xyzies.Notification.Data.Migrations
{
    [DbContext(typeof(NotificationContext))]
    partial class NotificationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Xyzies.Notification.Data.Entity.MessageTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cause")
                        .IsRequired();

                    b.Property<DateTime>("CreateOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("MessageBody");

                    b.Property<string>("Subject");

                    b.Property<Guid>("TypeOfMessageId");

                    b.HasKey("Id");

                    b.HasIndex("TypeOfMessageId");

                    b.ToTable("EmailTemplates");
                });

            modelBuilder.Entity("Xyzies.Notification.Data.Entity.TypeOfMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TypeOfMessages");
                });

            modelBuilder.Entity("Xyzies.Notification.Data.Entity.MessageTemplate", b =>
                {
                    b.HasOne("Xyzies.Notification.Data.Entity.TypeOfMessage", "TypeOfMessages")
                        .WithMany()
                        .HasForeignKey("TypeOfMessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}