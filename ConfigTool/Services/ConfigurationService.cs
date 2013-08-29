using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTool.Services
{
    public class ConfigurationService
    {
        public List<ConfigurationItem> GetAllConfigurations()
        {
            System.Threading.Thread.Sleep(5000);
            List<ConfigurationItem> ret = new List<ConfigurationItem>();
            ret.Add(new ConfigurationItem(){ Name="Cat", Value=1});
            ret.Add(new ConfigurationItem() { Name = "Dog", Value = 2 });
            return ret;
        }
    }
}
