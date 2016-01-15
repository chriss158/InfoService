using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoService.Utils
{
    public class LoadParameter<T> : LoadParameter
    {
        public T ParameterSetting { get; set; }

        public LoadParameter()
        {

        }

        public LoadParameter(string parameterName, T parameterSetting)
        {
            ParameterName = parameterName;
            ParameterSetting = parameterSetting;
        }
    }

    public abstract class LoadParameter
    {
        public string ParameterName { get; set; }
        //public ParameterSetting<> ParameterSetting { get; set; }

        protected LoadParameter()
        {
            ParameterName = string.Empty;
        }

        protected LoadParameter(string parameterName)
        {
            ParameterName = parameterName;
            //ParameterSetting = parameterSetting;
        }
    }
}
