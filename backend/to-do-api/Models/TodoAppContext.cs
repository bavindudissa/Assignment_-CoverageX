using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace to_do_api.Models;

public partial class TodoAppContext : DbContext
{
    public TodoAppContext()
    {
    }

    public TodoAppContext(DbContextOptions<TodoAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Task> Tasks { get; set; }

    //chnage this to link the default connetion
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), option =>{
            option.EnableRetryOnFailure(maxRetryCount: 2);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Task__7C6949B1A4C3D839");

            entity.ToTable("Task");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
