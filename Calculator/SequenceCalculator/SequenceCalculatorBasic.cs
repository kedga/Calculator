using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Calculate;

public class SequenceCalculatorBasic : SequenceCalculator
{
    public override double AddLogic(double[] numbers)
    {
        return numbers.Sum();
    }

    public override double DivideLogic(double[] numbers)
    {
        return numbers.Aggregate((acc, number) => acc / number);
    }

    public override double MultiplyLogic(double[] numbers)
    {
        return numbers.Aggregate((acc, number) => acc * number);
    }

    public override double SubtractLogic(double[] numbers)
    {
        return numbers.Aggregate((acc, number) => acc - number);
    }
}
