﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TBT;

namespace AIToolkitDemo
{
    public class AIEntityWorkingData : TBTWorkingData
    {
        public AIEntity entity{ get;set; }
        public Transform entityTF { get; set; }
        public Animator entityAnimator { get; set; }
        public float gameTime;
        public float deltaTime;
    }

    public class AIEntityBehaviourTreeFactory
    {
        private static TBTAction _bevTreeDemo;
        public static TBTAction GetBehaviourTreeDemo()
        {
            if (_bevTreeDemo != null)
                return _bevTreeDemo;
            _bevTreeDemo = new TBTActionPrioritizedSelector();
            _bevTreeDemo
                .AddChild(new TBTActionSequence()
                    .SetPrecondition(new TBTPreconditionNOT(new CON_HasReachedTarget()))
                    .AddChild(new NOD_TurnTo())
                    .AddChild(new NOD_MoveTo()))
                .AddChild(new TBTActionSequence()
                    .AddChild(new NOD_TurnTo())
                    .AddChild(new NOD_Attack())
                );
            return _bevTreeDemo;
        }
    }

    class CON_HasReachedTarget : TBTPreconditionLeaf
    {
        public override bool IsTrue(TBTWorkingData wData)
        {
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            Vector3 targetPos = TMathUtils.Vector3ZeroY(thisData.entity.GetBBValue<Vector3>(AIEntity.BBKEY_NEXTMOVINGPOSITION, Vector3.zero));
            Vector3 currentPos = TMathUtils.Vector3ZeroY(thisData.entityTF.position);
            return TMathUtils.GetDistance2D(targetPos, currentPos) < 1f;
        }
    }
    class NOD_Attack : TBTActionLeaf
    {
        private const float DEFAULT_WAITING_TIME = 5f;
        class UserContextData
        {
            internal float attackingTime;
        }
        protected override void onEnter(TBTWorkingData wData)
        {
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            UserContextData userData = getUserContexData<UserContextData>(wData);
            userData.attackingTime = DEFAULT_WAITING_TIME;
            thisData.entity.PlayAnimation("Attack");
        }
        protected override int onExecute(TBTWorkingData wData)
        {
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            UserContextData userData = getUserContexData<UserContextData>(wData);
            if (userData.attackingTime > 0)
            {
                userData.attackingTime -= thisData.deltaTime;
                if (userData.attackingTime <= 0)
                {
                    thisData.entityAnimator.SetInteger("DeadRnd", Random.Range(0, 3));
                    thisData.entity.PlayAnimation("Dead");
                    thisData.entity.IsDead = true;
                }
            }
            return TBTRunningStatus.EXECUTING;
        }
    }
    class NOD_MoveTo : TBTActionLeaf
    {
        protected override void onEnter(TBTWorkingData wData)
        {
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            if (thisData.entity.IsDead)
            {
                thisData.entity.PlayAnimation("Reborn");
            }
            else
            {
                thisData.entity.PlayAnimation("Walk");
            }
        }
        protected override int onExecute(TBTWorkingData wData)
        {
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            Vector3 targetPos = TMathUtils.Vector3ZeroY(thisData.entity.GetBBValue<Vector3>(AIEntity.BBKEY_NEXTMOVINGPOSITION, Vector3.zero));
            Vector3 currentPos = TMathUtils.Vector3ZeroY(thisData.entityTF.position);
            float distToTarget = TMathUtils.GetDistance2D(targetPos, currentPos);
            if (distToTarget < 1f)
            {
                thisData.entityTF.position = targetPos;
                return TBTRunningStatus.FINISHED;
            }
            else
            {
                int ret = TBTRunningStatus.EXECUTING;
                Vector3 toTarget = TMathUtils.GetDirection2D(targetPos, currentPos);
                float movingStep = 0.5f * thisData.deltaTime;
                if (movingStep > distToTarget)
                {
                    movingStep = distToTarget;
                    ret = TBTRunningStatus.FINISHED;
                }
                thisData.entityTF.position = thisData.entityTF.position + toTarget * movingStep;
                return ret;
            }
        }
    }
    class NOD_TurnTo : TBTActionLeaf
    {
        protected override void onEnter(TBTWorkingData wData)
        {
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            if (thisData.entity.IsDead)
            {
                thisData.entity.PlayAnimation("Reborn");
            }
            else
            {
                thisData.entity.PlayAnimation("Walk");
            }
        }
        protected override int onExecute(TBTWorkingData wData)
        {
            AIEntityWorkingData thisData = wData.As<AIEntityWorkingData>();
            Vector3 targetPos = TMathUtils.Vector3ZeroY(thisData.entity.GetBBValue<Vector3>(AIEntity.BBKEY_NEXTMOVINGPOSITION, Vector3.zero));
            Vector3 currentPos = TMathUtils.Vector3ZeroY(thisData.entityTF.position);
            if (TMathUtils.IsZero((targetPos - currentPos).sqrMagnitude))
            {
                return TBTRunningStatus.FINISHED;
            }
            else
            {
                Vector3 toTarget = TMathUtils.GetDirection2D(targetPos, currentPos);
                Vector3 curFacing = thisData.entityTF.forward;
                float dotV = Vector3.Dot(toTarget, curFacing);
                float deltaAngle = Mathf.Acos(Mathf.Clamp(dotV, -1f, 1f));
                if (deltaAngle < 0.1f)
                {
                    thisData.entityTF.forward = toTarget;
                    return TBTRunningStatus.FINISHED;
                }
                else
                {
                    Vector3 crossV = Vector3.Cross(curFacing, toTarget);
                    float angleToTurn = Mathf.Min(3f * thisData.deltaTime, deltaAngle);
                    if (crossV.y < 0)
                    {
                        angleToTurn = -angleToTurn;
                    }
                    thisData.entityTF.Rotate(Vector3.up, angleToTurn * Mathf.Rad2Deg, Space.World);
                }
            }
            return TBTRunningStatus.EXECUTING;
        }
    }

}