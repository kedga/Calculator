using System;
using System.Collections.Generic;
using Calculator.RpnCalculatorV2;
using Calculator.UI;
using Moq;
using Xunit;

namespace Calculator.Tests
{
	public class RpnCalculatorV2Tests
	{
		private readonly Mock<IBasicIO> _mockIo;
		private readonly RpnCalcV2 _calculator;

		public RpnCalculatorV2Tests()
		{
			_mockIo = new Mock<IBasicIO>();
			_calculator = new RpnCalcV2(_mockIo.Object);
		}

		[Fact]
		public void AddOperand_AddsOperandAndPushesOutput()
		{
			// Arrange
			double operand = 5.0;

			// Act
			_calculator.AddOperand(operand);

			// Assert
			Assert.Equal(1, _calculator.ItemCount);
			_mockIo.Verify(io => io.PushOutput($"Added number: {operand}"), Times.Once);
		}

		[Fact]
		public void AddOperator_AddsOperatorAndPushesOutput()
		{
			// Arrange
			var addOperator = Operator.Add;

			// Act
			_calculator.AddOperator(addOperator);

			// Assert
			Assert.Equal(1, _calculator.ItemCount);
			_mockIo.Verify(io => io.PushOutput($"Added operator: {addOperator}"), Times.Once);
		}

		[Fact]
		public void Clear_RemovesAllItemsAndResetsItemCount()
		{
			// Arrange
			_calculator.AddOperand(3);
			_calculator.AddOperator(Operator.Add);

			// Act
			_calculator.Clear();

			// Assert
			Assert.Equal(0, _calculator.ItemCount);
		}

		[Fact]
		public void TryPerformOperation_WithValidInput_ReturnsExpectedResult()
		{
			// Arrange
			_calculator.AddOperand(3);
			_calculator.AddOperand(5);
			_calculator.AddOperator(Operator.Add);
			_calculator.AddOperand(8);
			_calculator.AddOperator(Operator.Divide);
			_calculator.AddOperand(3);
			_calculator.AddOperand(6);
			_calculator.AddOperator(Operator.Add);
			_calculator.AddOperator(Operator.Multiply);

			// Act
			var result = _calculator.TryPerformOperation();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(9, result.Value);
			_mockIo.Verify(io => io.PushOutput("Performing calculation!"), Times.Once);
			_mockIo.Verify(io => io.PushOutput("Items:  [ 3 5 + 8 / 3 6 + * ]"), Times.Once);
			_mockIo.Verify(io => io.PushOutput("Step 1: [ 8 8 / 3 6 + * ]"), Times.Once);
			_mockIo.Verify(io => io.PushOutput("Step 2: [ 1 3 6 + * ]"), Times.Once);
			_mockIo.Verify(io => io.PushOutput("Step 3: [ 1 9 * ]"), Times.Once);
			_mockIo.Verify(io => io.PushOutput(It.Is<string>(s => s.Contains("[ 9 ]"))), Times.Once);
		}

		[Fact]
		public void TryPerformOperation_WithInvalidOperation_ReturnsNull()
		{
			// Arrange
			_calculator.AddOperand(3);
			_calculator.AddOperator(Operator.Divide);

			// Act
			var result = _calculator.TryPerformOperation();

			// Assert
			Assert.Null(result);
			_mockIo.Verify(io => io.PushOutput("Error: Not enough operands"), Times.Once);
		}

		[Fact]
		public void PrintStackContents_ReturnsCorrectContents()
		{
			// Arrange
			_calculator.AddOperand(3);
			_calculator.AddOperand(5);

			// Act
			var contents = _calculator.PrintStackContents();

			// Assert
			Assert.Equal("[ 3 5 ]", contents);
		}
	}
}