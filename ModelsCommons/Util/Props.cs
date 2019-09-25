using Microsoft.Extensions.Configuration;
using System;
using System.Linq;


namespace ModelsCommons.Util
{
    public class Props
    {

        public static string ConnectionString(IConfiguration config,string connString)
        {
 
            //TO DO: structure the appsettings.json/consider the VCAP
            var sec = config.GetSection("ConnectionString:" + connString);       
            return sec.Value;
        }

        public static string PopertyValue(IConfiguration config,string section, string name)
        {

            var sec = config.GetSection(section+":"+name);
            //TO DO: structure the appsettings.json/consider the VCAP
            //propValue = config.GetChildren().Where(o => o.Key.Equals(name)).Count() > 0 ? config.GetChildren().Where(o => o.Key.Equals(name)).FirstOrDefault().Value : "";

            return sec.Value;
        }
    }
}
