using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class PlanetHUD : MonoBehaviour
{
    [SerializeField] private Image _hp;
    [SerializeField] private Image _cooldown;

    private Entity _entity;
    private RectTransform _rectTransform;
    private int _screenWidth;
    private int _screenHeight;
    private int _canvasHeight = 600;
    private double _elapsedTime;

    private void Start()
    {        
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
    }

    public void Initialize(Entity entity)
    {
        _entity = entity;

        Scale scale = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Scale>(_entity);
        float size = 24f * scale.Value;
        Vector2 imgSize = new Vector2(size, size);
        _rectTransform = transform as RectTransform;
        _hp.rectTransform.sizeDelta = _cooldown.rectTransform.sizeDelta = imgSize;
    }

    public void UpdateHud(TimeData time)
    {
        if (_entity == Entity.Null)
        {
            Destroy(gameObject);
            return;
        }

        Translation translation = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(_entity);
        Health health = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Health>(_entity);
        ShootCooldown cooldown = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ShootCooldown>(_entity);
        ShootTime shootTime = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ShootTime>(_entity);

        var screenPoint = Camera.main.WorldToScreenPoint(new Vector3(translation.Value.x, translation.Value.y, translation.Value.z));
        Vector2 hudPos = new Vector3((screenPoint.x - _screenWidth * 0.5f) * ((float)_canvasHeight / _screenHeight), (screenPoint.y - _screenHeight * 0.5f) * ((float)_canvasHeight / _screenHeight));
        _rectTransform.anchoredPosition = hudPos;

        _hp.fillAmount = health.Value / 100f;
        _cooldown.fillAmount = 1f - (float)(time.ElapsedTime - shootTime.Value) / cooldown.Value;
    }
}
