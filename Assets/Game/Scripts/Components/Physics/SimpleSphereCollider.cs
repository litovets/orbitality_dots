using System;
using Unity.Entities;

namespace Game.Physics
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct SimpleSphereCollider : IComponentData
    {
        public float Radius;
    }
}