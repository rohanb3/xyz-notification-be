using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using Xyzies.Notification.Services.Common.Interfaces;
using Xyzies.Notification.Services.Helpers;
using Xyzies.Notification.Services.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xyzies.Notification.Data.Repository.Behaviour;
using System.Linq;

namespace Xyzies.Notification.Services.Services
{
    public class MailerService : IMailerService
    {
        private readonly SendGridClient _client;
        private readonly IMessageTemplateRepository _messagetTemplateRepository;

        public MailerService(IOptionsMonitor<MailerOptions> mailerOptionsMonitor,
            IMessageTemplateRepository messagetTemplateRepository)
        {
            var options = mailerOptionsMonitor.CurrentValue ??
                throw new ArgumentNullException(nameof(mailerOptionsMonitor));
            _client = new SendGridClient(options.ApiKey);
            _messagetTemplateRepository = messagetTemplateRepository ??
                throw new ArgumentNullException(nameof(messagetTemplateRepository));
        }

        public async Task<Response> SendMail(EmailParametersModel model)
        {
            var emailmodel = await PrepareEmail(model);

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(emailmodel.From, emailmodel.To, emailmodel.Subject, emailmodel.PlainTextContent, emailmodel.HtmlContent);
            var response = await _client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new ApplicationException(response.StatusCode.ToString());
            }

            return response;
        }

        public async Task<MailSendingModel> PrepareEmail(EmailParametersModel emailParams)
        {
            MailSendingModel email = new MailSendingModel();

            Dictionary<string, string> parameters = MailerParcer.PrepareDictionaryParams(emailParams);
            var emailtemplate = await _messagetTemplateRepository.GetAsync();

            //email.Subject = MailerParcer.ProcessTemplate(subjtempl , MailerParcer.PrepareDictionaryParams(emailParams));
            //email.From = new EmailAddress(dbmailfrom);
            //email.To.Add(new EmailAddress(dbmailTo)); 
            //email.HtmlContent = MailerParcer.ProcessTemplate(bodyEmail, MailerParcer.PrepareDictionaryParams(emailParams));

            return email;
        }
    }
}
