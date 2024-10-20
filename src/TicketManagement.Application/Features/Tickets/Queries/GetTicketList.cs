using AutoMapper;
using MediatR;
using FluentValidation;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Features.Tickets.Queries
{
    public class GetTicketListQuery : IRequest<TicketListVm>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetTicketListQueryHandler : IRequestHandler<GetTicketListQuery, TicketListVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTicketListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TicketListVm> Handle(GetTicketListQuery request, CancellationToken cancellationToken)
        {
            var (tickets, totalCount) = await _unitOfWork.Tickets.GetPagedResponseAsync(request.PageNumber, request.PageSize);
            var ticketDtos = _mapper.Map<List<TicketDto>>(tickets);

            return new TicketListVm
            {
                Tickets = ticketDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }

    public class GetTicketListQueryValidator : AbstractValidator<GetTicketListQuery>
    {
        public GetTicketListQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }

    public class TicketListVm
    {
        public IList<TicketDto> Tickets { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

    public class TicketDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public System.DateTime Date { get; set; }
    }

    public class GetTicketListProfile : Profile
    {
        public GetTicketListProfile()
        {
            CreateMap<Ticket, TicketDto>();
        }
    }
}