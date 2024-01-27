using Events;
using MassTransit;
using MongoDB.Entities;

namespace NotificationService.Consumers;
public class TaskCreatedConsumer : IConsumer<TaskCreated>
{
    
    public async Task Consume(ConsumeContext<TaskCreated> taskCreated)
    {
        Console.WriteLine(taskCreated == null);
        Console.WriteLine("Consuming task created " + taskCreated.Message.Id);

        // await taskCreated.SaveAsync();
    }
}