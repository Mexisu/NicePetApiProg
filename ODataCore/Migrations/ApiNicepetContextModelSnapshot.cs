// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nicepet_API.Models;

namespace Nicepet_API.Migrations
{
    [DbContext(typeof(ApiNicepetContext))]
    partial class ApiNicepetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Nicepet_API.Models.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AnimalBreedId")
                        .HasColumnType("int");

                    b.Property<int?>("AnimalProfileId")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FatherId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsHandicapped")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsPedigree")
                        .HasColumnType("bit");

                    b.Property<int?>("MatriculationNumber")
                        .HasColumnType("int");

                    b.Property<int?>("MotherId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PedigreeNumber")
                        .HasColumnType("int");

                    b.Property<bool?>("Sexe")
                        .HasColumnType("bit");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnimalBreedId");

                    b.HasIndex("AnimalProfileId");

                    b.HasIndex("FatherId");

                    b.HasIndex("MotherId");

                    b.HasIndex("UserId");

                    b.ToTable("Animal");
                });

            modelBuilder.Entity("Nicepet_API.Models.AnimalBreed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AnimalHairTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("AnimalSpeciesId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AnimalHairTypeId");

                    b.HasIndex("AnimalSpeciesId");

                    b.ToTable("AnimalBreed");
                });

            modelBuilder.Entity("Nicepet_API.Models.AnimalHairType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AnimalHairType");
                });

            modelBuilder.Entity("Nicepet_API.Models.AnimalProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AnimalProfile");
                });

            modelBuilder.Entity("Nicepet_API.Models.AnimalSpecies", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AnimalSpecies");
                });

            modelBuilder.Entity("Nicepet_API.Models.Announcement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AnimalId")
                        .HasColumnType("int");

                    b.Property<int?>("AnnouncementTypeId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("CreationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Photos")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("AnnouncementTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Announcement");
                });

            modelBuilder.Entity("Nicepet_API.Models.AnnouncementType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AnnouncementType");
                });

            modelBuilder.Entity("Nicepet_API.Models.BreedingProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BreedingName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StarsLevel")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BreedingProfile");
                });

            modelBuilder.Entity("Nicepet_API.Models.BreedingType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AnimalBreedId")
                        .HasColumnType("int");

                    b.Property<int?>("AnimalSpeciesId")
                        .HasColumnType("int");

                    b.Property<int?>("BreedingProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BreedingProfileId");

                    b.ToTable("BreedingType");
                });

            modelBuilder.Entity("Nicepet_API.Models.ChatHistory", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MessageBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("MessageDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("RecipientUserId")
                        .HasColumnType("int");

                    b.Property<int>("SenderUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipientUserId");

                    b.HasIndex("SenderUserId");

                    b.ToTable("ChatHistory");
                });

            modelBuilder.Entity("Nicepet_API.Models.FranceBreeding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BreedingProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BreedingProfileId");

                    b.ToTable("FranceBreeding");
                });

            modelBuilder.Entity("Nicepet_API.Models.HistoricalOwner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AcquisitionTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("AnimalId")
                        .HasColumnType("int");

                    b.Property<string>("OwnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("UserId");

                    b.ToTable("HistoricalOwner");
                });

            modelBuilder.Entity("Nicepet_API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset?>("CreationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsValidEmail")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserTypeId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Nicepet_API.Models.UserAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserAddress");
                });

            modelBuilder.Entity("Nicepet_API.Models.UserProfile", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("About")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gallery")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserProfile");
                });

            modelBuilder.Entity("Nicepet_API.Models.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserType");
                });

            modelBuilder.Entity("Nicepet_API.Models.Animal", b =>
                {
                    b.HasOne("Nicepet_API.Models.AnimalBreed", "AnimalBreed")
                        .WithMany()
                        .HasForeignKey("AnimalBreedId");

                    b.HasOne("Nicepet_API.Models.AnimalProfile", "AnimalProfile")
                        .WithMany()
                        .HasForeignKey("AnimalProfileId");

                    b.HasOne("Nicepet_API.Models.Animal", "Father")
                        .WithMany()
                        .HasForeignKey("FatherId");

                    b.HasOne("Nicepet_API.Models.Animal", "Mother")
                        .WithMany()
                        .HasForeignKey("MotherId");

                    b.HasOne("Nicepet_API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Nicepet_API.Models.AnimalBreed", b =>
                {
                    b.HasOne("Nicepet_API.Models.AnimalHairType", "AnimalHairType")
                        .WithMany()
                        .HasForeignKey("AnimalHairTypeId");

                    b.HasOne("Nicepet_API.Models.AnimalSpecies", "AnimalSpecies")
                        .WithMany()
                        .HasForeignKey("AnimalSpeciesId");
                });

            modelBuilder.Entity("Nicepet_API.Models.Announcement", b =>
                {
                    b.HasOne("Nicepet_API.Models.Animal", "Animal")
                        .WithMany()
                        .HasForeignKey("AnimalId");

                    b.HasOne("Nicepet_API.Models.AnnouncementType", "AnnouncementType")
                        .WithMany()
                        .HasForeignKey("AnnouncementTypeId");

                    b.HasOne("Nicepet_API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Nicepet_API.Models.BreedingProfile", b =>
                {
                    b.HasOne("Nicepet_API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Nicepet_API.Models.BreedingType", b =>
                {
                    b.HasOne("Nicepet_API.Models.BreedingProfile", "BreedingProfile")
                        .WithMany()
                        .HasForeignKey("BreedingProfileId");
                });

            modelBuilder.Entity("Nicepet_API.Models.ChatHistory", b =>
                {
                    b.HasOne("Nicepet_API.Models.User", "RecipientUser")
                        .WithMany()
                        .HasForeignKey("RecipientUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nicepet_API.Models.User", "SenderUser")
                        .WithMany()
                        .HasForeignKey("SenderUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nicepet_API.Models.FranceBreeding", b =>
                {
                    b.HasOne("Nicepet_API.Models.BreedingProfile", "BreedingProfile")
                        .WithMany()
                        .HasForeignKey("BreedingProfileId");
                });

            modelBuilder.Entity("Nicepet_API.Models.HistoricalOwner", b =>
                {
                    b.HasOne("Nicepet_API.Models.Animal", "Animal")
                        .WithMany()
                        .HasForeignKey("AnimalId");

                    b.HasOne("Nicepet_API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Nicepet_API.Models.User", b =>
                {
                    b.HasOne("Nicepet_API.Models.UserType", "UserType")
                        .WithMany()
                        .HasForeignKey("UserTypeId");
                });

            modelBuilder.Entity("Nicepet_API.Models.UserAddress", b =>
                {
                    b.HasOne("Nicepet_API.Models.User", "User")
                        .WithMany("UserAddressMany")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Nicepet_API.Models.UserProfile", b =>
                {
                    b.HasOne("Nicepet_API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
