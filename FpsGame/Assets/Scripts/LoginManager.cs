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
      notify.text = "���̵� ������ �Ϸ�ƽ��ϴ�.";
    }
    else
    {
      notify.text = "�̹� �����ϴ� ���̵��Դϴ�.";
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
      notify.text = "�Է��� ���̵� �Ǵ� �н����尡 ��ġ���� �ʽ��ϴ�.";
    }
  }

  public bool CheckIdOrPasswd(string id, string pwd)
  {
    if (id == "" || pwd == "")
    {
      notify.text = "���̵� �Ǵ� �н����带 �Է��ϼ���.";
      return false;
    }
    else
    {
      return true;
    }
  }
}
