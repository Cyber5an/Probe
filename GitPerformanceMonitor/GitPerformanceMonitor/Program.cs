using GitPerformanceMonitor;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<IOrchestrator, Orchestrator>();
builder.Services.AddSingleton(builder.Configuration.Get<AppSettings>());

builder.Services.AddSwaggerGen(swagger =>
{
	swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Command Orchestrator", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Command Orchestrator"));
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

//app.UseStaticFiles();

app.UseRouting();

//app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");
app.MapControllers();

app.Run();
