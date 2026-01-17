using System;
using System.Collections;
using QuestPackage;
using UnityEngine;

namespace QuestPackageSample
{
    public class SampleQuestManager : MonoBehaviour
    {
        [SerializeField]
        QuestSettings questSettings;
        QuestRuntimeManager questRuntimeManager;
        readonly MyGameEvent myGameEvent = new();

        IEnumerator Start()
        {
            questRuntimeManager = new QuestRuntimeManager(
                questSettings,
                new MyGame_SetupParameters(myGameEvent),
                new MyGame_StartedParameters(myGameEvent),
                new MyGame_ProgressParameters(myGameEvent),
                new MyGame_CompleteParameters(myGameEvent)
            );

            questRuntimeManager.SetupAllQuestRuntimes();
            questRuntimeManager.TryStartQuest(0,0);
            yield return new WaitForSeconds(1f);
            myGameEvent.OnQuest_EnemyKilled?.Invoke(1);
        }
    }

    public class MyGameEvent
    {
        public Action<int> OnQuest_EnemyKilled;
    }
}
