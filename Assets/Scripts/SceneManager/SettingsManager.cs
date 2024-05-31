using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public GameObject backButton;
    Animator backButtonAnim;
    Button backButtonB;

    public GameObject master;
    Animator masterAnim;
    Slider masterSlider;
    public TextMeshProUGUI masterText;

    public GameObject music;
    Animator musicAnim;
    Slider musicSlider;
    public TextMeshProUGUI musicText;

    public GameObject sfx;
    Animator sfxAnim;
    Slider sfxSlider;
    public TextMeshProUGUI sfxText;

    public GameObject car;
    Animator carAnim;
    Slider carSlider;
    public TextMeshProUGUI carText;

    private void Awake()
    {
        masterAnim = master.GetComponentInChildren<Animator>();
        masterSlider = master.GetComponentInChildren<Slider>();
        musicAnim = music.GetComponentInChildren <Animator>();
        musicSlider = music.GetComponentInChildren<Slider>();
        sfxAnim = sfx.GetComponentInChildren<Animator>();
        sfxSlider = sfx.GetComponentInChildren<Slider>();
        carAnim = car.GetComponentInChildren<Animator>();
        carSlider = car.GetComponentInChildren<Slider>();
        backButtonAnim = backButton.GetComponent<Animator>();
        backButtonB = backButton.GetComponent<Button>();
    }

    private void Start()
    {
        SetUpInitialSliderValues();
    }

    void SetUpInitialSliderValues() 
    {
        float musicVolume;
        float sfxVolume;
        float carVolume;
        float masterVolume;
        if (audioMixer.GetFloat("Music", out musicVolume) && audioMixer.GetFloat("SFX", out sfxVolume) && audioMixer.GetFloat("Car", out carVolume) && audioMixer.GetFloat("Master", out masterVolume))
        {
            masterSlider.value = Mathf.InverseLerp(-80, 20, masterVolume);
            musicSlider.value = Mathf.InverseLerp(-60, 20, musicVolume);
            sfxSlider.value = Mathf.InverseLerp(-60, 20, sfxVolume);
            carSlider.value = Mathf.InverseLerp(-60, 20, carVolume);
        }
    }

    public void OnMasterSliderValueChange() 
    {
        audioMixer.SetFloat("Master", Mathf.Lerp(-80, 20, masterSlider.value));
        masterText.text = ((int)Mathf.Lerp(0, 100, masterSlider.value)).ToString();
    }

    public void OnMusicSliderValueChange() 
    {
        audioMixer.SetFloat("Music", Mathf.Lerp(-60, 20, musicSlider.value));
        musicText.text = ((int)Mathf.Lerp(0, 100, musicSlider.value)).ToString();
    }

    public void OnSFXSliderValueChange() 
    {
        audioMixer.SetFloat("SFX", Mathf.Lerp(-60, 20, sfxSlider.value));
        sfxText.text = ((int)Mathf.Lerp(0, 100, sfxSlider.value)).ToString();
    }

    public void OnCarSliderValueChange() 
    {
        audioMixer.SetFloat("Car", Mathf.Lerp(-60, 20, carSlider.value));
        carText.text = ((int)Mathf.Lerp(0, 100, carSlider.value)).ToString();
    }

    public void OnPressedBackButton() 
    {
        StartCoroutine(OnExitCO());
    }

    IEnumerator OnExitCO() 
    {
        backButtonB.interactable = false;
        carAnim.Play("carExit");
        yield return new WaitForSeconds(0.25f);
        sfxAnim.Play("sfxExit");
        yield return new WaitForSeconds(0.25f);
        musicAnim.Play("musicExit");
        yield return new WaitForSeconds(0.25f);
        masterAnim.Play("masterExit");
        yield return new WaitForSeconds(0.25f);
        backButtonAnim.Play("backExit");
        yield return new WaitForSeconds(0.25f);
        backButtonB.interactable = true;
        SceneManager.LoadScene("MainMenu");
    }

}
