using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using DDBus.Models;

namespace DDBus.Entity
{


    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CollectionNameAttribute : Attribute
    {
        public string Name { get; }
        public CollectionNameAttribute(string name)
        {
            Name = name;
        }
    }



    [CollectionName("Account")]
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("username")]
        public string? Username { get; set; }

        [BsonElement("email")]
        public required string Email { get; set; }

        [BsonElement("password")]
        public required string Password { get; set; }

        [BsonElement("active")]
        public required bool active { get; set; }

        [BsonElement("root")]
        public required bool root { get; set; }

    }



    [CollectionName("Stops")]
    public class Stops
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("stop_name")]
        public string? stop_name { get; set; }

        [BsonElement("lat")]
        public required double lat { get; set; }

        [BsonElement("lon")]
        public required double lon { get; set; }

        [BsonElement("routes")]
        public required List<string> routes { get; set; }
    }


    [CollectionName("Routes")]
    public class Routes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("route_name")]
        public string? stop_name { get; set; }

        [BsonElement("start_time")]
        public required DateTime start_time { get; set; }

        [BsonElement("end_time")]
        public required DateTime end_time { get; set; }

        [BsonElement("interval")]
        public required int interval { get; set; }

        [BsonElement("price")]
        public required int price { get; set; }

        [BsonElement("outbound_stops")]
        public required List<string> outbound_stops { get; set; }

        [BsonElement("inbound_stops")]
        public required List<string> inbound_stops { get; set; }

        [BsonElement("route_length")]
        public required double route_length { get; set; }
    }

}
