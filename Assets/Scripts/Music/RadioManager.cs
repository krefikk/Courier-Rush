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
        CacheStartMusics();
        StartCoroutine(SetupInitialMusic());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (lastChannelNumber != currentChannelNumber)
        {
            StartCoroutine(OnChannelChangeCO());
            lastChannelNumber = currentChannelNumber;
        }

        if (!radio.isPlaying && radio.clip != null)
        {
            AdvanceToNextTrack();
        }
        UpdateSlider();
    }

    IEnumerator SetupInitialMusic()
    {
        yield return new WaitForEndOfFrame(); // Defer to the end of the frame
        currentChannelInfo = CalculateStartMusic();
    }

    void CacheStartMusics()
    {
        float startTime = Time.realtimeSinceStartup;

        startingSongForChannel0 = Random.Range(0, channel1.Count);
        startingSongForChannel1 = Random.Range(0, channel2.Count);
        startingSongForChannel2 = Random.Range(0, channel3.Count);
        startingSongForChannel3 = Random.Range(0, channel4.Count);

        float endTime = Time.realtimeSinceStartup;
        Debug.Log($"CacheStartMusics took {endTime - startTime} seconds");
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
        currentChannelNumber = (currentChannelNumber + 1) % (channelCount + 1);
        StartCoroutine(OnChannelChangeCO());
    }

    public void OnPreviousChannel()
    {
        currentChannelNumber = (currentChannelNumber - 1 + channelCount + 1) % (channelCount + 1);
        StartCoroutine(OnChannelChangeCO());
    }

    IEnumerator OnChannelChangeCO()
    {
        yield return new WaitForEndOfFrame();
        currentChannelInfo = CalculateStartMusic();
        PlayCurrentChannel();
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
        float startTime = Time.realtimeSinceStartup;

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

        float endTime = Time.realtimeSinceStartup;
        Debug.Log($"PlayCurrentChannel took {endTime - startTime} seconds");
    }

    float[] CalculateStartMusic()
    {
        float[] returnArray = new float[2];
        float startingTime = elapsedTime;
        int startMusicIndex = 0;
        int startingSong = 0;
        List<RadioMusic> currentChannel = GetCurrentChannel();

        if (currentChannel == null)
        {
            returnArray[0] = 0;
            returnArray[1] = 0;
            return returnArray;
        }

        startingSong = GetStartingSongForChannel(currentChannelNumber);

        for (int i = 0; i < currentChannel.Count; i++)
        {
            if (startingTime < currentChannel[startingSong].Seconds)
            {
                break;
            }
            startingTime -= currentChannel[startingSong].Seconds;
            startingSong = (startingSong + 1) % currentChannel.Count;
        }

        startMusicIndex = startingSong;
        returnArray[0] = startMusicIndex;
        returnArray[1] = startingTime;

        return returnArray;
    }

    List<RadioMusic> GetCurrentChannel()
    {
        switch (currentChannelNumber)
        {
            case 0: return channel1;
            case 1: return channel2;
            case 2: return channel3;
            case 3: return channel4;
            default: return null;
        }
    }

    int GetStartingSongForChannel(int channelNumber)
    {
        switch (channelNumber)
        {
            case 0: return startingSongForChannel0;
            case 1: return startingSongForChannel1;
            case 2: return startingSongForChannel2;
            case 3: return startingSongForChannel3;
            default: return 0;
        }
    }

}