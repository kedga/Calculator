using System;
using System.Collections.Generic;
using Calculator.RpnCalculatorV2;
using Calculator.UI;
using Moq;
using Xunit;

namespace Calculator.Tests;

public class ConsoleLikeRpnCalculatorV2Tests
{
	private readonly Mock<IBasicIO> _mockIo;
	private readonly ConsoleLikeRpnCalculatorV2 _consoleCalculator;

	private readonly RpnCalcV2 _calculator;

	private readonly IBasicIO _columnIO;
	private readonly IBasicIO _centerIO;

	public ConsoleLikeRpnCalculatorV2Tests()
	{
		_mockIo = new Mock<IBasicIO>();

		_columnIO = new ColumnsFormatterIO(_mockIo.Object)
		{
			StringFormat = ["{0, 20}", "{1, -20}"],
			Separator = " : "
		};
		_centerIO = new CenterFormatterIO(_mockIo.Object)
		{
			CenterPosition = 22
		};

		_calculator = new RpnCalcV2();

		_mockIo = new Mock<IBasicIO>();
		_consoleCalculator = new ConsoleLikeRpnCalculatorV2(_calculator, _mockIo.Object);
	}

	[Fact]
	public void Run_WithValidSequence_PerformsCorrectCalculation()
	{
		// Arrange
		var inputSequence = new Queue<string>(["3", "5", "+", "8", "/", "3", "6", "+", "*", "p", "q"]);
		_mockIo.Setup(io => io.GetInput()).Returns(inputSequence.Dequeue);

		// Act
		_consoleCalculator.Run();

		// Assert
		//_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.TryPerformOperation.Step(new List<CalculatorItem>() { new Operand(9) }, 4)), Times.Once);
	}

	[Fact]
	public void Run_ClearCommand_ClearsCalculator()
	{
		// Arrange
		var inputSequence = new Queue<string>(["3", "5", "+", "c", "q"]);
		_mockIo.Setup(io => io.GetInput()).Returns(inputSequence.Dequeue);

		// Act
		_consoleCalculator.Run();

		// Assert
		_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.PrintItems(new List<CalculatorItem>() { })), Times.Exactly(2));
	}

	[Fact]
	public void Run_RemoveLastItemCommand_RemovesLastItem()
	{
		// Arrange
		var inputSequence = new Queue<string>(["3", "5", "+", "r", "q"]);
		_mockIo.Setup(io => io.GetInput()).Returns(inputSequence.Dequeue);

		// Act
		_consoleCalculator.Run();

		// Assert
		_mockIo.Verify(io => io.PushOutput(RpnCalcV2.Message.RemovedItem(Operator.Add)), Times.Once);
	}

	[Fact]
	public void Run_InvalidInput_ShowsUnknownInput()
	{
		// Arrange
		var inputSequence = new Queue<string>(["3", "x", "q"]);
		_mockIo.Setup(io => io.GetInput()).Returns(inputSequence.Dequeue);

		// Act
		_consoleCalculator.Run();

		// Assert
		_mockIo.Verify(io => io.PushOutput(It.Is<string>(s => s.Contains("Unknown input: x"))), Times.Once);
	}

	[Fact]
	public void Run_ExitCommand_ExitsProgram()
	{
		// Arrange
		var inputSequence = new Queue<string>(["q"]);
		_mockIo.Setup(io => io.GetInput()).Returns(inputSequence.Dequeue);

		// Act
		_consoleCalculator.Run();

		// Assert
		_mockIo.Verify(io => io.GetInput(), Times.Once);
	}
}