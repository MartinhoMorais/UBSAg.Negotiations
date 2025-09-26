using FluentValidation.TestHelper;
using UBSAg.Negotiations.Application.Commands;
using UBSAg.Negotiations.Application.Requests;
using UBSAg.Negotiations.Application.Validator;

namespace UBSAg.Negotiations.UnitTests
{ 
    public class TradeCommandValidatorTests
    {
        private readonly TradeCommandValidator validator;

        public TradeCommandValidatorTests()
        {
            validator = new TradeCommandValidator();
        }

        [Fact]
        public void Should_HaveValidationError_When_TradesListIsNull()
        {
            // Arrange
            var command = new TradeCommand(null!);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Trades)
                  .WithErrorMessage("A lista de trades não pode ser nula.");
        }

        [Fact]
        public void Should_HaveValidationError_When_TradesListIsEmpty()
        {
            // Arrange
            var command = new TradeCommand([]);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Trades)
                  .WithErrorMessage("A lista de trades não pode ser vazia.");
        }

        [Fact]
        public void Should_HaveValidationError_When_TradeValueIsInvalid()
        {
            // Arrange
            var tradeRequests = new List<TradeRequest>
            {
                new(-100, "Public")
            };

            var command = new TradeCommand(tradeRequests);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor("Trades[0].Value")
                  .WithErrorMessage("O valor do trade deve ser maior que zero.");
        }

        [Fact]
        public void Should_HaveValidationError_When_ClientSectorIsInvalid()
        {
            // Arrange
            var tradeRequests = new List<TradeRequest>
            {
                new(1000, "")
            };

            var command = new TradeCommand(tradeRequests);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor("Trades[0].ClientSector")
                  .WithErrorMessage("O setor do cliente não pode ser nulo ou vazio.");
        }

        [Fact]
        public void Should_NotHaveValidationErrors_When_TradesAreValid()
        {
            // Arrange
            var tradeRequests = new List<TradeRequest>
            {
                new(1000, "Public"),
                new(2000000, "Private")
            };
            var command = new TradeCommand(tradeRequests);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}