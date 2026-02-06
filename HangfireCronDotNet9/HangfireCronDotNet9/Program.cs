
using Hangfire;
using HangfireBasicAuthenticationFilter;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(config =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[]
    {
        new HangfireBasicAuthenticationFilter.HangfireCustomBasicAuthenticationFilter
        {
            User = "admin",
            Pass = "admin"
        }
    }
});



app.Run();
