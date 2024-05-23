using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace UNOSChat.ConversationAPI.Data;

public class ListStringSerializer: IBsonSerializer<List<string>>
{
    public List<string> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonReader = context.Reader;
        var bsonType = bsonReader.GetCurrentBsonType();

        if (bsonType != BsonType.Array)
        {
            throw new InvalidOperationException("El valor no es un array.");
        }

        var values = new List<string>();

        bsonReader.ReadStartArray();
        while (bsonReader.ReadBsonType() != BsonType.EndOfDocument)
        {
            values.Add(bsonReader.ReadString());
        }
        bsonReader.ReadEndArray();

        return values;
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, List<string> value)
    {
        var bsonWriter = context.Writer;

        bsonWriter.WriteStartArray();
        foreach (var item in value)
        {
            bsonWriter.WriteString(item);
        }
        bsonWriter.WriteEndArray();
    }

    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        throw new NotImplementedException();
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        throw new NotImplementedException();
    }

    public Type ValueType => typeof(IList<string>);
}
