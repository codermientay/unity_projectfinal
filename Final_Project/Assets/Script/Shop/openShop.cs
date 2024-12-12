using Unity.VisualScripting;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private GameObject shop; // Giao diện shop
    [SerializeField] public GameObject player;
    private PlayerControl playerControl;
    private bool isPlayerInTrigger = false;  // Biến cờ để kiểm tra người chơi có trong vùng hay không
    [SerializeField] GameController gameController;
    void Start()
    {
        playerControl = player.GetComponent<PlayerControl>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && this.CompareTag("Shop"))
        {
            isPlayerInTrigger = true; // Đặt cờ khi người chơi vào vùng
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && this.CompareTag("Shop"))
        {
            isPlayerInTrigger = false; // Hủy cờ khi người chơi rời vùng
        }
    }

    private void Update()
    {
        // Kiểm tra nếu người chơi đang trong vùng và nhấn phím J
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.J))
        {
            gameController.ChangeGameStateToShop(); // Gọi hàm chuyển trạng thái game sang Shop
            shop.SetActive(true); // Bật giao diện shop
        }
    }
}
