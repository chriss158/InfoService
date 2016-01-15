using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoService.Utils
{
    public  class LoadParameter : IEquatable<LoadParameter>
    {
        public string ParameterName { get; set; }
        public ParameterSetting ParameterSetting { get; set; }

        public LoadParameter()
        {
            ParameterName = string.Empty;
            ParameterSetting = new ParameterSetting();
        }
        public LoadParameter(string parameterName)
        {
            ParameterName = parameterName;
            ParameterSetting = new ParameterSetting();
        }

        public LoadParameter(string parameterName, ParameterSetting parameterSetting)
        {
            ParameterName = parameterName.Trim();
            ParameterSetting = parameterSetting;
        }
        public LoadParameter(string parameterName, object parameterSetting)
        {
            ParameterName = parameterName.Trim();
            ParameterSetting = new ParameterSetting(parameterSetting);
        }

        public bool Equals(LoadParameter other)
        {
            if (other == null) return false;

            if (this.ParameterName == other.ParameterName) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return this.ParameterName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            LoadParameter parameter = obj as LoadParameter;
            if (parameter != null)
            {
                return Equals(parameter);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(LoadParameter loadParam1, LoadParameter loadParam2)
        {
            if (object.ReferenceEquals(loadParam1, loadParam2)) return true;
            if (object.ReferenceEquals(loadParam1, null)) return false;
            if (object.ReferenceEquals(loadParam2, null)) return false;

            return loadParam1.Equals(loadParam2);
        }

        public static bool operator !=(LoadParameter loadParam1, LoadParameter loadParam2)
        {
            if (object.ReferenceEquals(loadParam1, loadParam2)) return false;
            if (object.ReferenceEquals(loadParam1, null)) return true;
            if (object.ReferenceEquals(loadParam2, null)) return true;

            return !loadParam1.Equals(loadParam2);
        }
    }
}
