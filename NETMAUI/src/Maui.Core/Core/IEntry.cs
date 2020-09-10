namespace System.Maui
{
    public interface IEntry : ITextInput
    {
        bool IsPassword { get; }
        ReturnType ReturnType { get; }
        int CursorPosition { get; set; }
        int SelectionLength { get; set; }
        bool IsTextPredictionEnabled { get; }
        ClearButtonVisibility ClearButtonVisibility { get; }
    }
}
