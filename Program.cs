using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes; // deserialize subsections
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// Use an HttpClient object to send GET Request
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Add("Authorization", "59af72683221a1734f637eae7a7e8d9b");
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

// make the HTTP request and process the JSON 
await ProcessTripUpdatesAsync(client);

// Start web API application
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TripUpdatesContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/real-time/trip-updates", async (TripUpdatesContext db) =>
    await db.TripUpdates.Take(100).ToListAsync());

app.Run();

// Process TripUpdates JSON data from HTTP request 
static async Task ProcessTripUpdatesAsync(HttpClient client)
{
    // TODO: (EC) create command to accept args for API_KEY and GET params (format=json) 
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
            
            JsonNode tripUpdateData = tripUpdate["tripUpdate"]["trip"]["scheduleRelationship"];
            //JsonNode tripUpdateRow = tripUpdateData
            Console.WriteLine($"tripUpdateId : {tripUpdate["id"]}\t\tscheduleRelationship : {tripUpdateData}");
            
            
            // add tripUpdate to db
            if(!db.TripUpdates
                .Where(c => c.TripUpdateId == tripUpdate["id"].ToString())
                .ToList().Any())
            {
                dbWrites++;
                //Console.WriteLine($"TripUpdateId: {tripUpdate["id"].ToString()}\nTripId: {tripUpdate["tripUpdate"]["trip"]["tripId"].ToString()}\nVehicleId: {}");
                // add TripUpdate
                if(tripUpdate["tripUpdate"]["trip"]["vehicle"] != null){
                    db.TripUpdates.Add(new TripUpdate {
                        TripUpdateId = tripUpdate["id"].ToString(),
                        //TripId = tripUpdate["tripUpdate"]["trip"]["tripId"].ToString(),
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
                            ArrivalTime = stopTimeUpdate["arrival"]["time"].ToString(),
                            StopId = stopTimeUpdate["stopId"].ToString(),
                            ScheduleRelationship = stopTimeUpdate["scheduleRelationship"].ToString()
                        });
                    }
                }
                else {
                    Console.WriteLine("Debugging...1");
                    db.TripUpdates.Add(new TripUpdate {
                        TripUpdateId = tripUpdate["id"].ToString(),
                        // TripId = tripUpdate["tripUpdate"]["trip"]["tripId"].ToString(),
                        // no vehicle id
                        Timestamp = tripUpdate["tripUpdate"]["timestamp"].ToString()
                    });
                    Console.WriteLine("Debugging...2");
                }

                Console.WriteLine("Debugging...2.5");
                // add Trip
                // trip entity is always present regardless if SCHEDULED or CANCELED
                // also check if there is a TripId already present
                if(!db.Trips
                    .Where(c => c.TripId == tripUpdate["tripUpdate"]["trip"]["tripId"].ToString())
                    .Any())
                {
                    Console.WriteLine("Debugging...3");
                    db.Trips.Add(new Trip {
                        TripId = tripUpdate["tripUpdate"]["trip"]["tripId"].ToString(),
                        StartTime = tripUpdate["tripUpdate"]["trip"]["startTime"].ToString(),
                        StartDate = tripUpdate["tripUpdate"]["trip"]["startDate"].ToString(),
                        ScheduleRelationship = tripUpdate["tripUpdate"]["trip"]["scheduleRelationship"].ToString(),
                        RouteId = tripUpdate["tripUpdate"]["trip"]["routeId"].ToString(),
                        DirectionId = tripUpdate["tripUpdate"]["trip"]["directionId"].GetValue<int>(),
                        TripUpdateId = tripUpdate["id"].ToString()  // foreign key
                    });
                    Console.WriteLine("Debugging...4");
                }
                db.SaveChanges();
                Console.WriteLine("Debugging...5");
            }
            // show how dbWrites compare to t
            //Console.WriteLine($"TripUpdate DB Writes:\t{dbWrites}");
        }
        Console.WriteLine($"TripUpdate DB Writes:\t{dbWrites}");
    }
    
    int count = tripUpdatesArr.Count;
    Console.WriteLine($"TripUpdateArr Count:\t{count}");
    
}