﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Team1922.MVVM.Framework;

namespace Team1922.MVVM.ViewModels
{
    /// <summary>
    /// This is like <see cref="KeyValuePair{TKey, TValue}"/>, but the key is readonly and the value is read/write
    /// </summary>
    internal class VMKeyValuePair
    {
        public VMKeyValuePair(string key, ViewModelBase vm)
        {
            Set(key, vm);
        }
        public void Set(string key, ViewModelBase vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm", "ViewModel on VMKeyValuePair must not be null");
            _vm = vm;
            Key = key;
        }
        ViewModelBase _vm;
        public string Key { get; private set; }
        public string Value
        {
            get
            {
                return _vm[Key];
            }
            set
            {
                _vm[Key] = value;
            }
        }
    }

    /// <summary>
    /// This wraps the <see cref="BindableBase"/>, and the ability to access values based on a string key along
    /// with enumerate through it with read AND write access to the value
    /// </summary>
    internal abstract class ViewModelBase : BindableBase, IEnumerable<VMKeyValuePair>, IEnumerator<VMKeyValuePair>
    {
        private List<string> _keys = new List<string>();
        private int _enumeratorIndex = 0;
        VMKeyValuePair _current = null;

        private int EnumeratorIndex
        {
            get
            {
                return _enumeratorIndex;
            }

            set
            {
                _enumeratorIndex = value;
                RefreshCurrent();
            }
        }

        protected ViewModelBase()
        {
            _keys = GetKeys(); 
        }
        protected virtual List<string> GetKeys()
        {
            return (from x in GetType().GetProperties()
                where x.Name != "Properties" && x.Name != "Children" && x.Name != "this[string]"
                select x.Name).ToList();
        }
        public IEnumerator<VMKeyValuePair> GetEnumerator()
        {
            return this;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public bool MoveNext()
        {
            if (_enumeratorIndex + 1 > _keys.Count)
                return false;
            _enumeratorIndex++;
            return true;
        }
        public void Reset()
        {
            _enumeratorIndex = 0;
        }
        public VMKeyValuePair Current
        {
            get
            {
                return CurrentInternal;
            }
        }
        object IEnumerator.Current
        {
            get
            {
                return CurrentInternal;
            }
        }
        private VMKeyValuePair CurrentInternal
        {
            get
            {
                return _current;
            }
        }
        private void RefreshCurrent()
        {
            if (null == _current)
                _current = new VMKeyValuePair(_keys[_enumeratorIndex], this);
            else
                _current.Set(_keys[_enumeratorIndex], this);
        }
        public bool TryGetValue(string key, out string value)
        {
            try
            {
                value = this[key];
            }
            catch (Exception)
            {
                value = "";
                return false;
            }
            return true;
        }

        protected int SafeCastInt(string value)
        {
            int ret;
            bool success = int.TryParse(value, out ret);
            if (success)
                return ret;
            else
                throw new ArgumentException("Value Entered Not Integer");
        }
        protected long SafeCastLong(string value)
        {
            long ret;
            bool success = long.TryParse(value, out ret);
            if (success)
                return ret;
            else
                throw new ArgumentException("Value Entered Not Long");
        }
        protected double SafeCastDouble(string value)
        {
            double ret;
            bool success = double.TryParse(value, out ret);
            if (success)
                return ret;
            else
                throw new ArgumentException("Value Entered Not Double");
        }
        protected T SafeCastEnum<T>(string value) where T : struct
        {
            T ret;
            bool success = Enum.TryParse(value, true, out ret);
            if (success)
                return ret;
            else
                throw new ArgumentException($"Value Entered Not Compatable With {typeof(T).ToString()}");
        }
        protected bool SafeCastBool(string value)
        {
            bool ret;
            bool success = bool.TryParse(value, out ret);
            if (success)
                return ret;
            else
                throw new ArgumentException("Value Entered Not Boolean");
        }

        #region Abstract Methods
        /// <summary>
        /// This gives read/write access to the viewmodel based on the name of the property
        /// </summary>
        /// <param name="key">The name of the property</param>
        /// <returns>the string representation of the property</returns>
        public abstract string this[string key] { get; set; }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ViewModelBase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
