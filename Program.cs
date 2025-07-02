using challange_bikeRental.Config.db;
using challange_bikeRental.Config.Rabbit;
using challange_bikeRental.Repositories.Bikes;
using challange_bikeRental.Repositories.DeliveryUser;
using challange_bikeRental.Repositories.Logs;
using challange_bikeRental.Repositories.RentedMotorcycles;
using challange_bikeRental.Services.Bikes;
using challange_bikeRental.Services.DeliveryUsers;
using challange_bikeRental.Services.RentedMotorcycles;
using challange_bikeRental.Utils.Producers;
using MongoDB.Driver;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Config Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Challenge Bike Rental API",
        Version = "v1",
        Description = "Documentation of API integration - Technical challenge",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Habynner Silva",
            Email = "habynner.jeuel@gmail.com"
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Config MongoDB
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>();
    if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new InvalidOperationException("MongoDB settings or connection string is not configured properly.");
    }
    return new MongoClient(settings.ConnectionString);
});

// Repositories, Services and Controllers
builder.Services.AddControllers();
builder.Services.AddScoped<IBikeRepository, BikeRepository>();
builder.Services.AddScoped<BikeService>();

builder.Services.AddScoped<IDeliveryUserRepository, DeliveryUserRepository>();
builder.Services.AddScoped<DeliveryUserService>();

builder.Services.AddScoped<IRentedMotorcycleRepository, RentedMotorcycleRepository>();
builder.Services.AddScoped<RentedMotorcycleService>();

builder.Services.AddScoped<ILogsMotorcycleCreatedRepository, LogMotorcycleCreatedRepository>();

// Config RabbitMQ
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddSingleton<RabbitMqProducer>();
builder.Services.AddHostedService<RabbitMqConsumerHostedService>();

var app = builder.Build();


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
