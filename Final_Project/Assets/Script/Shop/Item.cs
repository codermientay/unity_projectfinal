using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable // Implement IInteractable
{
    [SerializeField] public GameObject player; // Reference to the player GameObject
    [SerializeField] private DialougeObject dialougeObject; // Reference to the dialogue object (if used)
    public bool isPokemon;
    public Pokemon pokemon;
    public ItemSlot item; // Information about the item
    private bool isNearPlayer = false; // Check if the player is near the item

    // Implement the Interact method from IInteractable
    public void Interact(PlayerControl player)
    {

        Debug.Log("Player has interacted with the item.");
        if (!isPokemon)
        {
            AddItemToInventory(); // Add the item to the player's inventory
        }
        else
        {
            AddPokemonToParty();
        }

        Destroy(gameObject); // Destroy the item object from the scene
        player.DialougeUI.ShowDialogue(dialougeObject);
        player.animator.SetFloat("speed", 0);
    }

    void Update()
    {
        // Check if the player is near the item and presses the J key
        if (isNearPlayer && Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Player has picked up the item.");
            Interact(player.GetComponent<PlayerControl>()); // Interact with the item
        }
    }

    // Handle when the player enters the trigger area
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl player))
        {
            player.Interactable = this; // Set the item as the interactable object for the player
            isNearPlayer = true; // Mark that the player is near the item
        }
    }

    // Handle when the player exits the trigger area
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl player))
        {
                player.Interactable = null; // Remove the reference to the interactable item
                isNearPlayer = false; // Mark that the player is no longer near the item

        }
    }

    // Add the item to the player's inventory
    void AddItemToInventory()
    {
        Inventory playerInventory = player.GetComponent<Inventory>();
        if (playerInventory != null)
        {
            bool itemAdded = false;

            // Check if the item already exists in the inventory
            foreach (ItemSlot slot in playerInventory.slots)
            {
                if (slot.Item.Name.Equals(item.Item.Name))
                {
                    slot.setCount(slot.Count + item.Count); // Increase the item count
                    itemAdded = true;
                    break;
                }
            }

            // If the item is not found, add a new slot for it
            if (!itemAdded)
            {
                ItemSlot newSlot = new ItemSlot();
                newSlot.setItem(item.Item);
                newSlot.setCount(item.Count);
                playerInventory.slots.Add(newSlot);
            }

            Debug.Log("Item added to inventory: " + item.Item.Name);
        }
        else
        {
            Debug.LogError("PlayerInventory not found on the player object!");
        }
    }

    void AddPokemonToParty()
    {
        PokemonParty playerparty = player.GetComponent<PokemonParty>();
        playerparty.AddPokemon(pokemon);
        Debug.Log(pokemon.Base.MaxHP);
    }
}
