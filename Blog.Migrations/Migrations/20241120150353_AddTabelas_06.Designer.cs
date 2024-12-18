﻿// <auto-generated />
using System;
using Blog.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Blog.Migrations.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20241120150353_AddTabelas_06")]
    partial class AddTabelas_06
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Blog.Domain.Entities.Postagem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Autor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_alteracao");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DataExclusao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DataRegistro")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_registro")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Postagens");
                });

            modelBuilder.Entity("Blog.Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_alteracao");

                    b.Property<DateTime?>("DataRegistro")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_registro")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });
#pragma warning restore 612, 618
        }
    }
}
