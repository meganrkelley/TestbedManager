﻿using System.Windows;

namespace TestBedManager
{
	public partial class AccountInfoView
	{
		private RemoteComputer computer;

		public AccountInfoView()
		{
			InitializeComponent();
			Show();
		}

		public void InitializeInfo(RemoteComputer comp)
		{
			computer = comp;

			TextBoxHostname.Text = comp.hostname;
			TextBoxIpAddress.Text = comp.ipAddressStr;
			TextBoxUsername.Text = comp.credentials.UserName;
			PasswordBoxPassword.Password = comp.credentials.Password;
		}

		private void Button_Change_Click(object sender, RoutedEventArgs e)
		{
			computer.credentials.UserName = TextBoxUsername.Text;
			computer.credentials.Password = PasswordBoxPassword.Password;
			Close();
		}

		private void Button_Close_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}