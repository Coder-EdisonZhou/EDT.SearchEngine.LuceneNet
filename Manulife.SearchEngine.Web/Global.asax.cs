using Manulife.SearchEngine.Web.Common;
using System;

namespace Manulife.SearchEngine.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // 启动日志组件
            log4net.Config.XmlConfigurator.Configure();
            // 启动索引管理器
            IndexManager.Instance.Start();
            // 启动定时任务
            SearchLogScheduler.Start();
        }

    }
}