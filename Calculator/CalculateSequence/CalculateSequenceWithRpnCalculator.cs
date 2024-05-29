using Calculator.Calculator;

namespace Calculator.Calculate;

public class CalculateSequenceWithRpnCalculator : CalculateSequence
{
    private readonly RpnCalculator _calculator = new();
    public override double AddCustom(double[] numbers)
    {
        AddNumbersAndOperate(numbers, Operator.Add);

        return _calculator.LastNumber;
    }

    public override double SubtractCustom(double[] numbers)
    {
        PerformPairwiseOperations(numbers, Operator.Subtract);

        return _calculator.LastNumber;
    }

    public override double MultiplyCustom(double[] numbers)
    {
        AddNumbersAndOperate(numbers, Operator.Multiply);

        return _calculator.LastNumber;
    }
    public override double DivideCustom(double[] numbers)
    {
        PerformPairwiseOperations(numbers, Operator.Divide);

        if (_calculator.OutputMessage.Contains("divide by zero", StringComparison.InvariantCultureIgnoreCase))
        {
            throw new DivideByZeroException(_calculator.OutputMessage);
        }

        return _calculator.LastNumber;
    }

    private void AddNumbers(double[] numbers)
    {
        foreach (var number in numbers)
        {
            _calculator.AddValue(number);
        }
    }

    private void PerformOperations(int numberOfTimes, Operator @operator)
    {
        for (var i = 0; i < numberOfTimes; i++)
        {
            _calculator.TryPerformOperation(@operator);
        }
    }

    private void AddNumbersAndOperate(double[] numbers, Operator @operator)
    {
        AddNumbers(numbers);

        PerformOperations(numbers.Length - 1, @operator);
    }

    private void PerformPairwiseOperations(double[] numbers, Operator @operator)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            _calculator.AddValue(numbers[i]);

            _calculator.OutputMessage = string.Empty;

            if (i > 0 && i % 2 == 1)
            {
                _calculator.TryPerformOperation(@operator);
            }

            else if (i == numbers.Length - 1)
            {
                _calculator.TryPerformOperation(@operator);
            }

            if (_calculator.OutputMessage.Length > 1)
            {
                break;
            }
        }
    }
}
