// <auto-generated />
using System;
using Hackathon_TEK;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Hackathon_TEK.Migrations
{
    [DbContext(typeof(HackathonContext))]
    [Migration("20210522205229_analyzes")]
    partial class analyzes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Hackathon_TEK.Models.Analyze", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EventType")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<string>("ObjectType")
                        .HasColumnType("text");

                    b.Property<double>("Probability")
                        .HasColumnType("double precision");

                    b.Property<int>("RegionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Analyzes");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Earthquake", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Elevation")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<double>("Lat")
                        .HasColumnType("double precision");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<double>("Lon")
                        .HasColumnType("double precision");

                    b.Property<double>("Magnitude")
                        .HasColumnType("double precision");

                    b.Property<int?>("ReasonId")
                        .HasColumnType("integer");

                    b.Property<int>("RegionId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime>("Update")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ReasonId");

                    b.HasIndex("RegionId");

                    b.ToTable("Earthquakes");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Fire", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("BrightT31")
                        .HasColumnType("double precision");

                    b.Property<double>("Brightness")
                        .HasColumnType("double precision");

                    b.Property<int>("Confidence")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("District")
                        .HasColumnType("text");

                    b.Property<string>("Federal")
                        .HasColumnType("text");

                    b.Property<double>("Frp")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<int?>("ReasonId")
                        .HasColumnType("integer");

                    b.Property<int>("RegionId")
                        .HasColumnType("integer");

                    b.Property<string>("Satellite")
                        .HasColumnType("text");

                    b.Property<double>("Scan")
                        .HasColumnType("double precision");

                    b.Property<string>("Subject")
                        .HasColumnType("text");

                    b.Property<double>("Track")
                        .HasColumnType("double precision");

                    b.Property<string>("Version")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ReasonId");

                    b.HasIndex("RegionId");

                    b.ToTable("Fires");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Reason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("EventType")
                        .HasColumnType("text");

                    b.Property<string>("IndustryType")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsWeather")
                        .HasColumnType("boolean");

                    b.Property<string>("ReasonDescription")
                        .HasColumnType("text");

                    b.Property<int>("RegionId")
                        .HasColumnType("integer");

                    b.Property<string>("TypeObject")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Reasons");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Coordinates")
                        .HasColumnType("text");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<string>("MapId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("RssUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Weather", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double?>("CloudsMax")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double?>("HumidityMax")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("boolean");

                    b.Property<double?>("Percipitation")
                        .HasColumnType("double precision");

                    b.Property<double?>("PressureMax")
                        .HasColumnType("double precision");

                    b.Property<int>("RegionId")
                        .HasColumnType("integer");

                    b.Property<int>("StationId")
                        .HasColumnType("integer");

                    b.Property<double>("StationLat")
                        .HasColumnType("double precision");

                    b.Property<double>("StationLon")
                        .HasColumnType("double precision");

                    b.Property<string>("StationName")
                        .HasColumnType("text");

                    b.Property<string>("StationRegion")
                        .HasColumnType("text");

                    b.Property<double?>("TempAverage")
                        .HasColumnType("double precision");

                    b.Property<double?>("TempAverage0")
                        .HasColumnType("double precision");

                    b.Property<double?>("TempDifNorm0")
                        .HasColumnType("double precision");

                    b.Property<double?>("TempMax0")
                        .HasColumnType("double precision");

                    b.Property<double?>("TempMin0")
                        .HasColumnType("double precision");

                    b.Property<double?>("WindDegMax")
                        .HasColumnType("double precision");

                    b.Property<double?>("WindSpeedMax")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Weather");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Analyze", b =>
                {
                    b.HasOne("Hackathon_TEK.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Earthquake", b =>
                {
                    b.HasOne("Hackathon_TEK.Models.Reason", "Reason")
                        .WithMany()
                        .HasForeignKey("ReasonId");

                    b.HasOne("Hackathon_TEK.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reason");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Fire", b =>
                {
                    b.HasOne("Hackathon_TEK.Models.Reason", "Reason")
                        .WithMany()
                        .HasForeignKey("ReasonId");

                    b.HasOne("Hackathon_TEK.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reason");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Reason", b =>
                {
                    b.HasOne("Hackathon_TEK.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Hackathon_TEK.Models.Weather", b =>
                {
                    b.HasOne("Hackathon_TEK.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });
#pragma warning restore 612, 618
        }
    }
}
