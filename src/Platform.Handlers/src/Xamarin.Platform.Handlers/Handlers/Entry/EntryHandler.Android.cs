using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class EntryHandler : AbstractViewHandler<IEntry, EditText>
	{
		OnEditorActionListener? _onEditorActionListener;

		protected override EditText CreateView()
		{
			var editText = new EditText(Context);

			_onEditorActionListener = new OnEditorActionListener(this);

			editText.AddTextChangedListener(_onEditorActionListener);
			editText.SetOnEditorActionListener(_onEditorActionListener);

			return editText;
		}

		public override void TearDown()
		{
			base.TearDown();

			_onEditorActionListener?.Dispose();
		}
	}

	class OnEditorActionListener : Java.Lang.Object, ITextWatcher, TextView.IOnEditorActionListener
	{
		readonly EntryHandler _entryHandler;

		public OnEditorActionListener(EntryHandler entryHandler)
		{
			_entryHandler = entryHandler;
		}

		public void AfterTextChanged(IEditable? s)
		{

		}

		public void BeforeTextChanged(Java.Lang.ICharSequence? s, int start, int count, int after)
		{
		
		}

		public bool OnEditorAction(TextView? textView, [GeneratedEnum] ImeAction actionId, KeyEvent? e)
		{
			return true;
		}

		public void OnTextChanged(Java.Lang.ICharSequence? s, int start, int before, int count)
		{
		
		}
	}
}