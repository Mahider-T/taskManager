using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace NotificationService.Helpers;

public class SendEmail{
    public static async Task<bool> SendEmailMethod(string email, string subject, string messageBody) {
        await Task.Run(() => {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TaskManagerTeam", "mahdertekola@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("html"){Text = messageBody};

         using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate("noreply.TaskManager1@gmail.com", "zfxa tlkz vzcd sius ");
            client.Send(message);
            client.Disconnect(true);
        }
        });
        return true;
    }
}
