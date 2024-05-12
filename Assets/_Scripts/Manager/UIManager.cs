using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Crosshair")]
    [SerializeField] private Image _crosshairImage;
    [SerializeField] private EventHandlerScriptableObject _evenHandler;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _coinText;


    private LevelManager _levelManager;

    private void OnEnable()
    {
        _evenHandler.OnCrosshairChange.AddListener(ChangeCrosshair);
    }

    private void OnDisable()
    {
        _evenHandler.OnCrosshairChange.RemoveListener(ChangeCrosshair);
    }

    private void Start()
    {
        _levelManager = LevelManager.instance;
    }
    private void ChangeCrosshair(bool canGrapple)
    {
        if(canGrapple)
        {
            _crosshairImage.sprite = _evenHandler.GrappleCrosshair;  
        }else
            _crosshairImage.sprite = _evenHandler.NormalCrosshair;  
    }

    private void Update()
    {
        SetCoinsCollected();
    }

    private void SetCoinsCollected()
    {
        _coinText.text = _levelManager.GetCoinCount().ToString() + "/" + _evenHandler.MaxCoinCount;
    }
}
