
using System.Threading;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Infrastructure.Repositories;

namespace TicketManagementSystem.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketManagementDbContext _context;

        public UnitOfWork(TicketManagementDbContext context)
        {
            _context = context;
            Tickets = new TicketRepository(_context);
        }

        public ITicketRepository Tickets { get; private set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
