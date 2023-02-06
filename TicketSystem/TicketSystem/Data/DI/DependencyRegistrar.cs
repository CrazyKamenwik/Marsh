﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicketSystem.Data.Repositories;

namespace TicketSystem.Data.DI
{
    public static class DependencyRegistrar
    {
        public static void AddDataAccessLevelServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DbConnection")));
            serviceCollection.AddTransient<ITicketRepository, TicketRepository>();
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
        }
    }
}
