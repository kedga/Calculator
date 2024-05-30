
namespace Calculator.Calculate;

public abstract class SequenceCalculator : ISequenceCalculator
{
	public double Add(params double[] numbers)
	{
		if (numbers.Length < 1) return 0;

		if (numbers.Length == 1) return numbers.First();

		return AddLogic(numbers);
	}

	public double Divide(params double[] numbers)
	{
		if (numbers.Length < 1) return 1;

		if (numbers.Length == 1) return numbers.First();

		var result = DivideLogic(numbers);

		if (double.IsInfinity(result)) throw new DivideByZeroException("Cannot divide by zero");

		return result;
	}

	public double Multiply(params double[] numbers)
	{
		if (numbers.Length < 1) return 1;

		if (numbers.Length == 1) return numbers.First();

		return MultiplyLogic(numbers);
	}

	public double Subtract(params double[] numbers)
	{
		if (numbers.Length < 1) return 0;

		if (numbers.Length == 1) return numbers.First();

		return SubtractLogic(numbers);
	}

	public List<MathOperation> GetOperations()
	{
		return 
		[
			new MathOperation(Add, '+'),
			new MathOperation(Subtract, '-'),
			new MathOperation(Multiply, '*'),
			new MathOperation(Divide, '/')
		];
	}

	public abstract double AddLogic(double[] numbers);
	public abstract double DivideLogic(double[] numbers);
	public abstract double MultiplyLogic(double[] numbers);
	public abstract double SubtractLogic(double[] numbers);
}