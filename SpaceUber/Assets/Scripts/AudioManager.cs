/*
 * AudioManagers.cs
 * Author(s): #Greg Brandt#
 * Created on: 9/24/2020 (en-US)
 * Provides a central manager to play audio sounds and transitions music tracks: 
 */

using System.Collections;
using System.Data.Common;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;

//Serialized wrapper of AudioSource 
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup mixer; //new

    [Range(0f, 1f)] public float volume = 1;
    [Range(0.5f, 1.5f)] public float pitch = 1;

    [Tooltip("On play the sound will have a random volume varience plus or minus this value. Does not effect music")]
    [Range(0, 0.5f)] public float volumeVarience = 0;
    [Tooltip("On play the sound will have a random volume varience plus or minus this value. Does not effect music")]
    [Range(0, 0.5f)] public float pitchVarience = 0;

    AudioSource source;
    AudioMixerGroup mix;


    public void SetSource(AudioSource sourceIn)
    {
        source = sourceIn;
        source.clip = clip;
        source.outputAudioMixerGroup = mixer;
    }

    public void SetMixer(AudioMixerGroup mixAssign)
    {
        mix = mixAssign;
    }

    public void SetVolume(float newVolume) { source.volume = newVolume; }

    public void ScaleVolume(float scale) { source.volume *= scale; }

    public void PlayOnce()
    {
        source.volume = volume * (1 + Random.Range(-volumeVarience / 2, volumeVarience / 2));
        source.pitch = pitch * (1 + Random.Range(-pitchVarience / 2, pitchVarience / 2)); ;
        source.Play();
    }

    public void PlayLoop()
    {
        source.loop = true;
        source.Play();
    }

    public void Pause() { source.Pause(); }
    public void UnPause() { source.UnPause(); }
    public void Stop() { source.Stop(); }
}

    

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [SerializeField] Sound[] musicTracks;
    [SerializeField] Sound[] sfxTracks;
    [Range(0f, 1f)] public float masterVolume = 1;
    [Range(0f, 1f)] public float sfxVolume = 1;
    [Range(0f, 1f)] public float musicVolume = 1;
    [Tooltip("Time it takes for current track to fade out")]
    [SerializeField] float fadeOutTime;
    [Tooltip("Time window of overlap of current track fade out and next track fade in")]
    [SerializeField] float trackOverlapTime;
    [Tooltip("Time it takes for next track to fade in")]
    [SerializeField] float fadeInTime;

    Sound currentlyPlayingMusic;

    public bool isMuted = false;

	private void Awake()
	{
        //Singleton pattern
		if (!instance) { instance = this; }
		else { Destroy(gameObject); return; }
        DontDestroyOnLoad(this);

        SanitizeInput();
    }

	private void Start()
	{
        //intialize sounds as gameobjects
		for(int i = 0; i < musicTracks.Length; i++)
		{
            GameObject obj = new GameObject("Sound_" + i + "_" + musicTracks[i].name);
            obj.transform.SetParent(transform);
            musicTracks[i].SetSource(obj.AddComponent<AudioSource>());
            //musicTracks[i].SetMixer(new AudioMixerGroup()); //new
    }

        for (int i = 0; i < sfxTracks.Length; i++)
        {
            GameObject obj = new GameObject("Sound_" + i + "_" + sfxTracks[i].name);
            obj.transform.SetParent(transform);
            sfxTracks[i].SetSource(obj.AddComponent<AudioSource>());
            //sfxTracks[i].SetMixer() = soundEffects; //new
        }
    }

	private void FixedUpdate()
	{
        //Ensures the volume can be adjusted by player dynamically. 
        if(currentlyPlayingMusic != null)currentlyPlayingMusic.SetVolume(currentlyPlayingMusic.volume * musicVolume);
	}

	public void PlayMusicWithTransition(string soundName)
	{
        //Search tracks for sound name
        for(int i = 0; i < musicTracks.Length; i++)
		{
            if(musicTracks[i].name == soundName)
			{
                if (currentlyPlayingMusic != null)
                {
                    StartCoroutine(Fade(currentlyPlayingMusic, fadeOutTime, false));
                    StartCoroutine(OverLap(i));
                }
                else
                {
                    currentlyPlayingMusic = musicTracks[i];
                    musicTracks[i].ScaleVolume(musicVolume);
                    if (isMuted) musicTracks[i].ScaleVolume(0);
                    musicTracks[i].PlayLoop();
                }
                return;
			}
		}
        Debug.LogWarning("AudioManager: Sound not found in List: " + soundName);
	}

    public void PlayMusicWithoutTransition(string soundName)
	{
        //Search tracks for sound name
        for (int i = 0; i < musicTracks.Length; i++)
        {
            if (musicTracks[i].name == soundName)
            {
                if (currentlyPlayingMusic != null) { currentlyPlayingMusic.Stop(); }
                currentlyPlayingMusic = musicTracks[i];
                musicTracks[i].ScaleVolume(musicVolume);
                if (isMuted) musicTracks[i].ScaleVolume(0);
                musicTracks[i].PlayLoop();
                
                return;
            }
        }
        Debug.LogWarning("AudioManager: Sound not found in List: " + soundName);
    }

    public void PlaySFX(string soundName)
    {
        //Search tracks for sound name
        for (int i = 0; i < sfxTracks.Length; i++)
        {
            if (sfxTracks[i].name == soundName)
            {
                sfxTracks[i].ScaleVolume(sfxVolume);
                if (isMuted) sfxTracks[i].ScaleVolume(0);
                sfxTracks[i].PlayOnce();
                return;
            }
        }
        Debug.LogWarning("AudioManager: Sound not found in List: " + soundName);
    }

    public void PauseMusic() { currentlyPlayingMusic.Pause(); }

    public void ResumeMusic() { currentlyPlayingMusic.UnPause(); }

    public void StopMusic() { currentlyPlayingMusic.Stop(); }

    IEnumerator Fade(Sound sound, float fadeTime, bool fadeIn)
    {
        //Sanitize Input
        fadeTime = Mathf.Abs(fadeTime);
        //Skip routine if fadeTime is 0
        if (fadeTime > 0)
        {
            int fadeInOrOut;
            float fadeStart;
            float startVolume = sound.volume;

            //Adjust values to fade in or fade out
            if (fadeIn)
            {
                fadeInOrOut = -1;
                sound.SetVolume(0.1f);
                fadeStart = 0;
            }
            else
            {
                fadeStart = 1;
                fadeInOrOut = 1;
            }

            float timeElapsed = 0;

            while (timeElapsed < fadeTime)
            {
                //Set volume to the percentage of time that has elapsed over fade time
                sound.SetVolume( startVolume *  (fadeStart - (fadeInOrOut * timeElapsed / fadeTime)));
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            //Stop sound and reset volume to original amount
            if (!fadeIn) sound.Stop();
            sound.volume = startVolume;
        }
    }

    IEnumerator OverLap(int nextSoundIndex )
	{
        //waits until there are trackOverlapTime seconds left in current tracks fadeout
        yield return new WaitForSeconds(fadeOutTime - trackOverlapTime);

        currentlyPlayingMusic = musicTracks[nextSoundIndex];

        //Set up and start next track
        musicTracks[nextSoundIndex].ScaleVolume(musicVolume);
        if (isMuted) musicTracks[nextSoundIndex].ScaleVolume(0);
        musicTracks[nextSoundIndex].PlayLoop();

        StartCoroutine( Fade(currentlyPlayingMusic, fadeInTime, true));
    }

    void SanitizeInput()
	{
        fadeOutTime = Mathf.Abs(fadeOutTime);
        fadeInTime = Mathf.Abs(fadeInTime);
        trackOverlapTime = Mathf.Abs(trackOverlapTime);
        if (trackOverlapTime > fadeOutTime) { trackOverlapTime = fadeOutTime; }
    }
}