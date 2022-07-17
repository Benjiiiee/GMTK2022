using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SoundEffect {
    public SoundName Name;
    public AudioClip Clip;
    [Range(0.0f, 10.0f)]
    public float Volume;
    [Range(-10.0f, 10.0f)]
    public float Pitch;

    public SoundEffect(SoundName name, AudioClip clip, float volume = 1.0f, float pitch = 1.0f) {
        Name = name;
        Clip = clip;
        Volume = volume;
        Pitch = pitch;
    }
}

public enum SoundName {
    DiceRolling,
    SwordRising,
    SwordSplash,
    SeaAmbiance
}