using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Radio Music", menuName = "Radio Music", order = 52)]
public class RadioMusic : ScriptableObject
{
    [SerializeField] private int musicID = 0;
    [SerializeField] private int channelNumber = 0;
    [SerializeField] private AudioClip music;
    [SerializeField] private string musicName = "";
    [SerializeField] private string artist = "";
    [SerializeField] private int seconds = 0;

    public int MusicID
    {
        get { return musicID; }
    }

    public int ChannelNumber
    {
        get { return channelNumber; }
    }

    public AudioClip Music
    {
        get { return music; }
    }

    public string Name
    {
        get { return musicName; }
    }

    public string Artist
    {
        get { return artist; }
    }

    public int Seconds
    {
        get { return seconds; }
    }
}
