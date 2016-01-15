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
        private LoadParameters _parameters;

        const string SearchRegexString = "(\\w*):(\".*?\")";
        //const string SearchRegexString = @"(\w*):[^,\\]*(?:\\.[^,\\]*)*";

        public LoadParameterParser(string loadParameters)
        {
            _loadParameters = loadParameters;
            _parameters = new LoadParameters();
            //ParseParameters();
        }
        
        public void Parse()
        {
            ParseParameters();
        }
        
        public LoadParameters GetAllParameters()
        {
            return _parameters;
        }

        public LoadParameter GetParameter(string parameterName)
        {
            if (_parameters.Contains(parameterName))
            {
                return _parameters.FirstOrDefault(parameter => parameter.ParameterName == parameterName);
            }
            else return null;
        }

        private void ParseParameters()
        {
            if (!string.IsNullOrEmpty(_loadParameters))
            {
                Regex regex = new Regex(SearchRegexString);
                MatchCollection coll = regex.Matches(_loadParameters);
                foreach (Match match in coll)
                {
                    string[] param = match.Value.Split(':');
                    if (param.Length == 2)
                    {
                        string paramName = param[0].Trim();
                        string paramSetting = param[1].Trim();
                        if (paramName.Length > 0 && paramSetting.Length > 0)
                        {
                            _parameters.Add(new LoadParameter(paramName, paramSetting));
                        }
                    }
                }
            }
        }
    }
}
