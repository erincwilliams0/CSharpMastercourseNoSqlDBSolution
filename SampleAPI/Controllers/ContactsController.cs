using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace SampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private static MongoDBDataAccess db;
        private static readonly string tableName = "Contacts";


        public ContactsController(MongoDBDataAccess database)
        {
            //_config = config;
            ////BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            //MongoClientSettings settings = MongoClientSettings.FromConnectionString(_config.GetConnectionString("Default"));


            db = database;


        }

        [HttpGet]
        public List<ContactModel> GetAll()
        {
            return db.LoadRecords<ContactModel>(tableName);
        }

        [HttpPost]
        public void InsertRecord(ContactModel contact)
        {
            db.UpsertRecord(tableName, contact.Id, contact);
        }
    }
}
