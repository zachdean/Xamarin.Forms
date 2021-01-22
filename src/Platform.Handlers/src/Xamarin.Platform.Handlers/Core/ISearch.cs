using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public interface ISearch : ITextInput 
	{
		public ICommand SearchCommand { get; }
		public object SearchCommandParameter { get; }
		public Color CancelButtonColor { get; }
	}
}