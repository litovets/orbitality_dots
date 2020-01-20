using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(PlanetDamageSystem))]
public class PlanetHUDSystem : ComponentSystem
{
    private Dictionary<Entity, PlanetHUD> _huds = new Dictionary<Entity, PlanetHUD>();

    protected override void OnUpdate()
    {
        if (GetSingleton<GamePause>().IsOn)
            return;

        Entities.ForEach((Entity e, ref AddPlanetHUD addHud) =>
        {
            PrefabsStorageComponent prefabsStorage = GetSingleton<PrefabsStorageComponent>();
            GameObject hudObj = GameObject.Instantiate(prefabsStorage.PlanetHUD, prefabsStorage.Canvas.transform);
            PlanetHUD hud = hudObj.GetComponent<PlanetHUD>();
            hud?.Initialize(e);
            _huds.Add(e, hud);

            PostUpdateCommands.RemoveComponent(e, typeof(AddPlanetHUD));
        });

        Entities.ForEach((Entity e, ref RemovePlanetHUD removeHud) =>
        {
            if (_huds.ContainsKey(e))
            {
                var hud = _huds[e];
                _huds.Remove(e);
                GameObject.Destroy(hud.gameObject, 0.1f);
            }
        });

        foreach (var hud in _huds.Values)
        {
            hud.UpdateHud(Time);
        }
    }
}