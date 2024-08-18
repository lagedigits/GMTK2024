using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _backgroundMusic;

    private AudioSource[] _audioSources;

    private int _sourceIndex;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        _audioSources = GetComponentsInChildren<AudioSource>();
        _sourceIndex = 0;

        PlayClip(AUDIOCLIPTYPE.Background);
    }

    public void PlayClip(AUDIOCLIPTYPE audioType)
    {
        switch (audioType)
        {
            case AUDIOCLIPTYPE.Background:
                if (UserSettings.enableMusic)
                {
                    _backgroundMusic.clip = _audioClips[(int)audioType];
                    _backgroundMusic.Play();
                }
                break;
            case AUDIOCLIPTYPE.ExplosionResizableObj:
            case AUDIOCLIPTYPE.Explosion:
            case AUDIOCLIPTYPE.Open:
                if (UserSettings.enableSoundFX)
                {
                    _audioSources[_sourceIndex].PlayOneShot(_audioClips[(int)audioType]);
                    IncSourceIndex();
                }
                break;
        }
    }

    void IncSourceIndex()
    {
        _sourceIndex++;

        if (_sourceIndex >= _audioClips.Length)
        {
            _sourceIndex = 0;
        }
    }
}
