using UnityEngine;
using UnityEngine.Windows;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform playerObj;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed;

    private InputsManager _inputs;
    private void Start()
    {
        _inputs = InputsManager.Instance;
    }
    private void Update()
    {
       
            
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            _orientation.forward = viewDir.normalized;

            //rotate player Obj
            Vector3 inputDir = _orientation.forward * _inputs.Move.y + _orientation.right * _inputs.Move.x;

            if (_inputs.Move != Vector2.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * _rotationSpeed);
            }
        
    }
}
