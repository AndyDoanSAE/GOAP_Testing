using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : GAgent
{
    protected override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("wander", 1, true);
        goals.Add(s1, 3);
    }
}
