using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SMS.Core.Models;

namespace SMS.Infrastructure.Models
{
    public partial class SMSContext : DbContext
    {
        public SMSContext(DbContextOptions<SMSContext> contextOptions) : base(contextOptions)
        {

        }
        public DbSet<ProductDetail> Products { get; set; }
    }
}
