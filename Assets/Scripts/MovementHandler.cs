using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class MovementHandler : MonoBehaviour, IIinteract
{
    public GameObject groupCamera;
    public CinemachineVirtualCamera virtualCamera;
    public List<Collider> nearbyCollider;
    public float movementSpeed = 10f;
    public float sprintSpeed = 20f;
    public float jumpForce = 5f;
    public Transform player;
    public Rigidbody rb;
    public int loadScene;    
    public float rotationSpeed = 2f;

    [Header("Joystick")]
    public Joystick joystick;
    public RectTransform joystickHandle;
    
    [Header("Animator")]
    public bool useAnimator = false;
    public Animator animator;

    private Vector3 lastMoveDirection;
    public Vector3 inputWithKeyboard;
    private bool cursorVisible = false;
    private bool isJumping = false;
    private bool canMoving = true;
    private bool isSprinting = false;
    private bool isMoving = false;
    private bool isAttacking = false;
    public List<Collider> enemyCollider;

    public bool CanMoving
    {
        get { return canMoving; }
        set { canMoving = value; }
    }
    public static MovementHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(groupCamera);
            DontDestroyOnLoad(virtualCamera);
            Instance = this;
            // SceneManager.sceneLoaded += OnSceneLoaded; // Daftarkan event saat scene dimuat
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            Destroy(groupCamera);
        }
    }

    void OnDestroy()
    {
        // SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister event saat objek dihancurkan
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Perbarui referensi player saat scene baru dimuat
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (virtualCamera != null)
        {
            virtualCamera.Follow = player;
            virtualCamera.LookAt = player;
        }
    }

    [Obsolete]
    void Start()
    {
        if (useAnimator == false)
        {
            Debug.Log("Use Animator: "+useAnimator);
        }
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        QualitySettings.vSyncCount = 0;
        lastMoveDirection = transform.forward;
        // Debug.Log(virtualCamera.name);
    }

    void OnDisable()
    {
        SavePlayerState();
    }

    void SavePlayerState()
    {
        PlayerPrefs.SetFloat("PlayerPosX", player.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.position.z);

        PlayerPrefs.SetFloat("PlayerRotX", player.rotation.eulerAngles.x);
        PlayerPrefs.SetFloat("PlayerRotY", player.rotation.eulerAngles.y);
        PlayerPrefs.SetFloat("PlayerRotZ", player.rotation.eulerAngles.z);
    }

    void LoadPlayerState()
    {
        float posX = PlayerPrefs.GetFloat("PlayerPosX", player.position.x);
        float posY = PlayerPrefs.GetFloat("PlayerPosY", player.position.y);
        float posZ = PlayerPrefs.GetFloat("PlayerPosZ", player.position.z);

        float rotX = PlayerPrefs.GetFloat("PlayerRotX", player.rotation.eulerAngles.x);
        float rotY = PlayerPrefs.GetFloat("PlayerRotY", player.rotation.eulerAngles.y);
        float rotZ = PlayerPrefs.GetFloat("PlayerRotZ", player.rotation.eulerAngles.z);

        player.position = new Vector3(posX, posY, posZ);
        player.rotation = Quaternion.Euler(rotX, rotY, rotZ);
    }

    public void HandleUpdate()
    {
        // HandleMouseInput();
        HandlePlayerMovement();
        // HandleJump();
        HandleSprint();
        // ShowCursor();
        Attack();
        Interact();
    }

    void HandlePlayerMovement()
    {
        if (isAttacking == true)
        {
            return;
        }
        if (canMoving == false)
        {
            return;
        }
        Vector3 keyBoardInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        inputWithKeyboard = keyBoardInput;
        animator.SetBool("isMoving", inputWithKeyboard.magnitude > 0.1f);
        Vector3 direct = keyBoardInput + new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        if (direct.magnitude > 0.1f)
        {
            MovePlayer(direct);
            UpdateJoystickHandle(keyBoardInput);
        }
        // AlignPlayerToCamera();
    }

    void HandleJump()
    {   
        if (isJumping == true)
        {
            return;
        }
        if (canMoving == false)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }            
    }

    public void HandleSprint()
    {
        if (canMoving == false)
        {
            return;
        }
        // Check for right mouse button press
        if (Input.GetMouseButtonDown(1))
        {
            OnSprintButtonPressed();
            isAttacking = false;
        }

        // Check for right mouse button release
        if (Input.GetMouseButtonUp(1))
        {
            OnSprintButtonReleased();
        }
        // This part is handled within the UI button methods
        movementSpeed = isSprinting ? sprintSpeed : 10f;
    }

    // Call this method when the UI sprint button is pressed
    public void OnSprintButtonPressed()
    {
        isSprinting = true;
    }

    // Call this method when the UI sprint button is released
    public void OnSprintButtonReleased()
    {
        isSprinting = false;
    }

    void Update()
    {
        // Toggle cursor visibility with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.Instance.SetPause();
            cursorVisible = !cursorVisible;
            Cursor.visible = cursorVisible;
            Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
        // Debug.Log("Current Animator: "+animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        // Debug.Log("Current Duration Of Animator: "+animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    private void MovePlayer(Vector3 direction)
    {
        if (!isMoving)
        {
            isMoving = true ? direction.magnitude > 0.1f : false;
        }
        direction = Quaternion.Euler(0, virtualCamera.transform.eulerAngles.y, 0) * direction;
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.Euler(0, direction.y, 0);
        FacePlayer(direction);
        // Debug.Log(transform.rotation);
    }

    private void FacePlayer(Vector3 moveDirection)
    {
        if (moveDirection.magnitude > 0.1f)
        {
            Debug.Log("Last Move Direction: "+lastMoveDirection);
            lastMoveDirection = moveDirection;
        }

        if (lastMoveDirection.magnitude > 0.1f)
        {
            // Tentukan rotasi target berdasarkan arah gerakan
            Quaternion targetRotation = Quaternion.LookRotation(lastMoveDirection, Vector3.up);
            transform.rotation = targetRotation;

            Debug.Log("Facing Direction Updated");
        }
    }

    private void OnTriggerEnter(Collider other) 
    {        
        
        if (other.CompareTag("Dialogueable"))
        {
            nearbyCollider.Add(other);
            Debug.Log("Count Nearby: " + nearbyCollider.Count);
            foreach (Collider collider in nearbyCollider)
            {
                Debug.Log("Nearby You: " + collider.name);
            }
        }
        if (other.CompareTag("Enemy"))
        {
            nearbyCollider.Add(other);
            enemyCollider.Add(other);
            foreach(Collider collEnemy in nearbyCollider)
            {
                Debug.Log("Nearby Enemy: " + collEnemy.name);
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Dialogueable"))
        {
            nearbyCollider.Remove(other);
        }
        if (other.CompareTag("Enemy"))
        {
            enemyCollider.Remove(other);
            nearbyCollider.Remove(other);
        }
    }

    private void Attack()
    {        
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            // if (enemyCollider.Count == 0)
            // {
            //     Debug.Log("No Enemy Found");
            //     return;
            // }
            if (isSprinting == true)
            {
                OnSprintButtonReleased();
            }
            animator.SetBool("isMoving", false);
            isAttacking = true;
            Debug.Log("IsAttacking: "+isAttacking);
            Debug.Log("You Now Attack");
            animator.SetTrigger("Attack");
            StartCoroutine(StopAttackHandler());
            // PlayerAttack.Instance.ShootBullet();
        }
    }

    private IEnumerator StopAttackHandler()
    {
        if (isAttacking == true && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Debug.Log("Stop Attack Handler Method");
            Debug.Log("Animator Name: "+animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            // float currentDurationAnimator = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            Debug.Log("Animator Length: "+GetAnimationClipDuration("attack"));
            yield return new WaitForSeconds(GetAnimationClipDuration("attack"));
            isAttacking = false;
            Debug.Log("Is Attacking: "+isAttacking);
        }
        yield return null;
    }

    private float GetAnimationClipDuration(string animationName)
    {
        AnimationClip[] clips =  animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                Debug.Log("Clip Length: "+clip.length);
                return clip.length;
            }
        }
        return 0f;
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (Collider collider in nearbyCollider)
            {
                IIinteract interactable = collider.GetComponent<IIinteract>();
                if (interactable != null)
                {
                    canMoving = false;
                    Debug.Log("Interacting with: " + collider.name);
                    interactable.Interaksi();
                }
                Debug.Log("Interacting with: " + collider.name);
                if (collider.name == "TransferPlayer")
                {
                    SceneManager.LoadScene(loadScene);
                }
            }
        }
    }

    void IIinteract.Interaksi()
    {
        throw new NotImplementedException();
    }

    private void UpdateJoystickHandle(Vector3 keyBoardInput)
    {
        if (joystickHandle != null)
        {
            Vector3 joystickPosition = new Vector2(keyBoardInput.x, keyBoardInput.z) * 90f;
            joystickHandle.anchoredPosition = joystickPosition;
        }
    }
}
