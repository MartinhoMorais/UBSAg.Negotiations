using Moq;
using UBSAg.Negotiations.Application.Commands;
using UBSAg.Negotiations.Application.Handlers;
using UBSAg.Negotiations.Application.Requests;
using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Interfaces;
using UBSAg.Negotiations.Domain.Repositories;

namespace UBSAg.Negotiations.UnitTests
{
    public class TradeHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITradeRepository> _tradeRepositoryMock;
        private readonly Mock<ICategorizerTrades> _categorizerTradesMock;
        private readonly TradeHandler tradeHandler;

        public TradeHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _tradeRepositoryMock= new Mock<ITradeRepository>();
            _categorizerTradesMock= new Mock<ICategorizerTrades>();
            tradeHandler = new TradeHandler(
                _unitOfWorkMock.Object,
                _tradeRepositoryMock.Object,
                _categorizerTradesMock.Object);
        }

        [Fact]
        public async Task Handle_ValidTrades_ShouldCategorizeAndSaveAndCommit()
        {
            // Arrange
            var tradeRequests = new List<TradeRequest>
            {
                new(2000000, "Private"),
                new(400000, "Public")
            };
            var command = new TradeCommand(tradeRequests);
            var tradeCategories = new List<string?> { "HIGHRISK", "LOWRISK" };

            _categorizerTradesMock.Setup(c => c.CategorizeTradesByRisk(It.IsAny<List<Trade>>()))
                                  .ReturnsAsync(tradeCategories);

            // Act
            var result = await tradeHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(tradeCategories, result);

            _categorizerTradesMock.Verify(c => c.CategorizeTradesByRisk(It.Is<List<Trade>>(
                list => list.Count == 2 && list.First().Value == 2000000)), Times.Once);

            _unitOfWorkMock.Verify(uow => uow.BeginAsync(), Times.Once);
            _tradeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Trade>()), Times.Exactly(2));
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_OnRepositoryFailure_ShouldRollbackAndThrowException()
        {
            // Arrange
            var tradeRequests = new List<TradeRequest>
            {
                new(2000000, "Private")
            };
            var command = new TradeCommand(tradeRequests);

            _tradeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Trade>()))
                .ThrowsAsync(new InvalidOperationException("Erro de banco de dados simulado."));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => tradeHandler.Handle(command, CancellationToken.None));

            _unitOfWorkMock.Verify(uow => uow.BeginAsync(), Times.Once);
            _tradeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Trade>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.Rollback(), Times.Once);
        }
    }
}
