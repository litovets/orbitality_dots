using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct CreatePlayers : IComponentData
{
    public int Value;
}
