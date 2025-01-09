using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform cameraOrbit; // Objek pivot rotasi kamera
    public CinemachineVirtualCamera virtualCamera;
    
    [Header("Camera Settings")]
    public float yCameraOrbit;
    public float rotationSpeed = 2f; // Kecepatan rotasi kamera
    public float minVerticalAngle = -30f; // Sudut minimum vertikal
    public float maxVerticalAngle = 60f; // Sudut maksimum vertikal
    public float zoomSpeed = 2f; // Kecepatan zoom kamera
    public float minZoom = 2f; // Jarak zoom minimum
    public float maxZoom = 10f; // Jarak zoom maksimum
    
    private float currentZoom; // Jarak zoom saat ini
    private CinemachinePOV povComponent;
    private bool cursorVisible = false;
    private Vector3 targetPosition;
    private bool isTouching = false; // Menyimpan status sentuhan

    private void Awake() 
    {
        DontDestroyOnLoad(cameraOrbit.gameObject);    
    }

    void Start()
    {
        if (cameraOrbit == null)
        {
            cameraOrbit = new GameObject("Camera Orbit").transform;            
            cameraOrbit.position = targetPosition;
            virtualCamera.Follow = cameraOrbit;
            virtualCamera.LookAt = player;
        }

        povComponent = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        if (povComponent == null)
        {
            povComponent = virtualCamera.AddCinemachineComponent<CinemachinePOV>();
        }

        povComponent.m_VerticalAxis.m_MaxSpeed = rotationSpeed;
        povComponent.m_HorizontalAxis.m_MaxSpeed = rotationSpeed;
        povComponent.m_VerticalAxis.m_MinValue = minVerticalAngle;
        povComponent.m_VerticalAxis.m_MaxValue = maxVerticalAngle;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate() 
    {
        if (!cursorVisible)
        {
            HandleInput();
        }

        cameraOrbit.position = targetPosition;
    }

    void Update()
    {
        targetPosition = new Vector3(player.transform.position.x, player.transform.position.y + yCameraOrbit, player.transform.position.z);
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorVisible = !cursorVisible;
            Cursor.visible = cursorVisible;
            Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
        ShowCursor();

        virtualCamera.Follow = cameraOrbit;
    }
    
    void HandleInput()
    {
        #if UNITY_STANDALONE || UNITY_EDITOR
            HandleMouseInput();
        #endif

        #if UNITY_ANDROID
            HandleTouchInput();
        #endif
    }

    void HandleMouseInput()
    {
        if (MovementHandler.Instance.CanMoving == false)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        povComponent.m_HorizontalAxis.Value += mouseX;
        povComponent.m_VerticalAxis.Value -= mouseY;

        Quaternion targetRotation = Quaternion.Euler(povComponent.m_VerticalAxis.Value, povComponent.m_HorizontalAxis.Value, 0f);
        cameraOrbit.LookAt(targetRotation * Vector3.forward + cameraOrbit.position, Vector3.up);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return; // Abaikan input jika menyentuh UI
            }

            if (touch.phase == TouchPhase.Began)
            {
                isTouching = true;
                Debug.Log("Touch Started");
            }
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                Debug.Log("Touch Moving");
                povComponent.m_VerticalAxis.Value -= touch.deltaPosition.y * rotationSpeed * Time.deltaTime;
                povComponent.m_HorizontalAxis.Value += touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
                Debug.Log("Touch Ended");
            }
        }
    }

    private void ShowCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorVisible = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorVisible = false;
        }
    }
}
