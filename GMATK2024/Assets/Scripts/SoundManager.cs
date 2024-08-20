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
            case AUDIOCLIPTYPE.Explosion:
                if (UserSettings.enableSoundFX)
                {
                    _audioSources[_sourceIndex].volume = 0.35f;
                    _audioSources[_sourceIndex].pitch = 1.0f;
                    _audioSources[_sourceIndex].PlayOneShot(_audioClips[(int)audioType]);
                    IncSourceIndex();
                }
                break;
            case AUDIOCLIPTYPE.Jump:
                if (UserSettings.enableSoundFX)
                {
                    _audioSources[_sourceIndex].volume = 0.35f;
                    _audioSources[_sourceIndex].pitch = Random.Range(0.6f, 1.6f);
                    _audioSources[_sourceIndex].PlayOneShot(_audioClips[(int)audioType]);
                    IncSourceIndex();
                }
                break;
            case AUDIOCLIPTYPE.ExplosionResizableObj:
                if (UserSettings.enableSoundFX)
                {
                    _audioSources[_sourceIndex].volume = 0.25f;
                    _audioSources[_sourceIndex].pitch = 1.0f;
                    _audioSources[_sourceIndex].PlayOneShot(_audioClips[(int)audioType]);
                    IncSourceIndex();
                }
                break;
            default:
            //case AUDIOCLIPTYPE.Open:
            //case AUDIOCLIPTYPE.Shoot:
                if (UserSettings.enableSoundFX)
                {
                    _audioSources[_sourceIndex].volume = 1.0f;
                    _audioSources[_sourceIndex].pitch = 1.0f;
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
