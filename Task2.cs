using System;
using System.IO;
using System.Text;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ConsoleApp2
{
    public enum Severity
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Criti
    }

    public class Logger : IDisposable
    {
        public Logger(string path)
        {
            m_sW = new StreamWriter(path, true, System.Text.Encoding.Default);
        }

        public void Log(string data, Severity severity)
        {
            m_sW.WriteLine($"[{DateTime.Now}] [{severity}]: {data}");
            m_sW.Flush();
        }

        public void Dispose()
        {
            m_sW.Close();
            m_sW.Dispose();
            GC.SuppressFinalize(this);
        }
        
        ~Logger()
        {
            m_sW.Close();
            m_sW.Dispose();
        }

        private StreamWriter m_sW;
    }
    class Task2
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger("log.txt");
            logger.Log("Gg, proizoshol vzriv", Severity.Debug);
        }
    }
}
