using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<(IEnumerable<Ticket> Tickets, int TotalCount)> GetPagedResponseAsync(int pageNumber, int pageSize);
    }
}