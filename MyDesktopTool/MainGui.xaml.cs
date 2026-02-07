using MyDesktopTool.DesktopManage;
using MyDesktopTool.HookManage;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MyDesktopTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainGui : Window
    {
        public MainGui()
        {
            InitializeComponent();
        }

        public bool IsLeftMouseDown = false;

        private void WinHead_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsLeftMouseDown = true;
            }

            if (IsLeftMouseDown)
            {
                try
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.DragMove();
                    }));

                    IsLeftMouseDown = false;
                }
                catch { }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DeskTopHelper.Init();
            DeskTopHelper.AutoShowKeyWord(true);
            GlobalKeyboardHook.SetHook();

            DeskTopHelper.StartWinFormIntervalAction(true,new Action(() => {
                LoopAction();
            }));

            DeskTopHelper.CountUPAction += SetCountUPTime;
            DeskTopHelper.StartCountUPService(true);
            DeskTopHelper.StartKebordInputSpeedListenService(true);

            new Thread(() => {

                while (true)
                {
                    var GetGhz = DeskTopHelper.GetCPUInfo();
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        CpuGhz.Content = string.Format("{0}Ghz", GetGhz);

                    }));
                }
               
            }).Start();

            new Thread(() => {

                UIThreadHelper.SendWord("又是新的一天.");
            }).Start();
        }

        public void SetCountUPTime(DateTime Value)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                CountUP.Content = Value.ToString("HH:mm:ss");
            }));
        }

        public void LoopAction()
        {
            if (DeskTopHelper.WaitTimeArrays.Count == 0)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    SoulState.Content = "蓄势待发";
                }));
            }
            else
            {
                if (DeskTopHelper.QueryTimeRangeByMaxWaitTime(30,3) >= 3 || DeskTopHelper.TotalKeyBoardIputCount > 220)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        SoulState.Content = "高度集中";
                    }));
                }
                else
                if (DeskTopHelper.QueryTimeRangeByMaxWaitTime(60,3) >= 3 || DeskTopHelper.TotalKeyBoardIputCount > 100)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        SoulState.Content = "正在突击";
                    }));
                }
                else
                if (DeskTopHelper.QueryTimeRangeByMaxWaitTime(500,3) >= 3 || DeskTopHelper.TotalKeyBoardIputCount > 50)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        SoulState.Content = "工作中";
                    }));
                }
                else
                if (DeskTopHelper.QueryTimeRangeByMaxWaitTime(999,3) >= 3)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        SoulState.Content = "决策中";
                    }));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        SoulState.Content = "摸鱼工作ing";
                    }));
                }
            }

            //DeskTopHelper.GetWeatherInFo();

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                string AutoStr = "";
                
            
                Time.Content = DateTime.Now.ToString("HH:mm:ss") + " " + DeskTopHelper.GetTimeRange() + " " + AutoStr;

                var GetDay = DeskTopHelper.GetDayInFo();
                if (GetDay != null)
                {
                    DayInFo.Content = GetDay;
                }
            
                KeyCount.Content = string.Format("{0}|Minute", DeskTopHelper.TotalKeyBoardIputCount);

                var GetGhz = DeskTopHelper.CalcBrainGhz();
                if (GetGhz != 0)
                {
                    BrainGhz.Content = string.Format("{0}Ghz", GetGhz);
                }
               
                if (DeskTopHelper.CurrentBrainGhz > 5)
                {
                    BackGroundForBrain.Background = new SolidColorBrush(Color.FromRgb(132, 24, 24));
                }
                else
                if (DeskTopHelper.CurrentBrainGhz > 3)
                {
                    BackGroundForBrain.Background = new SolidColorBrush(Color.FromRgb(160, 88, 15));
                }
                else
                if (DeskTopHelper.CurrentBrainGhz > 1)
                {
                    BackGroundForBrain.Background = new SolidColorBrush(Color.FromRgb(15, 87, 160));
                }
                else
                {
                    BackGroundForBrain.Background = new SolidColorBrush(Color.FromRgb(37, 162, 223));
                }
            }));
           
            if (DeskTopHelper.CanSet)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    WorkBar.Visibility = Visibility.Hidden;
                }));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    WorkBar.Visibility = Visibility.Visible;
                }));
            }
        }
    }
}
