using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sirens;
    [SerializeField] private AudioClip crashSound;

    private void Start()
    {
        audioSource.PlayOneShot(crashSound);
        Invoke("PlaySirenSounds", crashSound.length -1); // The -1 is a hack because I dont want to edit the audio file right now
        
    }

    private void PlaySirenSounds()
    {
        audioSource.PlayOneShot(sirens);
    }

    private void OnDestroy()
    {
        audioSource.Stop();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
