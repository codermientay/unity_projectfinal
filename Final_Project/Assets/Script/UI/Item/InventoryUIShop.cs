using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryShopUI : MonoBehaviour
{
    [SerializeField] GameObject listPLayer; // Danh sách hiển thị các slot
    [SerializeField] Slots itemPrefab; // Prefab cho mỗi item slot
    [SerializeField] GameObject listShop; // Danh sách hiển thị các slot
    [SerializeField] GameObject player;
    [SerializeField] SlotsShop itemShopPrefab; // Prefab cho mỗi item slot
    [SerializeField] UnityEngine.UI.Image icon;
    [SerializeField] Text describtion;

    [SerializeField] private Text totalPlayer;
    [SerializeField] private Text total;
    Shop shop; // Tham chiếu đến Inventory
    private int selected = 0;
    private List<SlotsShop> slotsUI = new List<SlotsShop>(); // Danh sách các UI Slots
    [SerializeField] GameObject ShopUI;


    private void Start()
    {
        // Tìm Inventory
        shop = this.GetComponent<Shop>();
        Inventory playerinventory = player.GetComponent<Inventory>();
        // Debug.Log("CC:" + inventory.slots.Count);

        if (shop != null)
        {
            UpdateUI();
        }
        else
        {
            Debug.LogError("Inventory không được tìm thấy!");
        }
    }

    public void UpdateUI()
    {
        totalPlayer.text = player.GetComponent<PlayerControl>().money.ToString() + "$";
        // Xóa tất cả các item cũ trong danh sách
        foreach (Transform child in listShop.transform)
        {
            Destroy(child.gameObject);
        }

        slotsUI.Clear();
        float totalFloat = 0;
        // Lặp qua danh sách slots trong Inventory và tạo UI
        foreach (ShopSlot slot in shop.slots)
        {
            SlotsShop newItemSlot = Instantiate(itemShopPrefab, listShop.transform);
            newItemSlot.SetSlot(slot.Item, slot.Count);
            slotsUI.Add(newItemSlot); // Lưu lại danh sách UI Slots

            totalFloat += (slot.Count * slot.Item.Pricebuy);

            total.text = totalFloat.ToString() + "$";
        }

        UpdateItems(); // Cập nhật hiển thị
    }

    public void ToggleBag()
    {
        // Kiểm tra phím L để đóng/mở túi
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShopUI.SetActive(false); // Hiển thị/ẩn túi
        }
    }

    public void HandleMenuNavigation()
    {
        int previousSelected = selected;
        Inventory playerinventory = player.GetComponent<Inventory>();
        // Điều hướng trong menu    
        if (Input.GetKeyDown(KeyCode.W)) // Lên
        {
            selected--;
        }
        else if (Input.GetKeyDown(KeyCode.S)) // Xuống
        {
            selected++;
        }
        else if (Input.GetKeyDown(KeyCode.D)) // phải
        {
            if (shop.slots[selected].Count - 1 != 99)
            {
                shop.slots[selected].setCount(shop.slots[selected].Count + 1);
                UpdateUI();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)) // trái
        {
            if (shop.slots[selected].Count - 1 != -1)
            {
                shop.slots[selected].setCount(shop.slots[selected].Count - 1);
                UpdateUI();
            }

        }
        else if (Input.GetKeyDown(KeyCode.Space)) // Chọn item cần mua
        {
            //Trừ tiền player done

            float totalFloat = 0;
            foreach (ShopSlot slot in shop.slots)
            {
                totalFloat += (slot.Count * slot.Item.Pricebuy);
                total.text = totalFloat.ToString() + "$";
                UpdateUI();
            }
            if (player.GetComponent<PlayerControl>().money - totalFloat >= 0)
            {

                player.GetComponent<PlayerControl>().setMoney(player.GetComponent<PlayerControl>().money - totalFloat);
                //Thêm vật phẩm đã mua cho player
                for (int i = 0; i < shop.slots.Count; i++)
                {
                    int check = 0;
                    if (shop.slots[i].Count != 0)
                    {

                        ItemSlot newslot = new ItemSlot();
                        newslot.setItem(shop.slots[i].Item);
                        newslot.setCount(shop.slots[i].Count);
                        Debug.Log("Gần xong cái này rồi tên : " + newslot.Item.Name);
                        foreach (ItemSlot sl in playerinventory.slots)
                        {
                            if (newslot.Item.Name.Equals(sl.Item.Name))
                            {
                                sl.setCount(sl.Count + newslot.Count);
                                check = 1;
                                break;
                            }
                        }
                        if (check == 0)
                        {
                            playerinventory.slots.Add(newslot);
                        }

                        Debug.Log("Gần xong cái này rồi: " + check);
                        shop.slots[i].setCount(0);

                    }
                    Debug.Log("Soos lanaf nos chayj: " + i);
                    UpdateUI();
                }
            }
            else
            {
                Debug.Log("Không đủ tiền!!!!");
            }
            totalPlayer.text = player.GetComponent<PlayerControl>().money.ToString() + "$";

        }
        if (!(selected == -1 || selected == shop.slots.Count))
        {
            Debug.Log("Đây là cái mà mình đang chọn: " + "Có tên là: " + shop.slots[selected].Item.Name + " Với số lượng là : " + shop.slots[selected].Count);

        }

        // Đảm bảo chỉ số nằm trong giới hạn danh sách
        selected = Mathf.Clamp(selected, 0, slotsUI.Count - 1);

        // Cập nhật trạng thái nếu có thay đổi
        if (previousSelected != selected)
        {
            UpdateItems();
        }
    }

    public void UpdateItems()
    {
        // Cập nhật màu sắc của các mục menu
        for (int i = 0; i < slotsUI.Count; i++)
        {
            if (i == selected)
            {
                slotsUI[i].Highlight(true); // Làm nổi bật slot
                icon.sprite = shop.slots[i].Item.Icon;
                describtion.text = shop.slots[i].Item.Description;
            }
            else
            {
                slotsUI[i].Highlight(false); // Bỏ làm nổi bật
            }
        }
    }
}