using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using JetBrains.Annotations;
using Microsoft.Phone.Controls;

namespace TimeTable.Utils
{
    public static class TombstoneHelper
    {
        public static void RestoreState(this PhoneApplicationPage page, Pivot pivot)
        {
            RoutedEventHandler handler = null;
            var selectedIndex = page.GetPivotSelectedIndex(pivot);
            if (!selectedIndex.HasValue) return;

            handler = (s, e) => pivot.SelectedIndex = selectedIndex.Value;
            pivot.Loaded += handler;
        }

        public static object RestoreState([NotNull] this PhoneApplicationPage page, [NotNull] string key)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            if (page.State.ContainsKey(key))
            {
                return page.State[key];
            }
            return null;
        }


        public static void RestoreState([NotNull] this PhoneApplicationPage page, [NotNull] TextBox textBox)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (textBox == null) throw new ArgumentNullException("textBox");
            string key = textBox.Name + "_Text";
            if (page.State.ContainsKey(key))
            {
                textBox.Text = page.State[key].ToString();
            }
        }

        public static void SaveState([NotNull] this PhoneApplicationPage page,
            [NotNull] Pivot pivot)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (pivot == null) throw new ArgumentNullException("pivot");
            string key = pivot.Name + "_SelectedIndex";
            if (pivot.SelectedIndex >= 0)
            {
                if (page.State.ContainsKey(key))
                {
                    page.State[key] = pivot.SelectedIndex;
                }
                else
                {
                    page.State.Add(key, pivot.SelectedIndex);
                }
            }
        }


        public static void SaveState([NotNull] this PhoneApplicationPage page, [NotNull] ScrollViewer scrollViewer)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (scrollViewer == null) throw new ArgumentNullException("scrollViewer");

            string key = scrollViewer.Name + "_VerticalOffset";
            if (scrollViewer.VerticalOffset > 0.0)
            {
                if (page.State.ContainsKey(key))
                {
                    page.State[key] = scrollViewer.VerticalOffset;
                }
                else
                {
                    page.State.Add(key, scrollViewer.VerticalOffset);
                }
            }
        }

        public static void SaveState([NotNull] this PhoneApplicationPage page, [NotNull] TextBox textBox)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (textBox == null) throw new ArgumentNullException("textBox");
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (page.State.ContainsKey(textBox.Name + "_Text"))
                {
                    page.State[textBox.Name + "_Text"] = textBox.Text;
                }
                else
                {
                    page.State.Add(textBox.Name + "_Text", textBox.Text);
                }
            }
        }

        public static void SaveState([NotNull] this PhoneApplicationPage page, [NotNull] string key,
            [NotNull] object value)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");
            if (page.State.ContainsKey(key))
            {
                page.State[key] = value;
            }
            else
            {
                page.State.Add(key, value);
            }
        }

        public static bool ShouldTombstone([NotNull] this PhoneApplicationPage page,
            [NotNull] NavigatingCancelEventArgs e)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (e == null) throw new ArgumentNullException("e");

            return !e.IsNavigationInitiator;
        }

        private static int? GetPivotSelectedIndex(this PhoneApplicationPage page, FrameworkElement pivot)
        {
            if (((page != null) && (pivot != null)) && (page.State != null))
            {
                var key = pivot.Name + "_SelectedIndex";
                if (page.State.ContainsKey(key))
                {
                    return int.Parse(page.State[key].ToString());
                }
            }
            return null;
        }
    }
}
