using System;
using System.Collections;
using System.Collections.Generic;

namespace QuestPackage
{
    public class SetuppedParameters
    {
        public Action<Quest, int> OnAddProgress { get; set; }
    }

    public class StartedParameters
    {
        public Action<Quest, int> OnAddProgress { get; set; }
    }

    public class ProgressedParameters 
    {
        public Action<Quest, int> OnAddProgress { get; set; }
    }

    public class CompletedParameters {}
}
