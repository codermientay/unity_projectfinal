using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Menu, Bag, Shop, Party, Cutscene , Save, Load }
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerControl playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] MenuControl menu;
    [SerializeField] InventoryUI bag;
    [SerializeField] InventoryShopUI shop;
    [SerializeField] GameData gameData;
    [SerializeField] GameObject battleHUD;
    [SerializeField] GameObject party_fix;
    [SerializeField] GameObject menuUI;
    GameState state;
    public static GameController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        ConditionDB.Init();
    }

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        playerController.OnEnterTrainersView += (Collider2D trainerCollider) =>
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if (trainer != null)
            {
                state = GameState.Cutscene;
                StartCoroutine(trainer.TriggerTrainerBattle(playerController));
            }
        };
        //DialogManager.Instance.OnShowDialog += () =>
        //{
        //    state = GameState.Dialog;
        //};
        //DialogManager.Instance.OnCloseDialog += () =>
        //{
        //    if (state == GameState.Dialog)
        //        state = GameState.FreeRoam;
        //};

    }
    public void ChangeGameStateToShop()
    {
        state = GameState.Shop;
    }
    void StartBattle()
    {
        party_fix.SetActive(false);
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        var wildPokemonCopy = new Pokemon(wildPokemon.Base, wildPokemon.Level);

        battleSystem.StartBattle(playerParty, wildPokemonCopy);
    }
    public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var trainerParty = trainer.GetComponent<PokemonParty>();

        battleSystem.StartTrainerBattle(playerParty, trainerParty);
    }
    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);

    }

    private void Update()
    {
        gameData.setSlots();

        if (state == GameState.FreeRoam)
        {
            if (menu.isMenuActive)
            {
                return; // Nếu menu đang mở, không xử lý di chuyển
            }

            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.N))
            {
                SavingSystem.i.Save("saveSlot2");
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                SavingSystem.i.Load("saveSlot2");
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                state = GameState.Menu;
                menu.ToggleMenu();
            }
            if (playerController.isPlay == true)
            {
                state = GameState.Cutscene;
                playerController.movement.x = 0;
                playerController.movement.y = 0;
                playerController.animator.SetFloat("speed", 0);
            }
            
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HanldeUpdate();
            playerController.movement.x = 0;
            playerController.movement.y = 0;
            playerController.animator.SetFloat("speed", 0);
        }
        else if (state == GameState.Menu)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                menu.ToggleMenu();
                state = GameState.FreeRoam;
            }

            menu.HandleMenuNavigation();
            if (menu.selected == 1 && menu.isOpen) // Nếu mở bag từ menu
            {
                state = GameState.Bag;
            }
            if (menu.selected == 0 && menu.isOpen) // Nếu mở party từ menu
            {
                state = GameState.Party;
                menuUI.SetActive(false);
            }
            if (menu.selected == 2 && menu.isOpen) // Save
            {
                state = GameState.Save;
                menuUI.SetActive(false);
                
            }
            if (menu.selected == 3 && menu.isOpen) // Load
            {
                state = GameState.Load;
                menuUI.SetActive(false);
            }

        }
        else if (state == GameState.Save)
        {
            menu.isOpen = false;
            state = GameState.FreeRoam;
        }
        else if (state == GameState.Load)
        {
            menu.isOpen = false;
            state = GameState.FreeRoam;
        }
        else if (state == GameState.Party)
        {

            if (Input.GetKeyDown(KeyCode.L))
            {
                menu.isOpen = false;
                battleHUD.SetActive(false);
                menuUI.SetActive(true);
                state = GameState.Menu;
            }
        }
        else if (state == GameState.Bag)
        {

            if (Input.GetKeyDown(KeyCode.L))
            {
                menu.isOpen = false;
                bag.ToggleBag();
                state = GameState.Menu;
            }
            bag.HandleMenuNavigation();
        }
        else if (state == GameState.Cutscene)
        {
            if (playerController.isPlay == false)
            {
                state = GameState.FreeRoam;
            }
        }
        else if (state == GameState.Shop)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                shop.ToggleBag();
                state = GameState.FreeRoam;
            }
            shop.HandleMenuNavigation();
        }
        //Debug.Log("Trạng thái hiện tại: " + state);
    }
}
