using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBT
{
    public class TBTActionLoop : TBTAction
    {
        public const int INIFITY = -1;

        protected class TBTActionLoopContext : TBTActionContext
        {
            internal int currentCount;
            public TBTActionLoopContext()
            {
                currentCount = 0;
            }
        }
        public int loopCount;
        public TBTActionLoop() : base(1)
        {
        }

        public TBTActionLoop SetLoopCount(int count)
        {
            loopCount = count;
            return this;
        }

        protected override bool onEvaluate(TBTWorkingData wData)
        {
            TBTActionLoopContext thisContext = getContext<TBTActionLoopContext>(wData);
            bool checkLoopCount = loopCount == INIFITY || thisContext.currentCount < loopCount;
            if (!checkLoopCount)
                return false;
            if (IsIndexValid(0))
            {
                TBTAction node = GetChild<TBTAction>(0);
                return node.Evaluate(wData);
            }
            return false;
        }

        protected override int onUpdate(TBTWorkingData wData)
        {
            TBTActionLoopContext thisContext = getContext<TBTActionLoopContext>(wData);
            int runningStatus = TBTRunningStatus.FINISHED;
            if (IsIndexValid(0))
            {
                TBTAction node = GetChild<TBTAction>(0);
                runningStatus = node.Update(wData);
                if (TBTRunningStatus.IsFinished(runningStatus))
                {
                    thisContext.currentCount++;
                    if (loopCount == INIFITY || thisContext.currentCount < loopCount)
                        runningStatus = TBTRunningStatus.EXECUTING;
                }
            }
            return runningStatus;
        }

        protected override void onTransition(TBTWorkingData wData)
        {
            TBTActionLoopContext thisContext = getContext<TBTActionLoopContext>(wData);
            if (IsIndexValid(0))
            {
                TBTAction node = GetChild<TBTAction>(0);
                node.Transition(wData);
            }
            thisContext.currentCount = 0;
        }

    }
}