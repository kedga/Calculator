using Calculator.UI;
using Calculator.Utilities;

namespace Calculator.Calculator;

public class RpnCalculator
{
    private readonly Stack<double> _numberStack = [];
	public string OutputMessage { get; set; } = string.Empty;

	private Func<bool> GetOperationMethod(Operator @operator) => @operator switch
    {
        Operator.Add => PerformAddition,
        Operator.Subtract => PerformSubtraction,
        Operator.Multiply => PerformMultiplication,
        Operator.Divide => PerformDivision,
        _ => throw new NotImplementedException("Operator case not implemented: " + @operator)
    };

    private bool PerformAddition()
    {
		if (!MinStackCountCheck(2)) return false;

		var rightHandNumber = _numberStack.Pop();
        var leftHandNumber = _numberStack.Pop();
        _numberStack.Push(leftHandNumber + rightHandNumber);
        return true;
    }

    private bool PerformSubtraction()
    {
		if (!MinStackCountCheck(2)) return false;

		var rightHandNumber = _numberStack.Pop();
        var leftHandNumber = _numberStack.Pop();
        _numberStack.Push(leftHandNumber - rightHandNumber);
        return true;
    }

    private bool PerformMultiplication()
    {
		if (!MinStackCountCheck(2)) return false;

		var rightHandNumber = _numberStack.Pop();
        var leftHandNumber = _numberStack.Pop();
        _numberStack.Push(leftHandNumber * rightHandNumber);
        return true;
    }

    private bool PerformDivision()
    {
        if (!MinStackCountCheck(2)) return false;

        var rightHandNumber = _numberStack.Pop();
        var leftHandNumber = _numberStack.Pop();

        if (rightHandNumber == 0)
        {
            _numberStack.Push(leftHandNumber);
            _numberStack.Push(rightHandNumber);

            OutputMessage = "Cannot divide by zero";

			return false;
        }

        _numberStack.Push(leftHandNumber / rightHandNumber);
        return true;
    }

    private bool MinStackCountCheck(int minCount)
    {
        if (StackCount < minCount)
        {
            OutputMessage = $"Not enough operands, {minCount} needed for this operation";
            return false;
        }
        return true;
    }

    public int StackCount => _numberStack.Count;
    public double LastNumber => _numberStack.Peek();

    public void AddValue(double value)
    {
        _numberStack.Push(value);
	}

    public void Clear()
    {
        _numberStack.Clear();
    }

    public bool TryPerformOperation(Operator @operator)
    {
        OutputMessage = string.Empty;

		var operationMethod = GetOperationMethod(@operator);
        var isSuccess = operationMethod();

        if (double.IsInfinity(LastNumber) || double.IsNaN(LastNumber))
        {
            OutputMessage = "Result is out of range for double type";
            return false;
        }

        return isSuccess;
    }

	public string PrintStackContents() => ToString();

    public override string ToString()
    {
        return $"[{_numberStack.Reverse().PrintCollection()}]";
	}
}

public enum Operator
{
	Add,
	Subtract,
	Multiply,
	Divide
}