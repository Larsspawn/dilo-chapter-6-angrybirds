using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
    static GameAssets gameAssets = GameObject.FindObjectOfType<GameAssets>();
    
    public enum Sound
    {
        bird01Collision1,
        bird01Collision2,
        bird01Collision3,
        bird01Flying,
        bird03Collision1,
        bird03Collision2,
        bird03Collision3,
        bird03Flying,
        bird04Collision1,
        bird04Collision2,
        bird04Collision3,
        bird04Flying,
        birdDestroyed,
        birdShot,
        pigCollision1,
        pigCollision2,
        pigDamage1,
        pigDamage2,
        pigDamage3,
        pigDamage4,
        pigDestroyed,
        woodDestroyed1,
        woodDestroyed2,
        slingshotStretched,
        explosion1,
        button1,
        button2,
        highscore,
        levelStart,
        levelClear,
        levelFailed,
        levelComplete,
        ambient1,
        mainTheme,
        
    }

    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    public static void PlayMusicLoop(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sound);
        audioSource.loop = true;
        audioSource.Play();
    }
    
    private static AudioClip GetAudioClip (Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClipin in gameAssets.soundAudioClipArray)
        {
            if (soundAudioClipin.sound == sound)
            {
                return soundAudioClipin.audioClip;
            }
        }
        
        Debug.Log("Sound " + sound + " not found!");
        return null;
    }
}
