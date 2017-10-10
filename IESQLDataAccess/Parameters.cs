using System;
using System.Collections.Generic;
using System.Data;

namespace CCIH.Utilities.IESQLDataAccess
{
    public class Parameters
    {
        public class Parameter
        {
            public string parameterName;
            public SqlDbType dbType;
            public object parameterValue;
            public int maxLength = -1;//varchar(max)
        }

        public List<Parameter> parameters
        {
            get;
            private set;
        }

        public Parameters()
        {
            parameters = new List<Parameter>();
        }

        /// <summary>
        /// Adds An Element To The List Of Parameters
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the stored procedure</param>
        /// <param name="type">System.Data.SqlTypes</param>
        /// <param name="myParameterValue">The Objects value; string/decimal/int/long/etc.</param>
        /// <param name="maxLength">For strings; If you need to pass in the max length of the string</param>
        /// <example>parametes.Add("@PersonID", SqlDbType.Int, 12345, 0)</example>
        /// <example>parametes.Add("@FirstName", SqlDbType.VarChar, "Eric", 50)</example>
        public void Add(string parameterName, SqlDbType dbType, object parameterValue, int maxLength = -2)
        {
            FixParameterName(ref parameterName);
            if (maxLength > 0)
            {
               parameterValue = Truncate(parameterValue, maxLength);
            }
            parameters.Add(new Parameter() { parameterName = parameterName, dbType = dbType, parameterValue = parameterValue, maxLength = maxLength });
        }

        private string Truncate(object parameterValue, int maxLength)
        {
            //todo: check for dbNull value also;
            maxLength = maxLength - 1;//account for starting index at 0;
            string _myString = Convert.ToString(parameterValue);

            if (string.IsNullOrEmpty(_myString))
            {
                _myString = string.Empty;
            }
            else
            {
                try
                {
                    if (_myString.Length > maxLength)
                    {
                        return _myString.Substring(0, maxLength);                        
                    }
                }
                catch (Exception)
                {
                    return Convert.ToString(parameterValue);
                }
            }
            return _myString;
        }

        public void Clear()
        {
            parameters.Clear();
        }

        public void Dispose()
        {
            Clear();
            parameters = null;
        }

        /// <summary>
        /// Trim, Prefix If Necessary An @ Character/Symbol;
        /// </summary>
        /// <param name="myParam">@MyParameter</param>
        private void FixParameterName(ref string myParam)
        {
            myParam = myParam.Trim();
            if (!myParam.StartsWith("@"))
            {
                myParam = "@" + myParam;
            }
        }
    }
}
