using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoService.Utils
{
    public class ParameterSetting
    {
        public object Setting { get; set; }

        public ParameterSetting()
        {
            Setting = new object();
        }

        public ParameterSetting(object setting)
        {
            Setting = setting;
            if (Setting.GetType() == typeof(string))
            {
                string sSetting = Setting.ToString();
                if (sSetting.Substring(0, 1) == "\"")
                {
                    sSetting = sSetting.Substring(1);
                }
                
                if (sSetting.Substring(sSetting.Length - 1, 1) == "\"")
                {
                    sSetting = sSetting.Substring(0, sSetting.Length - 1);
                }
                Setting = sSetting.Trim();
            }

        }
        public T ParseSetting<T>()
        {
            try
            {
                return (T)Convert.ChangeType(Setting, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}
