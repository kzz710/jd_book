using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memcached.ClientLibrary;

namespace Bookshop.Common
{
    public class MemcacheHelper
    {
        private static readonly MemcachedClient mc;
        static MemcacheHelper() 
        {
            //实际开发应发放入配置文件中
            string[] serverlist = { "127.0.0.1:11211", "10.0.0.132:11211" };

            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(serverlist);

            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();

            // 获得客户端实例
            mc = new MemcachedClient();
            mc.EnableCompression = false;


        }
        /// <summary>
        /// 存入数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Set(string key, object value) 
        {
            return mc.Set(key,value);
        }

        public static bool Set(string key, object value, DateTime time) 
        {
            return mc.Set(key,value,time);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key) 
        {
            return mc.Get(key);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(string key) 
        {
            return mc.Delete(key);
        }
        
    }
}
