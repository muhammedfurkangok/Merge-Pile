using System;
using Runtime.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : SingletonMonoBehaviour<CoreGameSignals>
    {
        public UnityAction onLevelInitialize = delegate { };
        public UnityAction onRestartLevel = delegate { };
        public UnityAction onPlay = delegate { }; 
    }
}