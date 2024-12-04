using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotsShop : MonoBehaviour
{
    [SerializeField] private Text nameText; // Tên item
    [SerializeField] private Text countText; // Số lượng item
    [SerializeField] private Text priceText; // Giá item

    [SerializeField] private Text total;

    public void SetSlot(baseItem item, int count)
    {
        nameText.text = item.Name;
        countText.text = "X" + count.ToString();
        Debug.Log("chay toi day r!!");
        priceText.text = item.Pricebuy.ToString() + "$";
        Debug.Log("CC " + item.Pricebuy.ToString());
    }
    public void SeTotal(baseItem item, int count)
    {
        Debug.Log("ccc!!!!!!!!!!!!!!!!");
    }
    public void Highlight(bool isSelected)
    {
        if (isSelected)
        {
            nameText.color = Color.red; // Nền nổi bật khi được chọn
            countText.color = Color.red; // Nền nổi bật khi được chọn
            priceText.color = Color.red;
        }
        else
        {
            nameText.color = Color.black; // Nền nổi bật khi được chọn
            countText.color = Color.black; // Nền nổi bật khi được chọn
            priceText.color = Color.black;
        }
    }
}
