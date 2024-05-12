using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]private EventHandlerScriptableObject _evenHandler;
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == Helper.COIN_TAG)
        {
            //_levelManager.AddCoinCount();
            _evenHandler.AddCoins();
            other.gameObject.SetActive(false);  
        }
    }

}
