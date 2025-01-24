using UnityEngine;
using Fusion;

public class Test : NetworkBehaviour
{
    [Networked]
    public int integer { get; set; }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        integer++;
    }
}
