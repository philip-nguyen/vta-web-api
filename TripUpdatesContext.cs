using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TripUpdatesContext : DbContext
{
	public DbSet<TripUpdate> TripUpdates { get; set; }
	public DbSet<StopTimeUpdate> StopTimeUpdates { get; set; }
    public DbSet<Trip> Trips { get; set; }

	public string DbPath { get; }

	public TripUpdatesContext()
	{
		var folder = Environment.SpecialFolder.LocalApplicationData;
		var path = Environment.GetFolderPath(folder);
		DbPath = System.IO.Path.Join(path, "tripUpdates.db");
	}

    // The following configures EF to create a Sqlite database file in the 
    // special "local" folder for your platform
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    /*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StopTimeUpdate>()
            .HasOne(e => e.TripUpdate)
            .WithMany(e => e.StopTimeUpdates)
            .HasForeignKey(e => e.TripUpdateId)
            .IsRequired();

        modelBuilder.Entity<Trip>()
            .HasOne(e => e.TripUpdate)
            .WithOne(e => e.Trip)
            .HasForeignKey(e => e.TripUpdateId)
            .IsRequired();
    }
    */
}

public class TripUpdate
{
    public String TripUpdateId { get; set; }
    
    public Trip Trip { get; set; }
    public List<StopTimeUpdate>? StopTimeUpdates { get; } = new List<StopTimeUpdate>();

    public String? VehicleId { get; set; }
    public String? Timestamp { get; set; }
}

public class StopTimeUpdate
{
    public int Id { get; set; }
    public int StopSequence { get; set; }
    public String? ArrivalTime { get; set; }
    public String? StopId { get; set; }
    public String? ScheduleRelationship { get; set; }

    [ForeignKey("TripUpdateId")]
    public String TripUpdateId { get; set; } = null!; // Required foreign key property
    public TripUpdate TripUpdate { get; set; } = null!; // Required reference navigation to principal
}

public class Trip
{
    public String TripId { get; set; } = null!;
    public String? StartTime { get; set; }
    public String? StartDate { get; set; }
    public String? ScheduleRelationship { get; set; }
    public String? RouteId { get; set; }
    public int DirectionId { get; set; }

    [ForeignKey("TripUpdateId")]
    public String TripUpdateId { get; set; } = null!; // Required foreign key property
    public TripUpdate TripUpdate { get; set; } = null!; // Required reference navigation to principal
}