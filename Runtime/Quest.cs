using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestPackage
{
    public abstract class Quest : ScriptableObject
    {
        [SerializeField]
        string id;

        [SerializeField]
        int targetProgress = 1;
        
        public string ID => id;
        public int TargetProgress => targetProgress;

        public virtual void OnSetupped(SetuppedParameters parameters) { }
        public virtual void OnStarted(StartedParameters parameters) { }
        public virtual void OnProgressed(ProgressedParameters parameters) { }
        public virtual void OnCompleted(CompletedParameters parameters) { }
    }
}
