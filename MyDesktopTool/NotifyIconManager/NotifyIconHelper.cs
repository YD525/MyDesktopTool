using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDesktopTool.NotifyIconManager
{
    public class NotifyIconHelper
    {
        public static Rectangle GetIconRect(NotifyIcon icon)
        {
            RECT rect = new RECT();
            NOTIFYICONIDENTIFIER notifyIcon = new NOTIFYICONIDENTIFIER();

            notifyIcon.cbSize = Marshal.SizeOf(notifyIcon);
            //use hWnd and id of NotifyIcon instead of guid is needed
            notifyIcon.hWnd = GetHandle(icon);
            notifyIcon.uID = GetId(icon);

            int hresult = Shell_NotifyIconGetRect(ref notifyIcon, out rect);
            //rect now has the position and size of icon

            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NOTIFYICONIDENTIFIER
        {
            public Int32 cbSize;
            public IntPtr hWnd;
            public Int32 uID;
            public Guid guidItem;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern int Shell_NotifyIconGetRect([In] ref NOTIFYICONIDENTIFIER identifier, [Out] out RECT iconLocation);

        private static FieldInfo windowField = typeof(NotifyIcon).GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        private static IntPtr GetHandle(NotifyIcon icon)
        {
            if (windowField == null) throw new InvalidOperationException("[Useful error message]");
            NativeWindow window = windowField.GetValue(icon) as NativeWindow;

            if (window == null) throw new InvalidOperationException("[Useful error message]");  // should not happen?
            return window.Handle;
        }

        private static FieldInfo idField = typeof(NotifyIcon).GetField("id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        private static int GetId(NotifyIcon icon)
        {
            if (idField == null) throw new InvalidOperationException("[Useful error message]");
            return (int)idField.GetValue(icon);
        }


        public static NotifyIcon OneNotifyIcon = null;
        public static System.Windows.Forms.MenuItem MaxWin;
        public static System.Windows.Forms.MenuItem CaptionBtn = new System.Windows.Forms.MenuItem();

        public static MainGui CurrentGui = null;
        public static NotifyIconExtend CurrentLayer = null;

        public static int InitCount = 0;
        public static void Initialization(MainGui Gui, string Tittle)
        {
            if (InitCount > 0)
            {
                return;
            }

            InitCount++;

            CurrentGui = Gui;

            CurrentLayer = new NotifyIconExtend();

            OneNotifyIcon = new NotifyIcon();

            OneNotifyIcon.BalloonTipText = Tittle;
            OneNotifyIcon.Text = Tittle;
            OneNotifyIcon.Visible = true;
            OneNotifyIcon.MouseClick += ShowWin;

            //MenuItem OpenItem = new MenuItem();
            //OpenItem.Text = "打开";
            //OpenItem.Click += ShowThis;

            //MenuItem CloseItem = new MenuItem();
            //CloseItem.Text = "退出";
            //CloseItem.Click += QuickExit;

            //OneNotifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { OpenItem,CloseItem });


            string IcoPath = DeFine.GetFullPath(@"\" + "ico.ico");

            if (File.Exists(IcoPath))
            {
                OneNotifyIcon.Icon = new System.Drawing.Icon(IcoPath);
            }
            else
            {

            }

            OneNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(ShowWin);
        }



        public static void ShowAny()
        {
            if (DeFine.WorkingWin != null)
            {
                DeFine.WorkingWin.Dispatcher.Invoke(new Action(() =>
                {
                   DeFine.WorkingWin.Show();
                }));
            }
        }




        public static void ShowMenu()
        {
            var GetRect = GetIconRect(OneNotifyIcon);

            DeFine.WorkingWin.Dispatcher.Invoke(new Action(() =>
            {
                CurrentLayer.ShowLayer(GetRect.Left);
            }));
        }

        public static void ShowWin(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ShowMenu();
            }
            else
            if (e.Button == MouseButtons.Left)
            {
                ShowAny();
            }
        }

        public static void ShowMsgInNotifyIcon(string ActionType, string ActionMessage, int MsgType, int TimeOut = 1000)
        {
            new Thread(() => { 
            CurrentGui.Dispatcher.Invoke(new Action(() => {
                OneNotifyIcon.ShowBalloonTip(TimeOut, ActionType, ActionMessage.Replace("_", "\r\n"), (ToolTipIcon)MsgType);
            }));
            }).Start();
        }

        public static void ShowThis(object sender, EventArgs e)
        {
            ShowAny();
        }
        public static void QuickExit(object sender, EventArgs e)
        {
            DeFine.ExitAny();
        }



    }
}
