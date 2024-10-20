using System;
using TicketManagementSystem.Domain.Enums;

namespace TicketManagementSystem.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime Date { get; set; }
    }
}