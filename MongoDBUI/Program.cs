using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace MongoDBUI
{
    internal class Program
    {
        private static MongoDBDataAccess db;
        private static readonly string tableName = "Contacts";

        static void Main(string[] args)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            MongoClientSettings settings = MongoClientSettings.FromConnectionString(GetConnectionString());
            


            db = new MongoDBDataAccess("MongoContactsDb", settings);

            ContactModel user = new ContactModel
            {
                FirstName = "Erin",
                LastName = "Williams"
            };
            user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "erin@mail.com" });
            user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "me@mail.com" });

            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "555-1212" });
            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "555-1234" });

            CreateContact(user);




            Console.WriteLine("Done Processing MongoDb");
            Console.ReadLine();
        }

        private static void CreateContact(ContactModel contact)
        {
            db.UpsertRecord(tableName, contact.Id, contact);
        }
        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string? output = "";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
