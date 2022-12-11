using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TestWPF.Commands;
using TKomp.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TKomp.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using Caliburn.Micro;
using System.Collections.ObjectModel;

namespace TKomp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
	private readonly DevDataContext _db;
	private readonly string DefaultUsername = "DefaultUser";
	private readonly string DefaultPassword = "DefaultPassword123#";
	private bool isLoggedIn = false;
	public ICommand ConnectToDatabase { get; set; }
	public ICommand FetchData { get; set; }

	private string _connectionStatus;

	public string ConnectionStatus
	{
		get { return _connectionStatus; }
		set { 
			_connectionStatus = value; 
			OnPropertyChanged();
		}
	}


	private string _username;

	public string Username
	{
		get { return _username; }
		set { 
			_username = value;
			OnPropertyChanged();
		}
	}

	private string _password;

	public string Password
	{
		get { return _password; }
		set { 
			_password = value;
			OnPropertyChanged(nameof(Password));
		}
	}


	private ObservableCollection<ColumnProperties> _Columns;
	public ObservableCollection<ColumnProperties> Columns
	{
		get { return _Columns; }
		set { 
			_Columns = value;
			OnPropertyChanged();
		}
	}

	public MainViewModel()
	{
		_db = new();
		ConnectToDatabase = new RelayCommand(ConnectToDb, CanLogIn);
		FetchData = new RelayCommand(GetData, CanFetchData);
		Columns = new();
		Username = "";
		Password = "";
		ConnectionStatus = "";
	}
	private bool CanLogIn(object obj)
	{
		return !isLoggedIn;
	}
	private bool CanFetchData(object obj)
	{
		return isLoggedIn;
	}
	
	private void ConnectToDb(object obj)
	{
		if (Username == DefaultUsername && Password == DefaultPassword)
		{
			isLoggedIn = true;
			ConnectionStatus = "Connection OK";
		}
		else
		{
			isLoggedIn = false;
			ConnectionStatus = "Invalid credentials";
		}
	}

	private void GetData(object obj)
	{
		List<Type> _types = new()
		{
			typeof(TableA),
			typeof(TableB),
			typeof(TableC)
		};
		List<ColumnProperties> columns = GetIntTypeColumnsFromTable(_types);
		Columns = new(columns);
	}

	private List<ColumnProperties> GetIntTypeColumnsFromTable(List<Type> types)
	{
		List<ColumnProperties> toReturn = new();
		foreach (var type in types)
		{
			var table = _db.Model.FindEntityType(type);
			foreach (var prop in table.GetProperties())
			{
				var columnType = prop.GetColumnType();
				if (columnType == "int")
				{
					toReturn.Add(new ColumnProperties {
						TableName = table.GetTableName(),
						ColumnName = prop.GetColumnName(),
						TypeName = columnType 
					});
				}
			}
		}
		return toReturn;
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
