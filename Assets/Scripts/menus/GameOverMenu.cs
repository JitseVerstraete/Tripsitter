using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sirens;
    [SerializeField] private AudioClip crashSound;

    [Tooltip("tv reference so we can turn it off")]
    [SerializeField] private GameObject tv;

    private void Start()
    {
        tv.SetActive(false); // turn off tv

        audioSource.clip = crashSound;
        audioSource.time = 0.8f;
        audioSource.Play();
        Invoke("PlaySirenSounds", crashSound.length -2f); // The -1 is a hack because I dont want to edit the audio file right now
    }

    private void PlaySirenSounds()
    {
        Debug.Log("Sirens");
        //audioSource.PlayOneShot(sirens);
        audioSource.clip = sirens;
        audioSource.Play();
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
