using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    [SerializeField] private Text nameText; // Tên item
    [SerializeField] private Text countText; // Số lượng item

    public void SetSlot(baseItem item, int count)
    {
        nameText.text = item.Name;
        countText.text = count.ToString();
    }

    public void Highlight(bool isSelected)
    {
        if (isSelected)
        {
            nameText.color = Color.red; // Nền nổi bật khi được chọn
            countText.color = Color.red; // Nền nổi bật khi được chọn
        }
        else
        {
            nameText.color = Color.black; // Nền nổi bật khi được chọn
            countText.color = Color.black; // Nền nổi bật khi được chọn
        }
    }
}
