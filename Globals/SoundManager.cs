using Godot;
using System;
using System.Collections.Generic;

public partial class SoundManager : Node
{
    private List<AudioStream> Gasps = new List<AudioStream>
    {
        GD.Load<AudioStream>("res://assets/sounds/gasp1.wav"),
        GD.Load<AudioStream>("res://assets/sounds/gasp2.wav"),
        GD.Load<AudioStream>("res://assets/sounds/gasp3.wav")
    };

    private List<AudioStream> PickUps = new List<AudioStream>
    {
        GD.Load<AudioStream>("res://assets/sounds/Positive Sounds/sfx_sounds_powerup1.wav"),
        GD.Load<AudioStream>("res://assets/sounds/Positive Sounds/sfx_sounds_powerup2.wav"),
        GD.Load<AudioStream>("res://assets/sounds/Positive Sounds/sfx_sounds_powerup3.wav"),
        GD.Load<AudioStream>("res://assets/sounds/Positive Sounds/sfx_sounds_powerup4.wav"),
        GD.Load<AudioStream>("res://assets/sounds/Positive Sounds/sfx_sounds_powerup5.wav"),
        GD.Load<AudioStream>("res://assets/sounds/Positive Sounds/sfx_sounds_powerup6.wav"),
        GD.Load<AudioStream>("res://assets/sounds/Positive Sounds/sfx_sounds_powerup7.wav"),
        GD.Load<AudioStream>("res://assets/sounds/Positive Sounds/sfx_sounds_powerup8.wav"),
        GD.Load<AudioStream>("res://assets/sounds/Positive Sounds/sfx_sounds_powerup9.wav")
    };

    private List<AudioStream> Laser = new List<AudioStream>
    {
        GD.Load<AudioStream>("res://assets/sounds/sfx_wpn_laser2.wav")
    };

    public static SoundManager Instance;

    public override void _Ready()
    {
        Instance = this;
    }

    private AudioStream GetRandomSoundFromList(List<AudioStream> soundList)
    {
        var random = new Random();
        return soundList[random.Next(soundList.Count)];
    }

    public static void PlayGasp(AudioStreamPlayer2D player)
    {
        player.Stream = Instance.GetRandomSoundFromList(Instance.Gasps);
        player.Play();
    }

    public static void PlayLaser(AudioStreamPlayer2D player)
    {
        player.Stream = Instance.GetRandomSoundFromList(Instance.Laser);
        player.Play();
    }

    public static AudioStream GetRandomPickupSound()
    {
        return Instance.GetRandomSoundFromList(Instance.PickUps);
    }
}
