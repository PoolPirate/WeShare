﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WeShare.Infrastructure.Persistence;

#nullable disable

namespace WeShare.Infrastructure.Migrations
{
    [DbContext(typeof(ShareDbContext))]
    [Migration("20220501203221_PostSendFailureAuditable")]
    partial class PostSendFailureAuditable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WeShare.Domain.Entities.Callback", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("SuccessfullySentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("OwnerId");

                    b.HasIndex("Secret")
                        .IsUnique();

                    b.HasIndex("Type", "OwnerId")
                        .IsUnique();

                    b.ToTable("Callbacks", (string)null);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Like", b =>
                {
                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<long>("ShareId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("OwnerId", "ShareId");

                    b.HasIndex("ShareId");

                    b.ToTable("Likes", (string)null);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("HeadersSize")
                        .HasColumnType("bigint");

                    b.Property<long>("PayloadSize")
                        .HasColumnType("bigint");

                    b.Property<long>("ShareId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ShareId");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.PostSendFailure", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint");

                    b.Property<long>("SubscriptionId")
                        .HasColumnType("bigint");

                    b.Property<int>("SubscriptionType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PostSendFailures", (string)null);

                    b.HasDiscriminator<int>("SubscriptionType").HasValue(0);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.SentPost", b =>
                {
                    b.Property<long>("PostId")
                        .HasColumnType("bigint");

                    b.Property<long>("SubscriptionId")
                        .HasColumnType("bigint");

                    b.Property<short>("Attempts")
                        .HasColumnType("smallint");

                    b.Property<bool>("Received")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("ReceivedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("PostId", "SubscriptionId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("SentPosts", (string)null);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Share", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("HeaderProcessingType")
                        .HasColumnType("integer");

                    b.Property<int>("LikeCount")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<int>("PayloadProcessingType")
                        .HasColumnType("integer");

                    b.Property<string>("Readme")
                        .IsRequired()
                        .HasMaxLength(4096)
                        .HasColumnType("character varying(4096)");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<int>("SubscriberCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Shares", (string)null);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Subscription", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("ShareId")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ShareId");

                    b.HasIndex("UserId");

                    b.ToTable("Subscriptions", (string)null);

                    b.HasDiscriminator<int>("Type").IsComplete(true).HasValue(0);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("boolean");

                    b.Property<bool>("LikesPublished")
                        .HasColumnType("boolean");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.WebhookPostSendFailure", b =>
                {
                    b.HasBaseType("WeShare.Domain.Entities.PostSendFailure");

                    b.Property<int>("ResponseLatency")
                        .HasColumnType("integer");

                    b.Property<int?>("StatusCode")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue(400);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.WebhookSubscription", b =>
                {
                    b.HasBaseType("WeShare.Domain.Entities.Subscription");

                    b.Property<string>("TargetUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue(400);
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Callback", b =>
                {
                    b.HasOne("WeShare.Domain.Entities.User", "Owner")
                        .WithMany("Callbacks")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Like", b =>
                {
                    b.HasOne("WeShare.Domain.Entities.User", "Owner")
                        .WithMany("Likes")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeShare.Domain.Entities.Share", "Share")
                        .WithMany("Likes")
                        .HasForeignKey("ShareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Share");
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Post", b =>
                {
                    b.HasOne("WeShare.Domain.Entities.Share", "Share")
                        .WithMany("Posts")
                        .HasForeignKey("ShareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Share");
                });

            modelBuilder.Entity("WeShare.Domain.Entities.SentPost", b =>
                {
                    b.HasOne("WeShare.Domain.Entities.Post", "Post")
                        .WithMany("SentPosts")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeShare.Domain.Entities.Subscription", "Subscription")
                        .WithMany("SentPosts")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Share", b =>
                {
                    b.HasOne("WeShare.Domain.Entities.User", "Owner")
                        .WithMany("Shares")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Subscription", b =>
                {
                    b.HasOne("WeShare.Domain.Entities.Share", "Share")
                        .WithMany()
                        .HasForeignKey("ShareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeShare.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Share");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Post", b =>
                {
                    b.Navigation("SentPosts");
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Share", b =>
                {
                    b.Navigation("Likes");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("WeShare.Domain.Entities.Subscription", b =>
                {
                    b.Navigation("SentPosts");
                });

            modelBuilder.Entity("WeShare.Domain.Entities.User", b =>
                {
                    b.Navigation("Callbacks");

                    b.Navigation("Likes");

                    b.Navigation("Shares");
                });
#pragma warning restore 612, 618
        }
    }
}
