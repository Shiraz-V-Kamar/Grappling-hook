using JetBrains.Annotations;
using UnityEngine;

public class GrappleIndicator : MonoBehaviour
{
    [SerializeField] private LayerMask _grappleLayer;

    [SerializeField] private EventHandlerScriptableObject _eventHandler;

    private float _grappleDistance;
    [SerializeField]private bool _canGrapple;
    private void Start()
    {
        _grappleDistance = _eventHandler.GrappleDistance;
    }
    private void Update()
    {
        RaycastHit hit;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _grappleLayer))
        {
            float distance = Vector3.Distance(hit.point, ray.origin);
            _canGrapple = distance <= _grappleDistance ? true : false;

            _eventHandler.SetCrosshair(_canGrapple);
        }else
        {
            _eventHandler.SetCrosshair(false);

        }

      
    }
}
