using ConsulAndVaultSample;
using ConsulAndVaultSample.VaultSharp;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);
var consulSettings = builder.Configuration.GetSection("ConsulConfiguration");
var addresses = consulSettings.GetSection("Addresses").Get<string[]>();
var key = consulSettings.GetValue<string>("Key");
var healthyAddress= ConsulFailover.SelectHealthyConsul(addresses!);

builder.Configuration.AddConsul(
    "ConsulAndVaultSample/Development/appsettings.json", // KV key in Consul
    options =>
    {
        options.ConsulConfigurationOptions = cco =>
        {
            cco.Address = new Uri(healthyAddress);
        };
        options.Optional = true;        // fallback to local JSON
        options.ReloadOnChange = true;  // reload on KV change
        options.OnLoadException = ctx =>
        {
            Console.WriteLine("❗ Failed to load config from Consul:");
            Console.WriteLine(ctx.Exception.Message);
            ctx.Ignore = true; // do not crash app
        };
    });

builder.Configuration.AddVaultSecurity(builder.Configuration);

builder.Services.Configure<SampleSetting>(builder.Configuration.GetSection("SampleSetting"));


// Add services to the container.

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
