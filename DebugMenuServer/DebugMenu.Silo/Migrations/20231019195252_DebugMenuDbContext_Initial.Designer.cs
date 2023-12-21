﻿// <auto-generated />
using System;
using DebugMenu.Silo.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DebugMenu.Silo.Migrations
{
    [DbContext(typeof(DebugMenuDbContext))]
    [Migration("20231019195252_DebugMenuDbContext_Initial")]
    partial class DebugMenuDbContext_Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DebugMenu.Silo.Persistence.AuthJs.AccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessToken")
                        .HasColumnType("text")
                        .HasColumnName("access_token");

                    b.Property<long?>("ExpiresAt")
                        .HasColumnType("bigint")
                        .HasColumnName("expires_at");

                    b.Property<string>("IdToken")
                        .HasColumnType("text")
                        .HasColumnName("id_token");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("provider");

                    b.Property<string>("ProviderAccountId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("providerAccountId");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.Property<string>("Scope")
                        .HasColumnType("text")
                        .HasColumnName("scope");

                    b.Property<string>("SessionState")
                        .HasColumnType("text")
                        .HasColumnName("session_state");

                    b.Property<string>("TokenType")
                        .HasColumnType("text")
                        .HasColumnName("token_type");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("type");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id")
                        .HasName("accounts_pkey");

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("DebugMenu.Silo.Persistence.AuthJs.SessionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires");

                    b.Property<string>("SessionToken")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("sessionToken");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id")
                        .HasName("sessions_pkey");

                    b.ToTable("sessions", (string)null);
                });

            modelBuilder.Entity("DebugMenu.Silo.Persistence.AuthJs.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<DateTime?>("EmailVerified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("emailVerified");

                    b.Property<string>("Image")
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("users_pkey");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("DebugMenu.Silo.Persistence.AuthJs.VerificationTokenEntity", b =>
                {
                    b.Property<string>("Identifier")
                        .HasColumnType("text")
                        .HasColumnName("identifier");

                    b.Property<string>("Token")
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires");

                    b.HasKey("Identifier", "Token")
                        .HasName("verification_token_pkey");

                    b.ToTable("verification_token", (string)null);
                });

            modelBuilder.Entity("DebugMenu.Silo.Web.Applications.Persistence.EntityFramework.ApplicationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("applications", (string)null);
                });

            modelBuilder.Entity("DebugMenu.Silo.Web.Applications.Persistence.EntityFramework.ApplicationUserEntity", b =>
                {
                    b.Property<int?>("ApplicationEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("UsersId")
                        .HasColumnType("integer");

                    b.Property<int>("ApplicationsId")
                        .HasColumnType("integer");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("ApplicationEntityId", "UsersId");

                    b.HasIndex("ApplicationsId");

                    b.HasIndex("UsersId");

                    b.ToTable("ApplicationUserEntity");
                });

            modelBuilder.Entity("DebugMenu.Silo.Web.Applications.Persistence.EntityFramework.ApplicationUserEntity", b =>
                {
                    b.HasOne("DebugMenu.Silo.Web.Applications.Persistence.EntityFramework.ApplicationEntity", null)
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("ApplicationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DebugMenu.Silo.Persistence.AuthJs.UserEntity", null)
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DebugMenu.Silo.Persistence.AuthJs.UserEntity", b =>
                {
                    b.Navigation("ApplicationUsers");
                });

            modelBuilder.Entity("DebugMenu.Silo.Web.Applications.Persistence.EntityFramework.ApplicationEntity", b =>
                {
                    b.Navigation("ApplicationUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
