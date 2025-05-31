using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

// Orchestrates the monitoring
class EdgeTabMonitor
{
	private readonly double _cpuThresholdPercent;
	private readonly ProcessFilterMode _filterMode;
	private const int SampleMilliseconds = 500;
	private readonly CpuUsageSampler _sampler = new(SampleMilliseconds);

	public EdgeTabMonitor(double cpuThresholdPercent, ProcessFilterMode filterMode)
	{
		_cpuThresholdPercent = cpuThresholdPercent;
		_filterMode = filterMode;
	}

	public async Task MonitorAsync()
	{
		var processes = EdgeProcessEnumerator.GetEdgeRendererProcesses();
		var tasks = new List<Task<EdgeTabInfo?>>();
		foreach (var p in processes)
		{
			tasks.Add(GetTabInfoIfActiveAsync(p));
		}
		var results = await Task.WhenAll(tasks);
		foreach (var info in results)
		{
			if (info != null)
			{
				Console.WriteLine($"PID {info.ProcessId,5}  {info.CpuPercent,5:F1}%  {info.WindowTitle}");
			}
		}
	}

	private async Task<EdgeTabInfo?> GetTabInfoIfActiveAsync(Process p)
	{
		double? cpu = await _sampler.SampleCpuPercentAsync(p);
		if (cpu.HasValue && cpu.Value >= _cpuThresholdPercent)
		{
			return new EdgeTabInfo(p.Id, Math.Round(cpu.Value, 1), p.MainWindowTitle);
		}
		return null;
	}

	public async Task<List<EdgeTabInfo>> GetMatchingTabsAsync()
	{
		Process[] processes;
		if (_filterMode == ProcessFilterMode.EdgeOnly)
		{
			processes = EdgeProcessEnumerator.GetEdgeRendererProcesses();
		}
		else
		{
			processes = Process.GetProcesses();
		}
		var tasks = new List<Task<EdgeTabInfo?>>();
		foreach (var p in processes)
		{
			tasks.Add(GetTabInfoIfActiveAsync(p));
		}
		var results = await Task.WhenAll(tasks);
		var matching = new List<EdgeTabInfo>();
		foreach (var info in results)
		{
			if (info != null)
			{
				matching.Add(info);
			}
		}
		return matching;
	}
}
