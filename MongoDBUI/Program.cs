using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Threading.Tasks.Dataflow;

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
                FirstName = "John",
                LastName = "Williams"
            };
            user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "John@mail.com" });
            user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "me@mail.com" });

            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "555-1212" });
            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "555-1234" });

            //CreateContact(user);
            //"d0e9af2f-4510-42f4-a6e2-b3085fb8ea8e"


            //GetAllContacts();
            //GetContactById("d0e9af2f-4510-42f4-a6e2-b3085fb8ea8e");

            //7756212c-92be-47d6-aff6-07b6b39256f6

            //UpdateContactsFirstName("John", "7756212c-92be-47d6-aff6-07b6b39256f6");

            //RemovePhoneNumberFromUser("555-1212", "7756212c-92be-47d6-aff6-07b6b39256f6");

            RemoveUser("d0e9af2f-4510-42f4-a6e2-b3085fb8ea8e");

            GetAllContacts();

            Console.WriteLine("Done Processing MongoDb");
            Console.ReadLine();
        }

        public static void RemoveUser(string id)
        {
            Guid guid = Guid.Parse(id);
            db.DeleteRecord<ContactModel>(tableName, guid);
        }

        public static void RemovePhoneNumberFromUser(string phoneNumber, string id)
        {
            Guid guid = Guid.Parse(id);
            var contact = db.LoadRecordById<ContactModel>(tableName, guid);

            contact.PhoneNumbers = contact.PhoneNumbers.Where(x => x.PhoneNumber != phoneNumber).ToList();

            db.UpsertRecord(tableName, contact.Id, contact);
        }

        private static void UpdateContactsFirstName(string firstName, string id)
        {
            Guid guid = Guid.Parse(id);
            var contact = db.LoadRecordById<ContactModel>(tableName, guid);

            contact.FirstName = firstName;

            db.UpsertRecord(tableName, contact.Id, contact);
        }

        private static void GetContactById(string id)
        {
            Guid guid = Guid.Parse(id);
            var contact = db.LoadRecordById<ContactModel>(tableName, guid);
            Console.WriteLine($"{contact.Id}: {contact.FirstName} {contact.LastName}");
        }

        private static void GetAllContacts()
        {
            var contacts = db.LoadRecords<ContactModel>(tableName);

            foreach (var contact in contacts)
            {
                Console.WriteLine($"{contact.Id}: {contact.FirstName} {contact.LastName}");
            }
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
