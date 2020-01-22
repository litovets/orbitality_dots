using System;
using Unity.Entities;

namespace Game.Physics
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct BodyPhysicsLayer : IComponentData
    {
        public PhysicsLayer Value;
    }
}