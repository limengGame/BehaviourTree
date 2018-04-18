using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBT
{
    public class TBTActionContext
    {
    }

    public abstract class TBTAction : TBTTreeNode
    {
        static private int sUNIQUEKEY = 0;
        static private int getUniqueKey()
        {
            if (sUNIQUEKEY >= int.MaxValue)
                return 0;
            else
                sUNIQUEKEY = sUNIQUEKEY + 1;
            return sUNIQUEKEY;
        }

        //--------------------
        protected int _uniqueKey;
        protected TBTPrecondition _precondition;
#if DEBUG
        protected string _name;
        protected string name
        {
            set
            {
                _name = value;
            }
            get
            {
                return _name;
            }
        }
#endif
        public TBTAction(int maxChildCount) : base(maxChildCount)
        {
            _uniqueKey = getUniqueKey();
        }
        ~TBTAction()
        {
            _precondition = null;
        }
        public bool Evaluate(TBTWorkingData wData)
        {
            return (_precondition == null || _precondition.IsTrue(wData)) && onEvaluate(wData);
        }
        public int Update(TBTWorkingData wData)
        {
            return onUpdate(wData);
        }
        public void Transition(TBTWorkingData wData)
        {
            onTransition(wData);
        }
        public TBTAction SetPrecondition(TBTPrecondition precondition)
        {
            _precondition = precondition;
            return this;
        }
        public override int GetHashCode()
        {
            return _uniqueKey;
        }

        protected T getContext<T>(TBTWorkingData wData) where T : TBTActionContext, new()
        {
            int uniqueKey = GetHashCode();
            T thisContext;
            if (wData.context.ContainsKey(uniqueKey) == false)
            {
                thisContext = new T();
                wData.context.Add(uniqueKey, thisContext);
            }
            else
            {
                thisContext = (T)wData.context[uniqueKey];
            }
            return thisContext;
        }

        protected virtual bool onEvaluate(TBTWorkingData wData)
        {
            return true;
        }
        protected virtual int onUpdate(TBTWorkingData wData)
        {
            return TBTRunningStatus.FINISHED;
        }
        protected virtual void onTransition(TBTWorkingData wData)
        {
        }

    }


}
