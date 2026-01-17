namespace QuestPackageSample
{
    public class MyGame_SetupParameters : QuestPackage.SetuppedParameters
    {
        public MyGameEvent gameEvent;
        public MyGame_SetupParameters(MyGameEvent gameEvent)
        {
            this.gameEvent = gameEvent;
        }
    }

    public class MyGame_StartedParameters : QuestPackage.StartedParameters
    {
        public MyGameEvent gameEvent;
        public MyGame_StartedParameters(MyGameEvent gameEvent)
        {
            this.gameEvent = gameEvent;
        }
    }

    public class MyGame_ProgressParameters : QuestPackage.ProgressedParameters
    {
        public MyGameEvent gameEvent;
        public MyGame_ProgressParameters(MyGameEvent gameEvent)
        {
            this.gameEvent = gameEvent;
        }
    }

    public class MyGame_CompleteParameters : QuestPackage.CompletedParameters
    {
        public MyGameEvent gameEvent;
        public MyGame_CompleteParameters(MyGameEvent gameEvent)
        {
            this.gameEvent = gameEvent;
        }
    }
}