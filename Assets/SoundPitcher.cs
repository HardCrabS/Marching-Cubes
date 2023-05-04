using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPitcher : MonoBehaviour
{
    public Vector2 volumeRange = new Vector2(0.9f, 1f);
    public Vector2 pitchRange = new Vector2(0.9f, 1f);

    AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        float volume = Random.Range(volumeRange.x, volumeRange.y);
        float pitch = Random.Range(pitchRange.x, pitchRange.y);

        audioSource.volume = volume;
        audioSource.pitch = pitch;

        audioSource.PlayOneShot(clip);
    }
}
