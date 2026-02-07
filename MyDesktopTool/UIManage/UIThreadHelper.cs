using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class UIThreadHelper
{
    public static object LockerTrd = new object();
    private static Thread MainUITrd = null;
    public static void CreatUITrd(Action OneAct)
    {
        lock (LockerTrd)
        {
            if (MainUITrd != null)
            {
                try
                {
                    MainUITrd.Abort();
                }
                catch { }
                MainUITrd = null;
            }

            MainUITrd = new Thread(() => {
                try
                {
                    OneAct.Invoke();
                }
                catch { }
                MainUITrd = null;
            });

            MainUITrd.Start();
        }
    }

    public static void SendWord(string Text,int AutoWait = 8000)
    {
        if (DeFine.WorkingWin != null)
        {
            DeFine.WorkingWin.Dispatcher.Invoke(new Action(() => {
                DeFine.WorkingWin.OneWord.Text = Text;
            }));

            double GetHeight = 0;

            DeFine.WorkingWin.Dispatcher.Invoke(new Action(() => {
                GetHeight = DeFine.WorkingWin.MainBackGround.Height;
            }));

            while (GetHeight < 130)
            {
                GetHeight++;

                DeFine.WorkingWin.Dispatcher.Invoke(new Action(() => 
                {
                    DeFine.WorkingWin.MainBackGround.Height = GetHeight;
                    DeFine.WorkingWin.Height = GetHeight;
                }));
                Thread.Sleep(2);
            }
            Thread.Sleep(AutoWait);

            DeFine.WorkingWin.Dispatcher.Invoke(new Action(() => {
                GetHeight = DeFine.WorkingWin.MainBackGround.Height;
            }));

            while (GetHeight > 80)
            {
                GetHeight--;

                DeFine.WorkingWin.Dispatcher.Invoke(new Action(() =>
                {
                    DeFine.WorkingWin.MainBackGround.Height = GetHeight;
                    DeFine.WorkingWin.Height = GetHeight;
                }));
                Thread.Sleep(2);
            }
        }
    }
}
