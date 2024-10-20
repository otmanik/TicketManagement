using MediatR;
using FluentValidation;
using TicketManagementSystem.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Features.Tickets.Commands
{
    public class DeleteTicketCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTicketCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.Id);

            if (ticket == null)
            {
                return false;
            }

            await _unitOfWork.Tickets.DeleteAsync(ticket);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }

    public class DeleteTicketCommandValidator : AbstractValidator<DeleteTicketCommand>
    {
        public DeleteTicketCommandValidator()
        {
            RuleFor(v => v.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}