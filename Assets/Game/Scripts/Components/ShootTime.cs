using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ShootTime : IComponentData
{
    public float Value;
}
