
using System;
using System.Collections.Generic;
using System.Globalization;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace MyDesktopTool.NotifyIconManager
{
    /// <summary>
    /// Interaction logic for NotifyIconExtend.xaml
    /// </summary>
    public partial class NotifyIconExtend : Window
    {
        public NotifyIconExtend()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        bool CanHide = false;

        public void ShowLayer(int AutoLeft)
        {
            //UIHelper.LastSelectTunnelItemWID = UIHelper.SelectedTunnelID;

            //if (IsExpand)
            //{
            //    this.Width = 300;
            //}
            //else
            //{
            //    this.Width = 150;
            //}
           
            //ReLoadLv2();
            ////MessageBox.Show(AutoLeft.ToString()+"像素");
            //this.Opacity = 0.01;
            //this.Show();
            //this.Activate();

            //var DesktopWorkingArea = System.Windows.SystemParameters.WorkArea;

            //var Source = PresentationSource.FromVisual(this);
            //Matrix TransformToDevice = Source.CompositionTarget.TransformToDevice;
            //var PixelSize = (System.Windows.Size)TransformToDevice.Transform(new Vector(this.Width, this.Height));

            //System.Windows.Forms.Panel Panel = new System.Windows.Forms.Panel();
            //System.Drawing.Graphics Graphics = System.Drawing.Graphics.FromHwnd(Panel.Handle);

            //this.Left = Convert.ToInt32((AutoLeft - (PixelSize.Width / 2) + 20)) * (96 / Graphics.DpiX);

            ////MessageBox.Show((GetRealLeft / 10).ToString() + "英寸");
            //this.Top = DesktopWorkingArea.Bottom + 10 - this.Height;
            //this.Topmost = true;

            //CanHide = true;
            //this.Opacity = 1;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void ExitThis(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (CanHide)
            {
                this.Hide();
            }
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CanHide)
            {
                this.Hide();
            }
        }

        private void ExitProgram(object sender, MouseButtonEventArgs e)
        {
            NotifyIconHelper.QuickExit(null, null);
        }

     

        public Thread AutoHideTrd = null;

     


      

        private void Window_LostMouseCapture(object sender, MouseEventArgs e)
        {

        }

        public Grid LastActivedGrid = null;

       

      

     

        private void QuickConnect(object sender, MouseButtonEventArgs e)
        {
           
        }

       
    }
}
