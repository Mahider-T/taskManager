using Events;
using MassTransit;
using MongoDB.Entities;
using NotificationService.Helpers;

namespace NotificationService.Consumers;
public class TaskUpdatedConsumer : IConsumer<TaskUpdated>
{
    
    public async Task Consume(ConsumeContext<TaskUpdated> taskUpdated)
    {
        // Console.WriteLine(taskUpdated == null);
        var email = taskUpdated.Message.userId;
        var oldName = taskUpdated.Message.oldName;
        var oldStatus = taskUpdated.Message.oldStatus;
        var oldDueDate = $"{taskUpdated.Message.oldDueDate.ToString("dd/MM/yyyy")} at {taskUpdated.Message.oldDueDate.TimeOfDay}";
        var nameOfUser = taskUpdated.Message.nameOfUser;
        var createdAt = $"{taskUpdated.Message.createdAt.ToString("dd/MM/yyyy")} at {taskUpdated.Message.createdAt.TimeOfDay}";

        var newName = taskUpdated.Message.newName;
        var newDueDate = $"{taskUpdated.Message.newDueDate.ToString("dd/MM/yyyy")} at {taskUpdated.Message.newDueDate.TimeOfDay}";
        var newStatus = taskUpdated.Message.newStatus;


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
                    <p>You just updated a task.</p></br>
                    <p>You can find the updated task details below</p>
                    <p><span class='task-name'>Task name</span>: {oldName} -> {newName}</p>
                    <p class='status'><span class='task-name'>Status</span>: {oldStatus} -> {newStatus}</p>
                    <p class='created-at'><span class='task-name'>Created at</span>: {createdAt}</p>
                    <p class='due-date'><span class='task-name'>Due date</span>: {oldDueDate} -> {newDueDate}</p>
                    <div class='email-footer'>
                        <p>With regards,</p>
                        <p>Task Manager Corp.</p>
                    </div>
                </div>
            </body>
            </html>";


        
        try{
            var sub = "Task updated successfully." ;
            await SendEmail.SendEmailMethod(email, sub, body);
        }
        catch(Exception e){
            Console.WriteLine(e);

        }
        // await taskUpdated.SaveAsync();
    }
}