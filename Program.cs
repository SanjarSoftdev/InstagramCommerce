using InstagramCommerce.Components;
using InstagramCommerce.Data;
using InstagramCommerce.Jobs;
using InstagramCommerce.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Configuration = builder.Configuration;
var services = builder.Services;

services.AddDbContext<InstagramCommerceContext>();

services.AddScoped<InstagramService>();
services.AddScoped<OrderService>();
services.AddScoped<SynchronizationService>();
services.AddScoped<TelegramBotService>();
services.AddHttpClient();
services.AddControllers();
//services.AddRazorPages();
//services.AddServerSideBlazor();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
