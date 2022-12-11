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
	public ICommand ConnectToDatabase { get; set; }
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
		ConnectToDatabase = new RelayCommand(ConnectToDb);
		Columns = new();
	}

	private void ConnectToDb(object obj)
	{
		List<Type> _types = new()
		{
			typeof(TableA),
			typeof(TableB),
			typeof(TableC)
		};
		List<ColumnProperties> columns = GetIntTypeColumnsFromTable(_types);
		Columns = new(columns);
		MessageBox.Show("done");
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
