using System;

namespace Game.Physics
{
    [Flags]
    public enum PhysicsLayer
    {
        None = 0x0,
        Planet = 0x1,
        Rocket = 0x2
    }
}