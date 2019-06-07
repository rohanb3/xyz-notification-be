using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using Xyzies.Notification.Services.Common.Interfaces;
using Xyzies.Notification.Services.Helpers;
using Xyzies.Notification.Services.Models;
using System.Collections.Generic;
using Xyzies.Notification.Data.Repository.Behaviour;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Xyzies.Notification.Services.Services
{
    public class MailerService : IMailerService
    {
        private readonly SendGridClient _client;
        private readonly IMessageTemplateRepository _messagetTemplateRepository;
        private readonly ILogger<MailerService> _logger = null;
        private readonly string _from;
        private readonly List<string> _to;
        private readonly string _toMail;



        public MailerService(IOptionsMonitor<MailerOptions> mailerOptionsMonitor,
            IMessageTemplateRepository messagetTemplateRepository,
            ILogger<MailerService> logger)
        {
            var options = mailerOptionsMonitor.CurrentValue ??
                throw new ArgumentNullException(nameof(mailerOptionsMonitor));

            _client = new SendGridClient(options.ApiKey);

            _to = options.To;
            _from = options.From;
            _toMail = options.MailTo;

            _messagetTemplateRepository = messagetTemplateRepository ??
                throw new ArgumentNullException(nameof(messagetTemplateRepository));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
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

            _logger.LogInformation("Email Sent for DeviceID:{model.UDID}, Status: {response.StatusCode}", model.UDID, response.StatusCode.ToString());
            return response;
        }

        public async Task<MailSendingModel> PrepareEmail(EmailParametersModel emailParams)
        {
            MailSendingModel email = new MailSendingModel();

            emailParams.MailTo = _toMail;

            Dictionary<string, string> parameters = MailerParcer.PrepareDictionaryParams(emailParams);

            var emailtemplate = (await _messagetTemplateRepository.GetAsync(x=>x.Id == Guid.Parse("58a6c1d8-fa41-4b0c-9c11-f6c20429bda7"))).FirstOrDefault();

            email.Subject = MailerParcer.ProcessTemplate(emailtemplate.Subject, parameters);
            email.From = new EmailAddress(_from);
            email.To.AddRange(_to.Select(x => new EmailAddress(x)));
            email.HtmlContent = MailerParcer.ProcessTemplate(emailtemplate.MessageBody, parameters);

            return email;
        }
    }
}
