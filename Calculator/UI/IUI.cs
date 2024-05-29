using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.UI;

public interface IUI
{
	void PushOutput(string text);
	string GetInput();
	void ClearOutput();
	T MakeSelection<T>(Action<MakeSelectionOptions<T>> configureOptions);
	public bool AskYesOrNo(Action<AskYesOrNoOptions> configureOptions);
}

public class AskYesOrNoOptions
{
	public string YesOption { get; set; } = "Yes";
	public string NoOption { get; set; } = "No";
	public string Prompt { get; set; } = string.Empty;
}
public class MakeSelectionOptions<T>
{
	//public T[] Options { get; set; } = [];
	//public string[] OptionNames { get; set; } = [];
	public List<T> Options { get; set; } = [];
	public List<string> OptionNames { get; set; } = [];
	public string Prompt { get; set; } = string.Empty;
	public void Deconstruct(out List<T> options, out List<string> optionNames, out string prompt)
	{
		options = Options;
		optionNames = OptionNames;
		prompt = Prompt;
	}
	//public void Deconstruct(out T[] options, out string[] optionNames, out string prompt)
	//{
	//	options = Options;
	//	optionNames = OptionNames;
	//	prompt = Prompt;
	//}
}