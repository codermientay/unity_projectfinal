using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private DialougeObject noPokemonDialogue; // Đoạn hội thoại khi không có Pokémon
    private bool isPlayerNear = false; // Kiểm tra xem người chơi có đang gần khối chắn không

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl player))
        {
            isPlayerNear = true; // Ghi nhận rằng người chơi đang ở gần
            CheckForPokemon(player); // Kiểm tra xem người chơi có Pokémon không
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerControl player))
        {
            isPlayerNear = false; // Người chơi đã rời khỏi khối chắn
        }
    }

    private void CheckForPokemon(PlayerControl player)
    {
        PokemonParty playerParty = player.GetComponent<PokemonParty>();

        if (playerParty == null || playerParty.pokemons.Count == 0) // Không có Pokémon trong đội
        {
            this.GetComponent<BoxCollider2D>().isTrigger = false;
            Debug.Log("Người chơi không có Pokémon, không thể qua khối chắn.");
            player.DialougeUI.ShowDialogue(noPokemonDialogue); // Hiển thị đoạn hội thoại
            player.GetComponent<PlayerControl>().animator.SetFloat("speed", 0);
            player.GetComponent<PlayerControl>().movement.Set(0,0);
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            this.GetComponent<BoxCollider2D>().isTrigger = true;
            Debug.Log("Người chơi có Pokémon, có thể đi qua khối chắn.");
        }
    }
}
