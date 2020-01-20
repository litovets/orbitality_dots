using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ChangeSphereColliderRadius : IComponentData
{
    public float Value;
}
