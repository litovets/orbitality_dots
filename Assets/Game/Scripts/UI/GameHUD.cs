using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Sprite[] _rocketSprites;
    [SerializeField] private Image _rocketIcon;

    public void UpdateHUD(int rocketIdx)
    {
        _rocketIcon.sprite = _rocketSprites[rocketIdx];
    }
}
