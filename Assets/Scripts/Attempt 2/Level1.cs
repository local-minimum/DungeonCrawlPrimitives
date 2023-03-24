using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungAtt2
{
    [ExecuteInEditMode]
    public class Level1 : Level
    {
        [HideInInspector]
        protected string[] startGrid = new string[] {
            "---xx---xx-------",
            "---xxx--xx----xxx",
            "---x----x----xxxx",
            "---x----x----xxxx",
            "---xxxxxxxxxxxxx-",
            "----------x------",
            "Pxxxxxxxxxx------",
        };

        protected override string[] grid { get => startGrid; }

        protected override int lvl => 1;
    }
}