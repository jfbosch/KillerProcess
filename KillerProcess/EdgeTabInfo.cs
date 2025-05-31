// Data class for Edge tab info
public class EdgeTabInfo
{
	public int ProcessId { get; }
	public double CpuPercent { get; }
	public string WindowTitle { get; }

	public EdgeTabInfo(int pid, double cpuPercent, string windowTitle)
	{
		ProcessId = pid;
		CpuPercent = cpuPercent;
		WindowTitle = windowTitle;
	}
}
