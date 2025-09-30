using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class MongoDBDataAccess
    {
        private IMongoDatabase db;
        public MongoDBDataAccess(string dbName, MongoClientSettings settings)
        {
            var client = new MongoClient(settings);
            db = client.GetDatabase(dbName);

        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordById<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            return collection.Find(filter).First();
        }

        //public void UpsertRecord<T>(string table, Guid id, T record)
        //{
        //    var collection = db.GetCollection<T>(table);


        //    var result = collection.ReplaceOne(
        //        new BsonDocument(),
        //        record,
        //        new ReplaceOptions { IsUpsert = true });
        //}

        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            var collection = db.GetCollection<T>(table);

            var filter = Builders<T>.Filter.Eq("_id", id);

            var result = collection.FindOneAndReplace(
                filter,
                record,
                new FindOneAndReplaceOptions<T> { IsUpsert = true, ReturnDocument = ReturnDocument.After });
        }

        public void DeleteRecord<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);

        }
    }
}
