using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace ItemsControlDemo
{
    internal class SimpleCommand : ICommand
    {
        private readonly Func<object, bool> _myPredicate;
        private readonly Action<object> _myAction;

        public SimpleCommand(Action<object> myAction, Func<object, bool> myPredicate = null)
        {
            _myAction = myAction ?? throw new ArgumentNullException(nameof(myAction));
            _myPredicate = myPredicate ?? DefaultPredicate;
        }

        public bool CanExecute(object parameter)
        {
            return _myPredicate(parameter);
        }

        public void Execute(object parameter)
        {
            _myAction(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private static bool DefaultPredicate(object parameter)
        {
            return true;
        }
    }

    public class MyModel : DependencyObject
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(MyModel),
            new FrameworkPropertyMetadata("My Text")
        );

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ICommand AppendSuffixClickAction { get; }
        public ICommand RemoveClickAction { get; }

        public MyModel()
        {
            AppendSuffixClickAction = new SimpleCommand(MyAppendSuffixAction);
            RemoveClickAction = new SimpleCommand(MyRemoveAction, MyRemovePredicate);
        }

        private bool MyRemovePredicate(object arg)
        {
            // можно удалить только если хотя-бы раз добавили !.
            return Text.Contains('!');
        }

        private void MyAppendSuffixAction(object p)
        {
            Text = $"{Text}!";
        }

        private void MyRemoveAction(object obj)
        {
            var window = obj as MainWindow;
            Debug.Assert(window != null, nameof(window) + " != null");
            window.Items.Remove(this);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ObservableCollection<MyModel> Items { get; } = new ObservableCollection<MyModel>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Items.Add(new MyModel());
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            Items.Add(new MyModel());
        }
    }
}
