using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct RocketBaseStats : IComponentData
{
    public float Acceleration;
    public float Weight;
    public float Cooldown;
    public int Damage;
}
