using System;
using System.Windows.Input;
using System.ComponentModel;
using WhisperDragonWPF;

public class BenchmarkViewModel : INotifyPropertyChanged
{
	public string Description { get; set; } = "Pressing benchmark button will start 5 second benchmark of PBKDF2 key derivation";

	public string Result { get; set; } = "Press benchmark button to start";

	private readonly Action closeWindow;

	public event PropertyChangedEventHandler PropertyChanged;

	public BenchmarkViewModel(Action callToClose)
	{
		this.closeWindow = callToClose;
	}
	
	#region Buttons

	private ICommand benchmarkCommand;
	public ICommand BenchmarkCommand
	{
		get
		{
			return benchmarkCommand 
				?? (benchmarkCommand = new ActionCommand(() =>
				{
					Result = "Started";
					OnPropertyChanged(nameof(Result));
					int runs = Benchmarker.Benchmark(5000, 100000);
					Result = $"In 5 seconds your computer run {runs} PBKDF2 brute force attempts";
					OnPropertyChanged(nameof(Result));
				}));
		}
	}

	private ICommand stopBenchmarkCommand;
	public ICommand StopBenchmarkCommand
	{
		get
		{
			return stopBenchmarkCommand 
				?? (stopBenchmarkCommand = new ActionCommand(() =>
				{
					Result = "Stopped";
					OnPropertyChanged(nameof(Result));
				}));
		}
	}

	private ICommand okCommand;
	public ICommand OkCommand
	{
		get
		{
			return okCommand 
				?? (okCommand = new ActionCommand(() =>
				{
					this.closeWindow();
				}));
		}
	}

	#endregion // Buttons

	#region Property changed

	// Create the OnPropertyChanged method to raise the event
	protected void OnPropertyChanged(string name)
	{
		PropertyChangedEventHandler handler = PropertyChanged;
		if (handler != null)
		{
			handler(this, new PropertyChangedEventArgs(name));
		}
	}

	#endregion // Property changed
}