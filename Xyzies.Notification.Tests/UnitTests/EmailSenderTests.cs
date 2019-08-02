﻿using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xyzies.Notification.Data.Entity;
using Xyzies.Notification.Data.Repository.Behaviour;
using Xyzies.Notification.Services.Common.Interfaces;
using Xyzies.Notification.Services.Helpers;
using Xyzies.Notification.Services.Models;
using Xyzies.Notification.Services.Services;
using Xyzies.Notification.Tests.Common;
using FluentAssertions;
using System.Net;
using SendGrid.Helpers.Mail;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace Xyzies.Notification.Tests.UnitTests
{
    public class EmailSenderTests : BaseTest
    {
        private Mock<ISendGridClient> _sendGridMock;
        private readonly IMailerService _mailerService = null;
        private ILogger<MailerService> _loggerMock;
        private Mock<ILogRepository> _loggerDbMock;
        private Mock<IMessageTemplateRepository> _messageRepositoryMock;
        private Mock<IOptionsMonitor<MailerOptions>> _mailerOptionsMonitorMock;

        public EmailSenderTests()
        {
            _loggerMock = Mock.Of<ILogger<MailerService>>();
            _loggerDbMock = new Mock<ILogRepository>();
            _messageRepositoryMock = new Mock<IMessageTemplateRepository>();
            _mailerOptionsMonitorMock = new Mock<IOptionsMonitor<MailerOptions>>();
            string MailTo = Fixture.Create<string>();
            var To = Fixture.Build<string>()
                .CreateMany(10).ToList();
            string From = Fixture.Create<string>();

            Mock<SendGridClientOptions> mock= new Mock<SendGridClientOptions>();

            _sendGridMock = new Mock<ISendGridClient>();
            //_sendGridMock.Object.op
            _mailerOptionsMonitorMock.Setup(x => x.CurrentValue).Returns(new MailerOptions()
            {
                From = From,
                MailTo = MailTo,
                To = To
            });

            _mailerService = new MailerService(_mailerOptionsMonitorMock.Object, _messageRepositoryMock.Object, _loggerMock, _loggerDbMock.Object, _sendGridMock.Object);
        }

        [Fact]
        public async Task ShouldReturnDictionaryWithParams()
        {
            // Arrange
            string UDID = Fixture.Create<string>();
            string Address = Fixture.Create<string>();
            string Town = Fixture.Create<string>();
            string PostCode = Fixture.Create<string>();
            string Country = Fixture.Create<string>();
            string Notes = Fixture.Create<string>();
            DateTime LastHeartBeat = Fixture.Create<DateTime>();
            DateTime PreviousHeartBeat = Fixture.Create<DateTime>();
            string Cause = Fixture.Create<string>();
            string MailTo = Fixture.Create<string>();

            EmailParameters emailParams = Fixture.Build<EmailParameters>()
                .With(x => x.Address, Address)
                .With(x => x.Cause, Cause)
                .With(x => x.Town, Town)
                .With(x => x.PostCode, PostCode)
                .With(x => x.Country, Country)
                .With(x => x.Notes, Notes)
                .With(x => x.MailTo, MailTo)
                .With(x => x.LastHeartBeat, LastHeartBeat)
                .With(x => x.UDID, UDID)
                .With(x => x.PreviousHeartBeat, PreviousHeartBeat)
                .Without(x => x.EmailsTo)
                .Create();

            Dictionary<string, string> expected = new Dictionary<string, string>{
                { "udid", UDID},
                { "address", Address},
                { "town", Town},
                { "postcode", PostCode},
                { "country", Country},
                { "notes", Notes},
                { "lastheartbeat", LastHeartBeat.ToString()},
                { "cause", Cause},
                { "mailto", MailTo},
                { "previousheartbeat", PreviousHeartBeat.ToString()},
                { "emailsto", null}
            };

            // Act
            var actual = MailerParser.PrepareDictionaryParams(emailParams);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ShouldReturnParsedStringWithParams()
        {
            // Arrange
            string template = "Tedsf {test1}, sdfaf{test2}";
            string test1 = Fixture.Create<string>();
            string test2 = Fixture.Create<string>();

            string expected = $"Tedsf {test1}, sdfaf{test2}";

            Dictionary<string, string> dict = new Dictionary<string, string>{
                { "test1", test1},
                { "test2", test2}
            };

            // Act
            var actual = MailerParser.ProcessTemplate(template, dict);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ShouldReturnPrepareEmailWithParams()
        {
            // Arrange
            string UDID = Fixture.Create<string>();
            string Address = Fixture.Create<string>();
            string Town = Fixture.Create<string>();
            string PostCode = Fixture.Create<string>();
            string Country = Fixture.Create<string>();
            string Notes = Fixture.Create<string>();
            DateTime LastHeartBeat = Fixture.Create<DateTime>();
            string Cause = Fixture.Create<string>();
            string EmailFrom = Fixture.Create<string>();
            string EmailTo = Fixture.Create<string>();
            string Subject = Fixture.Create<string>();
            string htmlContent = Fixture.Create<string>();
            string cause = Fixture.Create<string>();
            Guid idTempl = Fixture.Create<Guid>();
            string MailTo = Fixture.Create<string>();

            EmailParameters emailParams = Fixture.Build<EmailParameters>()
                .With(x => x.Address, Address)
                .With(x => x.Cause, Cause)
                .With(x => x.Town, Town)
                .With(x => x.PostCode, PostCode)
                .With(x => x.Country, Country)
                .With(x => x.Notes, Notes)
                .With(x => x.MailTo, MailTo)
                .With(x => x.LastHeartBeat, LastHeartBeat)
                .With(x => x.UDID, UDID)
                .Without(x => x.EmailsTo)
                .Create();

            MessageTemplate template = Fixture.Build<MessageTemplate>()
                .With(x => x.Cause, cause)
                .With(x => x.Id, idTempl)
                .With(x => x.Subject, Subject)
                .With(x => x.MessageBody, htmlContent)
                .Without(x => x.TypeOfMessages)
                .Without(x => x.TypeOfMessageId)
                .Create();

            List<MessageTemplate> listTemp = new List<MessageTemplate>();
            listTemp.Add(template);

            _messageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<MessageTemplate, bool>>>())).Returns(Task.FromResult(listTemp.AsQueryable()));

            // Act
            var actual = await _mailerService.PrepareEmail(emailParams);

            //Assert
            actual.HtmlContent.Should().Be(htmlContent);
            actual.Subject.Should().Be(Subject);
            actual.From.Email.Should().Be(_mailerOptionsMonitorMock.Object.CurrentValue.From);
        }

        [Fact]
        public async Task ShouldReturnArgumentNullException()
        {
            // Arrange

            EmailParameters emailParams = Fixture.Build<EmailParameters>()
                .Without(x => x.EmailsTo)
                .Create();

            // Act
            Func<Task> actual = async () => await _mailerService.PrepareEmail(emailParams);

            //Assert
            await actual.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ShouldThrowExceptionSendEmailToSendGrid()
        {
            // Arrange
            string UDID = Fixture.Create<string>();
            string Address = Fixture.Create<string>();
            string Town = Fixture.Create<string>();
            string PostCode = Fixture.Create<string>();
            string Country = Fixture.Create<string>();
            string Notes = Fixture.Create<string>();
            DateTime LastHeartBeat = Fixture.Create<DateTime>();
            DateTime PreviousHeartBeat = Fixture.Create<DateTime>();
            string Cause = Fixture.Create<string>();
            string MailTo = Fixture.Create<string>();

            EmailParameters emailParams = Fixture.Build<EmailParameters>()
                .With(x => x.Address, Address)
                .With(x => x.Cause, Cause)
                .With(x => x.Town, Town)
                .With(x => x.PostCode, PostCode)
                .With(x => x.Country, Country)
                .With(x => x.Notes, Notes)
                .With(x => x.MailTo, MailTo)
                .With(x => x.LastHeartBeat, LastHeartBeat)
                .With(x => x.UDID, UDID)
                .With(x => x.PreviousHeartBeat, PreviousHeartBeat)
                .Without(x => x.EmailsTo)
                .Create();

            // Act

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _mailerService.SendMail(emailParams));
        }
    }
}
