using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using TaskManager.Helpers;
using TaskManager.Models;
using TaskManager.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Add services to the container.
builder.Services.Configure<TaskManagerDatabaseSettings>(
    builder.Configuration.GetSection("TaskManagerDatabase"));

//Register Tasksservices
builder.Services.AddSingleton<TasksService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TokenService>();

builder.Services.AddMassTransit(x =>
{
    // Add outbox
    // x.AddEntityFrameworkOutbox<AnimalDbContext>(o =>
    // {
    //     o.QueryDelay = TimeSpan.FromSeconds(10);

    //     o.UseSqlServer();
    //     o.UseBusOutbox();
    // });

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("animal", false));

    // Setup RabbitMQ Endpoint
    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "AuthToken"; // Set to your cookie name
    options.ExpireTimeSpan = TimeSpan.FromHours(1); // Set to match your token expiration
});


builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new MappingProfile());
    }).CreateMapper());


// builder.Services.AddAutoMapper(typeof(MappingProfile));
// MapperConfiguration config = new MapperConfiguration(cfg =>
// {
//     cfg.AddProfile(new MappingProfile());
// });
// IMapper mapper = config.CreateMapper();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<TokenValidationMiddleware>("AuthToken", "Canthisnotbemysecretkeynowitislongenoughpleasesirthisisalot.");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


