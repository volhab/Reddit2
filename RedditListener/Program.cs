
using Microsoft.EntityFrameworkCore;
using RedditListener;
using RedditListener.Interfaces;
using RedditListener.Models;
using RedditListener.Services;
;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<RedditContext>(opt =>
{
    opt.UseInMemoryDatabase("RedditDatabase");
    if (builder.Environment.IsDevelopment())
    {
        opt.EnableSensitiveDataLogging();
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.Configure<RedditSettings>(builder.Configuration.GetSection("Reddit"));

builder.Services.AddScoped<IRedditService, RedditService>();
builder.Services.AddScoped<IDataAccessService, DataAccessService>();
builder.Services.AddHostedService<MainService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();