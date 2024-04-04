using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private List<AudioElement> audioList = new List<AudioElement>();
    [SerializeField] private AudioSource musicAudioSource;

    public AudioType sound;
    public AudioType music;

    private void Awake() {
        UserData.LoadAudioSettings(ref sound,ref music);

        musicAudioSource.enabled = music.IsEnabled();

        if(instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(SoundType soundType) {
        if(!sound.IsEnabled()) {
            return;
        }

        AudioElement audioElement = audioList.Find(element => element.type == soundType);

        if(audioElement.type == soundType) {
            audioElement.Play();
        }
    }

    public bool SwitchMusic() {
        music.Switch();
        musicAudioSource.enabled = music.IsEnabled();

        UserData.SaveAudioSettings(sound, music);
        return music.IsEnabled();
    }

    public bool SwitchSound() {
        sound.Switch();

        UserData.SaveAudioSettings(sound, music);
        return sound.IsEnabled();
    }

    public static AudioManager Instance {
        get {
            return instance;
        }
    }
}

public enum SoundType {
    BUTTON, SLIDE, LEVEL_COMPLETE, LEVEL_FAILED, STOPPABLE_BLOCK
}

[System.Serializable]
public struct AudioElement {
    public SoundType type;
    public AudioSource audioSource;

    public void Play() {
        audioSource.Play();
    }
}

public struct AudioType {
    private bool enabled;

    public void Switch() {
        enabled = !enabled;
    }

    public void SetEnabled(bool enabled) {
        this.enabled = enabled;
    }

    public bool IsEnabled() {
        return enabled;
    }
}

