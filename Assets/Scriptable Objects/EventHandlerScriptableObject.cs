using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EventHandlerScriptableObject", menuName = "CustomEventHandler")]
public class EventHandlerScriptableObject : ScriptableObject
{
    [Header("Crosshair image")]
    public Sprite NormalCrosshair;
    public Sprite GrappleCrosshair;

    [Header("Grapple info")]
    public float GrappleDistance;

    [Header("Coins")]
    public int MaxCoinCount;
    public GameObject CoinPrefab;

    [System.NonSerialized]
    public UnityEvent<bool> OnCrosshairChange;
    [System.NonSerialized]
    public UnityEvent OnCollectingCoin;
    [System.NonSerialized]
    public UnityEvent OnCollidingOnObject;

    private void OnEnable()
    {
        if(OnCrosshairChange == null)
            OnCrosshairChange = new UnityEvent<bool>();

        if(OnCollectingCoin== null)
            OnCollectingCoin = new UnityEvent();

        if(OnCollidingOnObject == null)
            OnCollidingOnObject = new UnityEvent();
    }

    public void SetCrosshair(bool canChange)
    {
        OnCrosshairChange.Invoke(canChange); 
    }

    public void AddCoins()
    {
        OnCollectingCoin.Invoke();
    }

    public void CollidedWithObject()
    {
        OnCollidingOnObject.Invoke();
    }
}
