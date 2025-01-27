using UnityEngine;
using Fusion;

public class Test : NetworkBehaviour, IAfterSpawned
{
    [Networked]
    public float hp { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsPlayer) return;
        base.FixedUpdateNetwork();

        hp -= Time.fixedDeltaTime;
    }

    public void AfterSpawned()
    {
        if (!Runner.IsPlayer) return;
        print("adasd");
        hp = 100;
    }
}
