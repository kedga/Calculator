
namespace Calculator.Calculator;

public interface IRpnCalculator<InputType, OperatorType>
{
    bool TryAddValue(InputType value);
    bool TryPerformOperation(OperatorType @operator);
}