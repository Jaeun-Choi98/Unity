using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
  public InputField id;
  public InputField password;

  public Text notify;

  private void Start()
  {
    notify.text = "";
  }

  public void SignUp()
  {
    if(!CheckIdOrPasswd(id.text, password.text))
    {
      return;
    }
    if (!PlayerPrefs.HasKey(id.text))
    {
      PlayerPrefs.SetString(id.text, password.text);
      notify.text = "아이디 생성이 완료됐습니다.";
    }
    else
    {
      notify.text = "이미 존재하는 아이디입니다.";
    }
  }

  public void SignIn()
  {
    if (!CheckIdOrPasswd(id.text, password.text))
    {
      return;
    }
    string pass = PlayerPrefs.GetString(id.text);
    if (password.text == pass)
    {
      SceneManager.LoadScene(1);
    }
    else
    {
      notify.text = "입력한 아이디 또는 패스워드가 일치하지 않습니다.";
    }
  }

  public bool CheckIdOrPasswd(string id, string pwd)
  {
    if (id == "" || pwd == "")
    {
      notify.text = "아이디 또는 패스워드를 입력하세요.";
      return false;
    }
    else
    {
      return true;
    }
  }
}
