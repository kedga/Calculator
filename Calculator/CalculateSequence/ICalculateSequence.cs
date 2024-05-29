using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Calculate;

public interface ICalculateSequence
{
    double Add(params double[] numbers);
    double Subtract(params double[] numbers);
    double Multiply(params double[] numbers);
    double Divide(params double[] numbers);
	List<MathOperation> GetOperations();
}

public record MathOperation(Func<double[], double> Operation, char Symbol);
