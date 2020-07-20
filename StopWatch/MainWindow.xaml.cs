using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StopWatchAndTimer
{
    internal class CommandAction : ICommand
    {
        private readonly Action<object> _myAction;

        public CommandAction(Action<object> myAction)
        {
            _myAction = myAction ?? throw new ArgumentNullException(nameof(myAction));
        }

        public bool CanExecute(object parameter)
        {
            return true;
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
    }

    public class StopWatch : DependencyObject
    {
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(string), typeof(StopWatch), new FrameworkPropertyMetadata("00:00:00.00"));
        public static readonly DependencyProperty StopWatchEnabledProperty = DependencyProperty.Register(
            "StopWatchEnabled", typeof(bool), typeof(StopWatch), new FrameworkPropertyMetadata(false));
        private readonly DispatcherTimer timer;
        private DateTime dateTime;
        public ICommand RunPauseAction { get; }
        public ICommand ResetAction { get; }
        public ICommand RemoveAction { get; }

        public StopWatch()
        {
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 10)
            };
            dateTime = new DateTime();
            timer.Tick += new EventHandler((object source, EventArgs e) =>
            {
                dateTime = dateTime.AddMilliseconds(10);
                Time = dateTime.ToString("HH:mm:ss.ff");
            });

            RunPauseAction = new CommandAction((object obj) =>
            {
                if (StopWatchEnabled)
                    this.Pause();
                else
                    this.Start();
            });

            ResetAction = new CommandAction((object obj) =>
            {
                this.Clear();
            });

            RemoveAction = new CommandAction((object obj) =>
            {
                if (obj != null)
                {
                    var window = obj as MainWindow;
                    window.StopWatchItems.Remove(this);
                }
            });
        }

        public void Clear()
        {
            timer.Stop();
            dateTime = new DateTime();
            Time = dateTime.ToString("HH:mm:ss.ff");
            StopWatchEnabled = false;
        }

        public void Start()
        {
            timer.Start();
            StopWatchEnabled = true;
        }

        public void Pause()
        {
            timer.Stop();
            StopWatchEnabled = false;
        }

        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public bool StopWatchEnabled
        {
            get { return (bool)GetValue(StopWatchEnabledProperty); }
            set { SetValue(StopWatchEnabledProperty, value); }
        }
    }

    public class MyTimer : DependencyObject
    {
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(string), typeof(MyTimer), new FrameworkPropertyMetadata("00:00:00.00"));
        public static readonly DependencyProperty HoursProperty = DependencyProperty.Register(
            "Hours", typeof(int), typeof(MyTimer), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty MinutesProperty = DependencyProperty.Register(
            "Minutes", typeof(int), typeof(MyTimer), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty SecondsProperty = DependencyProperty.Register(
            "Seconds", typeof(int), typeof(MyTimer), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty TimerEnabledProperty = DependencyProperty.Register(
            "TimerEnabled", typeof(bool), typeof(MyTimer), new FrameworkPropertyMetadata(false));
        private readonly DispatcherTimer timer;
        internal TimeSpan timeSpan;
        public ICommand RunPauseAction { get; }
        public ICommand SetTimeAction { get; }
        public ICommand RemoveAction { get; }

        public MyTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 10)
            };
            timeSpan = new TimeSpan();
            timer.Tick += new EventHandler((object source, EventArgs e) =>
            {
                if (timeSpan > TimeSpan.Zero)
                {
                    timeSpan = timeSpan.Add(new TimeSpan(0, 0, 0, 0, -10));
                    Time = timeSpan.ToString(@"hh\:mm\:ss\:ff");
                }
                else
                {
                    this.Pause();
                    timeSpan = new TimeSpan(0, 0, 0);
                }
            });

            RunPauseAction = new CommandAction((object obj) =>
            {
                if (TimerEnabled)
                    this.Pause();
                else
                    this.Start();
            });

            SetTimeAction = new CommandAction((object obj) =>
              {
                  this.Set();
              });

            RemoveAction = new CommandAction((object obj) =>
              {
                  if (obj != null)
                  {
                      var window = obj as MainWindow;
                      window.TimerItems.Remove(this);
                  }
              });
        }

        public void Set()
        {
            timer.Stop();
            timeSpan = new TimeSpan(0, Hours, Minutes, Seconds);
            Time = timeSpan.ToString(@"hh\:mm\:ss\:ff");
            TimerEnabled = false;
        }
        public void Start()
        {
            timer.Start();
            TimerEnabled = true;
        }
        public void Pause()
        {
            timer.Stop();
            TimerEnabled = false;
        }

        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        public bool TimerEnabled
        {
            get { return (bool)GetValue(TimerEnabledProperty); }
            set { SetValue(TimerEnabledProperty, value); }
        }
    }

    public class MyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Pause";
            else
                return "Run";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<MyTimer> TimerItems { get; } = new ObservableCollection<MyTimer>();
        public ObservableCollection<StopWatch> StopWatchItems { get; } = new ObservableCollection<StopWatch>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void AddTimer(object sender, RoutedEventArgs e)
        {
            TimerItems.Add(new MyTimer());
        }

        private void AddStopWatch(object sender, RoutedEventArgs e)
        {
            StopWatchItems.Add(new StopWatch());
        }
    }
}
