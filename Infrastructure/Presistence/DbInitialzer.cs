using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.OrderModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using Presistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presistence
    {
    public class DbInitialzer : IDbInitialzer
        {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _storeIdentityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitialzer(
            StoreDbContext context,
            StoreIdentityDbContext storeIdentityDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
            {
            _context = context;
            _storeIdentityDbContext = storeIdentityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            }
        public async Task InitialzeAsync()
            {
            // Create the database and apply Pending migrations
            #region Migrations

            if ( _context.Database.GetPendingMigrations().Any() )
                {
                await _context.Database.MigrateAsync();
                }

            #endregion

            // Data Seeding
            #region Seeds

            #region Product Types Seeding

            // Check if the ProductTypes table is empty before seeding
            if ( !_context.ProductTypes.Any() )
                {

                // Read the JSON file containing the product types data
                //..\Infrastructure\Presistence\Data\Seeding\types.json
                var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\Seeding\types.json");

                // Deserialize the JSON data into a list of ProductType objects
                var productTypes = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if ( productTypes is not null && productTypes.Any() )
                    {
                    // Add the product types to the database context
                    await _context.ProductTypes.AddRangeAsync(productTypes);
                    await _context.SaveChangesAsync();
                    }
                }

            #endregion

            #region Product Brands Seeding

            // Check if the ProductBrands table is empty before seeding
            if ( !_context.ProductBrands.Any() )
                {
                // Read the JSON file containing the product brands data
                //..\Infrastructure\Presistence\Data\Seeding\brands.json
                var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\Seeding\brands.json");

                // Deserialize the JSON data into a list of ProductBrand objects
                var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if ( productBrands is not null && productBrands.Any() )
                    {
                    // Add the product brands to the database context
                    await _context.ProductBrands.AddRangeAsync(productBrands);
                    await _context.SaveChangesAsync();
                    }
                }

            #endregion

            #region Products Seeding

            // Check if the Products table is empty before seeding
            if ( !_context.Products.Any() )
                {
                // Read the JSON file containing the products data
                //..\Infrastructure\Presistence\Data\Seeding\products.json
                var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\Seeding\products.json");

                // Deserialize the JSON data into a list of Product objects
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if ( products is not null && products.Any() )
                    {
                    // Add the products to the database context
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                    }
                }

            #endregion

            #region DeliveryMethod Seeding

            // Check if the Products table is empty before seeding
            if ( !_context.DeliveryMethods.Any() )
                {
                // Read the JSON file containing the products data
                //..\Infrastructure\Presistence\Data\Seeding\products.json
                var DeliveryMethodsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\Seeding\delivery.json");

                // Deserialize the JSON data into a list of Product objects
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if ( deliveryMethods is not null && deliveryMethods.Any() )
                    {
                    // Add the products to the database context
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await _context.SaveChangesAsync();
                    }
                }

            #endregion 

            #endregion

            }

        public async Task InitialzeIdentityAsync()
            {
            #region Migrations
            if ( _storeIdentityDbContext.Database.GetPendingMigrations().Any() )
                {
                await _storeIdentityDbContext.Database.MigrateAsync();
                }
            #endregion

            #region Seeding

            #region Seeding Roles

            if ( !_roleManager.Roles.Any() )
                {
                await _roleManager.CreateAsync(new IdentityRole()
                    {
                    Name = "Admin",
                    });
                await _roleManager.CreateAsync(new IdentityRole()
                    {
                    Name = "SuperAdmin",
                    });
                }


            #endregion

            #region Seeding Admins

            if ( !_userManager.Users.Any() )
                {
                var SuperAdminUser = new AppUser()
                    {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "1234567890",
                    };

                var AdminUser = new AppUser()
                    {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "1234567890",
                    };

                await _userManager.CreateAsync(SuperAdminUser, "Pa$$w0rd");
                await _userManager.CreateAsync(AdminUser, "Pa$$w0rd");


                #region Adding Roles to users

                await _userManager.AddToRoleAsync(SuperAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(AdminUser, "Admin");

                #endregion



                }
            #endregion




            #endregion

            }

        }
    }
