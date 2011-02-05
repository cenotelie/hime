namespace Hime.Kernel
{
    public static class LogUtils
    {
        public static log4net.ILog getLog(System.Type type)
        {
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%-5p: %m%n");
            log4net.Appender.ConsoleAppender appender = new log4net.Appender.ConsoleAppender(layout);
            log4net.Config.BasicConfigurator.Configure(appender);
            return log4net.LogManager.GetLogger(type);
        }
    }
}