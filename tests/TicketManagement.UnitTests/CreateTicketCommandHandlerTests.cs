using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using FluentAssertions;
using TicketManagementSystem.Application.Features.Tickets.Commands;
using TicketManagementSystem.Application.Features.Tickets.Queries;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Entities;
using TicketManagementSystem.Domain.Enums;
using System.Timers;

namespace TicketManagement.UnitTests.Application.Features.Tickets
{
    public class CreateTicketCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateTicketCommandHandler _handler;

        public CreateTicketCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new CreateTicketCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateTicket_WhenValidCommand()
        {
            // Arrange
            var command = new CreateTicketCommand
            {
                Description = "Test Ticket",
                Date = DateTime.Now
            };

            int expectedId = 1;
            _mockUnitOfWork.Setup(uow => uow.Tickets.AddAsync(It.IsAny<Ticket>()))
                .Callback<Ticket>(ticket => ticket.Id = expectedId)
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(expectedId);
            _mockUnitOfWork.Verify(uow => uow.Tickets.AddAsync(It.IsAny<Ticket>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    public class GetTicketListQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetTicketListQueryHandler _handler;

        public GetTicketListQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetTicketListQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTicketList_WhenValidQuery()
        {
            // Arrange
            var query = new GetTicketListQuery { PageNumber = 1, PageSize = 10 };
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Description = "Test Ticket 1", Status = TicketStatus.Open, Date = DateTime.Now },
                new Ticket { Id = 2, Description = "Test Ticket 2", Status = TicketStatus.Closed, Date = DateTime.Now.AddDays(-1) }
            };
            var ticketDtos = new List<TicketDto>
            {
                new TicketDto { Id = 1, Description = "Test Ticket 1", Status = "Open", Date = DateTime.Now },
                new TicketDto { Id = 2, Description = "Test Ticket 2", Status = "Closed", Date = DateTime.Now.AddDays(-1) }
            };
            int totalCount = 5;

            _mockUnitOfWork.Setup(uow => uow.Tickets.GetPagedResponseAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((tickets, totalCount));
            _mockMapper.Setup(m => m.Map<List<TicketDto>>(It.IsAny<List<Ticket>>()))
                .Returns(ticketDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Tickets.Should().HaveCount(2);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalCount.Should().Be(totalCount);

            _mockUnitOfWork.Verify(uow => uow.Tickets.GetPagedResponseAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _mockMapper.Verify(m => m.Map<List<TicketDto>>(It.IsAny<List<Ticket>>()), Times.Once);
        }
    }
}