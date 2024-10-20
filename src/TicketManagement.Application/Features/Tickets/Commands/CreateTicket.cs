using MediatR;
using FluentValidation;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Entities;
using TicketManagementSystem.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Features.Tickets.Commands
{
    public class CreateTicketCommand : IRequest<int>
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }

    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTicketCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = new Ticket
            {
                Description = request.Description,
                Date = request.Date,
                Status = TicketStatus.Open 
            };

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ticket.Id;
        }
    }

    public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketCommandValidator()
        {
            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

            RuleFor(v => v.Date)
                .NotEmpty().WithMessage("Date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");
        }
    }
}