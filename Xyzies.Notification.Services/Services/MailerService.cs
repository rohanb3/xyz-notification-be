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
using System.Linq.Expressions;
using Xyzies.Notification.Data.Entity;
using Xyzies.Notification.Data.Utils;

namespace Xyzies.Notification.Services.Services
{
    public class MailerService : IMailerService
    {
        private readonly SendGridClient _client;
        private readonly IMessageTemplateRepository _messagetTemplateRepository;
        private readonly ILogger<MailerService> _logger = null;
        private readonly ILogRepository _loggerDb = null;
        private readonly string _from;
        private readonly List<string> _to;
        private readonly string _toMail;

        public MailerService(IOptionsMonitor<MailerOptions> mailerOptionsMonitor,
            IMessageTemplateRepository messagetTemplateRepository,
            ILogger<MailerService> logger,
            ILogRepository loggerDb)
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

            _loggerDb = loggerDb ??
                throw new ArgumentNullException(nameof(loggerDb));
        }

        public async Task<Response> SendMail(EmailParameters model)
        {
            MailSendingModel emailmodel;
            try
            {
                emailmodel = await PrepareEmail(model);
            }
            catch (ArgumentNullException ex)
            {
                await AddLogInfo(string.Empty, ex.Message);
                throw new ArgumentNullException(ex.Message, ex);
            }

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(emailmodel.From, emailmodel.To, emailmodel.Subject, emailmodel.PlainTextContent, emailmodel.HtmlContent);
            var response = await _client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                await AddLogInfo(response.StatusCode.ToString(), $"Email send is failed for DeviceID:{model.UDID}");
                throw new ArgumentException(response.StatusCode.ToString());
            }

            await AddLogInfo(response.StatusCode.ToString(), $"Email Sent for DeviceID:{model.UDID}");
            
            return response;
        }

        public async Task<MailSendingModel> PrepareEmail(EmailParameters emailParams)
        {
            MailSendingModel email = new MailSendingModel();

            emailParams.MailTo = _toMail;

            Dictionary<string, string> parameters = MailerParser.PrepareDictionaryParams(emailParams);

            var emailtemplate = (await _messagetTemplateRepository.GetAsync(Filtering(emailParams.Cause)))
                .OrderByDescending(x => x.CreateOn)
                .FirstOrDefault();

            if (emailtemplate != null)
            {
                email.Subject = MailerParser.ProcessTemplate(emailtemplate.Subject, parameters);
                email.From = new EmailAddress(_from);
                email.To.AddRange(_to?.Select(x => new EmailAddress(x)) ?? new List<EmailAddress>());
               // email.To.AddRange(emailParams.EmailsTo?.Select(x => new EmailAddress(x)) ?? new List<EmailAddress>());
                email.HtmlContent = MailerParser.ProcessTemplate(emailtemplate.MessageBody, parameters);
                return email;
            }
            throw new ArgumentNullException("Message", "Email template not found");
        }

        private Expression<Func<MessageTemplate, bool>> Filtering(string Cause)
        {
            Expression<Func<MessageTemplate, bool>> expression = messagetemplate => messagetemplate.Cause == Cause;

            expression = expression.AND(messagetemplate => messagetemplate.TypeOfMessages.Type.ToLower() == "email");

            return expression;
        }

        private async Task AddLogInfo(string status, string message)
        {
            await _loggerDb.AddAsync(new Log
            {
                Message = message,
                CreateOn = DateTime.UtcNow,
                Status = status
            });
        }
    }
}
