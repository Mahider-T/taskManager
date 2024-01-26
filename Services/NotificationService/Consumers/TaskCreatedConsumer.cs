using Events;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;
public class TaskCreatedConsumer : IConsumer<TaskCreated>
{
    
    public async Task Consume(ConsumeContext<TaskCreated> taskCreated)
    {
        Console.WriteLine("Consuming task created " + taskCreated.Message.Id);

        await taskCreated.SaveAsync();
    }
}