﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WhisperDragonWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new WhisperDragonViewModel(tabSections, this);
		}

		void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			WhisperDragonViewModel current = (WhisperDragonViewModel)DataContext;

			CloseType closeType = current.CanExecuteClosing();
			
			if (closeType == CloseType.Cancel)
			{
				e.Cancel = true;
			}
			else if (closeType == CloseType.Close)
			{
				e.Cancel = false;
			}
			else if (closeType == CloseType.SaveAndClose)
			{
				e.Cancel = !current.ActualSaveCommonSecretsContainer();
			}
			else if (closeType == CloseType.SaveAs)
			{
				e.Cancel = !current.ActualSaveAsCommonSecretsContainer();
			}
		}
	}
}
