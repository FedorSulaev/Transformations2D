using System.ComponentModel;

namespace Transformations2D.WPF.Resourses.MVVM
{
	public class PropertyChangedImplementation : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}
	}
}
