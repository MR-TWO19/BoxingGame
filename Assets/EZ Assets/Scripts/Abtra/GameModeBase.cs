using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

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
        controllerCharacter.SetUp(name, extraData, team, isPlayer);
        if(team == TeamType.Ally)
           Allys.Add(controllerCharacter);
        else
            Enemys.Add(controllerCharacter);
    }

    public void AllyDead(Character character)
    {
        Allys.RemoveAll(item => item.character == character);

        if(Allys.Count <= 0)
        {
            OnCloseEvent?.Invoke();   
        }
    }

    public void EnemyDead(Character character)
    {
        Enemys.RemoveAll(item => item.character == character);

        if (Enemys.Count <= 0)
        {
            OnWinEvent?.Invoke();
        }
    }
}
