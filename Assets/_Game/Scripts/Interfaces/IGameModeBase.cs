using System.Collections.Generic;
using UnityEngine.Events;

public interface IGameModeBase
{
    List<CharacterController> Allys { get; set; }
    List<CharacterController> Enemys { get; set; }
    UnityEvent OnWinEvent { get; }
    UnityEvent OnCloseEvent { get; }

    void SetUpGame(int level);
    void PlayGame();
    void AllyDead(Character character);
    void EnemyDead(Character character);
    void ResetGame();
}