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

        public async Task<IEnumerable<Ticket>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _context.Tickets
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}