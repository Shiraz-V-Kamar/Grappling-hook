using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]private float _moveSpeed;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultipliyer;
    private bool _canJump = true;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundDistance = 0.2f;
    [SerializeField] private float _groundDrag;
    private bool _isGrounded;

    private bool _enableMovementOnNextToubh;

    [SerializeField] private Transform _orientation;

    [HideInInspector] public bool freezePlayer;
    [HideInInspector] public bool acitveGrappling;

    [Header("EventHandler")]
    [SerializeField] private EventHandlerScriptableObject _eventHandler;

    private Vector2 _move;

    private Vector3 _moveDirection;
    private Vector3 _grapppleVelocityToSet;

    private Rigidbody _rb;
    private InputsManager _inputs;
    //private Grappling _grapple;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //_grapple = GetComponent<Grappling>();
        _rb.freezeRotation = true;

        _inputs = InputsManager.Instance;
    }

    private void Update()
    {

        // Changing the drag to make the air movement more free
        if (_isGrounded && !acitveGrappling)
        {
            _rb.drag = _groundDrag;
        }
        else
        {
            _rb.drag = 0;
        }


        // Control the spped of the Player
        SpeedControl();

        // Jump
        if(_inputs.Jump && _isGrounded && _canJump)
        {
            _canJump = false;

            Jump();

            Invoke(nameof(ResetJump),_jumpCooldown);
        }

    }


    private void FixedUpdate()
    {
        if (!freezePlayer)
        {
            MovePlayer();
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
        GroundCheck();
    }

   

    private void SpeedControl()
    {
        // Dont change the speed when grappling
        if (acitveGrappling) return;

        Vector3 Velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        // Limiting the velocity when needed
        if(Velocity.magnitude > _moveSpeed)
        {
            Vector3 LimitedVelocity = Velocity.normalized * _moveSpeed;
            _rb.velocity = new Vector3(Velocity.x, _rb.velocity.y, Velocity.z);
        }
    }
    private void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        acitveGrappling = true;


        _grapppleVelocityToSet  = CalculateJumpVelocity(transform.position,targetPosition, trajectoryHeight);
        Invoke(nameof(SetGrappleVelocity), 0.1f);

        // Safety check
        Invoke(nameof(ResetRestriction), 3f);
    }

    private void SetGrappleVelocity()
    {
        _enableMovementOnNextToubh = true;
        _rb.velocity = _grapppleVelocityToSet;
    }

    private void MovePlayer()
    {
        if (acitveGrappling) return;

        //Gets the forward of the player and then moves the player accordingly
        _moveDirection = _orientation.forward* _inputs.Move.y + _orientation.right* _inputs.Move.x;

        if (_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed *10f, ForceMode.Force);

        else
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * _airMultipliyer , ForceMode.Force);

        ApplyGravity();
    }
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void RotatePlayerToShootPoint(Vector3 GrappleShootPoint)
    {
        Vector3 targetDir =   GrappleShootPoint - transform.position;
        targetDir.y = 0f;   // To make sure that the player doesnt look up or down

        if(targetDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _rb.MoveRotation(targetRotation);
        }
    }
    private void Jump()
    {
        
        //Reset y velocity to jump the same height every time
        _rb.velocity = new Vector3(_rb.velocity.x, 0f,_rb.velocity.z);

        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ApplyGravity()
    {
        if (acitveGrappling) return;
        if (!_isGrounded)
        {
            _rb.AddForce(Vector3.up * Physics.gravity.y, ForceMode.Acceleration);
        }
    }
    private void ResetJump()
    {
        _canJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_enableMovementOnNextToubh)
        {
            _enableMovementOnNextToubh = false;
            ResetRestriction();

            //_grapple.StopGrapple();
            _eventHandler.CollidedWithObject();
        }
    }

    public void ResetRestriction()
    {
        acitveGrappling = false;
    }
}
