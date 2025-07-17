using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameModeBase : IGameModeBase
{
    public List<CharacterController> Allys { get; set; } = new();
    public List<CharacterController> Enemys { get; set; } = new();

    public abstract void PlayGame();

    public abstract void SetUpGame(int level);

    public void CreateCharacter(GameObject playerPrefab, Transform posTran, CharacterData extraData, TeamType team, bool isPlayer) 
    {
        GameObject character = GameObject.Instantiate(playerPrefab, posTran.position, posTran.rotation);
        CharacterController controllerCharacter = character.GetComponent<CharacterController>();
        controllerCharacter.SetUp(extraData, team, isPlayer);
        if(team == TeamType.Ally)
           Allys.Add(controllerCharacter);
        else
            Enemys.Add(controllerCharacter);
    }
}
