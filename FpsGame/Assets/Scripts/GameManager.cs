using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public enum GameState
  {
    Ready,
    Run,
    Pause,
    GameOver
  }

  public static GameManager gm;

  public GameState gState;

  public GameObject gameLabel;

  public GameObject gameOption;

  Text gameText;

  PlayerMove playerMove;

  private void Start()
  {
    gState = GameState.Ready;
   
    gameText = gameLabel.GetComponent<Text>();

    gameText.text = "Ready...";
    gameText.color = new Color32(255, 185, 0, 255);
    StartCoroutine(ReadyToStart());

    playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
  }

  private void Awake()
  {
    if(gm==null)
    {
      gm = this;
    }
  }

  private void Update()
  {
    if(playerMove.hp <= 0)
    {
      playerMove.GetComponentInChildren<Animator>().SetFloat("MoveMotion",0f);
      gameLabel.SetActive(true);
      gameText.text = "Game Over";

      gameText.color = new Color32(255, 0, 0, 255);
      gState = GameState.GameOver;

      gameText.transform.GetChild(0).gameObject.SetActive(true);
    }
  }

  IEnumerator ReadyToStart()
  {
    yield return new WaitForSeconds(2f);
    gameText.text = "Go!";
    yield return new WaitForSeconds(0.5f);
    gameLabel.SetActive(false);
    gState = GameState.Run;
  }

  public void OpenOptionWindow()
  {
    gameOption.SetActive(true);
    Time.timeScale = 0f;
    gState = GameState.Pause;
  }

  public void CloseOptionWindow()
  {
    gameOption.SetActive(false);
    Time.timeScale = 1f;
    gState = GameState.Run;
  }

  public void RestartGame()
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene(1);
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
