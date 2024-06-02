using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.UI;

public interface IBasicIO
{
	void PushOutput(params string?[] text);
	string GetInput();
	void Clear();
}