using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIToolkitDemo
{
    public class GameStarter : MonoBehaviour
    {
        void Start()
        {
            GameTimer.Instance.Init();
            gameObject.AddComponent<GameUpdater>();
        }

    }
}