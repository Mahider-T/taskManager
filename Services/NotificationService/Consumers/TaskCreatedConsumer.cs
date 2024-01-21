using Events;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;
public class AnimalCreatedConsumer : IConsumer<TaskCreated>
{
    
    public async Task Consume(ConsumeContext<AnimalCreated> taskCreated)
    {
        Console.WriteLine("Consuming task created " + taskCreated.Message.Id);

        await Tasks.SaveAsync();
    }
}