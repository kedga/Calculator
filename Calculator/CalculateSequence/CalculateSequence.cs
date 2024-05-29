
namespace Calculator.Calculate;

public abstract class CalculateSequence : ICalculateSequence
{
	public double Add(params double[] numbers)
	{
		if (numbers.Length < 1) return 0;

		if (numbers.Length == 1) return numbers.First();

		return AddCustom(numbers);
	}

	public double Divide(params double[] numbers)
	{
		if (numbers.Length < 1) return 1;

		if (numbers.Length == 1) return numbers.First();

		var result = DivideCustom(numbers);

		if (double.IsInfinity(result)) throw new DivideByZeroException("Cannot divide by zero");

		return result;
	}

	public double Multiply(params double[] numbers)
	{
		if (numbers.Length < 1) return 1;

		if (numbers.Length == 1) return numbers.First();

		return MultiplyCustom(numbers);
	}

	public double Subtract(params double[] numbers)
	{
		if (numbers.Length < 1) return 0;

		if (numbers.Length == 1) return numbers.First();

		return SubtractCustom(numbers);
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

	public abstract double AddCustom(double[] numbers);
	public abstract double DivideCustom(double[] numbers);
	public abstract double MultiplyCustom(double[] numbers);
	public abstract double SubtractCustom(double[] numbers);
}