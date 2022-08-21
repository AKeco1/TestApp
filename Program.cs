using Microsoft.AspNetCore.Http.Features;
using TestProjectWthAngular.ErrorHandling;
using TestProjectWthAngular.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy1",
        policy =>
        {
            policy.WithOrigins("https://localhost:44452", "https://localhost:7159", "http://localhost:5159", "http://localhost:5400", "http://localhost:4200", "http://localhost:42170");
        });
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IXmlConverter, XmlConverterService>();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
var app = builder.Build();

var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.ConfigExceptionHandler();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("Policy1");

//app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
