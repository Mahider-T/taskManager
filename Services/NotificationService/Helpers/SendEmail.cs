using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace NotificationService.Helpers;

public class SendEmail{
    public static void  SendEmailMethod(string email, string subject, string messageBody) {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("TaskManagerTeam", "mahdertekola@gmail.com"));
        message.Subject = subject;
        message.Body = new TextPart("plain"){Text = messageBody};

         using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate("mahdertekola@gmail.com", "");
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
