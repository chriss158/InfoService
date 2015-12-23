using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FeedReader
{
    public static class ExtensionMethods
    {
        public static List<T> CloneList<T>(this List<T> listToClone) where T : ICloneable
        {
            List<T> newList = new List<T>();
            foreach (T t in listToClone)
            {
                newList.Add((T)t.Clone());
            }
            return newList;
        }
    }

}
