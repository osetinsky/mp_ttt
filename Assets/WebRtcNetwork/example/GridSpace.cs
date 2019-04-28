using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {

    public Button button;
    public Text buttonText;
    public int buttonIdx;

    private GameController gameController;

    public void SetSpace ()
    {
        buttonText.text = gameController.GetPlayerSide();
        button.interactable = false;
        gameController.EndTurn(buttonIdx);
    }

    public void SetSpaceForGrid (int gridButtonIdx, string playerSide, GameController gc)
    {
        GridSpace gridSpace = gc.buttonList[gridButtonIdx].GetComponentInParent<GridSpace>();

        gridSpace.buttonText.text = playerSide;
        gridSpace.button.interactable = false;

        gc.EndTurn(gridButtonIdx, false);
    }

    public void SetGameControllerReference (GameController controller)
    {
      gameController = controller;
    }
}
