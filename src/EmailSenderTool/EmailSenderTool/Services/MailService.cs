using Microsoft.Extensions.Logging;
using EmailSenderTool.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Linq;
using System;

namespace EmailSenderTool.Services
{
    /// <summary>
    /// Represents the service that handles mail messages
    /// </summary>
    public class MailService : IMailService
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailService"/> class
        /// </summary>
        /// <param name="logger">The logger</param>
        public MailService(ILogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task SendEmailAsync(Options options)
        {
            // when asking to send an email given an HTML file, 
            // then we need to properly load the file given its path
            // to get the content, read it, then send it via email
            if (options.MailFromFile)
            {
                logger.LogInformation($"Reading content from file located in {options.Body}...");
                options.Body = await File.ReadAllTextAsync(options.Body, System.Text.Encoding.UTF8);
            }

            logger.LogInformation("Creating message...");

            var message = new MailMessage
            {
                From = new MailAddress(options.Mail, options.DisplayName),
                Subject = options.Subject,
                Body = options.Body,
                IsBodyHtml = true
            };

            logger.LogInformation("Adding recipients...");

            foreach (var toAddress in options.ToEmail)
            {
                logger.LogInformation($"Adding: {toAddress.Trim()}");
                message.To.Add(new MailAddress(toAddress.Trim()));
            }

            if (options.Attachments.Any(x => x.Length > 0))
            {
                logger.LogInformation("Adding attachments...");

                foreach (var file in options.Attachments)
                {
                    if (!File.Exists(file))
                        continue;

                    logger.LogInformation($"Adding: {file}");
                    message.Attachments.Add(new Attachment(file));
                }
            }

            logger.LogInformation("Preparing the SMTP client configuration...");

            // configure SMTP
            var smtp = new SmtpClient
            {
                Port = options.Port,
                Host = options.Host,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(options.Mail, options.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            // send email
            logger.LogInformation("Sending message...");

            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (SmtpFailedRecipientException failedRecipientException)
            {
                logger.LogError($"There has been an issue while sending the message to: {failedRecipientException.FailedRecipient}");
                logger.LogError($"Failed with exception: {failedRecipientException.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"There has been an issue while sending the message: {ex.Message}");
            }
        }
    }
}