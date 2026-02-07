using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyDesktopTool
{
    /// <summary>
    /// Interaction logic for LoadingWin.xaml
    /// </summary>
    public partial class LoadingWin : Window
    {
        public LoadingWin()
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

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Any_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                string GetName = (sender as Label).Name;
                switch (GetName)
                {
                    case "CloseBtn":
                        {
                            DeFine.ExitAny();
                        }
                        break;
                }
            }
        }

        public Thread EffectTrd = null;

        public void StartEffectSevice()
        {
            EffectTrd = new Thread(() =>
            {
                while (true)
                {
                    var GetSleep = new Random(Guid.NewGuid().GetHashCode()).Next(0, 30) * 100;
                    Thread.Sleep(GetSleep);

                    for (int i = 0; i < 30; i++)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            this.Opacity -= 0.01;
                        }));

                        Thread.Sleep(50);
                    }

                    for (int i = 0; i < 30; i++)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            this.Opacity += 0.01;
                        }));

                        Thread.Sleep(50);
                    }
                }

            });
            EffectTrd.Start();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartEffectSevice();
            DeFine.Init();

            new Thread(() =>
            {
                Thread.Sleep(1000);

                if (EffectTrd != null)
                {
                    EffectTrd.Abort();
                }

                this.Dispatcher.Invoke(new Action(() => 
                {
                    DeFine.WorkingWin = new MainGui();
                    DeFine.WorkingWin.Owner = null;
                    DeFine.WorkingWin.Show();


                    this.Close();
                    
                }));
            }).Start();
        }
    }
}
