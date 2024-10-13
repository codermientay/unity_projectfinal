using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;
    int currentMove;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.SetUp();
        enemyUnit.SetUp();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"Một {enemyUnit.Pokemon.Base.Name} hoang dã xuất hiện.");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }
    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Hãy chọn hành động"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(true);
        dialogBox.EnableDialogText(false);
    }
    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} thi triển {move.Base.Name}");

        yield return new WaitForSeconds(1f);

        bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        enemyHud.UpdateHP();
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} đã bị hạ gục");
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} thi triển {move.Base.Name}");

        yield return new WaitForSeconds(1f);

        bool isFainted = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        playerHud.UpdateHP();
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} đã bị hạ gục");
        }
        else
        {
            PlayerAction();
        }
    }
    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }
    
    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction == 0) 
                currentAction = 2;
            else if (currentAction == 1)
                currentAction = 3;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction == 2) 
                currentAction = 0;
            else if (currentAction == 3)
                currentAction = 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction % 2 == 0 && currentAction < 3) // di chuyển qua phải
            {
                ++currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction % 2 == 1) // di chuyển qua trái
            {
                --currentAction;
            }
        }


        dialogBox.UpdateActionSelection(currentAction);
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if(currentAction == 0)
            {
                //Fight
                PlayerMove();
            }
            else if(currentAction == 1)
            {
                //Ball
            }
            else if (currentAction == 2)
            {
                //Pokemon
            }
            else if (currentAction == 3)
            {
                //Run
            }
        }

    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 2)
                currentMove += 2;

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
                currentMove -= 2;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 1)
            {
                ++currentMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
            {
                --currentMove;
            }
        }


        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
            if (currentMove == 0)
            {
                //Move 1
            }
            else if (currentMove == 1)
            {
                //Move 2
            }
            else if (currentMove == 2)
            {
                //Move 3
            }
            else if (currentMove == 3)
            {
                //Move 4
            }
        }

    }
}
