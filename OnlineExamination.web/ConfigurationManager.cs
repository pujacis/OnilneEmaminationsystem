using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace OnlineExamination.web
{
    public static class ConfigurationManager
    {
        private static IConfiguration configuration=null;
        static ConfigurationManager()
        {
            configuration=new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
        }
          public static string GetFilepath()
        {
            return configuration["CustomKeys:BaseUrl"] + "file/";
        }
    }
}
