using System.Diagnostics;

// Responsible for sampling CPU usage
public class CpuUsageSampler
{
	private readonly int _cpuCount;
	private readonly int _sampleMs;

	public CpuUsageSampler(int sampleMs)
	{
		_cpuCount = Environment.ProcessorCount;
		_sampleMs = sampleMs;
	}

	public async Task<double?> SampleCpuPercentAsync(Process p)
	{
		try
		{
			TimeSpan cpuStart = p.TotalProcessorTime;
			await Task.Delay(_sampleMs).ConfigureAwait(false);
			if (p.HasExited) return null;
			p.Refresh();
			TimeSpan cpuEnd = p.TotalProcessorTime;
			double cpuPercent = (cpuEnd - cpuStart).TotalMilliseconds / _sampleMs / _cpuCount * 100.0;
			return cpuPercent;
		}
		catch
		{
			return null;
		}
	}
}
