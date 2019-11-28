using MouseTrap.Binding;
using MouseTrap.Data;
using MouseTrap.UserInterface;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace MouseTrap.ViewModels
{
	public class WindowList : IViewModel, INotifyPropertyChanged
	{
		private IWindowListItem _selectedWindow;
		private readonly ObservableCollection<IWindowListItem> _windowListItems;

		public WindowList()
		{
			_windowListItems = new ObservableCollection<IWindowListItem>();
			DataSource = new ListCollectionView(_windowListItems);
			DataSource.GroupDescriptions.Add(new PropertyGroupDescription("IsMinimized", new MinimizedValueConverter()));
			DataSource.SortDescriptions.Add(new SortDescription("IsMinimized", ListSortDirection.Ascending));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ViewType ViewType => ViewType.WindowList;

		public ICollectionView DataSource { get; }

		public ObservableCollection<IWindowListItem> WindowListItems
		{
			get => _windowListItems;
			set
			{
				_windowListItems.Clear();
				foreach (var item in value)
				{
					_windowListItems.Add(item);
				}
			}
		}

		public IWindowListItem SelectedWindow
		{
			get => _selectedWindow;
			set
			{
				_selectedWindow = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedWindow)));
			}
		}
	}
}
