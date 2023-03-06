using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using SharpCompress.Common;
using System.Net.Mail;
using System.Threading;
using TS.MailService.Domain.Models;
using TS.MailService.Infrastructure.Abstraction.EmailSenders;
using TS.MailService.Infrastructure.Entities;
using TS.MailService.Tests.InitializeModels;

namespace TS.MailService.Tests.IntegrationTests;

public class TestHttpClientFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    internal static MongoDbRunner? _runner;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);
        _runner.Import("EmailMicroservice", "EmailMessageEntity", @"C:\Users\Vlad\Desktop\Projects\Ticket\Marsh\TS.MailService\TS.MailService.Tests\InitializeData.json", true);

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(IMongoClient));

            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            var mongoClient = GetMongloClientWithData();

            services.AddTransient<IMongoClient>(provider => mongoClient);

            var emailSender = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailSender));

            if(emailSender != null) services.Remove(emailSender);

            services.AddTransient<IEmailSender>(provider => GetEmailSenderMock());
        });
    }

    private IEmailSender GetEmailSenderMock()
    {
        Mock<IEmailSender> emailSenderMock = new();

        MailMessage newMessage = new MailMessage();

        emailSenderMock.Setup(x => x.SendEmail(newMessage, CancellationToken.None));

        return emailSenderMock.Object;
    }

    private IMongoClient GetMongloClientWithData()
    {
        var mongoClient = new MongoClient(_runner.ConnectionString);

        var entity = InitializeData.GetEmailMessageEntityFromUser();
        entity.Id = InitializeData.UserEmailGuidForGetByIdTest;

        var collection = mongoClient.GetDatabase("EmailMicroservice").GetCollection<EmailMessageEntity>(typeof(EmailMessageEntity).Name);

        collection.InsertOne(entity);

        return mongoClient;
    }
}