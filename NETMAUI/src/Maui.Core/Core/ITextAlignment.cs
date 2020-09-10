namespace System.Maui
{
    public interface ITextAlignment : IView
    {
        TextAlignment HorizontalTextAlignment { get; }

        TextAlignment VerticalTextAlignment { get; }
    }
}