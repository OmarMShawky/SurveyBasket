using Serilog;
using SurveyBasket;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("Polls", o => o
        .Cache()
        .Expire(TimeSpan.FromSeconds(60))
        .Tag("availableQuestions"));
});

builder.Services.AddOutputCache(outputCache =>
{
    outputCache.AddPolicy("Questions", policy =>
    policy.Cache().Expire(TimeSpan.FromHours(1)).Tag("newQuestions"));
});

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}
app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

//app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
