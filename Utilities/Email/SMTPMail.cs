using GPT.Application.Contracts.Infrastructure.Utilities;
using GPT.Application.Model;
using System.Net.Mail;


namespace GPT.Utilities.Email
{
    public class SmtpMail : IMail
    {
        /*
          * Cliente SMTP
          * Gmail:  smtp.gmail.com  port:587
          * Hotmail: smtp.liva.com  port:25
          */

        /// <summary>
        /// The server
        /// </summary>
        private SmtpClient _client;


        public void Configure(MailModel mailModel)
        {
            _client = new SmtpClient(mailModel.SmtpServer, mailModel.SmtpPort)
            {
                Credentials = new System.Net.NetworkCredential(mailModel.SmtpFromEmail, mailModel.SmtpPassword),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                DeliveryFormat = SmtpDeliveryFormat.International
        };

        }

        public async Task SendMailAsync(MailMessage message)
        {
            await _client.SendMailAsync(message);
        }
    }
}
