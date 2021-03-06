// <auto-generated />
using System;
using API_Proyecto2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API_Proyecto2.Migrations
{
    [DbContext(typeof(DbContextChild))]
    partial class DbContextChildModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Proyecto2_RubenLaraMarin.Models.Client", b =>
                {
                    b.Property<string>("ClientId")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Proyecto2_RubenLaraMarin.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Bathrooms")
                        .HasColumnType("int");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(12)");

                    b.Property<int>("ConstructionSize")
                        .HasColumnType("int");

                    b.Property<int>("FloorType")
                        .HasColumnType("int");

                    b.Property<int>("HalfBathrooms")
                        .HasColumnType("int");

                    b.Property<bool>("HaveSinkOutdoors")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLivingRoomKitchen")
                        .HasColumnType("bit");

                    b.Property<int>("KitchenFurnitureType")
                        .HasColumnType("int");

                    b.Property<int>("PriceBySquaredMeter")
                        .HasColumnType("int");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("Rooms")
                        .HasColumnType("int");

                    b.Property<int>("Terrace")
                        .HasColumnType("int");

                    b.HasKey("ProjectId");

                    b.HasIndex("ClientId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Proyecto2_RubenLaraMarin.Models.ProjectImage", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<byte[]>("ImageData")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("ProjectId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Proyecto2_RubenLaraMarin.Models.Project", b =>
                {
                    b.HasOne("Proyecto2_RubenLaraMarin.Models.Client", null)
                        .WithMany("Projects")
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("Proyecto2_RubenLaraMarin.Models.ProjectImage", b =>
                {
                    b.HasOne("Proyecto2_RubenLaraMarin.Models.Project", "Project")
                        .WithOne("ProjectImage")
                        .HasForeignKey("Proyecto2_RubenLaraMarin.Models.ProjectImage", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Proyecto2_RubenLaraMarin.Models.Client", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("Proyecto2_RubenLaraMarin.Models.Project", b =>
                {
                    b.Navigation("ProjectImage");
                });
#pragma warning restore 612, 618
        }
    }
}
