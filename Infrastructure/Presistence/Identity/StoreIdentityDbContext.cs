using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Identity
    {
    public class StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : IdentityDbContext<AppUser>(options)
        {
        protected override void OnModelCreating(ModelBuilder builder)
            {
            base.OnModelCreating(builder);
            builder.Entity<Address>().ToTable("Addresses");

            builder.Entity<AppUser>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<AppUser>().HasIndex(u => u.NormalizedEmail).IsUnique();
            }
        }
    }
