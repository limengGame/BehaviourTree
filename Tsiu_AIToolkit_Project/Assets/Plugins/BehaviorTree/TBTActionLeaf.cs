using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBT
{
    public class TBTActionLeaf : TBTAction
    {
        private const int ACTION_READY = 0;
        private const int ACTION_RUNNING = 1;
        private const int ACTION_FINISHED = 2;

        class TBTActionLeafContext : TBTActionContext
        {
            internal int status;
            internal bool needExit;
            private object _userData;

            public T GetUserData<T>() where T : class, new()
            {
                if (_userData == null)
                    _userData = new T();
                return (T)_userData;
            }

            public TBTActionLeafContext()
            {
                status = ACTION_READY;
                needExit = false;
                _userData = null;
            }
        }

        public TBTActionLeaf() : base(0)
        {
        }

        protected sealed override int onUpdate(TBTWorkingData wData)
        {
            int runningStatus = TBTRunningStatus.FINISHED;
            TBTActionLeafContext thisContext = getContext<TBTActionLeafContext>(wData);
            if (thisContext.status == ACTION_READY)
            {
                onEnter(wData);
                thisContext.needExit = true;
                thisContext.status = ACTION_RUNNING;
            }
            if (thisContext.status == ACTION_RUNNING)
            {
                runningStatus = onExecute(wData);
                if (TBTRunningStatus.IsFinished(runningStatus))
                    thisContext.status = ACTION_FINISHED;
            }
            if (thisContext.status == ACTION_FINISHED)
            {
                if (thisContext.needExit)
                    onExit(wData, runningStatus);
                thisContext.status = ACTION_READY;
                thisContext.needExit = false;
            }
            return runningStatus;
        }

        protected override void onTransition(TBTWorkingData wData)
        {
            TBTActionLeafContext thisContext = getContext<TBTActionLeafContext>(wData);
            if (thisContext.needExit)
                onExit(wData, TBTRunningStatus.TRANSITION);
            thisContext.needExit = true;
            thisContext.status = ACTION_READY;
        }

        protected T getUserContexData<T>(TBTWorkingData wData) where T : class, new()
        {
            return getContext<TBTActionLeafContext>(wData).GetUserData<T>();
        }

        protected virtual void onEnter(TBTWorkingData wData)
        {
        }
        protected virtual int onExecute(TBTWorkingData wData)
        {
            return TBTRunningStatus.FINISHED;
        }
        protected virtual void onExit(TBTWorkingData wData, int runningStatus)
        {
        }
        
    }
}
