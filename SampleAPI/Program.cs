
using DataAccessLibrary;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace SampleAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register Guid serializer (run once per app lifetime)
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));


            // Add services to the container.
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                MongoClientSettings settings = MongoClientSettings.FromConnectionString(
                    builder.Configuration.GetConnectionString("Default"));

                return new MongoClient(settings);
            });

            builder.Services.AddSingleton(sp =>
            {
                IMongoClient client = sp.GetRequiredService<IMongoClient>();

                return client.GetDatabase("MongoContactsDb");
            });

            builder.Services.AddScoped<MongoDBDataAccess>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }
    }
}
