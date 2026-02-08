using System;
using System.Collections.Generic;
using System.Text;

namespace DarwinDLL
{
    public class Par <T1, T2>: IEquatable <Par <T1, T2>>
    {
        public T1 item1;
        public T2 item2;

        public Par(T1 unItem1, T2 unItem2)
        {
            item1 = unItem1;
            item2 = unItem2;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            return SonIguales(obj);
        }

        private bool SonIguales(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Par<T1, T2> o2 = (Par<T1, T2>)obj;
            return (this.item1.Equals(o2.item1) && this.item2.Equals(o2.item2));
        }


        #region IEquatable<Par<T1,T2>> Members

        public bool Equals(Par<T1, T2> other)
        {
            return SonIguales (other);
        }

        #endregion
    }
}
