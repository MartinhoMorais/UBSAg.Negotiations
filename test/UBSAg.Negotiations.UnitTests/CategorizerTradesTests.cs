using Moq;
using UBSAg.Negotiations.Application.Categories;
using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Interfaces;

namespace UBSAg.Negotiations.UnitTests
{
    public class CategorizerTradesTests
    {
        private readonly Mock<ICategory> _riskChainMock;

        public CategorizerTradesTests()
        {
            _riskChainMock= new Mock<ICategory>();
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

            _riskChainMock.SetupSequence(c => c.Handle(It.IsAny<Trade>()))
                         .Returns("HIGHRISK")
                         .Returns("LOWRISK")
                         .Returns("LOWRISK")
                         .Returns("MEDIUMRISK")
                         .Returns("");
            
            var categorizerTrades = new CategorizerTrades(_riskChainMock.Object);

            // Act
            var result = await categorizerTrades.CategorizeTradeByRisk(trades);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal("HIGHRISK", result[0]);
            Assert.Equal("LOWRISK", result[1]);
            Assert.Equal("LOWRISK", result[2]);
            Assert.Equal("MEDIUMRISK", result[3]);
            Assert.Equal("", result[4]);

            _riskChainMock.Verify(c => c.Handle(It.IsAny<Trade>()), Times.Exactly(5));
        }
    }
}