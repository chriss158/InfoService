using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoService.Utils
{
    public class LoadParameter
    {
        public string ParameterName { get; set; }
        public object ParameterSetting { get; set; }

        public LoadParameter(string parameterName, object parameterSetting)
        {
            ParameterName = parameterName;
            ParameterSetting = parameterSetting;
        }
    }
}
