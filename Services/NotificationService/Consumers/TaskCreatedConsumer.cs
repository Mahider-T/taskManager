using Events;
using MassTransit;
using MongoDB.Entities;
using NotificationService.Helpers;

namespace NotificationService.Consumers;
public class TaskCreatedConsumer : IConsumer<TaskCreated>
{
    
    public async Task Consume(ConsumeContext<TaskCreated> taskCreated)
    {
        // Console.WriteLine(taskCreated == null);
        Console.WriteLine("Consuming task created with id " + taskCreated.Message.Id);
        Console.WriteLine("with name " + taskCreated.Message.name + " and email " + taskCreated.Message.userId);
        try{
            await SendEmailMethod("maider3991@gmail.com", "You created a task", taskCreated.Message.name);
        }
        catch(Exception e){
            Console.WriteLine(e);

        }
        // await taskCreated.SaveAsync();
    }
}