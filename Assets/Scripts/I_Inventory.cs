using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public Inventory inventory;
    public Item sampleItem1;
    public Item sampleItem2;

    void Start()
    {
        // Menambahkan item ke inventaris
        inventory.AddItem(sampleItem1);
        inventory.AddItem(sampleItem2);

        // Menampilkan semua item di inventaris
        inventory.ListItems();

        // Menghapus item dari inventaris
        inventory.RemoveItem(sampleItem1);

        // Menampilkan semua item di inventaris setelah penghapusan
        inventory.ListItems();
    }
}
