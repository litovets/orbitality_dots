using System;
using Unity.Entities;

[Serializable]
public struct TakeDamage : IComponentData
{
    public int Value;
}
