using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 2f;
    [SerializeField] private AudioClip successAudio;
    [SerializeField] private AudioClip failureAudio;
    [SerializeField] private ParticleSystem successParticles;
    [SerializeField] private ParticleSystem crashParticles;

    private AudioSource _audioSource;

    private bool _isControllable = true;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_isControllable)
            return;

        switch (other.gameObject.tag)
        { case "Friendly":
                Debug.Log("Cool");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence()
    {
        _isControllable = false;
        _audioSource.Stop();
        _audioSource.PlayOneShot(failureAudio);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(ReloadLevel), levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        _isControllable = false;
        _audioSource.Stop();
        _audioSource.PlayOneShot(successAudio);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }

    private void ReloadLevel()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void LoadNextLevel()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        var nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }
}