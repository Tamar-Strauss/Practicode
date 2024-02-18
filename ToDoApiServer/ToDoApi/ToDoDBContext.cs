using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToDoApi;

public partial class ToDoDBContext : DbContext
{
    public ToDoDBContext()
    {
    }

    public ToDoDBContext(DbContextOptions<ToDoDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));
    // {
    //     IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    //     optionsBuilder.UseMySql(configuration.GetConnectionString("ToDoDB"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("items");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
