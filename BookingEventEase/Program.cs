using Azure.Storage.Blobs;
using BookingEventEase.Data;
using BookingEventEase.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<BookingEventEaseDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnect")));

// Register BlobHelper
builder.Services.AddSingleton(x =>
{
    var connectionString = builder.Configuration["AzureStorage:ConnectionString"];
    var blobServiceClient = new BlobServiceClient(connectionString);
    return new BlobHelper(blobServiceClient);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
