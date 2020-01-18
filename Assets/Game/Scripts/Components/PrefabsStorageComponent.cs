using Unity.Entities;

[GenerateAuthoringComponent]
public struct PrefabsStorageComponent : IComponentData
{
    public Entity PlanetPrefab;
}
