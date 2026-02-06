using CustomeHangfireClean.Data;
using CustomeHangfireClean.Jobs;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(
              builder.Configuration.GetConnectionString("DefaultConnection"),
              new SqlServerStorageOptions
              {
                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                  QueuePollInterval = TimeSpan.Zero,
                  UseRecommendedIsolationLevel = true,
                  DisableGlobalLocks = true
              })
          .UseFilter(new CustomeExpirationTimeAttribute())
);


builder.Services.AddHangfireServer();
builder.Services.AddScoped<IncrementJob>();
//builder.Services.AddScoped<SquareJob>();
//builder.Services.AddScoped<NewNumberJob>();
builder.Services.AddScoped<DailyEMIJob>();
//builder.Services.AddScoped<Test1Job>();
//builder.Services.AddScoped<Test2Job>();

var app = builder.Build();

app.UseHangfireDashboard();

using (var scope = app.Services.CreateScope())
{
    var incrementJob = scope.ServiceProvider.GetRequiredService<IncrementJob>();
    //var squareJob = scope.ServiceProvider.GetRequiredService<SquareJob>();
    //var newNumberJob = scope.ServiceProvider.GetRequiredService<NewNumberJob>();
    var dailyEMIJob = scope.ServiceProvider.GetRequiredService<DailyEMIJob>();
    //var Test1job = scope.ServiceProvider.GetRequiredService<Test1Job>();
    //var Test2job = scope.ServiceProvider.GetRequiredService<Test2Job>();

    RecurringJob.AddOrUpdate<IncrementJob>("IncrementJob", x => x.Execute(), Cron.Minutely);

    //RecurringJob.AddOrUpdate<SquareJob>("SquareJob", x => x.Execute(), Cron.Minutely);

    //RecurringJob.AddOrUpdate<NewNumberJob>("NewNumberJob", x => x.Execute(), Cron.Minutely);

    BackgroundJob.Enqueue(() => dailyEMIJob.Execute());
    //BackgroundJob.Enqueue(() => Test1job.Execute());
    //BackgroundJob.Enqueue(() => Test2job.Execute());
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
