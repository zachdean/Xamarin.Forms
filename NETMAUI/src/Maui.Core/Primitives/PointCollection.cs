using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Maui
{
    [TypeConverter(typeof(Xaml.PointCollectionConverter))]
    public sealed class PointCollection : ObservableCollection<Point>
    {

    }
}