using System.Collections;
using System.Collections.Generic;
using static QuestPackage.QuestRuntimeManager;

namespace QuestPackage
{
    public interface Interfaces
    {
        public interface IQuestGroupRuntimeLoader
        {
            public string GetGroupID();
            public List<IQuestRuntimeLoader> GetQuestRuntimeList();
        }

        public interface IQuestRuntimeLoader
        {
            public string GetQuestID();
            public bool IsStarted();
            public int GetProgress();
            public bool IsCompleted();
            public bool IsRewarded();
        }
    }
}
