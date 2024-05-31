using Calculator.UI;

namespace Calculator.Calculator;

public class StringRpnCalculator : RpnCalculator
{
    public bool TryAddValue(string value)
    {
        if (!double.TryParse(value, out var doubleValue)) return false;

        AddValue(doubleValue);
        return true;
    }

    public bool TryPerformOperation(string stringOperator)
    {
        var @operator = TryGetOperator(stringOperator);

        if (@operator is not Operator validOperator)
        {
            OutputMessage = "Invalid operator: " + stringOperator;
			return false;
        }

        var isOperationSuccessful = TryPerformOperation(validOperator);
        return isOperationSuccessful;
    }

	protected static Operator? TryGetOperator(string stringOperator) => stringOperator switch
    {
        "+" => Operator.Add,
        "-" => Operator.Subtract,
        "*" => Operator.Multiply,
        "/" => Operator.Divide,
        _ => null
    };
}
