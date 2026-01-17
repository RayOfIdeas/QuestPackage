using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestPackage
{
    [CreateAssetMenu(fileName ="QuestGroup", menuName ="ScriptableObjects/Quest/QuestGroup")]
    [System.Serializable]
    public class QuestGroup : ScriptableObject
    {
        [SerializeField]
        string id;

        [SerializeField]
        List<Quest> quests = new();

        public string ID => id;
        public List<Quest> Quests => quests;
    }
}
