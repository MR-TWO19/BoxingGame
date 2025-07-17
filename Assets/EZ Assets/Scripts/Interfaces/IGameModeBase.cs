using System.Collections.Generic;

public interface IGameModeBase
{
    List<CharacterController> Allys { get; set; }
    List<CharacterController> Enemys { get; set; }

    void SetUpGame(int level);
    void PlayGame();
}