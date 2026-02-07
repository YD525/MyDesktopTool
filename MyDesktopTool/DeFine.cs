using MyDesktopTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DeFine
{
    public static MainGui WorkingWin = null;
    public static SqlCore<SQLiteHelper> GlobalSQL = null;

    public static void Init()
    { 
    
    }

    public static string GetFullPath(string Path)
    {
        string GetShellPath = System.Windows.Forms.Application.StartupPath;
        if (GetShellPath.EndsWith(@"\"))
        {
            GetShellPath = GetShellPath.Substring(0, GetShellPath.Length - 1);
        }
        if (!Path.StartsWith(@"\"))
        {
            Path = @"\" + Path;
        }
        return GetShellPath + Path;
    }

    public static void ExitAny()
    {
        Environment.Exit(0);
    }
}