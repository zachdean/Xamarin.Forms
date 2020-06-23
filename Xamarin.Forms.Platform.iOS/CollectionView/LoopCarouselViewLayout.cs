using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Forms.Platform.iOS
{
	public class LoopCarouselViewLayout : CarouselViewLayout
	{
        CarouselView _carouselView;

        public LoopCarouselViewLayout(ItemsLayout itemsLayout, CarouselView carouselView) : base(itemsLayout, carouselView)
		{
            _carouselView = carouselView;
        }

		public override void PrepareLayout()
		{
            if (_carouselView.Loop)
            {
                if (ScrollDirection == UICollectionViewScrollDirection.Vertical)
                {
                    if (CollectionView.ContentOffset.Y <= 0.0f)
                        CollectionView.SetContentOffset(new CGPoint(CollectionView.ContentOffset.X, base.CollectionViewContentSize.Height + MinimumLineSpacing), false);
                    else if (CollectionView.ContentOffset.Y > base.CollectionViewContentSize.Height + MinimumLineSpacing)
                        CollectionView.SetContentOffset(new CGPoint(CollectionView.ContentOffset.X, 0.0f), false);
                }
                else
                {
                    if (CollectionView.ContentOffset.X <= 0.0f)
                        CollectionView.SetContentOffset(new CGPoint(base.CollectionViewContentSize.Width + MinimumLineSpacing, CollectionView.ContentOffset.Y), false);
                    else if (CollectionView.ContentOffset.X > base.CollectionViewContentSize.Width + MinimumLineSpacing)
                        CollectionView.SetContentOffset(new CGPoint(0.0f, CollectionView.ContentOffset.Y), false);
                }
            }

            base.PrepareLayout();
		}

		public override CGSize CollectionViewContentSize
		{
			get
            {
                if (_carouselView.Loop)
                {
                    CGSize contentSize = base.CollectionViewContentSize;

                    // We add the height (or width) of the collection view to the content size to allow us to seemlessly wrap without any screen artifacts
                    if (ScrollDirection == UICollectionViewScrollDirection.Vertical)
                    {
                        contentSize = new CGSize(contentSize.Width, contentSize.Height + CollectionView.Bounds.Size.Height + MinimumLineSpacing);
                    }
                    else
                    {
                        contentSize = new CGSize(contentSize.Width + CollectionView.Bounds.Size.Width + MinimumLineSpacing, contentSize.Height);
                    }

                    return contentSize;
                }

                return base.CollectionViewContentSize;
            }
		}

		public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
        {
            if (_carouselView.Loop)
            {
                if (ScrollDirection == UICollectionViewScrollDirection.Vertical)
                {
                    if (newBounds.Y <= CollectionView.Bounds.Size.Height)
                        return true;

                    if (newBounds.Y >= base.CollectionViewContentSize.Height - CollectionView.Bounds.Size.Height)
                        return true;
                }
                else
                {
                    if (newBounds.X <= CollectionView.Bounds.Size.Width)
                        return true;

                    if (newBounds.X >= base.CollectionViewContentSize.Width - CollectionView.Bounds.Size.Width)
                        return true;
                }
            }

            return base.ShouldInvalidateLayoutForBoundsChange(newBounds);
		}

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var layoutAttributes = base.LayoutAttributesForElementsInRect(rect);
            if (_carouselView.Loop)
            {
                if (ScrollDirection == UICollectionViewScrollDirection.Vertical)
                {
                    UICollectionViewLayoutAttributes[] wrappingAttributes = base.LayoutAttributesForElementsInRect(new CGRect(
                        rect.X,
                        rect.Y - base.CollectionViewContentSize.Height,
                        rect.Size.Width,
                        rect.Size.Height));

                    foreach (UICollectionViewLayoutAttributes attributes in wrappingAttributes)
                        attributes.Center = new CGPoint(attributes.Center.X, attributes.Center.Y + base.CollectionViewContentSize.Height + MinimumLineSpacing);

                    //layoutAttributes = layoutAttributes.arrayByAddingObjectsFromArray(wrappingAttributes);
                }
                else
                {
                    UICollectionViewLayoutAttributes[] wrappingAttributes = base.LayoutAttributesForElementsInRect(new CGRect(
                        rect.X - base.CollectionViewContentSize.Width,
                        rect.Y,
                        rect.Size.Width,
                        rect.Size.Height));


                    foreach (UICollectionViewLayoutAttributes attributes in wrappingAttributes)
                        attributes.Center = new CGPoint(attributes.Center.X + base.CollectionViewContentSize.Width + MinimumLineSpacing, attributes.Center.Y);

                    //var section = 0;
                    var itemCount = layoutAttributes.GetLength(0) + wrappingAttributes.GetLength(0);//CollectionView.NumberOfItemsInSection(section);
                    var layoutAttributesForAllCells = new UICollectionViewLayoutAttributes[itemCount];

                    layoutAttributes.CopyTo(layoutAttributesForAllCells, 0);
                    wrappingAttributes.CopyTo(layoutAttributesForAllCells, layoutAttributes.GetLength(0));

                    return layoutAttributesForAllCells;
                }
            }

            return layoutAttributes;
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
        {
            if (_carouselView.Loop)
            {
                UICollectionViewLayoutAttributes layoutAttributes = base.LayoutAttributesForItem(indexPath);
                layoutAttributes.Center = new CGPoint(layoutAttributes.Center.X + CollectionView.Bounds.Size.Width, layoutAttributes.Center.Y);
                return layoutAttributes;
            }

            return base.LayoutAttributesForItem(indexPath);
        }
	}
}
