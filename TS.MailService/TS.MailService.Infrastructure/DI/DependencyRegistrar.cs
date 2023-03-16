using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TS.MailService.Infrastructure.Abstraction.EmailSenders;
using TS.MailService.Infrastructure.Abstraction.Repository;
using TS.MailService.Infrastructure.EmailSenders;
using TS.MailService.Infrastructure.Entities;
using TS.MailService.Infrastructure.Repositories;

namespace TS.MailService.Infrastructure.DI
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddInfrastructureLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(new MongoClient(configuration["MongoDb:MongoDbConnectionString"]));
            services.AddTransient<IGenericRepository<EmailMessageEntity>, GenericRepository<EmailMessageEntity>>();

            services.Configure<SmtpEntity>(
                configuration.GetSection(SmtpEntity.Position));

            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}