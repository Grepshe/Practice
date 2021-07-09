using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading;

namespace ConsoleApp3
{
    public class Cache<T>
    {
        public Cache(TimeSpan timeout, int maxSize)
        {
            m_timeout = timeout;
            m_maxSize = maxSize;
        }

        public void Refresh()
        {
            List<string> list = new System.Collections.Generic.List<string>();
            foreach (var el in m_timeouts)
            {
                if (DateTime.Now > el.Value + m_timeout)
                {
                    m_cache.Remove(el.Key);
                    list.Add(el.Key);
                }
            }
            foreach(var el in list)
            {
                m_timeouts.Remove(el);
            }
        }
        public void Save(T data, string key)
        {
            Refresh();
            if(m_cache.ContainsKey(key))
            {
                throw new ArgumentException("AlreadyExists");
            }
            if (m_cache.Count >= m_maxSize)
            {
                KeyValuePair<string, DateTime> min = new KeyValuePair<string, DateTime>("", DateTime.MaxValue);
                foreach (var el in m_timeouts)
                {
                    if(el.Value < min.Value)
                    {
                        min = el;
                    }
                }
                m_cache.Remove(min.Key);
                m_timeouts.Remove(min.Key);
            }
            m_cache[key] = data;
            m_timeouts[key] = DateTime.Now;
        }
        public T Get(string key)
        {
            Refresh();
            if (!m_cache.ContainsKey(key))
            {
                throw new KeyNotFoundException("Key not found");
            }
            return m_cache[key];
        }

        private int m_maxSize;
        private TimeSpan m_timeout;
        private Dictionary<string, T> m_cache = new System.Collections.Generic.Dictionary<string, T>();
        private Dictionary<string, DateTime> m_timeouts = new System.Collections.Generic.Dictionary<string, DateTime>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Cache<int> cache = new Cache<int>(TimeSpan.FromSeconds(5), 10);
            cache.Save(10, "tens");
            Console.WriteLine(cache.Get("tens"));
            System.Threading.Thread.Sleep(6000);
            try
            {
                Console.WriteLine(cache.Get("tens")); //throw
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
