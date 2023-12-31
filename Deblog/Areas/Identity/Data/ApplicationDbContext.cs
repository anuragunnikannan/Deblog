﻿using Deblog.Areas.Identity.Data;
using Deblog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Deblog.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public object userdata { get; internal set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<Bookmark>().HasKey(o => new { o.Id, o.BlogId });
    }

    public DbSet<Userdata> Userdata { get; set; }

    public DbSet<Blog> Blogs { get; set; }

    public DbSet<Bookmark> Bookmarks { get; set; }
}
