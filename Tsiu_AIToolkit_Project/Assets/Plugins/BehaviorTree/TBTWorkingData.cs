using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBT
{
    public class TBTWorkingData : TAny
    {
        internal Dictionary<int, TBTActionContext> _context;
        internal Dictionary<int, TBTActionContext> context
        {
            get
            {
                return _context;
            }
        }

        public TBTWorkingData()
        {
            _context = new Dictionary<int, TBTActionContext>();
        }

        ~TBTWorkingData()
        {
            _context = null;
        }
    }
}
