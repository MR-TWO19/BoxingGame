using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameModeBase : IGameModeBase
{
    public List<CharacterController> Allys { get; set; } = new();
    public List<CharacterController> Enemys { get; set; } = new();

    public UnityEvent OnWinEvent { get; } = new UnityEvent();
    public UnityEvent OnCloseEvent { get; } = new UnityEvent();

    public abstract void PlayGame();

    public abstract void SetUpGame(int level);

    public void CreateCharacter(string name, GameObject playerPrefab, Transform posTran, PowerExtraData extraData, TeamType team, bool isPlayer) 
    {
        GameObject character = GameObject.Instantiate(playerPrefab, posTran.position, posTran.rotation);
        CharacterController controllerCharacter = character.GetComponent<CharacterController>();
        if(team == TeamType.Ally)
           Allys.Add(controllerCharacter);
        else
            Enemys.Add(controllerCharacter);
        controllerCharacter.SetUp(name, extraData, team, isPlayer);
    }

    public void AllyDead(Character character)
    {
        Allys.RemoveAll(item => item.character == character);
        Enemys.ForEach(_ => _.character.Victory());
        if (Allys.Count <= 0)
        {
            Debug.Log("B-- Close");
            ShowResult(false);
            OnCloseEvent?.Invoke();   
        }
    }

    public void EnemyDead(Character character)
    {
        Enemys.RemoveAll(item => item.character == character);
        Allys.ForEach(_ => _.character.Victory());
        if (Enemys.Count <= 0)
        {
            Debug.Log("B-- win");
            ShowResult(true);
            OnWinEvent?.Invoke();
        }
    }

    private void ShowResult(bool isWin) {
        DOVirtual.DelayedCall(5, () =>
        {

        UIManager.Ins.resultPopup.Show(isWin);
        });
    }
}
