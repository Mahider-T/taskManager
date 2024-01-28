using Events;
using MassTransit;
using MongoDB.Entities;
using NotificationService.Helpers;

namespace NotificationService.Consumers;
public class TaskUpdatedConsumer : IConsumer<TaskUpdated>
{
    
    public async Task Consume(ConsumeContext<TaskUpdated> taskUpdated)
    {
        // Console.WriteLine(taskCreated == null);
        var email = taskCreated.Message.userId;
        var name = taskCreated.Message.name;
        var id = taskCreated.Message.Id;
        var status = taskCreated.Message.Status;
        var dueDate = $"{taskCreated.Message.dueDate.ToString("dd/MM/yyyy")} at {taskCreated.Message.dueDate.TimeOfDay}";
        var nameOfUser = taskCreated.Message.nameOfUser;
        var createdAt = $"{taskCreated.Message.createdAt.ToString("dd/MM/yyyy")} at {taskCreated.Message.createdAt.TimeOfDay}";
        // var body = $"<h3>Greetings, </h3></br> You just created a task: </br> Task name : <b><u>{name}</u></b> </br> and due date {dueDate}"; 
        var body = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: 'Arial', sans-serif;
                        color: #333333;
                    }}
                    .email-container {{
                        max-width: 600px;
                        margin: auto;
                        padding: 20px;
                        border: 1px solid #dddddd;
                        border-radius: 10px;
                        background-color: #f9f9f9;
                    }}
                    .email-header {{
                        font-size: 18px;
                        color: #444444;
                    }}
                    .task-name {{
                        font-weight: bold;
                        text-decoration: underline;
                        color: #555555;
                    }}
                    .due-date {{
                        color: #555555;
                    }}
                    .created-at {{
                        color : #555555
                    }}
                    .status {{
                        color : #555555
                    }}
                    .email-footer {{
                        margin-top: 20px;
                        padding-top: 20px;
                        border-top: 1px solid #eeeeee;
                        font-size: 14px;
                        text-align: center;
                        color: #777777;
                    }}
                </style>
            </head>
            <body>
                <div class='email-container'>
                    <p class='email-header'>Greetings, {nameOfUser} </p>
                    <p>You just created a task.</p>
                    <p><span class='task-name'>Task name</span>: {name}</p>
                    <p class='status'><span class='task-name'>Status</span>: {status}</p>
                    <p class='created-at'><span class='task-name'>Created at</span>: {createdAt}</p>
                    <p class='due-date'><span class='task-name'>Due date</span>: {dueDate}</p>
                    <div class='email-footer'>
                        <p>With regards,</p>
                        <p>Task Manager Corp.</p>
                    </div>
                </div>
            </body>
            </html>";


        
        try{
            var sub = "Task updated successfully." ;
            SendEmail.SendEmailMethod(email, sub, body);
        }
        catch(Exception e){
            Console.WriteLine(e);

        }
        // await taskCreated.SaveAsync();
    }
}