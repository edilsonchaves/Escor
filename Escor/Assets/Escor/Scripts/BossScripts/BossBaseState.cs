using System;
using UnityEngine;

[Serializable]
public abstract class BossBaseState
{
    public abstract void EnterState(BossScript boss);

    public abstract void UpdateState(BossScript boss);

    public abstract void OnCollisionEnter(BossScript boss, Collision2D collsion);

    public abstract string GetStateName();
}
