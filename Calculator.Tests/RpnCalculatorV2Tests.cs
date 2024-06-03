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

		private readonly IBasicIO _columnIO;
		private readonly IBasicIO _centerIO;

		public RpnCalculatorV2Tests()
		{
			_mockIo = new Mock<IBasicIO>();

			//_columnIO = new ColumnsFormatterIO(_mockIo.Object)
			//{
			//	StringFormat = ["{0, 20}", "{1, -20}"],
			//	Separator = " : "
			//};
			//_centerIO = new CenterFormatterIO(_mockIo.Object)
			//{
			//	CenterPosition = 22
			//};

			_calculator = new RpnCalcV2();

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
			_mockIo.Verify(io => io.PushOutput(It.Is<string>(s => s.Contains(RpnCalcV2.Message.AddedOperand(operand)))), Times.Once);
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
			_mockIo.Verify(io => io.PushOutput(It.Is<string>(s => s.Contains(RpnCalcV2.Message.AddedOperator(@operator)))), Times.Once);
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
			var operationUnit1 = OperationUnit.Create([operand1, operand2, operator1]);
			var result1 = operationUnit1.GetResultAsOperand();

			var sequence2 = new List<CalculatorItem>() { result1!, operand3, operator2, operand4, operand5, operator3, operator4 };
			var operationUnit2 = OperationUnit.Create([result1!, operand3, operator2]);
			var result2 = operationUnit2.GetResultAsOperand();

			var sequence3 = new List<CalculatorItem>() { result2!, operand4, operand5, operator3, operator4 };
			var operationUnit3 = OperationUnit.Create([operand4, operand5, operator3]);
			var result3 = operationUnit3.GetResultAsOperand();

			var sequence4 = new List<CalculatorItem>() { result2!, result3!, operator4 };
			var operationUnit4 = OperationUnit.Create([result2!, result3!, operator4]);
			var result4 = operationUnit4.GetResultAsOperand();

			var sequence5 = new List<CalculatorItem>() { result4! };

			// Act
			var result = _calculator.TryPerformOperation();

			// Assert
			//Assert.NotNull(result);
			//Assert.Equal(9, result.Value);
			//_mockIo.Verify(io => io.PushOutput(It.Is<string>(s => s.Contains(RpnCalcV2.Message.TryPerformOperation.StartMessage))), Times.Once);
			//_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.InitialItemsHeader(sequence1)), Times.Once);
			//_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.Step(sequence2, operationUnit1.GetOperationAsString())), Times.Once);
			//_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.Step(sequence3, operationUnit2.GetOperationAsString())), Times.Once);
			//_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.Step(sequence4, operationUnit3.GetOperationAsString())), Times.Once);
			//_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.Step(sequence5, operationUnit4.GetOperationAsString())), Times.Once);
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
			_mockIo.Verify(io => io.PushOutput(It.Is<string>(s => s.Contains(RpnCalcV2.Message.TryPerformOperation.NotEnoughOperands))), Times.Once);
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
			var contents = _calculator.GetStackContentsAsString();

			// Assert
			Assert.Equal(RpnCalcV2.Message.PrintItems([operand1, operand2]), contents);
		}
	}
}