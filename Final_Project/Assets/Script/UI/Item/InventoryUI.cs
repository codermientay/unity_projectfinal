
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject list; // Danh sách hiển thị các slot
    [SerializeField] Slots itemPrefab; // Prefab cho mỗi item slot
    [SerializeField] UnityEngine.UI.Image icon;
    [SerializeField] Text describtion;
    [SerializeField] GameObject Bag;

    public bool isBagActive;

    Inventory inventory; // Tham chiếu đến Inventory
    private int selected = 0;
    private List<Slots> slotsUI = new List<Slots>(); // Danh sách các UI Slots

    private void Start()
    {
        // Tìm Inventory
        inventory = FindObjectOfType<Inventory>();
        // Debug.Log("CC:" + inventory.slots.Count);

        if (inventory != null)
        {
            UpdateUI();
        }
        else
        {
            Debug.LogError("Inventory không được tìm thấy!");
        }
    }

    public void ToggleBag()
    {
        // Kiểm tra phím L để đóng/mở túi
        if (Input.GetKeyDown(KeyCode.L))
        {
            isBagActive = false; // Chuyển trạng thái túi
            Bag.SetActive(false); // Hiển thị/ẩn túi
        }
    }
    public void UpdateUI()
    {
        // Xóa tất cả các item cũ trong danh sách
        foreach (Transform child in list.transform)
        {
            Destroy(child.gameObject);
        }

        slotsUI.Clear();

        // Lặp qua danh sách slots trong Inventory và tạo UI
        foreach (ItemSlot slot in inventory.slots)
        {
            Slots newItemSlot = Instantiate(itemPrefab, list.transform);
            Debug.Log("OKE!");
            newItemSlot.SetSlot(slot.Item, slot.Count);
            slotsUI.Add(newItemSlot); // Lưu lại danh sách UI Slots
        }

        UpdateItems(); // Cập nhật hiển thị
    }

    public void HandleMenuNavigation()
    {
        int previousSelected = selected;
        UpdateUI();
        // Điều hướng trong menu
        if (Input.GetKeyDown(KeyCode.W)) // Lên
        {
            selected--;
        }
        else if (Input.GetKeyDown(KeyCode.S)) // Xuống
        {
            selected++;
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
                icon.sprite = inventory.slots[i].Item.Icon;
                describtion.text = inventory.slots[i].Item.Description;
            }
            else
            {
                slotsUI[i].Highlight(false); // Bỏ làm nổi bật
            }
        }
    }
}
