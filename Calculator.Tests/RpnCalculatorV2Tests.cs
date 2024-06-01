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
			_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.AddedOperand(operand)), Times.Once);
		}

		[Fact]
		public void AddOperator_AddsOperatorAndPushesOutput()
		{
			// Arrange
			var @operator = Operator.Add;

			// Act
			_calculator.AddOperator(@operator);

			// Assert
			Assert.Equal(1, _calculator.ItemCount);
			_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.AddedOperator(@operator)), Times.Once);
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
		public void TryPerformOperation_WithValidInput_OutputsCorrectSequence()
		{
			// Arrange
			var operand1 = new Operand(3);
			var operand2 = new Operand(5);
			var operator1 = Operator.Add;
			var operand3 = new Operand(8);
			var operator2 = Operator.Divide;
			var operand4 = new Operand(3);
			var operand5 = new Operand(6);
			var operator3 = Operator.Add;
			var operator4 = Operator.Multiply;

			_calculator.AddOperand(operand1);
			_calculator.AddOperand(operand2);
			_calculator.AddOperator(operator1);
			_calculator.AddOperand(operand3);
			_calculator.AddOperator(operator2);
			_calculator.AddOperand(operand4);
			_calculator.AddOperand(operand5);
			_calculator.AddOperator(operator3);
			_calculator.AddOperator(operator4);

			var sequence1 = new List<CalculatorItem>() { operand1, operand2, operator1, operand3, operator2, operand4, operand5, operator3, operator4 };
			var result1 = OperationUnit.CreateAndGetResultingOperand([operand1, operand2, operator1]);

			var sequence2 = new List<CalculatorItem>() { result1!, operand3, operator2, operand4, operand5, operator3, operator4 };
			var result2 = OperationUnit.CreateAndGetResultingOperand([result1!, operand3, operator2]);

			var sequence3 = new List<CalculatorItem>() { result2!, operand4, operand5, operator3, operator4 };
			var result3 = OperationUnit.CreateAndGetResultingOperand([operand4, operand5, operator3]);

			var sequence4 = new List<CalculatorItem>() { result2!, result3!, operator4 };
			var result4 = OperationUnit.CreateAndGetResultingOperand([result2!, result3!, operator4]);

			var sequence5 = new List<CalculatorItem>() { result4! };

			// Act
			var result = _calculator.TryPerformOperation();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(9, result.Value);
			_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.StartMessage), Times.Once);
			_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.InitialItems(sequence1)), Times.Once);
			_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.Step(sequence2, 1)), Times.Once);
			_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.Step(sequence3, 2)), Times.Once);
			_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.Step(sequence4, 3)), Times.Once);
			_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.FinalStep(sequence5, 4)), Times.Once);
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
			_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.NotEnoughOperands), Times.Once);
		}

		[Fact]
		public void PrintStackContents_ReturnsCorrectContents()
		{
			// Arrange
			var operand1 = new Operand(3);
			var operand2 = new Operand(5);
			_calculator.AddOperand(operand1);
			_calculator.AddOperand(operand2);

			// Act
			var contents = _calculator.PrintStackContents();

			// Assert
			Assert.Equal(RpnCalcV2.Message.PrintItems([operand1, operand2]), contents);
		}
	}
}