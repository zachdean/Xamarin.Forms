using System.ComponentModel;

namespace System.Maui
{
    [TypeConverter(typeof(Xaml.TransformTypeConverter))]
    public class Transform
    {
        public Matrix Value { get; set; }
    }
}
