using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIToolkitDemo
{
    public class AIBehaviourRequest
    {
        public float timeStamp
        {
            get;
            private set;
        }
        public Vector3 nextMovingTarget
        {
            get;
            private set;
        }
        public AIBehaviourRequest(float timeStamp, Vector3 nextMovingTarget)
        {
            this.timeStamp = timeStamp;
            this.nextMovingTarget = nextMovingTarget;
        }
    }
}