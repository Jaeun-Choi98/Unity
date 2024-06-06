using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

  // 싱글톤 객체
  public static ScoreManager Instance = null;

  public GameObject currentScoreUI;

  private int currentScore;

  public GameObject bestScoreUI;

  private int bestScore;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    bestScore = PlayerPrefs.GetInt("Best Score");
    bestScoreUI.GetComponent<Text>().text = "최고 점수 : " + bestScore;
  }

  // Update is called once per frame
  void Update()
  {

  }


  public int Score
  {
    get { return currentScore; }
    set
    {
      currentScore = value;
      currentScoreUI.GetComponent<Text>().text = "현재 점수 : " + currentScore;

      if (currentScore > bestScore)
      {
        bestScore = currentScore;
        bestScoreUI.GetComponent<Text>().text = "최고 점수 : " + bestScore;
        // 최고 점수 외부 저장
        PlayerPrefs.SetInt("Best Score", bestScore);
      }
    }
  }
}
