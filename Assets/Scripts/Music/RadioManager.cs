using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadioManager : MonoBehaviour
{
    AudioSource radio;
    List<RadioMusic> channel1 = new List<RadioMusic>();
    List<RadioMusic> channel2 = new List<RadioMusic>();
    List<RadioMusic> channel3 = new List<RadioMusic>();
    List<RadioMusic> channel4 = new List<RadioMusic>();
    int currentChannelNumber;
    Dictionary<int, string> channelNames = new Dictionary<int, string>();
    int channelCount = 4;
    RadioMusic[] allMusics;
    public TextMeshProUGUI songName;
    public TextMeshProUGUI artistName;
    public Slider slider; 
    AudioClip currentSong;
    float elapsedTime;
    int startingSongForChannel0;
    int startingSongForChannel1;
    int startingSongForChannel2;
    int startingSongForChannel3;
    float[] currentChannelInfo;
    int lastChannelNumber;
    private void Awake()
    {
        // Initializing components
        radio = GetComponent<AudioSource>();
        allMusics = Resources.LoadAll<RadioMusic>("Music/");
        channelNames.Add(0, "Queen Pop");
        channelNames.Add(1, "Police Radio");
        channelNames.Add(2, "SEK Channel");
        channelNames.Add(3, "91.7");
        channelNames.Add(4, "No Signal");
        // Placing musics to list according to their channel numbers
        foreach (RadioMusic music in allMusics)
        {
            if (music.ChannelNumber == 0)
            {
                channel1.Add(music);
            }
            else if (music.ChannelNumber == 1)
            {
                channel2.Add(music);
            }
            else if (music.ChannelNumber == 2)
            {
                channel3.Add(music);
            }
            else if (music.ChannelNumber == 3)
            {
                channel4.Add(music);
            }
        }
    }

    private void Start()
    {
        elapsedTime = 0;
        currentChannelNumber = 4;
        lastChannelNumber = currentChannelNumber;
        currentSong = null;
        artistName.text = "";
        songName.text = "";
        slider.value = 1;
        startingSongForChannel0 = Random.Range(0, 6);
        startingSongForChannel1 = Random.Range(0, 5);
        startingSongForChannel2 = Random.Range(0, 5);
        startingSongForChannel3 = Random.Range(0, 5);
        currentChannelInfo = CalculateStartMusic();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (lastChannelNumber != currentChannelNumber)
        {
            currentChannelInfo = CalculateStartMusic();
            lastChannelNumber = currentChannelNumber;
            PlayCurrentChannel();
        }
        // Check if the current song has finished playing
        if (!radio.isPlaying && radio.clip != null)
        {
            AdvanceToNextTrack();
        }
        UpdateSlider();   
    }

    void UpdateSlider() 
    {
        if (radio.isPlaying)
        {
            slider.value = radio.time / radio.clip.length;
        }
        else
        {
            slider.value = 1;
        }
    }

    void AdvanceToNextTrack()
    {
        List<RadioMusic> currentChannel = null;

        switch (currentChannelNumber)
        {
            case 0:
                currentChannel = channel1;
                break;
            case 1:
                currentChannel = channel2;
                break;
            case 2:
                currentChannel = channel3;
                break;
            case 3:
                currentChannel = channel4;
                break;
            case 4:
                radio.Stop();
                return;
        }

        int nextSongIndex = ((int)currentChannelInfo[0] + 1) % currentChannel.Count;
        currentChannelInfo[0] = nextSongIndex;
        currentChannelInfo[1] = 0;
        PlayCurrentChannel();
    }

    public void OnNextChannel() 
    {
        if (currentChannelNumber != 4)
        {
            currentChannelNumber++;
        }
        else { currentChannelNumber = 0; }
        currentChannelInfo = CalculateStartMusic();
    }

    public void OnPreviousChannel() 
    {
        if (currentChannelNumber != 0)
        {
            currentChannelNumber--;
        }
        else { currentChannelNumber = 4; }
        currentChannelInfo = CalculateStartMusic();
    }

    void DisplayCurrentMusic(RadioMusic radioMusic) 
    {
        if (radioMusic != null)
        {
            artistName.text = radioMusic.Artist;
            songName.text = radioMusic.Name;
            slider.value = radio.time / radioMusic.Seconds;
        }
        else 
        {
            artistName.text = "";
            songName.text = "";
            slider.value = 1;
        }      
    }

    void PlayCurrentChannel()
    {
        RadioMusic currentRadioMusic = null;
        if (currentChannelInfo != null)
        {
            switch (currentChannelNumber)
            {
                case 0:
                    currentRadioMusic = channel1[(int)currentChannelInfo[0]];
                    break;
                case 1:
                    currentRadioMusic = channel2[(int)currentChannelInfo[0]];
                    break;
                case 2:
                    currentRadioMusic = channel3[(int)currentChannelInfo[0]];
                    break;
                case 3:
                    currentRadioMusic = channel4[(int)currentChannelInfo[0]];
                    break;
                case 4:
                    currentRadioMusic = null;
                    break;
            }

            if (currentRadioMusic != null)
            {
                currentSong = currentRadioMusic.Music;
                if (radio.clip != currentSong || radio.time != currentChannelInfo[1])
                {
                    radio.clip = currentSong;
                    radio.time = currentChannelInfo[1];
                    radio.Play();
                }
            }
            else
            {
                radio.Stop();
            }
            DisplayCurrentMusic(currentRadioMusic);
        }
    }

    float[] CalculateStartMusic() // Returns starting music index and exact time (ex. 2, 123)
    {
        float[] returnArray = new float[2];
        float startingTime = elapsedTime;
        int tekrarCount = 0;
        List<RadioMusic> currentChannel = null;
        int startingSong = 0;

        switch (currentChannelNumber)
        {
            case 0:
                currentChannel = channel1;
                startingSong = startingSongForChannel0;
                break;
            case 1:
                currentChannel = channel2;
                startingSong = startingSongForChannel1;
                break;
            case 2:
                currentChannel = channel3;
                startingSong = startingSongForChannel2;
                break;
            case 3:
                currentChannel = channel4;
                startingSong = startingSongForChannel3;
                break;
            case 4:
                returnArray[0] = 0;
                returnArray[1] = 0;
                return returnArray;
        }

        for (int i = startingSong; i < currentChannel.Count; i++)
        {
            if (startingTime >= currentChannel[i].Seconds)
            {
                startingTime -= currentChannel[i].Seconds;
                tekrarCount++;
            }
            else
            {
                break;
            }

            if (i == currentChannel.Count - 1 && startingTime >= 0)
            {
                i = -1; // Restart loop
            }
        }

        int startMusicIndex = (startingSong + tekrarCount) % currentChannel.Count;
        returnArray[0] = startMusicIndex;
        returnArray[1] = startingTime;

        return returnArray;
    }

}
