using Moq;
using UBSAg.Negotiations.Application.Categories;
using UBSAg.Negotiations.Domain.Constants;
using UBSAg.Negotiations.Domain.Entities;
using UBSAg.Negotiations.Domain.Enums;
using UBSAg.Negotiations.Domain.Interfaces;

public class RiskCategoryTests
{
    private readonly Mock<ICategory> _nextHandlerMock;

    public RiskCategoryTests()
    {
        _nextHandlerMock= new Mock<ICategory>();
    }

    [Fact]
    public void HighRisk_ShouldReturnHighRisk_WhenTradeIsHighRisk()
    {
        // Arrange
        var trade = new Trade(
            RiskValueConstant.RISK_CUTOFF_VALUE + 1,
            ClientSectorConstant.PRIVATE_SECTOR.ToString());

        var highRiskHandler = new HighRisk();

        // Act
        var result = highRiskHandler.Handle(trade);

        // Assert
        Assert.Equal(TradeCategoriesEnum.HighRisk.ToString().ToUpper(), result);
        _nextHandlerMock.Verify(h => h.Handle(It.IsAny<Trade>()), Times.Never());
    }

    [Fact]
    public void HighRisk_ShouldPassToNextHandler_WhenTradeIsNotHighRisk()
    {
        // Arrange
        var trade = new Trade(
            RiskValueConstant.RISK_CUTOFF_VALUE - 1,
            ClientSectorConstant.PRIVATE_SECTOR.ToString());

        var highRiskHandler = new HighRisk();

        _nextHandlerMock.Setup(h => h.Handle(trade)).Returns("");
        highRiskHandler.SetNext(_nextHandlerMock.Object);

        // Act
        var result = highRiskHandler.Handle(trade);

        // Assert
        Assert.Equal("", result);
        _nextHandlerMock.Verify(h => h.Handle(trade), Times.Once());
    }

    [Fact]
    public void LowRisk_ShouldReturnLowRisk_WhenTradeIsLowRisk()
    {
        // Arrange
        var trade = new Trade(
            RiskValueConstant.RISK_CUTOFF_VALUE - 1,
            ClientSectorConstant.PUBLIC_SECTOR);

        var lowRiskHandler = new LowRisk();
        lowRiskHandler.SetNext(_nextHandlerMock.Object);

        // Act
        var result = lowRiskHandler.Handle(trade);

        // Assert
        Assert.Equal(TradeCategoriesEnum.LowRisk.ToString().ToUpper(), result);
        _nextHandlerMock.Verify(h => h.Handle(It.IsAny<Trade>()), Times.Never());
    }

    [Fact]
    public void LowRisk_ShouldPassToNextHandler_WhenTradeIsNotLowRisk()
    {
        // Arrange
        var trade = new Trade(
            RiskValueConstant.RISK_CUTOFF_VALUE + 1,
            ClientSectorConstant.PUBLIC_SECTOR);

        var lowRiskHandler = new LowRisk();
        
        lowRiskHandler.SetNext(_nextHandlerMock.Object);
        _nextHandlerMock.Setup(h => h.Handle(trade)).Returns("MEDIUMRISK");

        // Act
        var result = lowRiskHandler.Handle(trade);

        // Assert
        Assert.Equal("MEDIUMRISK", result);
        _nextHandlerMock.Verify(h => h.Handle(trade), Times.Once());
    }

    [Fact]
    public void MediumRisk_ShouldReturnMediumRisk_WhenTradeIsMediumRisk()
    {
        // Arrange
        var trade = new Trade(RiskValueConstant.RISK_CUTOFF_VALUE + 1,
            ClientSectorConstant.PUBLIC_SECTOR);

        var mediumRiskHandler = new MediumRisk();        
        mediumRiskHandler.SetNext(_nextHandlerMock.Object);

        // Act
        var result = mediumRiskHandler.Handle(trade);

        // Assert
        Assert.Equal(TradeCategoriesEnum.MediumRisk.ToString().ToUpper(), result);
        _nextHandlerMock.Verify(h => h.Handle(It.IsAny<Trade>()), Times.Never());
    }

    [Fact]
    public void MediumRisk_ShouldPassToNextHandler_WhenTradeIsNotMediumRisk()
    {
        // Arrange
        var trade = new Trade(
            RiskValueConstant.RISK_CUTOFF_VALUE + 1,
            ClientSectorConstant.PRIVATE_SECTOR);

        var mediumRiskHandler = new MediumRisk();
        
        mediumRiskHandler.SetNext(_nextHandlerMock.Object);
        _nextHandlerMock.Setup(h => h.Handle(trade)).Returns("HIGHRISK");

        // Act
        var result = mediumRiskHandler.Handle(trade);

        // Assert
        Assert.Equal("HIGHRISK", result);
        _nextHandlerMock.Verify(h => h.Handle(trade), Times.Once());
    }

    [Fact]
    public void NoCategory_ShouldReturnNoCategory()
    {
        // Arrange
        var trade = new Trade(1000, "NonExistent");

        var noCategoryHandler = new NoCategory();

        // Act
        var result = noCategoryHandler.Handle(trade);

        // Assert
        Assert.Equal("", result);
    }
}