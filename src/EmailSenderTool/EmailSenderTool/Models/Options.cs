using CommandLine;
using System.Collections.Generic;

namespace EmailSenderTool.Models
{
    public class Options
    {
        /// <summary>
        /// Gets or sets a value indicating the email address of the sender
        /// </summary>
        [Option(
            shortName: 'm',
            longName: "mail-address",
            HelpText = "Represents the email address of the sender",
            Required = true)]
        public string Mail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the display name associated to the sender email address
        /// </summary>
        [Option(
            shortName: 'd',
            longName: "display-name",
            HelpText = "Represents the name to be displayed associated to the email address of the sender",
            Required = true)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the password for the SMTP client
        /// </summary>
        [Option(
            shortName: 'p',
            longName: "password",
            HelpText = "Represents the password (in clear text) of the SMTP client",
            Required = true)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the hostname of the SMTP client
        /// </summary>
        [Option(
            longName: "host",
            HelpText = "Represents the name of the host of the SMTP client",
            Required = true)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port to be used by the SMTP service client
        /// </summary>
        [Option(
            longName: "port",
            HelpText = "Represents the port of the host for the SMTP client",
            Required = true)]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets a list of email addresses of the receivers
        /// </summary>
        [Option(
            shortName: 't',
            longName: "to",
            HelpText = "Represents a list of email addresses (separated by ',') of the receivers of the email",
            Required = true, 
            Default = null,
            Separator = ',')]
        public ICollection<string> ToEmail { get; set; } = new HashSet<string>();

        /// <summary>
        /// Gets or sets the subject of the email
        /// </summary>
        [Option(
           shortName: 's',
           longName: "subject",
           HelpText = "Represents the subject of the email",
           Required = true)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the content of the email (if the email is in clear text) or the path where to find the HTML file to be sent (otherwise, if the property <see cref="MailFromFile"/> is set)
        /// </summary>
        [Option(
            shortName: 'b',
            longName: "body",
            HelpText = "Represents the content of the email, if the email is in clear text, or the path where to find the HTML file containing the email otherwise",
            Required = true)]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a list of paths where to find attachments (if any)
        /// </summary>
        [Option(
            shortName: 'a',
            longName: "attachments",
            HelpText = "Represents a list of paths (separated by ',') where to find attachments to add to the email",
            Required = false, 
            Default = null, 
            Separator = ',')]
        public ICollection<string> Attachments { get; set; } = new HashSet<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the email content is stored in an HTML file
        /// </summary>
        [Option(
            longName: "from-file", 
            HelpText = "If set indicates that the content of the body is stored in an HTML file",
            Required = false)]
        public bool MailFromFile { get; set; }
    }
}