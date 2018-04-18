using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBT
{
    public class TAny
    {
        public T As<T>() where T : TAny
        {
            return (T)this;
        }
    }
}
