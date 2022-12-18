using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ElectronicShope.DBModel.DBModel
{
   public class E_Shopping_DbContext : IdentityDbContext
    {
        public E_Shopping_DbContext(DbContextOptions<E_Shopping_DbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderDetails)
                .WithOne(e => e.order)
                .IsRequired();


            modelBuilder.Entity<Category>().HasData(
                new Category {ID=1 , Name = "TVs"},
                new Category {ID =2, Name = "Labtops" },
                new Category {ID=3 ,  Name = "Sound Systems" }

                ) ;
            modelBuilder.Entity<Product>().HasData(
                new Product { ID = 1 ,CategoryID = 1 ,Name = "Samsung TV" , Description= "65 Inch TV Quantum Processor Lite 4K QLED", Price=4000 , Discount=10 },
                new Product { ID = 2, CategoryID = 1, Name = "LG TV", Description = "75 Inche Neo QLED 8K Smart TV with Solar Remote, ", Price = 8000 },
                new Product { ID = 3, CategoryID = 1, Name = "SamsungTV", Description = "85 Inch TV Neo Quantum Processor 4K ", Price = 5000 , Discount = 20},

                new Product { ID = 4, CategoryID = 2, Name = "Dell labtop", Description = "labtop coreI7", Price = 15000, Discount = 10 },
                new Product { ID = 5, CategoryID = 2, Name = "HP labtop", Description = "laptop corei7 and ram 8g", Price = 20000 },
                new Product { ID = 6, CategoryID = 2, Name = "Asus labtop", Description = "labtop coreI7", Price = 10000 } ,


                new Product { ID = 7, CategoryID = 3, Name = "Sony", Description = "best speaker brand", Price = 6000, Discount = 10 },
                new Product { ID = 8, CategoryID = 3, Name = "Pioneer", Description = "sound system with bluetooth and wi-fi ", Price = 8000 },
                new Product { ID = 9, CategoryID = 3, Name = "Kef", Description = "sound system with AI", Price = 10000 }

                );
        

            modelBuilder.Entity<IdentityRole>().HasData(
                 new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "Admin".ToUpper() } ,
                 new IdentityRole { Id = "2", Name = "User", NormalizedName = "User".ToUpper() } 
            );

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                    UserName = "Admin@gmail.com",
                    Email= "Admin@gmail.com", 
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    NormalizedEmail= "ADMIN@GMAIL.COM" ,
                    LockoutEnabled = true,
                    PasswordHash = hasher.HashPassword(null, "Admin@123")
                }
            );;

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            );

        }


            public DbSet<Category> Categories { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderDetails> OrderDetails { get; set; }
            public DbSet<Product> Products { get; set; }



    }
}
