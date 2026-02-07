
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Threading;

namespace MyDesktopTool.DesktopManage
{
    public class WordItem
    {
        public int Type = 0;
        public string Word = "";

        public WordItem(int type, string word)
        {
            Type = type;
            Word = word;
        }
    }

    public class DeskTopHelper
    {
        public static List<WordItem> WordItems = new List<WordItem>();

        public static List<double> WaitTimeArrays = new List<double>();


        public static DateTime CurrentCountUPTime = DateTime.MinValue;

        public delegate void SetCountUPTime(DateTime Value);

        public static SetCountUPTime CountUPAction = null;



        public static void Init()
        {
            WordItems.Clear();

            WordItems.Add(new WordItem(1, "不要忘记卿等为何而战!"));
            WordItems.Add(new WordItem(1, "去锲而不舍的斗争!"));
            WordItems.Add(new WordItem(3, "为了荣誉!"));
            WordItems.Add(new WordItem(1, "去锲而不舍的斗争!"));
            WordItems.Add(new WordItem(1, "去战斗,即使后悔,被悔恨击倒,也要战斗.既然决定战斗与反抗,那就全心全意地战斗.|不曾放弃一秒,一瞬间,一刹那都不放弃,贪婪地蚕食着可以预见的胜利.|还能站起来的话,手指还能动的话,獠牙还没有被折断的话,就再站起来. 再站起来,站起来站起来,再斩过去. | 只要还活着,就战斗.战斗.战斗....这就是,所谓的战斗. -- 剑圣 莱因哈鲁特·范·阿斯特雷亚"));
            WordItems.Add(new WordItem(1, "难道卿们是为了沉溺在耻辱之中,才来到这里的吗? --库珥修·卡尔斯腾"));
            WordItems.Add(new WordItem(1, "时间一直在前进 却不曾后退!"));
            WordItems.Add(new WordItem(2, "永不动摇的意志! --约定"));
        }

        public static List<string> QuickGetWordRangeByType(int Type)
        {
            List<string> Array = new List<string>();
            foreach (var GetKey in WordItems)
            {
                if (GetKey.Type == Type)
                {
                    Array.Add(GetKey.Word);
                }
            }
            return Array;
        }

        public static bool LockerAutoShowKeyWord = false;
        public static void AutoShowKeyWord(bool Check)
        {
            if (Check)
            {
                if (!LockerAutoShowKeyWord)
                {
                    LockerAutoShowKeyWord = true;

                    new Thread(() =>
                    {
                        while (LockerAutoShowKeyWord)
                        {
                            Thread.Sleep(1000);
                            if (new Random(Guid.NewGuid().GetHashCode()).Next(0, 100) > 80)
                            {
                                int Type = 0;
                                if (!CanSet)
                                {
                                    Type = 1;
                                }
                                else
                                {
                                    if (DeskTopHelper.QueryTimeRangeByMaxWaitTime(30, 3) >= 3 || DeskTopHelper.TotalKeyBoardIputCount > 220)
                                    {
                                        Type = 3;
                                    }
                                    else
                                    {
                                        Type = 2;
                                    }
                                }

                                var GetArray = QuickGetWordRangeByType(Type);
                                if (GetArray.Count > 0)
                                {
                                    int AutoWait = 8000;

                                    var GetWords = GetArray[new Random(Guid.NewGuid().GetHashCode()).Next(0, GetArray.Count)].Split('|');
                                    if (GetWords.Length > 1)
                                    {
                                        AutoWait = 3000;
                                    }
                                    foreach (var GetWord in GetWords)
                                    {
                                        UIThreadHelper.SendWord(GetWord, AutoWait);
                                    }
                                   
                                }
                            }
                        }
                    }).Start();
                }
                else
                {
                    LockerAutoShowKeyWord = false;
                }
            }
        }


        public static bool CanSet = false;


        public static double WaitTime = 0;
        public static Thread CountUPTrd = null;
        public static bool LockerCountUPService = false;

        public static void StartCountUPService(bool Check)
        {
            if (Check)
            {
                if (!LockerCountUPService)
                {
                    LockerCountUPService = true;

                    CountUPTrd = new Thread(() =>
                    {
                        CurrentCountUPTime = DateTime.Today;
                        while (LockerCountUPService)
                        {
                            Thread.Sleep(1000);

                            if (CanSet)
                            {
                                CurrentCountUPTime = CurrentCountUPTime.AddSeconds(1);
                                CountUPAction(CurrentCountUPTime);

                                if (WaitTimeArrays.Count > 100)
                                {
                                    WaitTimeArrays.RemoveAt(0);
                                }

                                if (WaitTime != 0)
                                {
                                    WaitTimeArrays.Add(WaitTime);
                                }

                                WaitTime = 0;
                            }
                            else
                            {
                                WaitTime += new Random(Guid.NewGuid().GetHashCode()).Next(1, 5);
                            }
                        }

                    });
                    CountUPTrd.Start();
                }
            }
            else
            {
                LockerCountUPService = false;

                if (CountUPTrd != null)
                {
                    CountUPTrd.Abort();
                    CountUPTrd = null;
                }
            }
        }


        public static int TotalKeyBoardIputCount = 0;
        private static int TempTotalKeyBoardIputCount = 0;
        //1 Min Reload
        public static Thread KeBoardInputSpeedListenTrd = null;
        public static bool LockerKeBoardInputSpeedListenService = false;

        public static void StartKebordInputSpeedListenService(bool Check)
        {
            if (Check)
            {
                if (!LockerKeBoardInputSpeedListenService)
                {
                    LockerKeBoardInputSpeedListenService = true;

                    KeBoardInputSpeedListenTrd = new Thread(() =>
                    {
                        while (LockerKeBoardInputSpeedListenService)
                        {
                            for (int i = 0; i < 120; i++)
                            {
                                Thread.Sleep(500);

                                if (TotalKeyBoardIputCount != 0)
                                {
                                    if (TempTotalKeyBoardIputCount == TotalKeyBoardIputCount)
                                    {
                                        CanSet = false;
                                    }
                                    else
                                    {
                                        TempTotalKeyBoardIputCount = TotalKeyBoardIputCount;
                                        CanSet = true;
                                    }
                                }
                            }

                            TotalKeyBoardIputCount = 0;
                        }

                    });
                    KeBoardInputSpeedListenTrd.Start();
                }
            }
            else
            {
                LockerKeBoardInputSpeedListenService = false;

                if (KeBoardInputSpeedListenTrd != null)
                {
                    KeBoardInputSpeedListenTrd.Abort();
                    KeBoardInputSpeedListenTrd = null;
                }
            }

        }


        public static bool LockerWinFormIntervalAction = false;
        public static void StartWinFormIntervalAction(bool Check, Action AnyAct)
        {
            if (Check)
            {
                if (!LockerWinFormIntervalAction)
                {
                    LockerWinFormIntervalAction = true;

                    new Thread(() =>
                    {
                        while (LockerWinFormIntervalAction)
                        {
                            Thread.Sleep(1000);
                            AnyAct.Invoke();
                        }
                    }).Start();
                }
            }
            else
            {
                LockerWinFormIntervalAction = false;
            }
        }

        public static Double CurrentBrainGhz = 0;
        public static double CalcBrainGhz()
        {
            if (TotalKeyBoardIputCount - WaitTime > 0)
            {
                if (TotalKeyBoardIputCount != 0)
                {
                    double Get = Math.Round((double)(((double)TotalKeyBoardIputCount - WaitTime) / (double)100), 2);
                    CurrentBrainGhz = Get;

                    if (Get > 9)
                    {
                        return 9;
                    }
                    else
                    {
                        return Get;
                    }
                }
            }

            return 0;
        }



        public static double GetCPUInfo()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor Information", "% Processor Performance", "_Total");
            double cpuValue = cpuCounter.NextValue();
            Thread.Sleep(1000);
            cpuValue = cpuCounter.NextValue();

            foreach (ManagementObject obj in new ManagementObjectSearcher("SELECT *, Name FROM Win32_Processor").Get())
            {
                double maxSpeed = Convert.ToDouble(obj["MaxClockSpeed"]) / 1000;
                double turboSpeed = maxSpeed * cpuValue / 100;
                return Math.Round(turboSpeed, 2);
            }

            return 0;
        }


        public static int QueryTimeRangeByMaxWaitTime(int MaxWaitTime, int Range = 1)
        {
            int MatchCount = 0;

            for (int i = WaitTimeArrays.Count; i > 0; i--)
            {
                if (Range > 0)
                {
                    Range--;
                    if (WaitTimeArrays[i - 1] < MaxWaitTime)
                    {
                        MatchCount++;
                    }
                }
            }

            if (Range == 0)
            {
                return MatchCount;
            }

            return 0;
        }

        public static DateTime LastGetDayInFoTime = DateTime.MinValue;

        public static string GetDayInFo()
        {
            if (LastGetDayInFoTime.Year != DateTime.Now.Year && LastGetDayInFoTime.Month != DateTime.Now.Month && LastGetDayInFoTime.Day != DateTime.Now.Day)
            {
                FallSeasonDates fallSeason = new FallSeasonDates(DateTime.Now.Year);

                string GetSeason = "";

                if ((DateTime.Now > fallSeason.AutumnEquinox))
                {
                    GetSeason = "立秋";
                }

                if ((DateTime.Now > fallSeason.LateSummer))
                {
                    GetSeason = "处暑";
                }

                if ((DateTime.Now > fallSeason.GreenRice))
                {
                    GetSeason = "白迹";
                }

                if ((DateTime.Now > fallSeason.AutumnalEquinox))
                {
                    GetSeason = "秋分";
                }

                if ((DateTime.Now > fallSeason.FirstFrost))
                {
                    GetSeason = "寒露";
                }

                if ((DateTime.Now > fallSeason.Deposition))
                {
                    GetSeason = "霜降";
                }

                string TimeFormat = string.Format("{0} {1}", DateTime.Now.ToString("MM月dd日"), GetSeason);
                LastGetDayInFoTime = DateTime.Now;
                return TimeFormat;
            }

            return null;
        }

        public static DateTime LastWeatherInFo = DateTime.MinValue;
        public static int TotalWeathCount = 0;

        public static string[] WeatherParams;
        public static void GetWeatherInFoA(string City = "")
        {
            //if (LastWeatherInFo.Day!= DateTime.Now.Day)
            //{
            //    WeatherWebService GetWeather = new WeatherWebService();
            //    WeatherParams = GetWeather.getWeatherbyCityName(City);
            //    LastWeatherInFo = DateTime.Now;
            //}

            //if (TotalWeathCount == 0)
            //{
            //    TotalWeathCount = 25;

            //    DeFine.WorkingWin.AutoWeather.Dispatcher.Invoke(new Action(() =>
            //    {
            //        try {
            //        DeFine.WorkingWin.AutoWeather.Content = WeatherParams[10].Replace("今日天气实况：", "");
            //        }
            //        catch { }
            //    }));
            //}
            //else
            //{
            //    TotalWeathCount--;
            //}
        }

        public static string GetTimeRange()
        {
            if (DateTime.Now.Hour >= 5 && DateTime.Now.Hour < 8)
            {
                return "早上";
            }
            else
            if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 12)
            {
                return "上午";
            }
            else
            if (DateTime.Now.Hour > 12 && DateTime.Now.Hour < 14)
            {
                return "中午";
            }
            else
            if (DateTime.Now.Hour > 14 && DateTime.Now.Hour < 18)
            {
                return "下午";
            }
            else
            if (DateTime.Now.Hour > 18 && DateTime.Now.Hour < 19)
            {
                return "晚上";
            }
            else
            if (DateTime.Now.Hour > 19 && DateTime.Now.Hour < 23)
            {
                return "傍晚";
            }
            else
            {
                return "凌晨";
            }
        }
    }



    public class FallSeasonDates
    {
        public DateTime AutumnEquinox { get; private set; }
        public DateTime LateSummer { get; private set; }
        public DateTime GreenRice { get; private set; }
        public DateTime AutumnalEquinox { get; private set; }
        public DateTime FirstFrost { get; private set; }
        public DateTime Deposition { get; private set; }

        public FallSeasonDates(int year)
        {
            // 设定起始日期为公历1月1日
            DateTime startDate = new DateTime(year, 1, 1);

            // 计算春分
            AutumnEquinox = CalculateEquinox(startDate, 225);

            // 计算处暑
            LateSummer = AutumnEquinox.AddDays(-15);

            // 计算白露
            GreenRice = AutumnEquinox.AddDays(15);

            // 计算秋分
            AutumnalEquinox = CalculateEquinox(startDate, 235);

            // 计算寒露
            FirstFrost = CalculateFrost(startDate, 75);

            // 计算霜降
            Deposition = AutumnalEquinox.AddDays(10);
        }

        private DateTime CalculateEquinox(DateTime startDate, int dayOfYear)
        {
            // 根据给定的日期和天数计算春分或秋分的具体日期
            return new DateTime(startDate.Year, 3, 21)
                .AddDays((dayOfYear - 81) / 365.25 * 365 + (dayOfYear - 81) % 365.25);
        }

        private DateTime CalculateFrost(DateTime startDate, int dayOfYear)
        {
            // 根据给定的日期和天数计算首次冻结的具体日期
            return new DateTime(startDate.Year, 12, 21)
                .AddDays((dayOfYear - 225) / 365.25 * 365 + (dayOfYear - 225) % 365.25);
        }
    }
}
