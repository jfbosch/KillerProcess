using System.Diagnostics;
using System.Management;

// Responsible for enumerating Edge renderer processes
class EdgeProcessEnumerator
{
	public static Process[] GetEdgeRendererProcesses()
	{
		var processes = Process.GetProcessesByName("msedge");
		var rendererList = new System.Collections.Generic.List<Process>();
		foreach (var p in processes)
		{
			string? cmd = GetCommandLine(p.Id);
			if (cmd is not null && cmd.Contains("--type=renderer", StringComparison.OrdinalIgnoreCase))
			{
				rendererList.Add(p);
			}
		}
		return rendererList.ToArray();
	}

	private static string? GetCommandLine(int pid)
	{
		try
		{
			using ManagementObjectSearcher searcher = new(
				 $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {pid}");
			foreach (ManagementObject mo in searcher.Get())
			{
				return mo["CommandLine"] as string;
			}
		}
		catch
		{
			// Ignore errors
		}
		return null;
	}
}
