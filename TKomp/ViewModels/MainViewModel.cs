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

namespace TKomp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
	private readonly DevDataContext _db;
	public ICommand ConnectToDatabase { get; set; }
	public MainViewModel()
	{
		_db = new();
		ConnectToDatabase = new RelayCommand(ConnectToDb);
	}

	private void ConnectToDb(object obj)
	{
		List<Type> _types = new()
		{
			typeof(TableA),
			typeof(TableB),
			typeof(TableC)
		};
		List<Tuple<string, string, string>> Columns = GetIntTypeColumnsFromTable(_types);
		
		MessageBox.Show("done");
	}

	private List<Tuple<string, string, string>> GetIntTypeColumnsFromTable(List<Type> types)
	{
		List<Tuple<string, string, string>> toReturn = new();
		foreach (var type in types)
		{
			var table = _db.Model.FindEntityType(type);
			foreach (var prop in table.GetProperties())
			{
				var columnType = prop.GetColumnType();
				if (columnType == "int")
				{
					toReturn.Add(new Tuple<string, string, string>(table.GetTableName(), prop.GetColumnName(), columnType));
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
