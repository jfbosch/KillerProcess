using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;
using System.Collections.Generic;

// Responsible for enumerating Edge renderer processes
public class EdgeProcessEnumerator
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

	public static async Task<Process[]> GetEdgeRendererProcessesAsync(int maxDegreeOfParallelism = 8)
	{
		var processes = Process.GetProcessesByName("msedge");
		var rendererList = new List<Process>();
		var tasks = new List<Task<(Process proc, string? cmd)>>();
		foreach (var p in processes)
		{
			tasks.Add(GetCommandLineAsync(p));
		}
		var results = await Task.WhenAll(tasks);
		foreach (var (proc, cmd) in results)
		{
			if (cmd is not null && cmd.Contains("--type=renderer", StringComparison.OrdinalIgnoreCase))
			{
				rendererList.Add(proc);
			}
		}
		return rendererList.ToArray();
	}

	private static async Task<(Process, string?)> GetCommandLineAsync(Process p)
	{
		return (p, await Task.Run(() => GetCommandLine(p.Id)));
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
