using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TBT
{
    public class TBTActionParellel : TBTAction
    {
        public enum ECHILDREN_RELATIONSHIP
        {
            AND,OR
        }

        protected class TBTActionParellelContext : TBTActionContext
        {
            internal List<bool> evaluationStatus;
            internal List<int> runningStatus;
            public TBTActionParellelContext()
            {
                evaluationStatus = new List<bool>();
                runningStatus = new List<int>();
            }
        }

        private ECHILDREN_RELATIONSHIP _evaluationRelationShip;
        private ECHILDREN_RELATIONSHIP _runningStatusRelationShip;

        public TBTActionParellel SetEvaluateRelationShip(ECHILDREN_RELATIONSHIP status)
        {
            _evaluationRelationShip = status;
            return this;
        }

        public TBTActionParellel SetRunningStatusRelationship(ECHILDREN_RELATIONSHIP status)
        {
            _runningStatusRelationShip = status;
            return this;
        }

        public TBTActionParellel() : base(-1)
        {
            _evaluationRelationShip = ECHILDREN_RELATIONSHIP.AND;
            _runningStatusRelationShip = ECHILDREN_RELATIONSHIP.OR;
        }

        protected override bool onEvaluate(TBTWorkingData wData)
        {
            TBTActionParellelContext thisContext = getContext<TBTActionParellelContext>(wData);
            initListTo<bool>(thisContext.evaluationStatus, false);
            bool finalResult = false;
            for (int i = 0; i < GetChildCount(); i++)
            {
                TBTAction node = GetChild<TBTAction>(i);
                bool ret = node.Evaluate(wData);
                if (_evaluationRelationShip == ECHILDREN_RELATIONSHIP.AND && ret == false)
                {
                    finalResult = false;
                    break;
                }
                if (ret)
                    finalResult = true;
                thisContext.evaluationStatus[i] = ret;
            }
            return finalResult;
        }

        protected override int onUpdate(TBTWorkingData wData)
        {
            TBTActionParellelContext thisContext = getContext<TBTActionParellelContext>(wData);
            if (thisContext.runningStatus.Count != GetChildCount())
                initListTo<int>(thisContext.runningStatus, TBTRunningStatus.EXECUTING);
            bool hasFinished = false;
            bool hasExecuting = false;
            for (int i = 0; i < GetChildCount(); i++)
            {
                if (thisContext.evaluationStatus[i] == false)
                    continue;
                if (TBTRunningStatus.IsFinished(thisContext.runningStatus[i]))
                {
                    hasFinished = true;
                    continue;
                }

                TBTAction node = GetChild<TBTAction>(i);
                int runningStatus = node.Update(wData);
                if (TBTRunningStatus.IsFinished(runningStatus))
                    hasFinished = true;
                else
                    hasExecuting = true;
                thisContext.runningStatus[i] = runningStatus;
            }
            if (_runningStatusRelationShip == ECHILDREN_RELATIONSHIP.OR && hasFinished || _runningStatusRelationShip == ECHILDREN_RELATIONSHIP.AND && hasExecuting == false)
            {
                initListTo<int>(thisContext.runningStatus, TBTRunningStatus.EXECUTING);
                return TBTRunningStatus.FINISHED;
            }
            return TBTRunningStatus.EXECUTING;
        }

        protected override void onTransition(TBTWorkingData wData)
        {
            TBTActionParellelContext thisContext = getContext<TBTActionParellelContext>(wData);
            for (int i = 0; i < GetChildCount(); i++)
            {
                TBTAction node = GetChild<TBTAction>(i);
                node.Transition(wData);
            }
            initListTo<int>(thisContext.runningStatus, TBTRunningStatus.EXECUTING);
        }

        private void initListTo<T>(List<T> list, T value)
        {
            int childCount = GetChildCount();
            if (list.Count != childCount)
            {
                list.Clear();
                for (int i = 0; i < childCount; i++)
                    list.Add(value);
            }
            else
            {
                for (int i = 0; i < childCount; i++)
                    list[i] = value;
            }
        }

    }
}