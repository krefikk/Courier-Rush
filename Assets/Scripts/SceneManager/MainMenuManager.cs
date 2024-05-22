using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject newG;
    Animator newGame;
    Button newGameButton;
    public GameObject continueB;
    Animator continueButton;
    Button continueBB;
    public GameObject ss;
    Animator settings;
    Button settingsButton;
    public GameObject cs;
    Animator credits;
    Button creditsButton;
    public GameObject htp;
    Animator howToPlay;
    Button htpButton;
    public GameObject exitObject;
    Animator exit;
    Button exitButton;
    public GameObject itch;
    Animator itchio;
    Button itchButton;
    public GameObject githubG;
    Animator github;
    Button githubButton;
    public GameObject saveWarning;
    Animator saveWarningAnim;
    public Button saveContinue;
    public Button saveCancel;

    private void Awake()
    {
        // Initializing components
        continueButton = continueB.GetComponent<Animator>();
        continueBB = continueB.GetComponent<Button>();
        newGame = newG.GetComponent<Animator>();
        newGameButton = newG.GetComponent<Button>();
        settings = ss.GetComponent<Animator>();
        settingsButton = settings.GetComponent<Button>();
        credits = cs.GetComponent<Animator>();
        creditsButton = cs.GetComponent<Button>();
        howToPlay = htp.GetComponent<Animator>();
        htpButton = htp.GetComponent<Button>();
        exit = exitObject.GetComponent<Animator>();
        exitButton = exit.GetComponent<Button>();
        itchio = itch.GetComponent<Animator>();
        itchButton = itch.GetComponent<Button>();
        github = githubG.GetComponent<Animator>();
        githubButton = githubG.GetComponent<Button>();
        saveWarningAnim = saveWarning.GetComponent<Animator>();
    }

    private void Start()
    {
        // Checking if player has a saved game
        if (!GameManager.gameManager.HasSavedGame())
        {
            continueBB.interactable = false;
        }
    }

    IEnumerator ExitAnimation() 
    {
        // Making buttons uninteractable during exit animation
        continueBB.interactable = false;
        newGameButton.interactable = false;
        settingsButton.interactable = false;
        creditsButton.interactable = false;
        htpButton.interactable = false;
        exitButton.interactable = false;
        itchButton.interactable = false;
        githubButton.interactable = false;
        // Playing exit animations
        itchio.Play("itchioExit");
        yield return new WaitForSeconds(0.25f);
        github.Play("githubExit");
        yield return new WaitForSeconds(0.25f);
        exit.Play("exitExit");
        yield return new WaitForSeconds(0.25f);
        howToPlay.Play("htpExit");
        yield return new WaitForSeconds(0.25f);
        credits.Play("creditsExit");
        yield return new WaitForSeconds(0.25f);
        settings.Play("settingsExit");
        yield return new WaitForSeconds(0.25f);
        continueButton.Play("continueExit");
        yield return new WaitForSeconds(0.25f);
        newGame.Play("newGameExit");
        yield return new WaitForSeconds(0.25f);
        // Making buttons interactable again after exit animation
        if (GameManager.gameManager.HasSavedGame())
        {
            continueBB.interactable = true;
        }
        newGameButton.interactable = true;
        settingsButton.interactable = true;
        creditsButton.interactable = true;
        htpButton.interactable = true;
        exitButton.interactable = true;
        itchButton.interactable = true;
        githubButton.interactable = true;
    }

    public void OnClickNewGame() 
    {
        if (GameManager.gameManager.HasSavedGame()) 
        {
            SaveWarning();
        }
    }
    public void OnClickContinue() 
    {
        StartCoroutine(ExitAnimation());
        SceneManager.LoadScene("MainGame");
    }

    public void OnClickSettings() 
    {
        StartCoroutine(ExitAnimation());
        SceneManager.LoadScene("Settings");
    }

    public void OnClickCredits() 
    {
        StartCoroutine(ExitAnimation());
        SceneManager.LoadScene("Credits");
    }

    public void OnClickHTP() 
    {
        StartCoroutine(ExitAnimation());
        SceneManager.LoadScene("HowToPlay");
    }

    public void OnClickExit() 
    {
        StartCoroutine(ExitAnimation());
        Application.Quit();
    }

    public void OnClickItchio() 
    {
        StartCoroutine(ExitAnimation());
        Application.OpenURL("https://github.com/krefikk/Courier-Rush");
    }

    public void OnClickGithub() 
    {
        StartCoroutine(ExitAnimation());
        Application.OpenURL("https://github.com/krefikk/Courier-Rush");
    }

    public void SaveWarning() 
    {
        saveWarning.transform.position = new Vector3(0, -1300, 0);
        saveWarning.SetActive(true);
        saveContinue.interactable = true;
        saveCancel.interactable = true;
        continueBB.interactable = false;
        newGameButton.interactable = false;
        settingsButton.interactable = false;
        creditsButton.interactable = false;
        htpButton.interactable = false;
        exitButton.interactable = false;
        itchButton.interactable = false;
        githubButton.interactable = false;
        saveWarningAnim.Play("saveEnter");
    }

    public void OnClickSaveContinue() 
    {
        StartCoroutine(SaveContinue());
    }

    public void OnClickSaveCancel() 
    {
        StartCoroutine(SaveCancel());
    }

    IEnumerator SaveCancel() 
    {
        saveContinue.interactable = false;
        saveCancel.interactable = false;
        saveWarningAnim.Play("saveExit");
        newGameButton.interactable = true;
        settingsButton.interactable = true;
        creditsButton.interactable = true;
        htpButton.interactable = true;
        exitButton.interactable = true;
        itchButton.interactable = true;
        githubButton.interactable = true;
        yield return new WaitForSeconds(0.5f);
        saveWarning.SetActive(false);
    }

    IEnumerator SaveContinue()
    {
        saveContinue.interactable = false;
        saveCancel.interactable = false;
        saveWarningAnim.Play("saveExit");
        newGameButton.interactable = true;
        settingsButton.interactable = true;
        creditsButton.interactable = true;
        htpButton.interactable = true;
        exitButton.interactable = true;
        itchButton.interactable = true;
        githubButton.interactable = true;
        yield return new WaitForSeconds(0.5f);
        saveWarning.SetActive(false);
        StartCoroutine(ExitAnimation());
        // Delete the save files and create new
        SceneManager.LoadScene("MainGame");
    }
}
