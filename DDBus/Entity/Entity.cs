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

        [BsonElement("role")]
        public required int role { get; set; }

    }



    [CollectionName("Stops")]
    public class Stops
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? name { get; set; }

        [BsonElement("lat")]
        public required double lat { get; set; }

        [BsonElement("lon")]
        public required double lon { get; set; }

    }


    [CollectionName("Routes")]
    public class Routes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? name { get; set; }

        [BsonElement("index_route")]
        public required string index_route { get; set; }

        [BsonElement("start_time")]
        public required string start_time { get; set; }

        [BsonElement("end_time")]
        public required string end_time { get; set; }

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




    [CollectionName("Notifications")]
    public class Notifications
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string? title { get; set; }

        [BsonElement("description")]
        public string? description { get; set; }

        [BsonElement("expire_at")]
        public required DateTime expire_at { get; set; }

    }

}
