using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.Abstractions.Models;

namespace Sofisoft.MongoDb.Models
{
    public abstract class Document : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; private set; }

        [BsonRepresentation(BsonType.DateTime)]
        [BsonElement("createdAt")]
        public DateTime CreatedAt => new ObjectId(Id).CreationTime;

        [BsonElement("createdBy")]
        public string? CreatedBy { get; set; }

        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonElement("modifiedAt")]
        public DateTime? ModifiedAt { get; private set; }

        [BsonIgnoreIfNull]
        [BsonElement("modifiedBy")]
        public string? ModifiedBy { get; set; }
    }
}