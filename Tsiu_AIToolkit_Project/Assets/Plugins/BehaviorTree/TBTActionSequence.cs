using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBT
{
    public class TBTActionSequence : TBTAction
    {
        private bool _continueIfErrorOccurs;
        protected class TBTActionSequenceContext : TBTActionContext
        {
            internal int currentSelectedIndex = -1;
            public TBTActionSequenceContext()
            {
                currentSelectedIndex = 0;
            }
        }

        public TBTActionSequence() : base(-1)
        {
            _continueIfErrorOccurs = false;
        }
        public void SetContinueIfErrorOccurs(bool value)
        {
            _continueIfErrorOccurs = value;
        }

        protected override bool onEvaluate(TBTWorkingData wData)
        {
            TBTActionSequenceContext thisContext = getContext<TBTActionSequenceContext>(wData);
            int checkNodeIndex = IsIndexValid(thisContext.currentSelectedIndex) ? thisContext.currentSelectedIndex : 0;
            if (IsIndexValid(checkNodeIndex))
            {
                TBTAction node = GetChild<TBTAction>(checkNodeIndex);
                if (node.Evaluate(wData))
                {
                    thisContext.currentSelectedIndex = checkNodeIndex;
                    return true;
                }
            }
            return false;
        }

        protected override int onUpdate(TBTWorkingData wData)
        {
            TBTActionSequenceContext thisContext = getContext<TBTActionSequenceContext>(wData);
            int runningStatus = TBTRunningStatus.FINISHED;
            if (IsIndexValid (thisContext.currentSelectedIndex))
            {
                TBTAction node = GetChild<TBTAction>(thisContext.currentSelectedIndex);
                runningStatus = node.Update(wData);
                if (_continueIfErrorOccurs == false && TBTRunningStatus.IsError(runningStatus))
                {
                    thisContext.currentSelectedIndex = - 1;
                    return runningStatus;
                }
            }
            if (TBTRunningStatus.IsFinished(runningStatus))
            {
                thisContext.currentSelectedIndex++;
                if (IsIndexValid(thisContext.currentSelectedIndex))
                {
                    runningStatus = TBTRunningStatus.EXECUTING;
                }
                else
                {
                    thisContext.currentSelectedIndex = -1;
                }
            }
            return runningStatus;
        }

        protected override void onTransition(TBTWorkingData wData)
        {
            TBTActionSequenceContext thisContext = getContext<TBTActionSequenceContext>(wData);
            TBTAction node = GetChild<TBTAction>(thisContext.currentSelectedIndex);
            if(node != null)
                node.Transition(wData);
        }

    }
}