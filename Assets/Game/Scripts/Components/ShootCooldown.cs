using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ShootCooldown : IComponentData
{
    public float Value;
}
