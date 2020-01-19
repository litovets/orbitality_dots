using System;
using Unity.Entities;

[Serializable]
public struct CreationTime : IComponentData
{
    public float Value;
}
