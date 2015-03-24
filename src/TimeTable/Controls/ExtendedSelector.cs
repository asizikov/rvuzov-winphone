using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace TimeTable.Controls
{
    public class ExtendedSelector : Microsoft.Phone.Controls.LongListSelector
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof (object), typeof (ExtendedSelector),
                new PropertyMetadata(default(object)));

        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof (SelectionMode), typeof (ExtendedSelector),
                new PropertyMetadata(SelectionMode.Single));

        public static readonly DependencyProperty RepositionOnAddStyleProperty =
            DependencyProperty.Register("RepositionOnAddStyle", typeof (PositionOnAdd), typeof (ExtendedSelector),
                new PropertyMetadata(PositionOnAdd.Default));

        public PositionOnAdd RepositionOnAddStyle
        {
            get { return (PositionOnAdd) GetValue(RepositionOnAddStyleProperty); }
            set { SetValue(RepositionOnAddStyleProperty, value); }
        }

        public SelectionMode SelectionMode
        {
            get { return (SelectionMode) GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public void ResetSelection()
        {
            base.SelectedItem = null;
        }

        public ExtendedSelector()
        {
            SelectionChanged += (sender, args) =>
            {
                if (SelectionMode == SelectionMode.Single)
                    SelectedItem = args.AddedItems[0];
                else if (SelectionMode == SelectionMode.Multiple)
                {
                    if (SelectedItem == null)
                    {
                        SelectedItem = new List<object>();
                    }

                    foreach (var item in args.AddedItems)
                    {
                        ((List<object>) SelectedItem).Add(item);
                    }

                    foreach (var removedItem in args.RemovedItems)
                    {
                        if (((List<object>) SelectedItem).Contains(removedItem))
                        {
                            ((List<object>) SelectedItem).Remove(removedItem);
                        }
                    }
                }
            };

            Loaded += (sender, args) =>
            {
                if (ItemsSource == null) return;
                ((INotifyCollectionChanged) ItemsSource).CollectionChanged += (sender2, args2) =>
                {
                    if (ItemsSource.Count > 0 && args2.NewItems != null)
                    {
                        switch (RepositionOnAddStyle)
                        {
                            case PositionOnAdd.NewItem:
                                int index = ItemsSource.IndexOf(args2.NewItems[0]);

                                if (index >= 0)
                                    ScrollTo(ItemsSource[index]);
                                break;
                            case PositionOnAdd.Top:
                                ScrollTo(ItemsSource[0]);
                                break;
                        }
                    }
                };
            };
        }
    }
}