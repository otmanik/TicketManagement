using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Infrastructure.Data;
using TicketManagementSystem.Infrastructure.Repositories;

namespace TicketManagementSystem.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITicketRepository, TicketRepository>();
        }
    }
}