using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _movement;

    private AudioSource[] _audioSources;

    private int _sourceIndex;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        _audioSources = GetComponentsInChildren<AudioSource>();
        _sourceIndex = 0;

        _movement.clip = _audioClips[(int)AUDIOCLIPTYPE.Movement];

        PlayClip(AUDIOCLIPTYPE.Background);
    }

    public void PlayClip(AUDIOCLIPTYPE audioType)
    {
        switch (audioType)
        {
            case AUDIOCLIPTYPE.Movement:
                if (_movement.isPlaying == false)
                {
                    _movement.Play();
                }
                break;
            case AUDIOCLIPTYPE.Background:
                if (UserSettings.enableMusic)
                {
                    _backgroundMusic.clip = _audioClips[(int)audioType];
                    _backgroundMusic.Play();
                }
                break;
            default:
            //case AUDIOCLIPTYPE.ExplosionResizableObj:
            //case AUDIOCLIPTYPE.Explosion:
            //case AUDIOCLIPTYPE.Open:
            //case AUDIOCLIPTYPE.Shoot:
                if (UserSettings.enableSoundFX)
                {
                    _audioSources[_sourceIndex].PlayOneShot(_audioClips[(int)audioType]);
                    IncSourceIndex();
                }
                break;
        }
    }

    public void StopClip(AUDIOCLIPTYPE audioType)
    {
        switch (audioType)
        {
            case AUDIOCLIPTYPE.Movement:
                _movement.Pause();
                break;
            default:
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
