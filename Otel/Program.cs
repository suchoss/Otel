using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Otel.Services;
using Elastic.Apm.NetCoreAll;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHostedService<MyBackgroundService>();

builder.Services.AddHostedService<MyHostedService>();

//telemetry
builder.Services.AddOpenTelemetry()
    .WithMetrics(meterProviderBuilder =>
    {
        meterProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HlidacAdminTrace"));
        meterProviderBuilder.AddHttpClientInstrumentation();
        meterProviderBuilder.AddAspNetCoreInstrumentation();
        meterProviderBuilder.AddRuntimeInstrumentation();
        meterProviderBuilder.AddMeter("AdminMetrics");
        meterProviderBuilder.AddOtlpExporter(options => options.Endpoint = new Uri("http://10.10.150.199:8200"));
    })
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HlidacAdminTrace"));
        tracerProviderBuilder.AddHttpClientInstrumentation();
        tracerProviderBuilder.AddAspNetCoreInstrumentation();
        tracerProviderBuilder.AddSqlClientInstrumentation();
        tracerProviderBuilder.AddElasticsearchClientInstrumentation();
        tracerProviderBuilder.AddEntityFrameworkCoreInstrumentation();
        tracerProviderBuilder.AddSource("AdminTracing");
        tracerProviderBuilder.AddOtlpExporter(options => options.Endpoint = new Uri("http://10.10.150.199:8200"));
    });

// telemetry logging
builder.Logging.AddOpenTelemetry(loggerOptions =>
{
    loggerOptions.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HlidacAdminLogs"));
    loggerOptions.IncludeFormattedMessage = true;
    loggerOptions.IncludeScopes = true;
    loggerOptions.ParseStateValues = true;
    loggerOptions.AddOtlpExporter(options =>
    {
        options.Endpoint = new Uri("http://10.10.150.199:8200");
    });
});


var app = builder.Build();

app.UseAllElasticApm(builder.Configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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