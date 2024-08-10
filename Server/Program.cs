using InstagramCommerce.Server.Jobs;
using InstagramCommerce.Shared.Data;
using InstagramCommerce.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
var services = builder.Services;

services.AddDbContext<InstagramCommerceContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

services.AddScoped<InstagramService>();
services.AddScoped<OrderService>();
services.AddScoped<SynchronizationService>();
services.AddScoped<TelegramBotService>();
services.AddHttpClient();
services.AddControllers();
services.AddRazorPages();
services.AddServerSideBlazor();

// Add Quartz services
services.AddQuartz(q =>
{
#pragma warning disable CS0618 // Type or member is obsolete
    q.UseMicrosoftDependencyInjectionJobFactory();
#pragma warning restore CS0618 // Type or member is obsolete

    // Create a "key" for the job
    var jobKey = new JobKey("SyncJob");

    // Register the job with the DI container
    q.AddJob<SynchronizationJob>(opts => opts.WithIdentity(jobKey));

    // Create a trigger for the job
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("SyncJob-trigger")
        .WithCronSchedule("0 0 * * * ?")); // Runs every hour
});

services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();
var env = app.Environment;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("/_Host");

app.Run();
