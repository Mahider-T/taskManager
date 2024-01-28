using Events;
using MassTransit;
using MongoDB.Entities;

namespace NotificationService.Consumers;
public class TaskCreatedConsumer : IConsumer<TaskCreated>
{
    
    public async Task Consume(ConsumeContext<TaskCreated> taskCreated)
    {
        // Console.WriteLine(taskCreated == null);
        Console.WriteLine("Consuming task created with id " + taskCreated.Message.Id);
        Console.WriteLine("with name " + taskCreated.Message.name + " and email " + taskCreated.Message.userId);

        // await taskCreated.SaveAsync();
    }
}