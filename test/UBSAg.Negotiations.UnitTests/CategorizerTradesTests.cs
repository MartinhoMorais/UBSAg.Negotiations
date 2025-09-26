using Moq;
using UBSAg.Negotiations.Application.Categories;
using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Interfaces;

namespace UBSAg.Negotiations.UnitTests
{
    public class CategorizerTradesTests
    {
        private readonly Mock<ICategory> _riskChainMockMock;
        private readonly CategorizerTrades categorizerTrades;

        public CategorizerTradesTests()
        {
            _riskChainMockMock= new Mock<ICategory>();
            categorizerTrades = new CategorizerTrades(_riskChainMockMock.Object);
        }

        [Fact]
        public async Task CategorizeTradeByRisk_ShouldCallHandleForEachTrade()
        {
            // Arrange
            var trades = new List<Trade>
            {
                new(2000000, "Private" ),
                new(400000, "Public" ),
                new(500000, "Public" ),
                new(3000000, "Public" ),
                new(1000, "NonExistent")
            };

            _riskChainMockMock.SetupSequence(c => c.Handle(It.IsAny<Trade>()))
                .Returns("HIGHRISK")
                .Returns("LOWRISK")
                .Returns("LOWRISK")
                .Returns("MEDIUMRISK")
                .Returns("");

            // Act
            var result = await categorizerTrades.CategorizeTradesByRisk(trades);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal("HIGHRISK", result[0]);
            Assert.Equal("LOWRISK", result[1]);
            Assert.Equal("LOWRISK", result[2]);
            Assert.Equal("MEDIUMRISK", result[3]);
            Assert.Equal("", result[4]);

            _riskChainMockMock.Verify(c => c.Handle(It.IsAny<Trade>()), Times.Exactly(5));
        }

        [Fact]
        public async Task CategorizeTradeByRisk_ShouldCallHandleAndReturnCategory()
        {
            // Arrange          
            var trade = new Trade(2000000, "Private");

            _riskChainMockMock.Setup(c => c.Handle(trade)).Returns("HIGHRISK");

            // Act            
            var result = await categorizerTrades.CategorizeTradeByRisk(trade);

            // Assert            
            Assert.Equal("HIGHRISK", result);
            _riskChainMockMock.Verify(c => c.Handle(trade), Times.Once());
        }
    }
}