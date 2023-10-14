using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : TutorialBase
{
    [SerializeField]
    private bool isInteracting;

    [SerializeField]
    private List<TileButton> buttons;

    public override void Enter()
    {
        List<Tile> tiles = GameManager.Instance.SpawnedTiles;
        foreach (Tile tile in tiles)
        {
            TileButton tileButton = tile.GetComponent<TileButton>();

            if (tileButton != null)
            {
                buttons.Add(tileButton);
                tileButton.canDoMove = isInteracting;
            }
        }
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
    }

    public override void Exit()
    {
    }
}
