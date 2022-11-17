using System;
using System.Collections.Generic;
using System.Text;

namespace NEA.Tasks
{
    internal class dump
    {
        private readonly Dictionary<string, string[]> Muscles = new Dictionary<string, string[]>()
        {
                {"Chest",new string[]{"upper","lower","inner"} },
                {"Triceps",new string[]{"longhead","lateralhead","medialhead"} },
                {"Back",new string[]{"lats","middleback","lowerback"} },
                {"Biceps",new string[]{"long","short"} },
                {"Shoulders",new string[]{"anteriordelts","medialdelts","posteriordelts"} },
                {"Legs", new string[] {"quadriceps","hamstrings","glutes","calves","abductors"} },
        };

    }
}
