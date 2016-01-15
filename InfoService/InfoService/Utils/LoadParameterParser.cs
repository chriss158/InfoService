using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InfoService.Utils
{
    public class LoadParameterParser
    {

        string _loadParameters;
        private List<LoadParameter> _parameters;

        public LoadParameterParser(string loadParameters)
        {
            _loadParameters = loadParameters;
            //_parameters = new List<LoadParameter<T>>();
            ParseParameters();
        }

        public LoadParameter GetParameter(string parameterName)
        {
            return _parameters.FirstOrDefault(parameter => parameter.ParameterName == parameterName);
        }

        private void ParseParameters()
        {
            if (!string.IsNullOrEmpty(_loadParameters))
            {
                Regex regex = new Regex(@"(\w*):[^,\\]*(?:\\.[^,\\]*)*");
                MatchCollection coll = regex.Matches(_loadParameters);
                foreach (Match match in coll)
                {
                    string[] param = match.Value.Split(':');
                    if (param.Length == 2)
                    {
                        LoadParameter loadParam = null;
                        string paramName = param[0].Trim();
                        string paramSetting = param[1].Trim();
                        if (paramName.Length > 0 && paramSetting.Length > 0)
                        {
                            if (paramSetting.All(char.IsDigit))
                            {
                                try
                                {
                                    loadParam = new LoadParameter<int>(paramName, Convert.ToInt32(paramSetting));
                                }
                                catch (Exception)
                                {
                                    loadParam = null;
                                }
                            }
                            else
                            {
                                loadParam = new LoadParameter<string>(paramName, paramSetting);
                            }
                        }
                        if (loadParam != null)
                        {
                            _parameters.Add(loadParam);
                        }
                    }
                }
            }
        }

    }

}
