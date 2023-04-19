using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
}

public class TripUpdate
{
    public String TripUpdateId { get; set; }
    public String TripId { get; set; } = default!;

    public List<StopTimeUpdate>? StopTimeUpdates { get; } = new();

    public String? VehicleId { get; set; }
    public String? Timestamp { get; set; }
}

public class StopTimeUpdate
{
    [Key]
    public String TripUpdateId { get; set; }
    public int StopSequence { get; set; }
    public String? ArrivalTime { get; set; }
    public String? StopId { get; set; }
    public String? ScheduleRelationship { get; set; }
}

public class Trip
{
    public String TripId { get; set; }
    public String? StartTime { get; set; }
    public String? StartDate { get; set; }
    public String? ScheduleRelationship { get; set; }
    public String? RouteId { get; set; }
    public int DirectionId { get; set; }
}