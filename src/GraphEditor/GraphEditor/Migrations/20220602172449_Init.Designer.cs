﻿// <auto-generated />
using GraphEditor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GraphEditor.Migrations
{
    [DbContext(typeof(GraphDBContext))]
    [Migration("20220602172449_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GraphEditor.Models.GraphRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GraphRecords");
                });

            modelBuilder.Entity("GraphEditor.Models.GraphRecord", b =>
                {
                    b.OwnsOne("GraphEditor.DataTypes.GraphData", "Data", b1 =>
                        {
                            b1.Property<string>("GraphRecordId")
                                .HasColumnType("text");

                            b1.HasKey("GraphRecordId");

                            b1.ToTable("GraphRecords");

                            b1.WithOwner()
                                .HasForeignKey("GraphRecordId");

                            b1.OwnsMany("GraphEditor.DataTypes.GraphLink", "Links", b2 =>
                                {
                                    b2.Property<string>("GraphDataGraphRecordId")
                                        .HasColumnType("text");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b2.Property<int>("Id"));

                                    b2.Property<string>("Source")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Target")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("GraphDataGraphRecordId", "Id");

                                    b2.ToTable("GraphLink");

                                    b2.WithOwner()
                                        .HasForeignKey("GraphDataGraphRecordId");
                                });

                            b1.OwnsMany("GraphEditor.DataTypes.GraphNode", "Nodes", b2 =>
                                {
                                    b2.Property<string>("GraphDataGraphRecordId")
                                        .HasColumnType("text");

                                    b2.Property<string>("Id")
                                        .HasColumnType("text");

                                    b2.Property<float>("X")
                                        .HasColumnType("real");

                                    b2.Property<float>("Y")
                                        .HasColumnType("real");

                                    b2.HasKey("GraphDataGraphRecordId", "Id");

                                    b2.ToTable("GraphNode");

                                    b2.WithOwner()
                                        .HasForeignKey("GraphDataGraphRecordId");
                                });

                            b1.Navigation("Links");

                            b1.Navigation("Nodes");
                        });

                    b.Navigation("Data")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
