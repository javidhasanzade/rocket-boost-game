using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;
    [SerializeField] private float thrustStrength = 100f;
    [SerializeField] private float rotationStrength = 100f;
    [SerializeField] private AudioClip mainEngineAudio;

    private Rigidbody _rb;
    private AudioSource _audioSource;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            _rb.AddRelativeForce(Vector3.up * (thrustStrength * Time.fixedDeltaTime));
            if (!_audioSource.isPlaying)
                _audioSource.PlayOneShot(mainEngineAudio);
        }
        else
        {
            _audioSource.Stop();
        }
    }

    private void ProcessRotation()
    {
        var rotationInput = rotation.ReadValue<float>();

        switch (rotationInput)
        {
            case < 0:
                ApplyRotation(rotationStrength);
                break;
            case > 0:
                ApplyRotation(-rotationStrength);
                break;
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        _rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * (rotationThisFrame * Time.fixedDeltaTime));
        _rb.freezeRotation = false;
    }
}