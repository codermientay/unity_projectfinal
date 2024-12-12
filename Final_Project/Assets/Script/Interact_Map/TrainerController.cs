using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, ISavable
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;
    [SerializeField] GameObject exclaimation;
    [SerializeField] GameObject fov;
    [SerializeField] Dialog dialog;

    bool battleLost = false;
    public IEnumerator TriggerTrainerBattle(PlayerControl player)
    {
        exclaimation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclaimation.SetActive(false);
        //StartCoroutine(DialogManager.Instance.ShowDialog(dialog), () =>
        //{
        //    GameController.Instance.StartTrainerBattle(this);
        //});
        GameController.Instance.StartTrainerBattle(this);
    }
    public void BattleLost()
    {
        battleLost = true;
        fov.gameObject.SetActive(false);
    }

    public object CaptureState()
    {
        return battleLost;
    }

    public void RestoreState(object state)
    {
        battleLost = (bool)state;
    }

    public string Name
    {
        get => name;
    }
    public Sprite Sprite
    {
        get => sprite;
    }
}