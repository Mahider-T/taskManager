using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Services;

namespace NotificationService.Data;
public class DbInitializer
{


    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("Task", MongoClientSettings
                    .FromConnectionString("mongodb://localhost:27017"));

        // await DB.Index<Animal>()
        //     .Key(x => x.Type, KeyType.Text)
        //     .Key(x => x.Sex, KeyType.Text)
        //     .CreateAsync();

        var count = await DB.CountAsync<Tasks>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<TaskServiceHttpClient>();

        var tasks = await httpClient.GetTasksForNotificationDB();
        Console.WriteLine(tasks.Count + " returned from the tasks service");

        if (tasks.Count > 0) await DB.SaveAsync(tasks);
    }
}