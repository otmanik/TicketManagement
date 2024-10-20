using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Entities;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketManagementDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Ticket> Tickets, int TotalCount)> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Tickets.CountAsync();
            var tickets = await _context.Tickets
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (tickets, totalCount);
        }

    }
}