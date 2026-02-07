using System;
using System.Collections.Generic;
using System.Text;

namespace DarwinDLL
{

    public class EventList<T>: List<T>
    {
        public delegate void beforeAddDelegator(T item);
        public event beforeAddDelegator beforeAdd;

        public new void Add(T item)
        {
            if (beforeAdd != null)
            {
                beforeAdd(item);
            }
            base.Add(item);
        }
    }
}
