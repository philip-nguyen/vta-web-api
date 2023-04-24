using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes; // deserialize subsections
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// command line arguments
String api_key = "", format = "";

if(args.Count() >= 2) {
    if(args[0] != "json")
        Console.WriteLine("Format requred is json or just run 'dotnet run'.");
    else
        format = args[0];
    
    if(args[1] != "59af72683221a1734f637eae7a7e8d9b")
        Console.WriteLine("API Key is required or just run 'dotnet run'.");
    else
        api_key = args[1];
}
else {
    // These are here is user just uses `dotnet run` command
    api_key = "59af72683221a1734f637eae7a7e8d9b";
    format = "json";
}



// Use an HttpClient object to send GET Request
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Add("Authorization", api_key);
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/"+format));

// make the HTTP request and process the JSON 
await ProcessTripUpdatesAsync(client);

// Start web API application
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TripUpdatesContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Using this endpoint returns trip updates upto 100 records
app.MapGet("/real-time/trip-updates", async (TripUpdatesContext db) =>
    await db.TripUpdates.Take(100).ToListAsync());

app.Run();

// Process TripUpdates JSON data from HTTP request 
static async Task ProcessTripUpdatesAsync(HttpClient client)
{ 
    var json = await client.GetStringAsync(
        "https://api.goswift.ly/real-time/vta/gtfs-rt-trip-updates?format=json"
    );

    //Console.Write(json);
    // Parse all of the JSON
    JsonNode tripNode = JsonNode.Parse(json)!;

    // Get the header subsection
    JsonNode headerObject = tripNode!["header"]; 
    Console.WriteLine($"header={headerObject.ToJsonString()}");

    // Get the entity subsection
    JsonArray tripUpdatesArr = tripNode!["entity"]!.AsArray();

    // Send JsonArray to DB Write function
    writeTripUpdatesDbContext(tripUpdatesArr);
}

// Function to write to the DB 
static void writeTripUpdatesDbContext(JsonArray tripUpdatesArr)
{
    // Start DB instance
    using (var db = new TripUpdatesContext())
    {
        int dbWrites = 0;
        // Parse through tripUpdate nodes
        foreach(JsonNode? tripUpdate in tripUpdatesArr)
        {
            // add all tripUpdate ids to ActiveTripUpdate
            db.ActiveTripUpdates.Add(new ActiveTripUpdate { ActiveTripUpdateId = tripUpdate["id"].ToString()});
            
            JsonNode tripUpdateData = tripUpdate["tripUpdate"]["trip"]["scheduleRelationship"];
            
            Console.WriteLine($"tripUpdateId : {tripUpdate["id"]}\t\tscheduleRelationship : {tripUpdateData}");
            //Console.WriteLine($"VehicleId : {tripUpdate["tripUpdate"]["trip"]["vehicle"]}");
            
            
            // add tripUpdate to db IF the tripupdate does not already exist
            if(!db.TripUpdates
                .Where(c => c.TripUpdateId == tripUpdate["id"].ToString())
                .ToList().Any())
            {
                dbWrites++;
                // add TripUpdate
                if(tripUpdate["tripUpdate"]["trip"]["scheduleRelationship"].ToString() == "SCHEDULED") {

                    Console.WriteLine($"VehicleId : {tripUpdate["tripUpdate"]["vehicle"]["id"]}");
                    db.TripUpdates.Add(new TripUpdate {
                        TripUpdateId = tripUpdate["id"].ToString(),
                        
                        VehicleId = tripUpdate["tripUpdate"]["vehicle"]["id"].ToString(),
                        Timestamp = tripUpdate["tripUpdate"]["timestamp"].ToString()
                    });

                    // add the multiple stopTimeUpdates 
                    JsonArray stopTimeUpdatesArr = tripUpdate["tripUpdate"]["stopTimeUpdate"]!.AsArray();

                    foreach(JsonNode? stopTimeUpdate in stopTimeUpdatesArr) {
                        // add StopTimeUpdate
                        db.StopTimeUpdates.Add(new StopTimeUpdate {
                            TripUpdateId = tripUpdate["id"].ToString(), // foreign key
                            StopSequence = stopTimeUpdate["stopSequence"].GetValue<int>(),
                            ArrivalTime = stopTimeUpdate["arrival"]?["time"].ToString(),
                            StopId = stopTimeUpdate["stopId"].ToString(),
                            ScheduleRelationship = stopTimeUpdate["scheduleRelationship"].ToString()
                        });
                    }
                }
                else {
                    
                    db.TripUpdates.Add(new TripUpdate {
                        TripUpdateId = tripUpdate["id"].ToString(),
                        // TripId = tripUpdate["tripUpdate"]["trip"]["tripId"].ToString(),
                        // no vehicle id
                        Timestamp = tripUpdate["tripUpdate"]["timestamp"].ToString()
                    });
                }

                // add Trip
                // trip entity is always present regardless if SCHEDULED or CANCELED
                // also check if there is a TripId already present
                if(!db.Trips
                    .Where(c => c.TripId == tripUpdate["tripUpdate"]["trip"]["tripId"].ToString())
                    .Any())
                {
                    
                    db.Trips.Add(new Trip {
                        TripId = tripUpdate["tripUpdate"]["trip"]["tripId"].ToString(),
                        StartTime = tripUpdate["tripUpdate"]["trip"]["startTime"].ToString(),
                        StartDate = tripUpdate["tripUpdate"]["trip"]["startDate"].ToString(),
                        ScheduleRelationship = tripUpdate["tripUpdate"]["trip"]["scheduleRelationship"].ToString(),
                        RouteId = tripUpdate["tripUpdate"]["trip"]["routeId"].ToString(),
                        DirectionId = tripUpdate["tripUpdate"]["trip"]["directionId"].GetValue<int>(),
                        TripUpdateId = tripUpdate["id"].ToString()  // foreign key
                    });
                    
                }
                db.SaveChanges();
            }
            
        }
        RemoveExpiredTripUpdates();
        // delete all records from activeTripUpdates
        // show how dbWrites compared to tripUpdate nodes from api
        Console.WriteLine($"TripUpdate DB Writes:\t{dbWrites}");
    }
    
    int count = tripUpdatesArr.Count;
    Console.WriteLine($"TripUpdateArr Count:\t{count}");
    
}

// Extra Credit 3
static void RemoveExpiredTripUpdates()
{
    using (var db = new TripUpdatesContext())
    {
        var activeTripUpdates = db.ActiveTripUpdates.ToList(); // List<ActiveTripUpdate>
        var tripUpdatesToDelete = db.TripUpdates.ToList(); //.Except(deleteExpiredTripUpdates); List<TripUpdates>
        //var expiredTripUdpates = tripUpdatesToDelete.Except(activeTripUpdates);

        foreach (var tripUpdateToDelete in tripUpdatesToDelete)
        {
            // if there is NO record in activeTripUpdates table with tripUpdateToDelete.TripUpdateId
            // then delete from TripUpdates
            if(!db.ActiveTripUpdates.Where(c => c.ActiveTripUpdateId == tripUpdateToDelete.TripUpdateId).ToList().Any())
            {
                db.TripUpdates.Remove(tripUpdateToDelete);
            }
            db.SaveChanges();
        }
        
        // remove all records from ActiveTripUdpates
        foreach (var item in db.ActiveTripUpdates) 
        {
            db.ActiveTripUpdates.Remove(item);
        }
        db.SaveChanges();
    }
}