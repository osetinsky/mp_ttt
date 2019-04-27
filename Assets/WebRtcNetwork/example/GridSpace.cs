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

    public void SetGameControllerReference (GameController controller)
    {
      gameController = controller;
    }
}
