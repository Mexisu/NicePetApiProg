using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nicepet_API.Models
{
    public class ApiNicepetContext : DbContext
    {
        public ApiNicepetContext() : base()
        {
        }
        public ApiNicepetContext(DbContextOptions<ApiNicepetContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //}
        //------------------------------------------User---------------------------------------
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<UserAddress> UserAddress { get; set; }

        //------------------------------------------Animal---------------------------------------
        public virtual DbSet<Animal> Animal { get; set; }
        public virtual DbSet<AnimalProfile> AnimalProfile { get; set; }
        public virtual DbSet<AnimalBreed> AnimalBreed { get; set; }
        public virtual DbSet<AnimalSpecies> AnimalSpecies { get; set; }
        public virtual DbSet<AnimalHairType> AnimalHairType { get; set; }
        public virtual DbSet<HistoricalOwner> HistoricalOwner { get; set; }

        //------------------------------------------Announcement---------------------------------------
        public virtual DbSet<Announcement> Announcement { get; set; }
        public virtual DbSet<AnnouncementType> AnnouncementType { get; set; }

        //------------------------------------------Breeding---------------------------------------
        public virtual DbSet<BreedingProfile> BreedingProfile { get; set; }
        public virtual DbSet<BreedingType> BreedingType { get; set; }
        public virtual DbSet<FranceBreeding> FranceBreeding { get; set; }

        //----------------------------------------Message--------------------------------------------------
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //------------------------------------------User---------------------------------------
            modelBuilder.Entity<User>().HasOne(c => c.UserType);
            modelBuilder.Entity<UserProfile>().HasOne(c => c.User);
            modelBuilder.Entity<UserAddress>().HasOne(c => c.User).WithMany(c => c.UserAddressMany);
            //------------------------------------------Animal---------------------------------------
            modelBuilder.Entity<Animal>().HasOne(c => c.User);
            modelBuilder.Entity<Animal>().HasOne(c => c.AnimalBreed);
            modelBuilder.Entity<Animal>().HasOne(c => c.AnimalProfile);
            modelBuilder.Entity<Animal>().HasOne(c => c.Father);
            modelBuilder.Entity<Animal>().HasOne(c => c.Mother);
            modelBuilder.Entity<AnimalBreed>().HasOne(c => c.AnimalSpecies);
            modelBuilder.Entity<AnimalBreed>().HasOne(c => c.AnimalHairType);
            //------------------------------------------Announcement---------------------------------------
            modelBuilder.Entity<Announcement>().HasOne(c => c.AnnouncementType);
            //------------------------------------------Breeding---------------------------------------
            modelBuilder.Entity<BreedingProfile>().HasOne(c => c.User);
            modelBuilder.Entity<BreedingType>().HasOne(c => c.BreedingProfile);
            modelBuilder.Entity<FranceBreeding>().HasOne(c => c.BreedingProfile);
            //------------------------------------------Message-------------------------------------------
            modelBuilder.Entity<Message>().HasOne(c => c.RecipientUser);
            modelBuilder.Entity<Message>().HasOne(c => c.SenderUser);
            modelBuilder.Entity<Contacts>().HasKey(c => new { c.UserId, c.ContactId });
            modelBuilder.Entity<Contacts>().HasOne(c => c.UserContact);
        }



    }
}
