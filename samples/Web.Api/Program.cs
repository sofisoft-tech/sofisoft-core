using MongoDB.Driver;
using Sofisoft.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSofisoft()
    .AddLogging(options =>
    {
        options.SetBaseAddress("http://localhost:3000");
        options.SetTokenValue("dXNybG9nZ2luZzpBMTIzNDU2YQ==");
        options.SetSource("Sample Test");
    })
    .AddIdentity(options =>
    {
        options.SetUserIdKey("user_id");
    })
    .AddMongoDb(options =>
    {
        options.SetConnectionString("connection");
        options.SetDatabase("database");
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var supportedCultures = new[] { "es", "en" };

app.UseRequestLocalization(options =>
{
    options.SetDefaultCulture(supportedCultures[0]);
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

app.UseLogging();
app.UseDbTransaction<IClientSessionHandle>();

// app.UseMiddleware<SofisoftLoggingMiddleware>();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllers();

app.Run();
