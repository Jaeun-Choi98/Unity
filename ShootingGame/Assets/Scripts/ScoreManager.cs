using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

  // �̱��� ��ü
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
    bestScoreUI.GetComponent<Text>().text = "�ְ� ���� : " + bestScore;
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
      currentScoreUI.GetComponent<Text>().text = "���� ���� : " + currentScore;

      if (currentScore > bestScore)
      {
        bestScore = currentScore;
        bestScoreUI.GetComponent<Text>().text = "�ְ� ���� : " + bestScore;
        // �ְ� ���� �ܺ� ����
        PlayerPrefs.SetInt("Best Score", bestScore);
      }
    }
  }
}
