using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MikPicture
{
    public class Dispatcher : ISynchronizeInvoke
    {
        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            throw new NotImplementedException();
        }

        public object EndInvoke(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public object Invoke(Delegate method, object[] args)
        {
            ((Action<string>)method)((string)args[0]);
            return null;
        }

        public bool InvokeRequired
        {
            get { throw new NotImplementedException(); }
        }
    }
}
