using System;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITicketRepository Tickets { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}