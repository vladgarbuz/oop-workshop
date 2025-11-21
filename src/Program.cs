using System;
using OopWorkshop.Presentation;

namespace OopWorkshop
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				ConsoleUI ui = new ConsoleUI();
				ui.Start();
			}
			catch (Exception ex)
			{
				Console.WriteLine("An unexpected error occurred: " + ex.Message);
				
			}
		}
	}
}
