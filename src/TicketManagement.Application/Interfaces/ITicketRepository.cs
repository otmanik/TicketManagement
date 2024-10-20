using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Application.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        public Task<IEnumerable<Ticket>> GetPagedReponseAsync(int pageNumber, int pageSize);
    }
}