using System;
using Unity.Entities;

[Serializable]
public struct GamePause : IComponentData
{
    public bool IsOn;
}
