using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using log4net;
using System.Diagnostics;

public static class UnityLogger
{

    public static void Init()
    {
        Application.logMessageReceived += onLogMessageReceived;
        Common.Log.Init("Unity");
    }

    private static ILog log = LogManager.GetLogger("Unity");

    private static void onLogMessageReceived(string condition, string stackTrace, UnityEngine.LogType type)
    {
        switch(type)
        {
            case LogType.Error:
                log.ErrorFormat("{0}\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                break;
            case LogType.Assert:
                log.DebugFormat("{0}\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                break;
            case LogType.Exception:
                log.FatalFormat("{0\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                break;
            case LogType.Warning:
                log.WarnFormat("{0}\r\n{1}", condition, stackTrace.Replace("\n", "\r\n"));
                break;
            default:
                log.Info(condition);
                break;
        }
    }

    public static void stacktraceLog(string str)
    {

        string info = null;
        //设置为true，这样才能捕获到文件路径名和当前行数，当前行数为GetFrames代码的函数，也可以设置其他参数
        StackTrace st = new StackTrace(true);
        //得到当前的所以堆栈
        StackFrame[] sf = st.GetFrames();
        for (int i = 0; i < sf.Length; ++i)
        {
            info = info + "\r\n" + " FileName=" + sf[i].GetFileName() + " fullname=" + sf[i].GetMethod().DeclaringType.FullName + " function=" + sf[i].GetMethod().Name + " FileLineNumber=" + sf[i].GetFileLineNumber();
        }
        UnityEngine.Debug.Log(info);
    }
}