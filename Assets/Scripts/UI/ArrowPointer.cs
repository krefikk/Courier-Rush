using UnityEngine;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    GameObject carObject;
    Transform car; // Reference to the car
    RectTransform arrowUI; // Reference to the arrow UI element

    private Camera mainCamera;
    private Vector2 screenCenter;
    private float screenWidth;
    private float screenHeight;
    private Vector3 targetPoint;
    private bool hasTargetPoint = false;

    void Start()
    {
        carObject = GameObject.FindGameObjectWithTag("PlayerCar");
        car = carObject.transform;
        arrowUI = gameObject.GetComponent<RectTransform>();
        mainCamera = Camera.main;
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        gameObject.SetActive(hasTargetPoint);
    }

    void Update()
    {
        if (hasTargetPoint)
        {
            UpdateArrowPositionAndRotation();
        }
    }

    void UpdateArrowPositionAndRotation()
    {
        // Calculate the direction from the car to the target point
        Vector3 directionToTarget = targetPoint - car.position;
        directionToTarget.z = 0; // Ignore the z-axis

        // Calculate the angle to rotate the arrow
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // Rotate the arrow
        arrowUI.rotation = Quaternion.Euler(0, 0, angle);

        // Calculate the position of the arrow on the screen edge
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(car.position + directionToTarget.normalized * 10);
        screenPosition = ClampToScreenEdge(screenPosition);

        // Convert the screen position to the UI canvas position
        arrowUI.position = screenPosition;

        // Ensure the arrow is visible
        gameObject.SetActive(true);
    }

    Vector2 ClampToScreenEdge(Vector2 screenPosition)
    {
        Vector2 directionFromCenter = screenPosition - screenCenter;

        // Calculate ratios
        float xRatio = Mathf.Abs(directionFromCenter.x) / (screenWidth / 2);
        float yRatio = Mathf.Abs(directionFromCenter.y) / (screenHeight / 2);

        // Clamp based on the larger ratio
        if (xRatio > yRatio)
        {
            screenPosition.x = directionFromCenter.x > 0 ? screenWidth - arrowUI.rect.width / 2 : arrowUI.rect.width / 2;
            screenPosition.y = screenCenter.y + (screenWidth / 2 - arrowUI.rect.width / 2) * directionFromCenter.y / Mathf.Abs(directionFromCenter.x);
        }
        else
        {
            screenPosition.y = directionFromCenter.y > 0 ? screenHeight - arrowUI.rect.height / 2 : arrowUI.rect.height / 2;
            screenPosition.x = screenCenter.x + (screenHeight / 2 - arrowUI.rect.height / 2) * directionFromCenter.x / Mathf.Abs(directionFromCenter.y);
        }

        screenPosition.x = Mathf.Clamp(screenPosition.x, arrowUI.rect.width / 2, screenWidth - arrowUI.rect.width / 2);
        screenPosition.y = Mathf.Clamp(screenPosition.y, arrowUI.rect.height / 2, screenHeight - arrowUI.rect.height / 2);

        return screenPosition;
    }

    public void SetTargetPoint(Vector3 newTargetPoint)
    {
        targetPoint = newTargetPoint;
        hasTargetPoint = true;
        gameObject.SetActive(hasTargetPoint);
    }

    public void ClearTargetPoint()
    {
        hasTargetPoint = false;
        gameObject.SetActive(hasTargetPoint);
        ChangeColorToCyan();
    }

    public void ChangeColorToCyan()
    {
        Image image = GetComponent<Image>();
        image.color = Color.cyan;
    }

    public void ChangeColorToGreen() // 66 224 57
    {
        Image image = GetComponent<Image>();
        image.color = Color.green;
    }

    public void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
