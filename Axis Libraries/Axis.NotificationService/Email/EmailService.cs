using Axis.Constant;
using Axis.Helpers.Caching;
using Axis.Model.Setting;
using Axis.NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Axis.NotificationService.Email
{
    /// <summary>
    /// Notification service for sending email
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Sends the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public bool Send(EmailMessageModel email, out string error)
        {
            //Read application level settings
            var emailServerSettings = GetEmailSettings();
            var emailServer = ConvertToEmailServerSettings(emailServerSettings);
            if (emailServer != null)
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.UseDefaultCredentials = emailServer.UseDefaultCredentials;
                    smtpClient.Host = emailServer.Host;
                    smtpClient.Port = emailServer.Port;
                    smtpClient.EnableSsl = emailServer.EnableSsl;
                    if (emailServer.UseDefaultCredentials)
                        smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
                    else
                        smtpClient.Credentials = new NetworkCredential(emailServer.Username, emailServer.Password);

                    var message = ComposeMessage(email);
                    smtpClient.Send(message);
                }
                error = string.Empty;
            }
            else
            {
                error = "Missing/Incorrect application email settings";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Composes the message.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        private MailMessage ComposeMessage(EmailMessageModel email)
        {
            var message = new MailMessage();

            //Add sender address
            message.From = new MailAddress(email.From.EmailAddress, email.From.DisplayName);

            //Add recipient address
            email.To.ForEach(delegate(EmailAddressModel to)
            {
                message.To.Add(new MailAddress(to.EmailAddress, to.DisplayName));
            });

            //Add CC
            if (email.CC != null)
            {
                email.CC.ForEach(delegate(EmailAddressModel cc)
                {
                    message.CC.Add(new MailAddress(cc.EmailAddress, cc.DisplayName));
                });
            }

            //Add BCC
            if (email.BCC != null)
            {
                email.BCC.ForEach(delegate(EmailAddressModel bcc)
                {
                    message.Bcc.Add(new MailAddress(bcc.EmailAddress, bcc.DisplayName));
                });
            }

            message.Subject = email.Subject;
            message.Body = email.Body;
            message.IsBodyHtml = email.IsBodyHtml;

            //create attachment for this e-mail message
            if (email.Attachments != null)
            {
                email.Attachments.ForEach(delegate(EmailAttachmentModel emailAttachment)
                {
                    var attachment = new Attachment(new MemoryStream(emailAttachment.AttachmentContent), emailAttachment.AttachmentName);

                    if (!String.IsNullOrEmpty(emailAttachment.AttachmentName))
                    {
                        attachment.Name = emailAttachment.AttachmentName;
                    }
                    message.Attachments.Add(attachment);
                });
            }

            return message;
        }

        /// <summary>
        /// Converts to email server settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        private EmailServerModel ConvertToEmailServerSettings(List<SettingModel> settings)
        {
            try
            {
                var emailServerDetails = new EmailServerModel();
                if (settings != null && settings.Count > 0)
                {
                    emailServerDetails.Host = settings.FirstOrDefault(host => host.Settings == ApplicationEmailSettings.Host).Value;
                    emailServerDetails.Port = Convert.ToInt32(settings.FirstOrDefault(port => port.Settings == ApplicationEmailSettings.Port).Value);
                    emailServerDetails.EnableSsl = Convert.ToBoolean(settings.FirstOrDefault(enableSsl => enableSsl.Settings == ApplicationEmailSettings.EnableSsl).Value);
                    emailServerDetails.UseDefaultCredentials = Convert.ToBoolean(settings.FirstOrDefault(useDefaultCredentials => useDefaultCredentials.Settings == ApplicationEmailSettings.UseDefaultCredentials).Value);
                    emailServerDetails.Username = settings.FirstOrDefault(username => username.Settings == ApplicationEmailSettings.Username).Value;
                    emailServerDetails.Password = settings.FirstOrDefault(password => password.Settings == ApplicationEmailSettings.Password).Value;
                }

                return emailServerDetails;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all of the settings needed for the forgot password functionality
        /// </summary>
        /// <returns></returns>
        private List<SettingModel> GetEmailSettings()
        {
            SettingsCacheManager cacheManager = new SettingsCacheManager();
            List<SettingModel> settings = cacheManager.GetAllAppSettings();

            return settings;
        }
    }
}