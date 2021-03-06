using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    public MusicTrack[] Music;
    public SoundEffect[] Sounds;

    public AudioPoolManager EffectsPool;
    public MusicController musicController;

    public MusicTrackType CurrentPlaylistType { get => m_CurrentPlaylistType; set => PlayMusic(value); }
    private MusicTrackType m_CurrentPlaylistType;
    private List<MusicTrack> m_Playlist;
    private int lastTrackIndex;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        m_Playlist = new List<MusicTrack>();
    }

    private void OnEnable() {
        MusicController.TrackDone += SetNextTrack;
        //GameManager.FadeOut += StopAllLoopingSounds;
    }

    private void OnDisable() {
        MusicController.TrackDone -= SetNextTrack;
        //GameManager.FadeOut -= StopAllLoopingSounds;
    }

    public void PlaySound(SoundName soundName, float pitch = 1f, float volume = 1f) {
        SoundEffect sound = FindSoundByName(soundName);

        EffectsPool.ActivateAudioSource(sound, pitch, volume);
    }

    public void PlayMusic(MusicTrackType type) {
        musicController.Track = FindNextTrackOfType(type);
    }

    private void PopulatePlaylist(MusicTrackType type) {
        m_CurrentPlaylistType = type;
        m_Playlist = new List<MusicTrack>();
        for(int i = 0; i < Music.Length; i++) {
            if (Music[i].Type == type) m_Playlist.Add(Music[i]);
        }
        //Shuffle playlist
        int n = m_Playlist.Count;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            MusicTrack value = m_Playlist[k];
            m_Playlist[k] = m_Playlist[n];
            m_Playlist[n] = value;
        }
    }

    private void SetNextTrack() {
        if (m_CurrentPlaylistType != MusicTrackType.NoMusic)
            musicController.Track = FindNextTrackOfType(m_CurrentPlaylistType);
        else
            StopMusic();
    }

    public void StopMusic() {
        musicController.Stop();
    }

    private void StopAllLoopingSounds() {
        EffectsPool.StopAllLoopingSounds();
    }

    public AudioSourceController PlaySoundLoop(SoundName soundName, float duration = 0.0f, float pitch = 1f, float volume = 1f) {
        SoundEffect sound = FindSoundByName(soundName);
        if(duration == 0.0f) {
            //Loop until Stop() is called
            return EffectsPool.ActivateAudioSource(sound, pitch, volume, true);
        }
        else {
            //Play for given duration
            return EffectsPool.ActivateAudioSource(sound, pitch, volume, true, duration);
        }
    }

    private SoundEffect FindSoundByName(SoundName name) {
        for(int i = 0; i < Sounds.Length; i++) {
            if (Sounds[i].Name == name) return Sounds[i];
        }
        return null;
    }

    private MusicTrack FindNextTrackOfType(MusicTrackType type) {
        if(type != m_CurrentPlaylistType || m_Playlist.Count == 0) {
            PopulatePlaylist(type); 
        }
        MusicTrack track = m_Playlist[0];
        m_Playlist.RemoveAt(0);
        return track;
    }
}
