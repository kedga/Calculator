using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Calculate;

public class CalculateSequenceBasic : CalculateSequence
{
    public override double AddCustom(double[] numbers)
    {
        return numbers.Sum();
    }

    public override double DivideCustom(double[] numbers)
    {
        return numbers.Aggregate((acc, number) => acc / number);
    }

    public override double MultiplyCustom(double[] numbers)
    {
        return numbers.Aggregate((acc, number) => acc * number);
    }

    public override double SubtractCustom(double[] numbers)
    {
        return numbers.Aggregate((acc, number) => acc - number);
    }
}
