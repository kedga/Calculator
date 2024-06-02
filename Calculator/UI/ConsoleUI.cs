using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.UI;

public class ConsoleUI : ICanMakeSelectionIO
{
	public void PushOutput(params string?[] text)
	{
		foreach (var item in text)
		{
			Console.WriteLine(item);
		}
	}

	public string GetInput()
	{
		return Console.ReadLine()?.Trim() ?? "";
	}

	public void Clear()
	{
		Console.Clear();
	}

	public T MakeSelection<T>(Action<MakeSelectionOptions<T>> configureOptions)
	{
		var opt = new MakeSelectionOptions<T>();
		configureOptions(opt);

		var (options, optionNames, prompt) = opt;

		int selectedIndex = 0;

		ConsoleKey key;
		do
		{
			Console.Clear();

			if (opt.Prompt.Length > 0)
			{
				Console.WriteLine(prompt);
			}

			for (int i = 0; i < options.Count; i++)
			{
				if (optionNames.Count > 0)
				{
					if (i == selectedIndex)
					{
						Console.WriteLine("» " + optionNames[i]);
					}
					else
					{
						Console.WriteLine("  " + optionNames[i]);
					}
				}
				else if (options is Delegate[] delegates)
				{
					if (i == selectedIndex)
					{
						Console.WriteLine("» " + delegates[i].Method.Name);
					}
					else
					{
						Console.WriteLine("  " + delegates[i].Method.Name);
					}
				}
				else
				{
					if (i == selectedIndex)
					{
						Console.WriteLine("» " + options[i]?.ToString());
					}
					else
					{
						Console.WriteLine("  " + options[i]?.ToString());
					}
				}
			}

			key = Console.ReadKey(true).Key;

			switch (key)
			{
				case ConsoleKey.UpArrow:
					selectedIndex--;
					if (selectedIndex < 0) selectedIndex = options.Count - 1;
					break;

				case ConsoleKey.DownArrow:
					selectedIndex++;
					if (selectedIndex >= options.Count) selectedIndex = 0;
					break;
			}
		} while (key != ConsoleKey.Enter);

		return options[selectedIndex];
	}

	public bool AskYesOrNo(Action<AskYesOrNoOptions> configureOptions)
	{
		var opt = new AskYesOrNoOptions();
		configureOptions(opt);

		var selectedOption = MakeSelection<string>(o =>
		{
			o.Options = [opt.YesOption, opt.NoOption];
			o.Prompt = opt.Prompt;
		});

		return selectedOption == opt.YesOption;
	}
}
