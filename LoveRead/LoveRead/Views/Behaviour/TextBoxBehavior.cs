using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoveRead.Views.Behaviour
{
    public static class TextBoxBehavior
    {
        public static readonly DependencyProperty TripleClickSelectAllProperty = DependencyProperty.RegisterAttached(
            "TripleClickSelectAll", typeof(bool), typeof(TextBoxBehavior), new PropertyMetadata(false, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBox tb))
                return;
            if ((bool)e.NewValue)
                tb.PreviewMouseLeftButtonDown += OnTextBoxMouseDown;
            else
                tb.PreviewMouseLeftButtonDown -= OnTextBoxMouseDown;
        }

        private static void OnTextBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 3)
                ((TextBox) sender).SelectAll();
        }

        public static void SetTripleClickSelectAll(DependencyObject element, bool value)
            => element.SetValue(TripleClickSelectAllProperty, value);

        public static bool GetTripleClickSelectAll(DependencyObject element)
            => (bool)element.GetValue(TripleClickSelectAllProperty);
    }
}
