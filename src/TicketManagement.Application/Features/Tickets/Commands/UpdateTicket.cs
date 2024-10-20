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
    public class UpdateTicketCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime Date { get; set; }
    }

    public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTicketCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.Id);

            if (ticket == null)
            {
                return false;
            }

            ticket.Description = request.Description;
            ticket.Status = request.Status;
            ticket.Date = request.Date;

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }

    public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommand>
    {
        public UpdateTicketCommandValidator()
        {
            RuleFor(v => v.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

            RuleFor(v => v.Status)
                .IsInEnum().WithMessage("Invalid status.");

            RuleFor(v => v.Date)
                .NotEmpty().WithMessage("Date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");
        }
    }
}