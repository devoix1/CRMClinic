using System.ComponentModel;

namespace CRMBase.Model
{
	public class Avatar : HashComparator, INotifyPropertyChanged
	{
		private byte[] image;
		public byte[] Image
		{
			get => image;
			set
			{
				image = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
