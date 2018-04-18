using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBT
{
    public class TBTActionPrioritizedSelector : TBTAction
    {
        protected class TBTActionPrioritizedSelectorContext : TBTActionContext
        {
            internal int currentSelectedIndex;
            internal int lastSelectedIndex;
            public TBTActionPrioritizedSelectorContext()
            {
                currentSelectedIndex = -1;
                lastSelectedIndex = -1;
            }
        }

        public TBTActionPrioritizedSelector() : base(-1)
        {
        }

        protected override bool onEvaluate(TBTWorkingData wData)
        {
            TBTActionPrioritizedSelectorContext thisContext = getContext<TBTActionPrioritizedSelectorContext>(wData);
            thisContext.currentSelectedIndex = -1;
            for (int i = 0; i < GetChildCount(); i++)
            {
                TBTAction node = GetChild<TBTAction>(i);
                if (node.Evaluate(wData))
                {
                    thisContext.currentSelectedIndex = i;
                    return true;
                }
            }
            return false;
        }

        protected override int onUpdate(TBTWorkingData wData)
        {
            TBTActionPrioritizedSelectorContext thisContext = getContext<TBTActionPrioritizedSelectorContext>(wData);
            int runningStatus = TBTRunningStatus.FINISHED;
            if (thisContext.lastSelectedIndex != thisContext.currentSelectedIndex)
            {
                if (IsIndexValid(thisContext.lastSelectedIndex))
                {
                    TBTAction node = GetChild<TBTAction>(thisContext.lastSelectedIndex);
                    node.Transition(wData);
                }
                thisContext.lastSelectedIndex = thisContext.currentSelectedIndex;
            }
            if (IsIndexValid(thisContext.currentSelectedIndex))
            {
                TBTAction node = GetChild<TBTAction>(thisContext.currentSelectedIndex);
                runningStatus = node.Update(wData);
                if (TBTRunningStatus.IsFinished(runningStatus))
                    thisContext.lastSelectedIndex = -1;
            }
            return runningStatus;
        }

        protected override void onTransition(TBTWorkingData wData)
        {
            TBTActionPrioritizedSelectorContext thisContext = getContext<TBTActionPrioritizedSelectorContext>(wData);
            TBTAction node = GetChild<TBTAction>(thisContext.lastSelectedIndex);
            if (node != null)
                node.Transition(wData);
            thisContext.lastSelectedIndex = -1;
        }

    }
}