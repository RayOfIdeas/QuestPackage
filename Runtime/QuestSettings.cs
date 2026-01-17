using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestPackage
{
    [CreateAssetMenu(fileName ="QuestSettings", menuName ="ScriptableObjects/Quest/QuestSettings")]

    public class QuestSettings : ScriptableObject
    {
        [SerializeField]
        List<QuestGroup> questGroups = new();
        public List<QuestGroup> QuestGroups => questGroups;
    }
}
