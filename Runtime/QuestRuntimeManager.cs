using System;
using System.Collections;
using System.Collections.Generic;
using static QuestPackage.Interfaces;

namespace QuestPackage
{
    public class QuestRuntimeManager
    {
        #region [Classes]

        public class QuestRuntime
        {
            public Quest Quest { get; }
            public bool IsSetupped { get; set; }
            public bool IsStarted { get; set; }
            public int CurrentProgress { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsRewarded { get; set; }

            public QuestRuntime(Quest quest)
            {
                Quest = quest;
                IsSetupped = false;
                IsStarted = false;
                CurrentProgress = 0;
                IsCompleted = false;
                IsRewarded = false;
            }
        }

        public class QuestGroupRuntime
        {
            public QuestGroup QuestGroup { get; }
            public List<QuestRuntime> QuestRuntimes { get; } = new();

            public QuestGroupRuntime(QuestGroup questGroup)
            {
                QuestGroup = questGroup;
            }
        }

        #endregion

        #region [Variables]
            
        public QuestSettings Settings { get; }
        public SetuppedParameters SetuppedParameters { get; }
        public StartedParameters StartedParameters { get; }
        public ProgressedParameters ProgressedParameters { get; }
        public CompletedParameters CompletedParameters { get; }
        public List<QuestGroupRuntime> QuestGroupRuntimes { get; private set; }
        public QuestRuntime LastCompletedQuestRuntime { get; private set; }
        
        #endregion
        
        public QuestRuntimeManager(
            QuestSettings settings, 
            SetuppedParameters setuppedParameters = null,
            StartedParameters startedParameters = null, 
            ProgressedParameters progressedParameters = null,
            CompletedParameters completedParameters = null)
        {
            Settings = settings;
            SetuppedParameters = setuppedParameters ?? new();
            SetuppedParameters.OnAddProgress += AddQuestProgress;

            StartedParameters = startedParameters ?? new();
            StartedParameters.OnAddProgress += AddQuestProgress;

            ProgressedParameters = progressedParameters ?? new();
            ProgressedParameters.OnAddProgress += AddQuestProgress;

            CompletedParameters = completedParameters ?? new();
            CreateRuntimeBySettings();
        }

        #region [Methods: Create Runtime]
            
        public void CreateRuntimeBySettings()
        {
            QuestGroupRuntimes = CreateQuestGroupRuntimeList(Settings);
        }

        static List<QuestGroupRuntime> CreateQuestGroupRuntimeList(QuestSettings settings)
        {
            var runtimes = new List<QuestGroupRuntime>();
            foreach (var group in settings.QuestGroups)
                runtimes.Add(CreateQuestGroupRuntime(group));
            return runtimes;
        }

        static QuestGroupRuntime CreateQuestGroupRuntime(QuestGroup group)
        {
            var groupRuntime = new QuestGroupRuntime(group);
            foreach (var quest in group.Quests)
                groupRuntime.QuestRuntimes.Add(new QuestRuntime(quest));
            return groupRuntime;
        }

        #endregion

        #region [Methods: Set Runtime]
            
        public void SetRuntime(List<IQuestGroupRuntimeLoader> questGroupRuntimeLoaders)
        {
            foreach (var groupLoader in questGroupRuntimeLoaders)
            {
                if (TryFindOrCreateQuestGroupRuntime(groupLoader.GetGroupID(), out var groupRuntime))
                {
                    var questRuntimeList = groupLoader.GetQuestRuntimeList();
                    foreach (var questLoader in questRuntimeList)
                        SetRuntime(groupRuntime, questLoader);
                }
            }
        }

        static void SetRuntime(QuestGroupRuntime groupRuntime, IQuestRuntimeLoader questLoader)
        {
            var foundQuestRuntime = groupRuntime.QuestRuntimes.Find(q => q.Quest.ID == questLoader.GetQuestID());
            if (foundQuestRuntime != null)
            {
                foundQuestRuntime.CurrentProgress = questLoader.GetProgress();
                foundQuestRuntime.IsCompleted = questLoader.IsCompleted();
                foundQuestRuntime.IsRewarded = questLoader.IsRewarded();
            }
        }

        bool TryFindOrCreateQuestGroupRuntime(string groupID, out QuestGroupRuntime questGroupRuntime)
        {
            var foundGroupRuntime = QuestGroupRuntimes.Find(g => g.QuestGroup.ID == groupID);
            if (foundGroupRuntime != null)
            {
                questGroupRuntime = foundGroupRuntime;
                return true;
            }

            var group = Settings.QuestGroups.Find(g => g.ID == groupID);
            if (group != null)
            {
                var newGroupRuntime = CreateQuestGroupRuntime(group);
                QuestGroupRuntimes.Add(newGroupRuntime);
                questGroupRuntime = newGroupRuntime;
                return true;
            }

            questGroupRuntime = null;
            return false;
        }

        #endregion
    
        #region [Methods: Manage Runtime]
            
        public void SetupAllQuestRuntimes()
        {
            foreach (var groupRuntime in QuestGroupRuntimes)
                foreach (var questRuntime in groupRuntime.QuestRuntimes)
                    SetupQuestRuntime(questRuntime);
        }

        void SetupQuestRuntime(QuestRuntime questRuntime)
        {
            if (!questRuntime.IsSetupped)
            {
                questRuntime.Quest.OnSetupped(SetuppedParameters);
                questRuntime.IsSetupped = true;
            }

            if (questRuntime.IsStarted)
                questRuntime.Quest.OnStarted(StartedParameters);
        }

        public bool TryStartQuest(Quest targetQuest)
        {
            if (TryFindQuestRuntime(targetQuest, out var foundQuestRuntime) &&
                !foundQuestRuntime.IsStarted &&
                !foundQuestRuntime.IsCompleted)
            {
                foundQuestRuntime.Quest.OnStarted(StartedParameters);
                foundQuestRuntime.IsStarted = true;
                return true;
            }
            return false;
        }

        public bool TryStartQuest(int groupIndex, int questIndex)
        {
            if (groupIndex >= 0 && groupIndex < QuestGroupRuntimes.Count)
            {
                var groupRuntime = QuestGroupRuntimes[groupIndex];
                if (questIndex >= 0 && questIndex < groupRuntime.QuestRuntimes.Count)
                {
                    var questRuntime = groupRuntime.QuestRuntimes[questIndex];
                    return TryStartQuest(questRuntime.Quest);
                }
            }
            return false;
        }

        bool TryFindQuestRuntime(Quest targetQuest, out QuestRuntime foundQuestRuntime)
        {
            foreach (var groupRuntime in QuestGroupRuntimes)
            {
                foreach (var questRuntime in groupRuntime.QuestRuntimes)
                {
                    if (questRuntime.Quest == targetQuest)
                    {
                        foundQuestRuntime = questRuntime;
                        return true;
                    }
                }
            }

            foundQuestRuntime = null;
            return false;
        }

        public void AddQuestProgress(Quest quest, int progressIncrement)
        {
            if (TryFindQuestRuntime(quest, out var foundQuestRuntime))
                AddQuestProgress(foundQuestRuntime, progressIncrement);
        }

        void AddQuestProgress(QuestRuntime questRuntime, int progressIncrement)
        {
            questRuntime.CurrentProgress += progressIncrement;
            if (questRuntime.CurrentProgress >= questRuntime.Quest.TargetProgress)
                CompleteQuest(questRuntime);
        }

        public void CompleteQuest(Quest quest)
        {
            if (TryFindQuestRuntime(quest, out var foundQuestRuntime))
                CompleteQuest(foundQuestRuntime);
        }

        void CompleteQuest(QuestRuntime questRuntime)
        {
            if (!questRuntime.IsCompleted)
            {
                questRuntime.Quest.OnCompleted(CompletedParameters);
                questRuntime.IsCompleted = true;
                LastCompletedQuestRuntime = questRuntime;
            }
        }

        #endregion
    }
}
