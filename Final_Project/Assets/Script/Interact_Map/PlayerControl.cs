using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour, ISavable
{
    [SerializeField] private DialougeUI dialougeUI;
    [SerializeField] public CinemachineVirtualCamera camera;
    [SerializeField] string name;
    [SerializeField] Sprite sprite;
    public DialougeUI DialougeUI => dialougeUI;
    private int confinerResetCount = 0;  // Biến đếm số lần gọi ResetConfiner
    public IInteractable Interactable { get; set; }
    public float moveSpeed = 5f;
    public event Action OnEncountered;
    public event Action<Collider2D> OnEnterTrainersView;
    public Rigidbody2D rb;
    public Vector2 movement;
    public Animator animator;
    public bool isStop = false;
    public GameData gameData;

    [SerializeField] public float money;

    private bool hasTriggeredEncounter = false;
    private float moveTimer = 0f;  // Timer to track movement steps
    private const float stepInterval = 0.5f; // Interval for each step (0.5 seconds)

    public void setMoney(float money)
    {
        this.money = money;
    }
    void Start()
    {
        List<PositionOfScene> positionOfScenes = GameData.Instance.positionOfScenes; // Lấy danh sách từ GameData
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của Player

        // Lấy scene index hiện tại
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        // Tìm vị trí tương ứng trong danh sách
        foreach (PositionOfScene po in positionOfScenes)
        {
            if (po.index == currentSceneIndex) // Nếu index khớp với scene hiện tại
            {
                rb.position = po.playerPosition; // Đặt vị trí của player
                break; // Thoát khỏi vòng lặp sau khi tìm thấy
            }
        }
    }
    private void ResetConfiner()
    {
        // Lấy CinemachineVirtualCamera
        var vCam = camera.GetComponent<CinemachineVirtualCamera>();

        if (vCam != null)
        {
            // Kiểm tra xem có CinemachineConfiner2D hay không
            var confiner = vCam.GetComponent<CinemachineConfiner2D>();

            if (confiner != null)
            {
                confiner.InvalidateCache();  // Gọi InvalidateCache
            }
            else
            {
                Debug.LogWarning("Không tìm thấy CinemachineConfiner2D.");
            }
        }
        else
        {
            Debug.LogWarning("CinemachineVirtualCamera không được gắn vào camera.");
        }

        confinerResetCount++;  // Tăng biến đếm sau mỗi lần gọi
    }


    public void HandleUpdate()
    {
        if (dialougeUI.isOpen) return;

        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (!isStop)
        {
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
            animator.SetFloat("speed", movement.sqrMagnitude);
            
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }
        // Set animation parameters
        

    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("npc"))
    //     {
    //         Interactable.Interact(this);
    //     }
    // }

    void FixedUpdate()
    {
        ResetConfiner();

        // Normalize movement to avoid faster diagonal movement
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        // Move player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer);
        // Check if the player is on grass
        if (other.CompareTag("grass"))
        {
            if (movement.x != 0 || movement.y != 0) // Ensure movement is happening
            {
                // Increment the timer by deltaTime
                moveTimer += Time.deltaTime;

                // Check if it's time for a new step
                if (moveTimer >= stepInterval)
                {
                    // Reset the timer and invoke the step logic
                    moveTimer = 0f;

                    // Logic for random event when moving on grass
                    int random = UnityEngine.Random.Range(1, 101);
                    if (random <= 10)  // 10% chance of encountering something
                    {
                        Debug.Log("Random event occurred while on grass!");
                        // Call OnEncountered or any other logic
                        OnEncountered?.Invoke();
                    }
                }
            }
        }
        
        if (collider != null && !hasTriggeredEncounter)
        {
            hasTriggeredEncounter = true;
            animator.SetFloat("speed", 0);
            movement.x = 0;
            movement.y = 0;
            Debug.Log("!!!!!");
            OnEnterTrainersView?.Invoke(collider);
            
        } 
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        hasTriggeredEncounter = false;
    }

    public object CaptureState()
    {
        float[] position = new float[] { transform.position.x, transform.position.y };
        return position;
    }

    public void RestoreState(object state)
    {
        var position = (float[])state;
        transform.position = new Vector3(position[0], position[1]);
    }

    public string Name
    {
        get => name;
    }
    public Sprite Sprite
    {
        get => sprite;
    }
    //private void OnMoveOver()
    //{
    //    CheckIfInTrainersView();
    //}

    //private void CheckIfInTrainersView()
    //{
    //    if(Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer) != null)
    //    {
    //        Debug.Log("!!!");
    //    }
    //}

}
