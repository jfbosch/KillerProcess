# KillerProcess

KillerProcess is a command-line utility for monitoring and managing running processes on Windows, with a special focus on Microsoft Edge browser renderer processes. It allows you to:

- List all running processes or only Edge renderer processes.
- Filter processes by CPU usage percentage.
- Refresh the process list in real time.
- Kill all processes that exceed a specified CPU usage threshold.

## Features
- Interactive menu for selecting process filter mode (Edge-only or all processes).
- Set a CPU usage threshold to display only processes above that value.
- Option to kill all matching processes directly from the interface.

## Usage

1. **Build the project**
   - Open the solution in Visual Studio 2022 or later, or use the .NET CLI:
     ```powershell
     dotnet build
     ```

2. **Run the program**
   - From the project directory:
     ```powershell
     dotnet run --project KillerProcess
     ```
   - Or run the built executable from `bin/Debug/net9.0/`:
     ```powershell
     .\KillerProcess.exe
     ```

3. **Follow the prompts**
   - Select the process filter mode: Edge-only or all processes.
   - Enter the CPU usage threshold (0-100).
   - View the list of matching processes, refresh, kill, or change filters as needed.

## Requirements
- .NET 9.0 or later
- Windows OS

## Example
```
Select process filter mode:
  e - List only Edge related processes
  a - List all processes
Enter your choice (e or a): e

Enter CPU threshold percentage (0-100): 10

Listing Edge renderer processes (CPU > 10%)
PID 12345   15.2%  Edge Tab Title
...
Total matching processes: 2

Options:
  r   - Refresh the list
  k   - Kill all matching processes
  1-100 - Show only processes using more than this % CPU
  f   - Change process filter mode
  q   - Quit
Enter your choice:
```

## License
See [LICENSE](LICENSE) for details.