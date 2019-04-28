using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// webrtc
using System.Text;
using System;
using Byn.Net;
using System.Collections.Generic;
using Byn.Common;

[System.Serializable]
public class Player {
  public Image panel;
  public Text text;
  public Button button;
}

[System.Serializable]
public class CreateJoinGame {
  public Image panel;
  public Text text;
  public Button button;
}

[System.Serializable]
public class PlayerColor {
  public Color panelColor;
  public Color textColor;
}

public class GameController : MonoBehaviour {
  public Text[] buttonList;
  public GameObject gameOverPanel;
  public Text gameOverText;
  public GameObject restartButton;

  public Player playerX;
  public Player playerO;
  public PlayerColor activePlayerColor;
  public PlayerColor inactivePlayerColor;

  public GameObject startInfo;
  public GameObject createJoinGameInfo;
  public GameObject waitingForPlayerInfo;

  public GameObject yourTurnInfo;
  public GameObject theirTurnInfo;
  public GameObject joiningInfo;

  public CreateJoinGame createGame;
  public CreateJoinGame joinGame;

  private string playerSide;
  public int moveCount;

  private ChatApp cApp;

  private const string DEFAULT_ROOM_NAME = "foo-bar";

  void Awake ()
  {
    SetGameControllerReferenceOnButtons();
    gameOverPanel.SetActive(false);
    moveCount = 0;
    restartButton.SetActive(false);
    startInfo.SetActive(false);

    SetYourTurnInfo(false);
    SetTheirTurnInfo(false);
    SetJoiningInfo(false);

    waitingForPlayerInfo.SetActive(false);
    SetCreateJoinButtons(true);
    cApp = new ChatApp();
  }

  // void SetPlayers (bool toggle)
  // {
  //   playerX.panel.enabled = toggle;
  //   playerX.text.enabled = toggle;
  //   playerX.button.enabled = toggle;
  //
  //   playerO.panel.enabled = toggle;
  //   playerO.text.enabled = toggle;
  //   playerO.button.enabled = toggle;
  // }

  private void OpenRoom(string startingSide)
  {
      Debug.Log(startingSide);

      cApp.StartMe();
      cApp.OpenRoomButtonPressed(DEFAULT_ROOM_NAME, startingSide, gameObject);
  }

  private void JoinRoom()
  {
      // cApp = new ChatApp();
      cApp.StartMe();
      cApp.JoinRoomButtonPressed(DEFAULT_ROOM_NAME, gameObject);
  }

  // player1 starts game (presses PLAY in unity)
  // player1 chooses side (X), creating a new room "foobar"
  // - this room needs a property "side" indicating what player1 chose as side
  // game goes into "waiting" state, waiting for player2 to join room "foobar"
  // after player2 joins
  // - player2 sees message at top showing them their side (opposite of player1, which is "O")
  // - player1 sees message saying that player2 has joined, it's player1's turn
  // player1 plays the first grid (0-8) as X (EndTurn())
  // - this sends a message to the room indicating: side (X or O), grid (0-8) and moveCount (0-8)
  // - HandleIncommingMessage() should update board ...

  // need a new state: waiting to create/join room state


  private void FixedUpdate()
  {
      //check each fixed update if we have got new events
      cApp.HandleNetwork();
  }

  void SetPlayerColorsInactive ()
  {
    playerX.panel.color = inactivePlayerColor.panelColor;
    playerX.text.color = inactivePlayerColor.textColor;
    playerO.panel.color = inactivePlayerColor.panelColor;
    playerO.text.color = inactivePlayerColor.textColor;
  }

  public void CreateGame ()
  {
      SetCreateJoinButtons(false);
      createJoinGameInfo.SetActive(false);

      startInfo.SetActive(true);
      SetPlayerButtons(true);
  }

  // should
  public void JoinGame ()
  {
      SetCreateJoinButtons(false);
      createJoinGameInfo.SetActive(false);
      SetJoiningInfo(true);
      SetPlayerButtons(false);

      // startInfo.SetActive(true);
      // SetPlayerButtons(true);

      // will need to ensure other side starts
      JoinRoom();
      // StartGame ();
  }

  public void SetStartingSide (string startingSide)
  {
    playerSide = startingSide;
    if (playerSide == "X")
    {
      SetPlayerColors(playerX, playerO);
    }
    else
    {
      SetPlayerColors(playerO, playerX);
    }

    OpenRoom(startingSide);

    SetPlayerButtons(false);
    SetWaitingForPlayer(true);
  }

  public void SetWaitingForPlayer(bool toggle)
  {
      waitingForPlayerInfo.SetActive(toggle);
  }

  public void SetJoiningInfo(bool toggle)
  {
      joiningInfo.SetActive(toggle);
  }

  public void SetYourTurnInfo(bool toggle)
  {
      yourTurnInfo.SetActive(toggle);
  }

  public void SetTheirTurnInfo(bool toggle)
  {
      theirTurnInfo.SetActive(toggle);
  }

  void SetPlayerButtons (bool toggle)
  {
    playerX.button.interactable = toggle;
    playerO.button.interactable = toggle;
  }

  void SetCreateJoinButtons (bool toggle)
  {

    createGame.panel.gameObject.SetActive(toggle);
    joinGame.panel.gameObject.SetActive(toggle);

    // createGame.button.interactable = toggle;
    // joinGame.button.interactable = toggle;
  }

  public void SetPlayerColors (Player newPlayer, Player oldPlayer)
  {
    newPlayer.panel.color = activePlayerColor.panelColor;
    newPlayer.text.color = activePlayerColor.textColor;
    oldPlayer.panel.color = inactivePlayerColor.panelColor;
    oldPlayer.text.color = inactivePlayerColor.textColor;
  }

  void SetGameControllerReferenceOnButtons ()
  {
    for (int i = 0; i < buttonList.Length; i++)
    {
      buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
    }
  }

  public string GetPlayerSide ()
  {
    return playerSide;
  }

  public void EndTurn (int buttonIdx, bool shouldBroadcast = true)
  {
    moveCount++;

    // clean this up
    if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
    {
      GameOver(playerSide);
    }

    else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
    {
      GameOver(playerSide);
    }

    else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
    {
      GameOver(playerSide);
    }

    else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
    {
      GameOver(playerSide);
    }

    else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
    {
      GameOver(playerSide);
    }

    else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
    {
      GameOver(playerSide);
    }

    else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
    {
      GameOver(playerSide);
    }

    else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
    {
      GameOver(playerSide);
    }

    else if (moveCount >= 9)
    {
      GameOver("draw");
    }
    else
    {

      if (shouldBroadcast)
      {
          cApp.SendButtonPressed("MOVE:" + playerSide + ":" + buttonIdx);
      }

      ChangeSides ();
    }
  }

  void ChangeSides ()
  {
    playerSide = (playerSide == "X") ? "O" : "X";

    if (playerSide == "X")
    {
      SetPlayerColors(playerX, playerO);
    }
    else
    {
      SetPlayerColors(playerO, playerX);
    }

    if (yourTurnInfo.activeSelf)
    {
        SetYourTurnInfo(false);
        SetTheirTurnInfo(true);
        SetBoardInteractable(true);
    } else
    {
        SetYourTurnInfo(true);
        SetTheirTurnInfo(false);
        SetBoardInteractable(false);
    }
  }

  void GameOver (string winningPlayer)
  {
    SetBoardInteractable(false);

    if (winningPlayer == "draw")
    {
      SetGameOverText("It's a Draw!");
    }
    else
    {
      SetGameOverText(winningPlayer + " Wins!");
    }

    restartButton.SetActive(true);
  }

  void SetGameOverText (string value)
  {
    gameOverPanel.SetActive(true);
    gameOverText.text = value;
  }

  public void StartGame (bool isOpener)
  {
     Debug.Log("game started");
     SetPlayerButtons(false);

    if (isOpener) {
        Debug.Log("you need to start");
        SetBoardInteractable(true);

        // show new panel: "it's your turn!"
    }

    if (!isOpener) {
        SetBoardInteractable(false);

        // show new panel: "it's their turn!"
    }

    startInfo.SetActive(false);
  }

  public void RestartGame ()
  {
    moveCount = 0;
    gameOverPanel.SetActive(false);

    for (int i = 0; i < buttonList.Length; i++)
    {
      buttonList [i].text = "";
    }

    startInfo.SetActive(true);
    SetPlayerColorsInactive();
    restartButton.SetActive(false);
    SetPlayerButtons(true);
  }

  void SetBoardInteractable (bool toggle)
  {
    for (int i = 0; i < buttonList.Length; i++)
    {
      buttonList[i].GetComponentInParent<Button>().interactable = toggle;
    }
  }
}
