using UnityEngine;

public class MusicManagedr : MonoBehaviour
{
    public AudioSource musicSource;

    void Start()
    {
        musicSource.Play();  // Starts music
    }

    public void StopMusic()
    {
        musicSource.Stop();  // Stops music
    }
}
