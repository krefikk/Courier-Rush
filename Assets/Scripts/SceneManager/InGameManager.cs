using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    float moneyGainedInDay = 0;
    public bool dayEnded = false;
    bool displayedDayEndedMenu = false;
    bool gamePaused = false;
    bool gameOver = false;
    public GameObject shop;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timeText;
    public Sprite dropPointSprite;
    int day;
    int packagesDeliveredInDay = 0;
    GameObject[] dropPoints;
    public Delivery[] deliveriesToDisplay = new Delivery[3];
    public RectTransform CANVAS;
    public ArrowPointer arrowPointerPrefab;
    ArrowPointer[] arrowPointers = new ArrowPointer[3];
    GameObject player;
    Car playerCar;
    GameObject spawnPoint;
    float elapsedTime = 0;

    // Deliveries HUD
    public GameObject deliveriesTab;
    Animator deliveriesTabAnim;
    public Animator delivery1;
    public Animator delivery2;
    public Animator delivery3;
    public TextMeshProUGUI delivery1Distance;
    public TextMeshProUGUI delivery1Worth;
    public Button delivery1Choose;
    public GameObject delivery1ChooseFill;
    public Button delivery1Cancel;
    public TextMeshProUGUI delivery2Distance;
    public TextMeshProUGUI delivery2Worth;
    public Button delivery2Choose;
    public GameObject delivery2ChooseFill;
    public Button delivery2Cancel;
    public TextMeshProUGUI delivery3Distance;
    public TextMeshProUGUI delivery3Worth;
    public Button delivery3Choose;
    public GameObject delivery3ChooseFill;
    public Button delivery3Cancel;

    // End Game Menus
    public GameObject endGameMenu;
    Animator endGameMenuAnim;
    public TextMeshProUGUI endGameTitle;
    public TextMeshProUGUI moneyGainedText;
    public TextMeshProUGUI packagesDeliveredText;
    public TextMeshProUGUI playersShare;
    public TextMeshProUGUI shopsShare;
    public Slider shareSlider;
    public TextMeshProUGUI shopsNeededMoney;
    public Button confirmButton;

    private void Awake()
    {
        day = GameManager.gameManager.GetCurrentDay();
        dropPoints = GameObject.FindGameObjectsWithTag("DropPoint");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        deliveriesTabAnim = deliveriesTab.GetComponent<Animator>();
        endGameMenuAnim = endGameMenu.GetComponent<Animator>();
        CreateNewDropPoint(0);
        CreateNewDropPoint(1);
        CreateNewDropPoint(2);
        DisplayDeliveries();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerCar");
        playerCar = player.GetComponent<Car>();
        for (int i = 0; i < 3; i++) 
        {
            ArrowPointer pointer = Instantiate(arrowPointerPrefab, CANVAS);
            arrowPointers[i] = pointer;
        }
        
    }

    private void Update()
    {
        if (gameOver) 
        {
            GameManager.gameManager.DeleteSave();
            // open game over menu
        }
        if (dayEnded) 
        {
            UpdateEndDayMenu();
        }
        DisplayMoney();
        DisplayTime();
        CheckInteractivityOfChooseButtons();
        if (!gamePaused) 
        {
            KeepTime();
            FinishDay();
            CheckCarsDamage();
        }
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            OpenDeliveriesTab();
        }
    }

    //----------------------- HUD -----------------------------------
    public void DisplayMoney()
    {
        int money = (int) GameManager.gameManager.GetMoney();
        moneyText.text = money.ToString();
    }

    void KeepTime() 
    {
        elapsedTime += Time.deltaTime;
    }

    public void DisplayTime() 
    {
        int minutes = (int) elapsedTime / 60;
        int seconds = (int) elapsedTime % 60;
        if (minutes < 10 && seconds < 10)
        {
            timeText.text = "0" + minutes.ToString() + ":0" + seconds.ToString();
        }
        else if (minutes < 10 && seconds >= 10)
        {
            timeText.text = "0" + minutes.ToString() + ":" + seconds.ToString();
        }
        else if (minutes >= 10 && seconds < 10)
        {
            timeText.text = minutes.ToString() + ":0" + seconds.ToString();
        }
        else if (minutes >= 10 && seconds >= 10)
        {
            timeText.text = minutes.ToString() + ":" + seconds.ToString();
        }
    }

    

    //----------------------- Delivery System -----------------------------------
    void CreateNewDropPoint(int oldIndex) 
    { // Creates a new random drop point to old index
        int index = Random.Range(0, dropPoints.Length);
        deliveriesToDisplay[oldIndex] = new Delivery(shop, dropPoints[index]);
    }

    void DisplayDeliveries() 
    {
        delivery1Distance.text = "Distance: " + ((int)deliveriesToDisplay[0].GetDistance()).ToString();
        delivery1Worth.text = "Worth: " + deliveriesToDisplay[0].GetWorth().ToString();
        delivery2Distance.text = "Distance: " + ((int)deliveriesToDisplay[1].GetDistance()).ToString();
        delivery2Worth.text = "Worth: " + deliveriesToDisplay[1].GetWorth().ToString();
        delivery3Distance.text = "Distance: " + ((int)deliveriesToDisplay[2].GetDistance()).ToString();
        delivery3Worth.text = "Worth: " + deliveriesToDisplay[2].GetWorth().ToString();
    }

    bool CanChooseNewDelivery() 
    {
        if (playerCar.maxPackage > GetChosenDeliveryCount())
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    public int GetChosenDeliveryCount() 
    {      
        if (deliveriesToDisplay != null)
        {
            int count = 0;
            foreach (Delivery delivery in deliveriesToDisplay)
            {
                if (delivery.IsChosen())
                {
                    count++;
                }
            }
            return count;
        }
        else { return 0; }
    }

    public List<int> CheckForDeliveredPackages() 
    {
        int index = 0;
        List<int> indexList = new List<int>();
        foreach (Delivery delivery in deliveriesToDisplay) 
        {
            if (delivery.IsDelivered()) 
            {
                indexList.Add(index);
            }
            index++;
        }
        return indexList;
    }

    public void HandleArrowPointersOnDelivery()
    {
        int index = 0;
        foreach (Delivery delivery in deliveriesToDisplay)
        {
            if (delivery.IsDelivered())
            {
                arrowPointers[index].ChangeColorToGreen();
                arrowPointers[index].SetTargetPoint(shop.transform.position);
            }
            index++;
        }
    }

    public void CheckInteractivityOfChooseButtons() 
    {
        if (!CanChooseNewDelivery())
        {
            delivery1Choose.interactable = false;
            delivery2Choose.interactable = false;
            delivery3Choose.interactable = false;
        }
        else
        {
            for (int i = 0; i < deliveriesToDisplay.Length; i++)
            {
                if (!deliveriesToDisplay[i].IsChosen())
                {
                    if (i == 0) { delivery1Choose.interactable = true; }
                    if (i == 1) { delivery2Choose.interactable = true; }
                    if (i == 2) { delivery3Choose.interactable = true; }
                }
            }
        }
    }

    IEnumerator ChangeDeliveryCO(int index)
    {
        if (deliveriesToDisplay[index].IsChosen())
        {
            GameObject dropPoint = deliveriesToDisplay[index].GetDropPoint();
            dropPoint.transform.localScale = new Vector3(0.125f, 0.125f, 1);
            dropPoint.AddComponent<SpriteRenderer>();
            SpriteRenderer dropPointSpriteLocal = dropPoint.GetComponent<SpriteRenderer>();
            dropPointSpriteLocal.sprite = null;
            arrowPointers[index].ClearTargetPoint();
        }
        if (index == 0) 
        {
            delivery1Choose.interactable = false;
            delivery1Cancel.interactable = false;
            delivery1.Play("toRight1");
            yield return new WaitForSeconds(0.5f); // Wait for animation to finish
            CreateNewDropPoint(index);
            delivery1Distance.text = "Distance: " + ((int)deliveriesToDisplay[index].GetDistance()).ToString();
            delivery1Worth.text = "Worth: " + deliveriesToDisplay[index].GetWorth().ToString();
            delivery1Choose.interactable = true;
            delivery1Cancel.interactable = true;
            delivery1Cancel.gameObject.SetActive(false);
            delivery1ChooseFill.SetActive(false);
            delivery1.Play("fromLeft1");
            yield return new WaitForSeconds(0.5f); // Wait for animation to finish
        }
        else if (index == 1)
        {
            delivery2Choose.interactable = false;
            delivery2Cancel.interactable = false;
            delivery2.Play("toRight2");
            yield return new WaitForSeconds(0.5f); // Wait for animation to finish
            CreateNewDropPoint(index);
            delivery2Distance.text = "Distance: " + ((int)deliveriesToDisplay[index].GetDistance()).ToString();
            delivery2Worth.text = "Worth: " + deliveriesToDisplay[index].GetWorth().ToString();
            delivery2Choose.interactable = true;
            delivery2Cancel.interactable = true;
            delivery2Cancel.gameObject.SetActive(false);
            delivery2ChooseFill.SetActive(false);
            delivery2.Play("fromLeft2");
            yield return new WaitForSeconds(0.5f); // Wait for animation to finish
        }
        else if (index == 2)
        {
            delivery3Choose.interactable = false;
            delivery3Cancel.interactable = false;
            delivery3.Play("toRight3");
            yield return new WaitForSeconds(0.5f); // Wait for animation to finish
            CreateNewDropPoint(index);
            delivery3Distance.text = "Distance: " + ((int)deliveriesToDisplay[index].GetDistance()).ToString();
            delivery3Worth.text = "Worth: " + deliveriesToDisplay[index].GetWorth().ToString();
            delivery3Choose.interactable = true;
            delivery3Cancel.interactable = true;
            delivery3Cancel.gameObject.SetActive(false);
            delivery3ChooseFill.SetActive(false);
            delivery3.Play("fromLeft3");
            yield return new WaitForSeconds(0.5f); // Wait for animation to finish
        }
    }

    public void OnChangeDelivery(int index) 
    {
        StartCoroutine(ChangeDeliveryCO(index));
    }

    public void OnChooseDelivery(int index) 
    {
        if (CanChooseNewDelivery())
        {
            deliveriesToDisplay[index].SetChosen();
            GameObject dropPoint = deliveriesToDisplay[index].GetDropPoint();
            dropPoint.transform.localScale = new Vector3(0.125f, 0.125f, 1);
            if (dropPoint.GetComponent<SpriteRenderer>() == null) 
            {
                dropPoint.AddComponent<SpriteRenderer>();
            }
            SpriteRenderer dropPointSpriteLocal = dropPoint.GetComponent<SpriteRenderer>();
            dropPointSpriteLocal.sprite = dropPointSprite;
            BoxCollider2D collider2D = dropPoint.GetComponent<BoxCollider2D>();
            if (collider2D != null)
            {
                Destroy(collider2D);
            }
            collider2D = dropPoint.AddComponent<BoxCollider2D>();
            collider2D.isTrigger = true;
            Debug.Log("Is Trigger: " + collider2D.isTrigger);
            arrowPointers[index].SetTargetPoint(deliveriesToDisplay[index].GetDropPoint().transform.position);
        }
        else 
        {
            // müzik çal titreþtir zort falan
        }
    }

    public void OnArriveToShop() 
    {
        OpenDeliveriesTab();
        if (CheckForDeliveredPackages() != null) 
        {
            for (int i = 0; i < CheckForDeliveredPackages().Count; i++) 
            {
                Debug.Log("Has to change delivery");
                moneyGainedInDay += deliveriesToDisplay[CheckForDeliveredPackages()[i]].GetWorth();
                OnChangeDelivery(CheckForDeliveredPackages()[i]);
                Debug.Log("Money gained: " + moneyGainedInDay);
            }
            
        }
    }

    public void FinishDay() 
    {
        if (dayEnded) 
        {
            gamePaused = true;
            dayEnded = true;
            OnDayEnd();
        }
        if (elapsedTime >= 900) 
        {
            if (GetChosenDeliveryCount() == 0)
            {
                gamePaused = true;
                dayEnded = true;
                OnDayEnd();    
            }
            else 
            { // Giving player an extra 100 seconds for finish their last delivery
                if (elapsedTime >= 1000)
                {
                    gamePaused = true;
                    dayEnded = true;
                    OnDayEnd();
                }
            }
        }
    }

    public void OpenDeliveriesTab() 
    {
        StartCoroutine(OpenDeliveriesTabCO());
    }

    public void CloseDeliveriesTab()
    {
        StartCoroutine(CloseDeliveriesTabCO());
    }

    IEnumerator OpenDeliveriesTabCO() 
    {
        deliveriesTab.SetActive(true);
        deliveriesTabAnim.Play("deliveryEnter");
        yield return new WaitForSeconds(0.5f); // Wait for animation to end
    }

    IEnumerator CloseDeliveriesTabCO()
    {       
        deliveriesTabAnim.Play("deliveryExit");
        yield return new WaitForSeconds(0.5f); // Wait for animation to end
        deliveriesTab.SetActive(false);
    }

    void UpdateEndDayMenu() 
    {
        playersShare.text = ((int)(shareSlider.value * moneyGainedInDay)).ToString();
        shopsShare.text = ((int)((1 - shareSlider.value) * moneyGainedInDay)).ToString();
        if (GameManager.gameManager.GetShopsNeed() - ((int)((1 - shareSlider.value) * moneyGainedInDay)) >= 0) 
        {
            shopsNeededMoney.text = "Shop needs " + GameManager.gameManager.GetShopsNeed() + " coins by the end of the week to keep it afloat.";
        }
        else 
        {
            shopsNeededMoney.text = "the shop will make a profit of " + (((int)((1 - shareSlider.value) * moneyGainedInDay)) - GameManager.gameManager.GetShopsNeed()) + " coins.";
        }      
    }

    IEnumerator OnDayEndCO() 
    {
        displayedDayEndedMenu = true;
        if (day == 7 && moneyGainedInDay < GameManager.gameManager.GetShopsNeed()) 
        {
            OpenLoseMenu();
            yield break;
        }
        endGameTitle.text = "Day " + day + " ended.";
        endGameMenu.SetActive(true);
        shareSlider.value = 0.5f;
        packagesDeliveredText.text = "Packages Delivered: " + packagesDeliveredInDay;
        moneyGainedText.text = "Money Gained: " + moneyGainedInDay;
        endGameMenuAnim.Play("endGameEnter");
        yield return new WaitForSeconds(0.25f);
        
    }

    public void OnDayEnd() 
    {
        StartCoroutine(OnDayEndCO());
    }

    public void OnClickConfirm() 
    {
        StartCoroutine(OnClickConfirmCO());
    }

    IEnumerator OnClickConfirmCO() 
    {
        GameManager.gameManager.IncreaseMoney((int)(moneyGainedInDay * shareSlider.value));
        GameManager.gameManager.SetShopsNeed(GameManager.gameManager.GetShopsNeed() - (int)(moneyGainedInDay * (1 - shareSlider.value)));
        endGameMenuAnim.Play("endGameExit");
        yield return new WaitForSeconds(0.25f);
        endGameMenu.SetActive(false);
        GameManager.gameManager.IncreaseDay();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Garage");
    }

    public void OpenLoseMenu()
    {
        StartCoroutine(OpenLoseMenuCO());
    }
    IEnumerator OpenLoseMenuCO()
    {
        yield return null;
    }

    void CheckCarsDamage() 
    {
        if (playerCar.GetDamage() >= playerCar.GetMaxHealth())
        {
            dayEnded = true;
        }
    }

    public void IncreaseDeliveredPackageCount() 
    {
        packagesDeliveredInDay++;
    }
}
