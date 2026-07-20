using KocurmeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<CheatingStudent> CheatingStudents => Set<CheatingStudent>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Contingent> Contingents => Set<Contingent>();
    public DbSet<ImtReh> ImtRehs => Set<ImtReh>();
    public DbSet<ImtRehBina> ImtRehBinas => Set<ImtRehBina>();
    public DbSet<CheatingRoomStatsResult> CheatingRoomStatsResults => Set<CheatingRoomStatsResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Exam>()
            .HasMany(e => e.CheatingStudents)
            .WithOne(s => s.Exam)
            .HasForeignKey(s => s.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Exam>()
            .HasMany(e => e.Rooms)
            .WithOne(r => r.Exam)
            .HasForeignKey(r => r.ExamId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ImtRehBina>()
            .HasNoKey();
        modelBuilder.Entity<ImtReh>()
            .HasNoKey();
        modelBuilder.Entity<CheatingRoomStatsResult>()
            .HasNoKey();

    }
}
