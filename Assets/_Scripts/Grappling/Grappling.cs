using System.Collections;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement _playerMovement;
    public Transform _gunTip;
    [SerializeField] private LayerMask _grappleLayer;
    [SerializeField] private LineRenderer _grappleLine;

    [Header("Grappling")]
    [SerializeField] private float _grappleDelayTime;
    [SerializeField] private float _stopGrappleDelayTime;
    [SerializeField] private float overshootYAxis;
    [SerializeField] private EventHandlerScriptableObject _eventHandler;
    private float _maxGrappleDistance;
    private bool _isGrappling;

    private Vector3 _grapplePoint;

    [Header("Cooldown")]
    [SerializeField]private float grapplingCooldown;
    private float grapplingCooldownTimer;

    InputsManager _inputs;
    AudioManager _audioManager;
    Coroutine _startGrappleCoroutine;
    Coroutine _ExecuteCoroutine;
    private void Start()
    {
        _inputs = InputsManager.Instance;

        _playerMovement = GetComponent<PlayerMovement>();
        _audioManager = AudioManager.instance;
        _maxGrappleDistance = _eventHandler.GrappleDistance;
    }

    private void OnEnable()
    {
        _eventHandler.OnCollidingOnObject.AddListener(StopGrapple);
    }

    private void OnDisable()
    {
        _eventHandler.OnCollidingOnObject.RemoveListener(StopGrapple);
    }
    private void Update()
    {
        if (_inputs.Grapple)
        {
            _startGrappleCoroutine = StartCoroutine(StartGrapple());
            _inputs.Grapple = false;    
        }

        if(grapplingCooldownTimer > 0)
        {
            grapplingCooldownTimer -= Time.deltaTime;   
        }
    }

    private IEnumerator StartGrapple()
    {
        if (grapplingCooldownTimer > 0) yield break;

        _isGrappling = true;

        // Freeze the player to let him reach the grapple point
        //_playerMovement.freezePlayer = true;

        RaycastHit hit;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out hit, _maxGrappleDistance, _grappleLayer))
        {
            _grapplePoint = hit.point;

            // Use the grapple delay time to do the grapple animation and effects
            yield return new WaitForSeconds(_grappleDelayTime);
            _ExecuteCoroutine = StartCoroutine(ExecuteGrapple());
            
        }else
        {
            _grapplePoint = ray.origin+ ray.direction * _maxGrappleDistance;

            yield return new WaitForSeconds(_stopGrappleDelayTime);
            StopGrapple();
              
        }
        _audioManager.PlaySound(Helper.GRAPPLE_SOUND);
        _playerMovement.RotatePlayerToShootPoint(_grapplePoint);

        yield break;
    }

    private IEnumerator ExecuteGrapple()
    {
        //_playerMovement.freezePlayer = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        
        float grapplePointRelativeYPos = _grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if(grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        _playerMovement.JumpToPosition(_grapplePoint, highestPointOnArc);

        
        yield return new WaitForSeconds(1f);
        StopGrapple();
        yield break;
    }

    public void StopGrapple()
    {
        //_playerMovement.freezePlayer = false;

        _isGrappling = false;

        grapplingCooldownTimer = grapplingCooldown;
    }

    public bool IsGrappling()
    {
        return _isGrappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return _grapplePoint;
    }
}
