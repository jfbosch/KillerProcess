internal class Program
{
	public static async Task Main()
	{
		// Prompt for process filter mode
		ProcessFilterMode filterMode = ProcessFilterMode.EdgeOnly;
		while (true)
		{
			Console.Clear();
			Console.WriteLine("Select process filter mode:");
			Console.WriteLine("  1 - List only Edge related processes");
			Console.WriteLine("  2 - List all processes");
			Console.Write("Enter your choice (1 or 2): ");
			string? modeInput = Console.ReadLine();
			if (modeInput == "1")
			{
				filterMode = ProcessFilterMode.EdgeOnly;
				break;
			}
			else if (modeInput == "2")
			{
				filterMode = ProcessFilterMode.All;
				break;
			}
			else
			{
				Console.WriteLine("Invalid input. Press Enter to try again...");
				Console.ReadLine();
			}
		}

		// Prompt for CPU threshold
		double threshold = 0.0;
		while (true)
		{
			Console.Clear();
			Console.Write("Enter CPU threshold percentage (0-100): ");
			string? thresholdInput = Console.ReadLine();
			if (double.TryParse(thresholdInput, out double t) && t >= 0 && t <= 100)
			{
				threshold = t;
				break;
			}
			else
			{
				Console.WriteLine("Invalid input. Press Enter to try again...");
				Console.ReadLine();
			}
		}

		while (true)
		{
			Console.Clear();
			Console.WriteLine(filterMode == ProcessFilterMode.EdgeOnly
				 ? $"Listing Edge renderer processes (CPU > {threshold}%)\n"
				 : $"Listing all processes (CPU > {threshold}%)\n");
			var monitor = new ProcessMonitor(threshold, filterMode);
			var matchingProcesses = await monitor.GetMatchingProcessesAsync();
			foreach (var info in matchingProcesses)
			{
				Console.WriteLine($"PID {info.ProcessId,5}  {info.CpuPercent,5:F1}%  {info.WindowTitle}");
			}
			Console.WriteLine();
			Console.WriteLine($"Total matching processes: {matchingProcesses.Count}");
			Console.WriteLine();
			Console.WriteLine("Options:");
			Console.WriteLine("  r   - Refresh the list");
			Console.WriteLine("  k   - Kill all matching processes");
			Console.WriteLine("  1-100 - Show only processes using more than this % CPU");
			Console.WriteLine("  q   - Quit");
			Console.Write("Enter your choice: ");
			string? input = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(input))
				continue;
			input = input.Trim().ToLower();
			if (input == "q")
				break;
			if (input == "r")
			{
				// Just refresh (continue loop)
				continue;
			}
			if (input == "k")
			{
				var killed = new List<ProcessInfo>();
				foreach (var info in matchingProcesses)
				{
					try
					{
						var proc = System.Diagnostics.Process.GetProcessById(info.ProcessId);
						proc.Kill();
						killed.Add(info);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Failed to kill PID {info.ProcessId}: {ex.Message}");
					}
				}
				Console.WriteLine();
				if (killed.Count > 0)
				{
					Console.WriteLine($"Killed {killed.Count} processes:");
					foreach (var info in killed)
						Console.WriteLine($"  PID {info.ProcessId,5}  {info.CpuPercent,5:F1}%  {info.WindowTitle}");
				}
				else
				{
					Console.WriteLine("No processes were killed.");
				}
				// After killing, immediately continue to the top of the loop to show the refreshed list and options
				continue;
			}
			if (int.TryParse(input, out int num) && num >= 1 && num <= 100)
			{
				threshold = num;
				continue;
			}
			Console.WriteLine("Invalid input. Press Enter to continue...");
			Console.ReadLine();
		}
	}
}
