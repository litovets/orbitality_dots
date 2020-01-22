using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct RocketBaseStats : IComponentData
{
    public float Acceleration;
    public float Lifetime;
    public int Damage;
}
