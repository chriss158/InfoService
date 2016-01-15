using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoService.Utils
{    
    public class LoadParameters : IList<LoadParameter>
    {
        private List<LoadParameter> _list = new List<LoadParameter>();
        public int IndexOf(LoadParameter item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, LoadParameter item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public LoadParameter this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public void Add(LoadParameter item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(LoadParameter item)
        {
            return _list.Contains(item);
        }
        public bool Contains(string parameterName)
        {
            return _list.Contains(new LoadParameter(parameterName));
        }

        public void CopyTo(LoadParameter[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(LoadParameter item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<LoadParameter> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
