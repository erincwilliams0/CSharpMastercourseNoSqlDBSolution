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
        private IConfiguration _config;
        private static MongoDBDataAccess db;
        private static readonly string tableName = "Contacts";


        public ContactsController(IConfiguration config)
        {
            //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            _config = config;
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_config.GetConnectionString("Default"));


            db = new MongoDBDataAccess("MongoContactsDb", settings);
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
