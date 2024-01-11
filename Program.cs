using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using TaskManager.Helpers;
using TaskManager.Models;
using TaskManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.Configure<TaskManagerDatabaseSettings>(
    builder.Configuration.GetSection("TaskManagerDatabase"));

//Register Tasksservices
builder.Services.AddSingleton<TasksService>();
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new MappingProfile());
    }).CreateMapper());


// builder.Services.AddAutoMapper(typeof(MappingProfile));
MapperConfiguration config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});
IMapper mapper = config.CreateMapper();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

