using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct RocketBaseStats : IComponentData
{
    public float Acceleration;
    public float Weight;
    public float Lifetime;
    public int Damage;
}
