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

    public void SetSpaceForGrid (int gridButtonIdx, string playerSide)
    {
        // gameController = new GameController();
        GridSpace gridSpace = gameController.buttonList[gridButtonIdx].GetComponentInParent<GridSpace>();

        gridSpace.buttonText.text = playerSide;
        gridSpace.button.interactable = false;

        gameController.EndTurn(gridButtonIdx);
    }

    public void SetGameControllerReference (GameController controller)
    {
      gameController = controller;
    }
}
