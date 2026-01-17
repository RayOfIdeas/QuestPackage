using UnityEngine;
using QuestPackage;

namespace QuestPackageSample
{
    [CreateAssetMenu(fileName = "Quest_TestMe", menuName = "ScriptableObjects/Quest/Quest_TestMe")]
    public class SampleQuest_TestMe : Quest
    {
        public override void OnSetupped(SetuppedParameters parameters)
        {
            var customParameters = parameters as MyGame_SetupParameters;
            customParameters.gameEvent.OnQuest_EnemyKilled += (amount) =>
            {
                parameters.OnAddProgress(this, 1);
            };
            Debug.Log($"Quest: Setupped");
        }

        public override void OnStarted(StartedParameters parameters)
        {
            Debug.Log($"Quest: Started");
        }

        public override void OnProgressed(ProgressedParameters parameters)
        {
            Debug.Log($"Quest: Progressed");
        }

        public override void OnCompleted(CompletedParameters parameters)
        {
            Debug.Log($"Quest: Completed");
        }
    }
}
