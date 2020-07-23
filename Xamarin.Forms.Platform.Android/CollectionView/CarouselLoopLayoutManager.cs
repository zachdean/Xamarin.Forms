using System;
using System.Collections.Generic;
using Android.Content;
#if __ANDROID_29__
using AndroidX.RecyclerView.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;
#else
using Android.Support.V7.Widget;
using static Android.Support.V7.Widget.RecyclerView;
#endif
using Android.Views;

using Android.OS;
using Math = Java.Lang.Math;

namespace Xamarin.Forms.Platform.Android
{
	internal class CarouselLoopLayoutManager : LayoutManager, RecyclerView.SmoothScroller.IScrollVectorProvider
	{

		public const int HORIZONTAL = OrientationHelper.Horizontal;
		public const int VERTICAL = OrientationHelper.Vertical;
		public const int MAX_VISIBLE_ITEMS = 2;
		public const int INVALID_POSITION = -1;
		const bool CIRCLE_LAYOUT = true;

		readonly int _orientation;
		readonly bool _circleLayout;
		readonly LayoutHelper _layoutHelper = new LayoutHelper(MAX_VISIBLE_ITEMS);
		readonly List<OnCenterItemSelectionListener> _onCenterItemSelectionListeners = new List<OnCenterItemSelectionListener>();

		bool _decoratedChildSizeInvalid;
		int _decoratedChildWidth;
		int _decoratedChildHeight;
		int _pendingScrollPosition;
		int _centerItemPosition = INVALID_POSITION;
		int _itemsCount;
		CarouselSavedState _pendingCarouselSavedState;
		PostLayoutListener _viewPostLayout;

		CarouselView _carousel;
		public CarouselLoopLayoutManager(CarouselView carousel, Context context) : this(HORIZONTAL, CIRCLE_LAYOUT)
		{
			_carousel = carousel;
		}

		CarouselLoopLayoutManager(int orientation, bool circleLayout)
		{
			if (HORIZONTAL != orientation && VERTICAL != orientation)
			{
				throw new ArgumentException("orientation should be HORIZONTAL or VERTICAL");
			}
			_orientation = orientation;
			_circleLayout = circleLayout;
			_pendingScrollPosition = INVALID_POSITION;
		}

		internal void SetPostLayoutListener(PostLayoutListener postLayoutListener)
		{
			_viewPostLayout = postLayoutListener;
			RequestLayout();
		}

		public void SetMaxVisibleItems(int maxVisibleItems)
		{
			if (0 >= maxVisibleItems)
			{
				throw new ArgumentException("maxVisibleItems can't be less then 1");
			}
			_layoutHelper.MaxVisibleItems = maxVisibleItems;
			RequestLayout();
		}

		public int GetMaxVisibleItems()
		{
			return _layoutHelper.MaxVisibleItems;
		}

		public override LayoutParams GenerateDefaultLayoutParams()
		{
			return new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
		}

		public int GetOrientation()
		{
			return _orientation;
		}

		public override bool CanScrollHorizontally()
		{
			return 0 != ChildCount && HORIZONTAL == _orientation;
		}

		public override bool CanScrollVertically()
		{
			return 0 != ChildCount && VERTICAL == _orientation;
		}

		public int GetCenterItemPosition()
		{
			return _centerItemPosition;
		}

		internal void AddOnItemSelectionListener(OnCenterItemSelectionListener onCenterItemSelectionListener)
		{
			_onCenterItemSelectionListeners.Add(onCenterItemSelectionListener);
		}

		internal void RemoveOnItemSelectionListener(OnCenterItemSelectionListener onCenterItemSelectionListener)
		{
			_onCenterItemSelectionListeners.Remove(onCenterItemSelectionListener);
		}

		public override void ScrollToPosition(int position)
		{
			if (0 > position)
			{
				throw new ArgumentException("position can't be less then 0. position is : " + position);
			}

			_pendingScrollPosition = position;
			RequestLayout();
		}

		public override void SmoothScrollToPosition(RecyclerView recyclerView, State state, int position)
		{
			LinearSmoothScroller linearSmoothScroller = new CustomLinearSmoothScroller(recyclerView.Context, this);

			linearSmoothScroller.TargetPosition = position;
			StartSmoothScroll(linearSmoothScroller);
		}

		public global::Android.Graphics.PointF ComputeScrollVectorForPosition(int targetPosition)
		{
			if (0 == ChildCount)
			{
				return null;
			}

			float directionDistance = GetScrollDirection(targetPosition);
			//noinspection NumericCastThatLosesPrecision
			int direction = (int)-Java.Lang.Math.Signum(directionDistance);

			if (HORIZONTAL == _orientation)
			{
				return new global::Android.Graphics.PointF(direction, 0);
			}
			else
			{
				return new global::Android.Graphics.PointF(0, direction);
			}
		}

		float GetScrollDirection(int targetPosition)
		{
			float currentScrollPosition = MakeScrollPositionInRange0ToCount(GetCurrentScrollPosition(), _itemsCount);

			if (_circleLayout)
			{
				float t1 = currentScrollPosition - targetPosition;
				float t2 = Math.Abs(t1) - _itemsCount;
				if (Math.Abs(t1) > Math.Abs(t2))
				{
					return Java.Lang.Math.Signum(t1) * t2;
				}
				else
				{
					return t1;
				}
			}
			else
			{
				return currentScrollPosition - targetPosition;
			}
		}

		public override int ScrollHorizontallyBy(int dx, Recycler recycler, State state)
		{
			if (VERTICAL == _orientation)
				return 0;
			return ScrollBy(dx, recycler, state);
		}

		public override int ScrollVerticallyBy(int dy, Recycler recycler, State state)
		{
			if (HORIZONTAL == _orientation)
				return 0;

			return ScrollBy(dy, recycler, state);
		}

		protected int ScrollBy(int diff, Recycler recycler, State state)
		{
			if (0 == _decoratedChildWidth || 0 == _decoratedChildHeight)
			{
				return 0;
			}
			if (0 == ChildCount || 0 == diff)
			{
				return 0;
			}

			int resultScroll;
			if (_circleLayout)
			{
				resultScroll = diff;

				_layoutHelper.mScrollOffset += resultScroll;

				int maxOffset = GetScrollItemSize() * _itemsCount;
				while (0 > _layoutHelper.mScrollOffset)
				{
					_layoutHelper.mScrollOffset += maxOffset;
				}
				while (_layoutHelper.mScrollOffset > maxOffset)
				{
					_layoutHelper.mScrollOffset -= maxOffset;
				}

				_layoutHelper.mScrollOffset -= resultScroll;
			}
			else
			{
				int maxOffset = GetMaxScrollOffset();

				if (0 > _layoutHelper.mScrollOffset + diff)
				{
					resultScroll = -_layoutHelper.mScrollOffset; //to make it 0
				}
				else if (_layoutHelper.mScrollOffset + diff > maxOffset)
				{
					resultScroll = maxOffset - _layoutHelper.mScrollOffset; //to make it maxOffset
				}
				else
				{
					resultScroll = diff;
				}
			}
			if (0 != resultScroll)
			{
				_layoutHelper.mScrollOffset += resultScroll;
				FillData(recycler, state);
			}
			return resultScroll;
		}

		public override void OnMeasure(Recycler recycler, State state, int widthSpec, int heightSpec)
		{
			_decoratedChildSizeInvalid = true;

			base.OnMeasure(recycler, state, widthSpec, heightSpec);
		}

		public override void OnAdapterChanged(Adapter oldAdapter, Adapter newAdapter)
		{
			base.OnAdapterChanged(oldAdapter, newAdapter);
			RemoveAllViews();
		}

		public override void OnLayoutChildren(Recycler recycler, State state)
		{
			if (0 == state.ItemCount)
			{
				RemoveAndRecycleAllViews(recycler);
				SelectItemCenterPosition(INVALID_POSITION);
				return;
			}

			if (0 == _decoratedChildWidth || _decoratedChildSizeInvalid)
			{
				var view = recycler.GetViewForPosition(0);
				AddView(view);
				MeasureChildWithMargins(view, 0, 0);

				int decoratedChildWidth = GetDecoratedMeasuredWidth(view);
				int decoratedChildHeight = GetDecoratedMeasuredHeight(view);
				RemoveAndRecycleView(view, recycler);

				if (0 != _decoratedChildWidth && (_decoratedChildWidth != decoratedChildWidth || _decoratedChildHeight != decoratedChildHeight))
				{
					if (INVALID_POSITION == _pendingScrollPosition && null == _pendingCarouselSavedState)
					{
						_pendingScrollPosition = _centerItemPosition;
					}
				}

				_decoratedChildWidth = decoratedChildWidth;
				_decoratedChildHeight = decoratedChildHeight;
				_decoratedChildSizeInvalid = false;
			}

			if (INVALID_POSITION != _pendingScrollPosition)
			{
				int itemsCount = state.ItemCount;
				_pendingScrollPosition = 0 == itemsCount ? INVALID_POSITION : Math.Max(0, Math.Min(itemsCount - 1, _pendingScrollPosition));
			}
			if (INVALID_POSITION != _pendingScrollPosition)
			{
				_layoutHelper.mScrollOffset = CalculateScrollForSelectingPosition(_pendingScrollPosition, state);
				_pendingScrollPosition = INVALID_POSITION;
				_pendingCarouselSavedState = null;
			}
			else if (null != _pendingCarouselSavedState)
			{
				_layoutHelper.mScrollOffset = CalculateScrollForSelectingPosition(_pendingCarouselSavedState.mCenterItemPosition, state);
				_pendingCarouselSavedState = null;
			}
			else if (state.DidStructureChange() && INVALID_POSITION != _centerItemPosition)
			{
				_layoutHelper.mScrollOffset = CalculateScrollForSelectingPosition(_centerItemPosition, state);
			}

			FillData(recycler, state);
		}

		int CalculateScrollForSelectingPosition(int itemPosition, RecyclerView.State state)
		{
			int fixedItemPosition = itemPosition < state.ItemCount ? itemPosition : state.ItemCount - 1;
			return fixedItemPosition * (VERTICAL == _orientation ? _decoratedChildHeight : _decoratedChildWidth);
		}

		void FillData(Recycler recycler, State state)
		{
			float currentScrollPosition = GetCurrentScrollPosition();

			GenerateLayoutOrder(currentScrollPosition, state);
			DetachAndScrapAttachedViews(recycler);

			RecyclerOldViews(recycler);

			int width = GetWidthNoPadding();
			int height = GetHeightNoPadding();
			if (LinearLayoutManager.Vertical == _orientation)
			{
				FillDataVertical(recycler, width, height);
			}
			else
			{
				FillDataHorizontal(recycler, width, height);
			}

			recycler.Clear();

			DetectOnItemSelectionChanged(currentScrollPosition, state);
		}

		void DetectOnItemSelectionChanged(float currentScrollPosition, RecyclerView.State state)
		{
			float absCurrentScrollPosition = MakeScrollPositionInRange0ToCount(currentScrollPosition, state.ItemCount);
			int centerItem = Java.Lang.Math.Round(absCurrentScrollPosition);

			if (_centerItemPosition != centerItem)
			{
				_centerItemPosition = centerItem;
				new Handler(Looper.MainLooper).Post(() => SelectItemCenterPosition(centerItem));
			}
		}

		void SelectItemCenterPosition(int centerItem)
		{
			foreach (var onCenterItemSelectionListener in _onCenterItemSelectionListeners)
			{
				onCenterItemSelectionListener.onCenterItemChanged(centerItem);
			}
		}

		void FillDataVertical(RecyclerView.Recycler recycler, int width, int height)
		{
			int start = (width - _decoratedChildWidth) / 2;
			int end = start + _decoratedChildWidth;

			int centerViewTop = (height - _decoratedChildHeight) / 2;

			for (int i = 0, count = _layoutHelper.mLayoutOrder.Length; i < count; ++i)
			{
				LayoutOrder layoutOrder = _layoutHelper.mLayoutOrder[i];
				int offset = GetCardOffsetByPositionDiff(layoutOrder.mItemPositionDiff);
				int top = centerViewTop + offset;
				int bottom = top + _decoratedChildHeight;
				FillChildItem(start, top, end, bottom, layoutOrder, recycler, i);
			}
		}

		void FillDataHorizontal(Recycler recycler, int width, int height)
		{
			int top = (height - _decoratedChildHeight) / 2;
			int bottom = top + _decoratedChildHeight;

			int centerViewStart = (width - _decoratedChildWidth) / 2;

			for (int i = 0, count = _layoutHelper.mLayoutOrder.Length; i < count; ++i)
			{
				LayoutOrder layoutOrder = _layoutHelper.mLayoutOrder[i];
				int offset = GetCardOffsetByPositionDiff(layoutOrder.mItemPositionDiff);
				int start = centerViewStart + offset;
				int end = start + _decoratedChildWidth;
				FillChildItem(start, top, end, bottom, layoutOrder, recycler, i);
			}
		}

		void FillChildItem(int start, int top, int end, int bottom, LayoutOrder layoutOrder, Recycler recycler, int i)
		{
			global::Android.Views.View view = BindChild(layoutOrder.mItemAdapterPosition, recycler);

			view.SetElevation(i);

			ItemTransformation transformation = null;
			if (null != _viewPostLayout)
			{
				transformation = _viewPostLayout.transformChild(view, layoutOrder.mItemPositionDiff, _orientation);
			}
			if (null == transformation)
			{
				view.Layout(start, top, end, bottom);
			}
			else
			{
				view.Layout(Math.Round(start + transformation.TranslationX), Math.Round(top + transformation.TranslationY),
						Math.Round(end + transformation.TranslationX), Math.Round(bottom + transformation.TranslationY));

				view.ScaleX = transformation.ScaleX;
				view.ScaleY = transformation.ScaleY;
			}
		}

		float GetCurrentScrollPosition()
		{
			int fullScrollSize = GetMaxScrollOffset();
			if (0 == fullScrollSize)
			{
				return 0;
			}
			return 1.0f * _layoutHelper.mScrollOffset / GetScrollItemSize();
		}

		int GetMaxScrollOffset()
		{
			return GetScrollItemSize() * (_itemsCount - 1);
		}

		void GenerateLayoutOrder(float currentScrollPosition, RecyclerView.State state)
		{
			_itemsCount = state.ItemCount;
			float absCurrentScrollPosition = MakeScrollPositionInRange0ToCount(currentScrollPosition, _itemsCount);
			int centerItem = Math.Round(absCurrentScrollPosition);

			if (_circleLayout && 1 < _itemsCount)
			{
				int layoutCount = Math.Min(_layoutHelper.MaxVisibleItems * 2 + 3, _itemsCount);// + 3 = 1 (center item) + 2 (addition bellow maxVisibleItems)

				_layoutHelper.InitLayoutOrder(layoutCount);

				int countLayoutHalf = layoutCount / 2;
				// before center item
				for (int i = 1; i <= countLayoutHalf; ++i)
				{
					int position = Math.Round(absCurrentScrollPosition - i + _itemsCount) % _itemsCount;
					_layoutHelper.SetLayoutOrder(countLayoutHalf - i, position, centerItem - absCurrentScrollPosition - i);
				}
				// after center item
				for (int i = layoutCount - 1; i >= countLayoutHalf + 1; --i)
				{
					int position = Math.Round(absCurrentScrollPosition - i + layoutCount) % _itemsCount;
					_layoutHelper.SetLayoutOrder(i - 1, position, centerItem - absCurrentScrollPosition + layoutCount - i);
				}
				_layoutHelper.SetLayoutOrder(layoutCount - 1, centerItem, centerItem - absCurrentScrollPosition);

			}
			else
			{
				int firstVisible = Math.Max(centerItem - _layoutHelper.MaxVisibleItems - 1, 0);
				int lastVisible = Math.Min(centerItem + _layoutHelper.MaxVisibleItems + 1, _itemsCount - 1);
				int layoutCount = lastVisible - firstVisible + 1;

				_layoutHelper.InitLayoutOrder(layoutCount);

				for (int i = firstVisible; i <= lastVisible; ++i)
				{
					if (i == centerItem)
					{
						_layoutHelper.SetLayoutOrder(layoutCount - 1, i, i - absCurrentScrollPosition);
					}
					else if (i < centerItem)
					{
						_layoutHelper.SetLayoutOrder(i - firstVisible, i, i - absCurrentScrollPosition);
					}
					else
					{
						_layoutHelper.SetLayoutOrder(layoutCount - (i - centerItem) - 1, i, i - absCurrentScrollPosition);
					}
				}
			}
		}


		public int GetWidthNoPadding()
		{
			return Width - PaddingStart - PaddingEnd;
		}

		public int GetHeightNoPadding()
		{
			return Height - PaddingEnd - PaddingStart;
		}

		global::Android.Views.View BindChild(int position, Recycler recycler)
		{
			global::Android.Views.View view = recycler.GetViewForPosition(position);

			AddView(view);
			MeasureChildWithMargins(view, 0, 0);

			return view;
		}

		void RecyclerOldViews(Recycler recycler)
		{
			//foreach (var viewHolder in recycler.ScrapList)
			//{

			//	int adapterPosition = viewHolder.AdapterPosition;
			//	var found = false;
			//	foreach (var layoutOrder in mLayoutHelper.mLayoutOrder)
			//	{
			//		if (layoutOrder.mItemAdapterPosition == adapterPosition)
			//		{
			//			found = true;
			//			break;
			//		}
			//	}
			//	if (!found)
			//	{
			//		recycler.RecycleView(viewHolder.ItemView);
			//	}
			//}
		}

		int GetCardOffsetByPositionDiff(float itemPositionDiff)
		{
			double smoothPosition = ConvertItemPositionDiffToSmoothPositionDiff(itemPositionDiff);

			int dimenDiff;
			if (LinearLayoutManager.Vertical == _orientation)
			{
				dimenDiff = (GetHeightNoPadding() - _decoratedChildHeight) / 2;
			}
			else
			{
				dimenDiff = (GetWidthNoPadding() - _decoratedChildWidth) / 2;
			}
			//noinspection NumericCastThatLosesPrecision
			return (int)Math.Round(Math.Signum(itemPositionDiff) * dimenDiff * smoothPosition);
		}

		double ConvertItemPositionDiffToSmoothPositionDiff(float itemPositionDiff)
		{
			// generally item moves the same way above center and bellow it. So we don't care about diff sign.
			float absIemPositionDiff = Math.Abs(itemPositionDiff);

			// we detect if this item is close for center or not. We use (1 / maxVisibleItem) ^ (1/3) as close definer.
			if (absIemPositionDiff > Java.Lang.StrictMath.Pow(1.0f / _layoutHelper.MaxVisibleItems, 1.0f / 3))
			{
				// this item is far from center line, so we should make it move like square root function
				return Java.Lang.StrictMath.Pow(absIemPositionDiff / _layoutHelper.MaxVisibleItems, 1 / 2.0f);
			}
			else
			{
				// this item is close from center line. we should slow it down and don't make it speed up very quick.
				// so square function in range of [0, (1/maxVisible)^(1/3)] is quite good in it;
				return Java.Lang.StrictMath.Pow(absIemPositionDiff, 2.0f);
			}
		}

		int GetScrollItemSize()
		{
			if (LinearLayoutManager.Vertical == _orientation)
			{
				return _decoratedChildHeight;
			}
			else
			{
				return _decoratedChildWidth;
			}
		}

		public override IParcelable OnSaveInstanceState()
		{
			if (null != _pendingCarouselSavedState)
			{
				return new CarouselSavedState(_pendingCarouselSavedState);
			}
			CarouselSavedState savedState = new CarouselSavedState(base.OnSaveInstanceState());
			savedState.mCenterItemPosition = _centerItemPosition;
			return savedState;
		}

		public override void OnRestoreInstanceState(IParcelable state)
		{
			if (state is CarouselSavedState)
			{
				_pendingCarouselSavedState = (CarouselSavedState)state;

				base.OnRestoreInstanceState(_pendingCarouselSavedState.mSuperState);
			}
			else
			{
				base.OnRestoreInstanceState(state);
			}
		}

		public int GetOffsetCenterView()
		{
			return Math.Round(GetCurrentScrollPosition()) * GetScrollItemSize() - _layoutHelper.mScrollOffset;
		}

		public int GetOffsetForCurrentView(global::Android.Views.View view)
		{
			int targetPosition = GetPosition(view);
			float directionDistance = GetScrollDirection(targetPosition);

			int distance = Math.Round(directionDistance * GetScrollItemSize());
			if (_circleLayout)
			{
				return distance;
			}
			else
			{
				return distance;
			}
		}

		static float MakeScrollPositionInRange0ToCount(float currentScrollPosition, int count)
		{
			float absCurrentScrollPosition = currentScrollPosition;
			while (0 > absCurrentScrollPosition)
			{
				absCurrentScrollPosition += count;
			}
			while (Math.Round(absCurrentScrollPosition) >= count)
			{
				absCurrentScrollPosition -= count;
			}
			return absCurrentScrollPosition;
		}
	}

	class CarouselSavedState : Java.Lang.Object, IParcelable
	{
		public IParcelable mSuperState;
		public int mCenterItemPosition;
		public CarouselSavedState(IParcelable superState)
		{
			mSuperState = superState;
		}

		public CarouselSavedState(Parcel inn)
		{
			var cc = Java.Lang.Class.FromType(typeof(IParcelable)).ClassLoader;
			mSuperState = (IParcelable)inn.ReadParcelable(cc);
			//mSuperState = inn.readParcelable(Parcelable.class.getClassLoader());
			mCenterItemPosition = inn.ReadInt();
		}

		protected CarouselSavedState(CarouselSavedState other)
		{
			mSuperState = other.mSuperState;
			mCenterItemPosition = other.mCenterItemPosition;
		}


		public int DescribeContents()
		{
			return 0;
		}

		public void WriteToParcel(Parcel parcel, [global::Android.Runtime.GeneratedEnum] ParcelableWriteFlags flags)
		{
			parcel.WriteParcelable(mSuperState, flags);
			parcel.WriteInt(mCenterItemPosition);
		}
	}

	internal class LayoutOrder
	{
		public int mItemAdapterPosition;
		public float mItemPositionDiff;
	}

	interface PostLayoutListener
	{
		ItemTransformation transformChild(global::Android.Views.View child, float itemPositionToCenterDiff, int orientation);
	}

	interface OnCenterItemSelectionListener
	{
		void onCenterItemChanged(int adapterPosition);
	}

	interface IOnCenterItemClickListener
	{
		void OnCenterItemClicked(RecyclerView recyclerView, CarouselLoopLayoutManager carouselLayoutManager, global::Android.Views.View v);
	}

	internal class LayoutHelper
	{
		public int MaxVisibleItems { get; set; }

		public int mScrollOffset { get; set; }

		public LayoutHelper(int maxVisibleItems)
		{
			MaxVisibleItems = maxVisibleItems;
		}

		internal LayoutOrder[] mLayoutOrder;

		List<WeakReference<LayoutOrder>> mReusedItems = new List<WeakReference<LayoutOrder>>();

		public void InitLayoutOrder(int layoutCount)
		{
			if (null == mLayoutOrder || mLayoutOrder.Length != layoutCount)
			{
				if (null != mLayoutOrder)
				{
					RecycleItems(mLayoutOrder);
				}
				mLayoutOrder = new LayoutOrder[layoutCount];
				FillLayoutOrder();
			}
		}

		public void SetLayoutOrder(int arrayPosition, int itemAdapterPosition, float itemPositionDiff)
		{
			LayoutOrder item = mLayoutOrder[arrayPosition];
			item.mItemAdapterPosition = itemAdapterPosition;
			item.mItemPositionDiff = itemPositionDiff;
		}

		bool HasAdapterPosition(int adapterPosition)
		{
			if (null != mLayoutOrder)
			{
				foreach (var layoutOrder in mLayoutOrder)
				{
					if (layoutOrder.mItemAdapterPosition == adapterPosition)
					{
						return true;
					}
				}
			}
			return false;
		}

		void RecycleItems(LayoutOrder[] layoutOrders)
		{
			foreach (var layoutOrder in layoutOrders)
			{
				mReusedItems.Add(new WeakReference<LayoutOrder>(layoutOrder));
			}
		}

		void FillLayoutOrder()
		{
			for (int i = 0, length = mLayoutOrder.Length; i < length; ++i)
			{
				if (null == mLayoutOrder[i])
				{
					mLayoutOrder[i] = CreateLayoutOrder();
				}
			}
		}

		LayoutOrder CreateLayoutOrder()
		{
			var iterator = mReusedItems.GetEnumerator();
			while (iterator.MoveNext())
			{
				WeakReference<LayoutOrder> layoutOrderWeakReference = iterator.Current;
				LayoutOrder layoutOrder;

				if (layoutOrderWeakReference.TryGetTarget(out layoutOrder))
				{
					return layoutOrder;
				}
			}
			return new LayoutOrder();
		}
	}

	internal class ItemTransformation
	{
		public float ScaleX;
		public float ScaleY;
		public float TranslationX;
		public float TranslationY;

		public ItemTransformation(float scaleX, float scaleY, float translationX, float translationY)
		{
			ScaleX = scaleX;
			ScaleY = scaleY;
			TranslationX = translationX;
			TranslationY = translationY;
		}
	}

	internal class CustomLinearSmoothScroller : LinearSmoothScroller
	{
		readonly CarouselLoopLayoutManager _manager;
		public CustomLinearSmoothScroller(Context context, CarouselLoopLayoutManager carouselLoopLayoutManager) : base(context)
		{
			_manager = carouselLoopLayoutManager;
		}

		public override int CalculateDyToMakeVisible(global::Android.Views.View view, int snapPreference)
		{
			if (!_manager.CanScrollVertically())
			{
				return 0;
			}

			return _manager.GetOffsetForCurrentView(view);
		}

		public override int CalculateDxToMakeVisible(global::Android.Views.View view, int snapPreference)
		{
			if (!_manager.CanScrollHorizontally())
			{
				return 0;
			}
			return _manager.GetOffsetForCurrentView(view);
		}
	}

	internal class CarouselZoomPostLayoutListener : PostLayoutListener
	{
		public ItemTransformation transformChild(global::Android.Views.View child, float itemPositionToCenterDiff, int orientation)
		{
			float scale = (float)(2 * (2 * -Java.Lang.StrictMath.Atan(Java.Lang.Math.Abs(itemPositionToCenterDiff) + 1.0) / Math.Pi + 1));

			// because scaling will make view smaller in its center, then we should move this item to the top or bottom to make it visible
			float translateY;
			float translateX;
			if (CarouselLoopLayoutManager.VERTICAL == orientation)
			{
				float translateYGeneral = child.MeasuredHeight * (1 - scale) / 2f;
				translateY = Java.Lang.Math.Signum(itemPositionToCenterDiff) * translateYGeneral;
				translateX = 0;
			}
			else
			{
				float translateXGeneral = child.MeasuredWidth * (1 - scale) / 2f;
				translateX = Java.Lang.Math.Signum(itemPositionToCenterDiff) * translateXGeneral;
				translateY = 0;
			}

			return new ItemTransformation(scale, scale, translateX, translateY);
		}
	}

	internal abstract class CarouselChildSelectionListener
	{
		internal RecyclerView _recyclerView;
		internal CarouselLoopLayoutManager _carouselLayoutManager;

		global::Android.Views.View.IOnClickListener mOnClickListener;
		protected CarouselChildSelectionListener(RecyclerView recyclerView, CarouselLoopLayoutManager carouselLayoutManager)
		{
			_recyclerView = recyclerView;
			_carouselLayoutManager = carouselLayoutManager;

			mOnClickListener = new CustomOnClickListener(this);
			_recyclerView.AddOnChildAttachStateChangeListener(new CustomAttachStateListener(mOnClickListener));
		}

		public abstract void OnCenterItemClicked(RecyclerView recyclerView, CarouselLoopLayoutManager carouselLayoutManager, global::Android.Views.View v);

		public abstract void OnBackItemClicked(RecyclerView recyclerView, CarouselLoopLayoutManager carouselLayoutManager, global::Android.Views.View v);
	}

	class CustomOnClickListener : Java.Lang.Object, global::Android.Views.View.IOnClickListener
	{
		readonly CarouselChildSelectionListener _childSelectionListener;

		public CustomOnClickListener(CarouselChildSelectionListener childSelectionListener)
		{
			_childSelectionListener = childSelectionListener;
		}

		public void OnClick(global::Android.Views.View v)
		{
			RecyclerView.ViewHolder holder = _childSelectionListener._recyclerView.GetChildViewHolder(v);
			int position = holder.AdapterPosition;

			if (position == _childSelectionListener._carouselLayoutManager.GetCenterItemPosition())
			{
				_childSelectionListener.OnCenterItemClicked(_childSelectionListener._recyclerView, _childSelectionListener._carouselLayoutManager, v);
			}
			else
			{
				_childSelectionListener.OnBackItemClicked(_childSelectionListener._recyclerView, _childSelectionListener._carouselLayoutManager, v);
			}
		}
	}

	class CustomAttachStateListener : Java.Lang.Object, RecyclerView.IOnChildAttachStateChangeListener
	{
		readonly global::Android.Views.View.IOnClickListener _onClickListener;

		public CustomAttachStateListener(global::Android.Views.View.IOnClickListener mOnClickListener)
		{
			_onClickListener = mOnClickListener;
		}

		public void OnChildViewAttachedToWindow(global::Android.Views.View view)
		{
			view.SetOnClickListener(_onClickListener);
		}

		public void OnChildViewDetachedFromWindow(global::Android.Views.View view)
		{
			view.SetOnClickListener(null);
		}
	}

	class CenterScrollListener : RecyclerView.OnScrollListener
	{
		bool _autoSet = true;

		public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
		{
			base.OnScrollStateChanged(recyclerView, newState);
			RecyclerView.LayoutManager layoutManager = recyclerView.GetLayoutManager();
			if (!(layoutManager is CarouselLoopLayoutManager))
			{
				_autoSet = true;
				return;
			}

			var lm = (CarouselLoopLayoutManager)layoutManager;
			if (!_autoSet)
			{
				if (RecyclerView.ScrollStateIdle == newState)
				{
					int scrollNeeded = lm.GetOffsetCenterView();
					if (CarouselLoopLayoutManager.HORIZONTAL == lm.GetOrientation())
					{
						recyclerView.SmoothScrollBy(scrollNeeded, 0);
					}
					else
					{
						recyclerView.SmoothScrollBy(0, scrollNeeded);
					}
					_autoSet = true;
				}
			}
			if (RecyclerView.ScrollStateDragging == newState || RecyclerView.ScrollStateSettling == newState)
			{
				_autoSet = false;
			}
		}
	}

	internal class DefaultChildSelectionListener : CarouselChildSelectionListener
	{
		IOnCenterItemClickListener _onCenterItemClickListener;

		public DefaultChildSelectionListener(IOnCenterItemClickListener onCenterItemClickListener, RecyclerView recyclerView, CarouselLoopLayoutManager carouselLayoutManager) : base(recyclerView, carouselLayoutManager)
		{
			_onCenterItemClickListener = onCenterItemClickListener;
		}


		public static DefaultChildSelectionListener InitCenterItemListener(IOnCenterItemClickListener onCenterItemClickListener, RecyclerView recyclerView, CarouselLoopLayoutManager carouselLayoutManager)
		{
			return new DefaultChildSelectionListener(onCenterItemClickListener, recyclerView, carouselLayoutManager);
		}


		public override void OnBackItemClicked(RecyclerView recyclerView, CarouselLoopLayoutManager carouselLayoutManager, global::Android.Views.View v)
		{
			recyclerView.SmoothScrollToPosition(carouselLayoutManager.GetPosition(v));
		}

		public override void OnCenterItemClicked(RecyclerView recyclerView, CarouselLoopLayoutManager carouselLayoutManager, global::Android.Views.View v)
		{
			_onCenterItemClickListener.OnCenterItemClicked(recyclerView, carouselLayoutManager, v);
		}
	}

	internal class CustomOnCenterItemClickListener : IOnCenterItemClickListener
	{
		public void OnCenterItemClicked(RecyclerView recyclerView, CarouselLoopLayoutManager carouselLayoutManager, global::Android.Views.View v)
		{
			int position = recyclerView.GetChildLayoutPosition(v);

			System.Diagnostics.Debug.WriteLine($"Item cliccked {position}");
		}
	}

	class CustomOnCenterItemSelectionListener : OnCenterItemSelectionListener
	{
		Adapter _adapter;
		public CustomOnCenterItemSelectionListener(Adapter adapter)
		{
			_adapter = adapter;
		}
		public void onCenterItemChanged(int adapterPosition)
		{
			if (CarouselLoopLayoutManager.INVALID_POSITION != adapterPosition)
			{
				//		{
				//	int value = _adapter[adapterPosition];
			}
		}
	}

}