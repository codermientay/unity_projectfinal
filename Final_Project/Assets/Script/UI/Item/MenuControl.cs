using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] GameObject menu; // Tham chiếu đến menu
    [SerializeField] GameObject bag; //  Tham chiếu đến túi
    private List<Text> menuItems; // Danh sách các mục trong menu
    public int selected = 0; // Mục được chọn hiện tại
    public bool isMenuActive = false; // Trạng thái mở/đóng menu
    public bool isOpen = false; // Trạng thái mở/đóng menu
    private PlayerControl playerControl;
    [SerializeField] PokemonParty playerParty;
    [SerializeField] PartyScreen party;
    [SerializeField] GameObject battleHUD;
    [SerializeField] GameObject battle_canvas;
    [SerializeField] GameObject PList;
    void Start()
    {
        playerParty = this.GetComponent<PokemonParty>();
        // Debug.Log("Cái này có bị null không?: " + playerParty.pokemons.Count);
        // Lấy tất cả các TextMeshPro từ các mục con trong menu
        menuItems = menu.GetComponentsInChildren<Text>().ToList();
        menu.SetActive(false); // Ẩn menu ban đầu
        playerControl = this.GetComponent<PlayerControl>();
    }

    public void ToggleMenu()
    {
        // Mở hoặc đóng menu khi nhấn phím L
        if (Input.GetKeyDown(KeyCode.L))
        {
            isMenuActive = !isMenuActive;
            menu.SetActive(isMenuActive);
            // Tắt script PlayerControl trên đối tượng hiện tại
            if (isMenuActive)
            {
                playerControl.isStop = true;
                playerControl.animator.SetFloat("speed", 0);
                this.GetComponent<PlayerControl>().enabled = false;


            }
            // Tắt script PlayerControl trên đối tượng hiện tại

            else
            {
                // Tắt script PlayerControl trên đối tượng hiện tại
                this.GetComponent<PlayerControl>().enabled = true;
                playerControl.isStop = false;
            }


            if (isMenuActive)
            {
                UpdateMenuItems(); // Đảm bảo cập nhật trạng thái mục
            }
        }
    }

    public void HandleMenuNavigation()
    {
        if (!isMenuActive) return; // Nếu menu chưa được mở, thoát ngay

        int previousSelected = selected;

        // Điều hướng trong menu
        if (Input.GetKeyDown(KeyCode.W)) // Lên
        {
            selected--;
        }
        else if (Input.GetKeyDown(KeyCode.S)) // Xuống
        {
            selected++;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selected == 1)
            {
                bag.SetActive(true);
                isOpen = true;
            }
            if (selected == 0)
            {
                battle_canvas.SetActive(true);
                battleHUD.SetActive(true);
                Debug.Log("aaaaa: " + battleHUD.activeSelf);
                party.Init();
                party.SetPartyData(playerParty.Pokemons);
                // Debug.Log("Lỗi ở đây!!!!");
                party.gameObject.SetActive(true);
                party.gameObject.gameObject.SetActive(true);
                isOpen = true;
                PList.SetActive(true);
            }
            if (selected == 2)
            {
                SavingSystem.i.Save("saveSlot1");
            }
            if (selected == 3)
            {
                SavingSystem.i.Load("saveSlot1");
            }
        }
        if (Input.GetKeyDown(KeyCode.L)) // Xuống
        {
            battleHUD.SetActive(false);
            party.gameObject.SetActive(false);
            PList.SetActive(false);
        }
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     if (isOpen)
        //     {
        //         menu.SetActive(true);
        //         isMenuActive = true;
        //         bag.SetActive(false);
        //         isOpen = false;
        //     }
        // }

        // Đảm bảo chỉ số được giữ trong giới hạn danh sách
        selected = Mathf.Clamp(selected, 0, menuItems.Count - 1);
        // Debug.Log("Chọn: " + selected);
        // Chỉ cập nhật menu nếu có thay đổi
        if (previousSelected != selected)
        {
            UpdateMenuItems();
        }
    }

    public void UpdateMenuItems()
    {
        // Cập nhật màu sắc của các mục menu
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (i == selected)
            {
                menuItems[i].color = Color.red; // Mục được chọn
            }
            else
            {
                menuItems[i].color = Color.black; // Mục không được chọn
            }
        }
    }

    // private void Update()
    // {
    //     ToggleMenu(); // Xử lý việc mở/đóng menu
    //     HandleMenuNavigation(); // Xử lý điều hướng menu
    // }
}
