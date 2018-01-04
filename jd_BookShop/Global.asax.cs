using jd_BookShop.Models;
using log4net;
using log4net.Config;
using Spring.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace jd_BookShop
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : SpringMvcApplication
    {
        protected void Application_Start()
        {
            //读取配置文件中关于log4net的配置信息
            XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //开启一个线程，扫描异常信息队列
            string filePath = Server.MapPath("/Log/");
            ThreadPool.QueueUserWorkItem((a) => {
                while (true)
                {
                    if (MyExceptionAttribute.ExceptionQueue.Count() > 0)
                    {
                        Exception ex = MyExceptionAttribute.ExceptionQueue.Dequeue();
                        if (ex != null)
                        {
                            ILog logger = LogManager.GetLogger("errorMsg");
                            logger.Error(ex.ToString());
                        }
                    }
                    else 
                    {
                        Thread.Sleep(3000);
                    }
                }
            },filePath);
        }
    }
}