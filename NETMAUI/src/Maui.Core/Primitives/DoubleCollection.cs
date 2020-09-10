using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Maui
{
    [TypeConverter(typeof(Xaml.DoubleCollectionConverter))]
    public sealed class DoubleCollection : ObservableCollection<double>
    {

    }
}