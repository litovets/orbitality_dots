using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct NextRocketType : IComponentData
{
    public int Value;
}
