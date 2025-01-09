using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] public List<AudioClip> listBgm;
    public AudioSource music;
    private AudioLowPassFilter currentMusic;
    public static MusicManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        music.Play();
        currentMusic = music.GetComponent<AudioLowPassFilter>();
        PlayRandomMusic();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayRandomMusic();
        }

    }

    public void ChangeBgm(int index)
    {
        music.clip = listBgm[index];
        music.Play();
    }

    public void PlayRandomMusic()
    {
        music.clip = listBgm[Random.Range(0, listBgm.Count)];
        music.Play();
    }

    public IEnumerator ReduceBass(float targetReduce)
    {
        currentMusic.cutoffFrequency = targetReduce;
        yield return null;
    }

    public IEnumerator ResetBass()
    {
        float fullBass = 22000f;
        currentMusic.cutoffFrequency = fullBass;
        yield return null;
    }
}
