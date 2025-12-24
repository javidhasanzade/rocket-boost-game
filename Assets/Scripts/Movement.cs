using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;
    [SerializeField] private float thrustStrength = 100f;
    [SerializeField] private float rotationStrength = 100f;
    [SerializeField] private AudioClip mainEngineAudio;
    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem rightThrustParticles;
    [SerializeField] private ParticleSystem leftThrustParticles;
    
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
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        _rb.AddRelativeForce(Vector3.up * (thrustStrength * Time.fixedDeltaTime));
        if (!_audioSource.isPlaying)
            _audioSource.PlayOneShot(mainEngineAudio);
            
        if (!mainEngineParticles.isPlaying)
            mainEngineParticles.Play();
    }
    
    private void StopThrusting()
    {
        _audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ProcessRotation()
    {
        var rotationInput = rotation.ReadValue<float>();

        switch (rotationInput)
        {
            case < 0:
                RotateRight();
                break;
            case > 0:
                RotateLeft();
                break;
            default:
                StopRotating();
                break;
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(-rotationStrength);
        if (!leftThrustParticles.isPlaying)
            leftThrustParticles.Play();
    }

    private void RotateRight()
    {
        ApplyRotation(rotationStrength);
        if (!rightThrustParticles.isPlaying)
            rightThrustParticles.Play();
    }
    
    private void StopRotating()
    {
        leftThrustParticles.Stop();
        rightThrustParticles.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        _rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * (rotationThisFrame * Time.fixedDeltaTime));
        _rb.freezeRotation = false;
    }
}