using System.Diagnostics;

// Orchestrates the monitoring of processes (Edge or all)
// class ProcessMonitor
// {
// 	private readonly double _cpuThresholdPercent;
// 	private readonly ProcessFilterMode _filterMode;
// 	private const int SampleMilliseconds = 500;
// 	private readonly CpuUsageSampler _sampler = new(SampleMilliseconds);
//
// 	public ProcessMonitor(double cpuThresholdPercent, ProcessFilterMode filterMode)
// 	{
// 		_cpuThresholdPercent = cpuThresholdPercent;
// 		_filterMode = filterMode;
// 	}
//
// 	public async Task MonitorAsync()
// 	{
// 		var processes = _filterMode == ProcessFilterMode.EdgeOnly
// 			? EdgeProcessEnumerator.GetEdgeRendererProcesses()
// 			: Process.GetProcesses();
// 		var tasks = new List<Task<ProcessInfo?>>();
// 		foreach (var p in processes)
// 		{
// 			tasks.Add(GetProcessInfoIfActiveAsync(p));
// 		}
// 		var results = await Task.WhenAll(tasks);
// 		foreach (var info in results)
// 		{
// 			if (info != null)
// 			{
// 				Console.WriteLine($"PID {info.ProcessId,5}  {info.CpuPercent,5:F1}%  {info.WindowTitle}");
// 			}
// 		}
// 	}
//
// 	private async Task<ProcessInfo?> GetProcessInfoIfActiveAsync(Process p)
// 	{
// 		double? cpu = await _sampler.SampleCpuPercentAsync(p);
// 		if (cpu.HasValue && cpu.Value >= _cpuThresholdPercent)
// 		{
// 			return new ProcessInfo(p.Id, Math.Round(cpu.Value, 1), p.MainWindowTitle);
// 		}
// 		return null;
// 	}
//
// 	public async Task<List<ProcessInfo>> GetMatchingProcessesAsync()
// 	{
// 		Process[] processes;
// 		if (_filterMode == ProcessFilterMode.EdgeOnly)
// 		{
// 			processes = EdgeProcessEnumerator.GetEdgeRendererProcesses();
// 		}
// 		else
// 		{
// 			processes = Process.GetProcesses();
// 		}
// 		var tasks = new List<Task<ProcessInfo?>>();
// 		foreach (var p in processes)
// 		{
// 			tasks.Add(GetProcessInfoIfActiveAsync(p));
// 		}
// 		var results = await Task.WhenAll(tasks);
// 		var matching = new List<ProcessInfo>();
// 		foreach (var info in results)
// 		{
// 			if (info != null)
// 			{
// 				matching.Add(info);
// 			}
// 		}
// 		return matching;
// 	}
// }
