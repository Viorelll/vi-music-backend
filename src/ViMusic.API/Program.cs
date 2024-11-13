using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddApiServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    //USE MIGRATOR
    //using (var scope = app.Services.CreateScope())
    //{
    //    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    //    await initialiser.InitialiseAsync();
    //    await initialiser.SeedAsync();
    //}
}

// Configure App Configuration
var appConfigEndpoint = Environment.GetEnvironmentVariable("AppConfigEndpoint");

if (!string.IsNullOrEmpty(appConfigEndpoint))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
        options
            .ConfigureKeyVault(kv =>
                kv.SetCredential(new DefaultAzureCredential()))
            .Connect(
                new Uri(appConfigEndpoint),
                new DefaultAzureCredential()));
}

builder.Configuration.AddUserSecrets<Program>();

app.UseHealthChecks("/health");

app.UseStaticFiles();
app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
