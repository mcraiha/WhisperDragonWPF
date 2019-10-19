using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using WhisperDragonWPF;

public class CreateKeyDerivationFunctionViewModel : INotifyPropertyChanged
{
	public ObservableCollection<string> Algorithms { get; }
	public string selectedAlgorithm;

	public string SelectedAlgorithm
    {
        get
        {
            return this.selectedAlgorithm;
        }
        set
        {
            if (this.selectedAlgorithm != value)
            {
                this.selectedAlgorithm = value;
                OnPropertyChanged(nameof(SelectedAlgorithm));
            }
        }
    }

	public ObservableCollection<string> PseudorandomFunctions { get; }

	public string selectedPseudorandomFunction;

	public string SelectedPseudorandomFunction
	{
        get
        {
            return this.selectedPseudorandomFunction;
        }
        set
        {
            if (this.selectedPseudorandomFunction != value)
            {
                this.selectedPseudorandomFunction = value;
                OnPropertyChanged(nameof(SelectedPseudorandomFunction));
            }
        }
    }

	private byte[] salt;

	public string Salt
	{
        get
        {
            return BitConverter.ToString(this.salt).Replace("-", string.Empty);
        }
    }

	public int Iterations { get; set; }

	public string Identifier { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	private readonly Action callOnClose;

	public CreateKeyDerivationFunctionViewModel()
	{
		Algorithms = this.GenerateAlgorithms();
		this.selectedAlgorithm = Algorithms[0];

		PseudorandomFunctions = this.GeneratePseudorandomFunctions();
		this.selectedPseudorandomFunction = PseudorandomFunctions[0];

		this.salt = new byte[16] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160 };
	}

	private ObservableCollection<string> GenerateAlgorithms()
	{
		var returnValue = new ObservableCollection<string>();

		//foreach ()
		returnValue.Add("PBKDF2");

		return returnValue;
	}

	private ObservableCollection<string> GeneratePseudorandomFunctions()
	{
		var returnValue = new ObservableCollection<string>();

		returnValue.Add("HMAC-SHA256");
		returnValue.Add("HMAC-SHA512");

		return returnValue;
	}

	#region Buttons

	private ICommand createCommand;
	public ICommand CreateCommand
	{
		get
		{
			return createCommand 
				?? (createCommand = new ActionCommand(() =>
				{
					
				}));
		}
	}

	private ICommand cancelCommand;
	public ICommand CancelCommand
	{
		get
		{
			return cancelCommand 
				?? (cancelCommand = new ActionCommand(() =>
				{
					
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