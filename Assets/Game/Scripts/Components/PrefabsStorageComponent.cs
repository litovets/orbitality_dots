using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PrefabsStorageComponent : IComponentData
{
    public Entity PlanetPrefab;
    public Entity TomahawkRocketPrefab;
    public Entity RocketPrefab;
    public Entity HeavyRocketPrefab;
    public GameObject Canvas;
    public GameObject PlanetHUD;
    public GameObject GameHUD;
}
