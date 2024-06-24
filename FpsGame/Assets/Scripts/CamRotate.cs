using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{

  public float rotSpeed = 200f;

  float mx = 0f;
  float my = 0f;

  void Start()
  {
  }

  void Update()
  {
    // 게임 상태가 '게임 중' 상태일 때만 조작
    if (GameManager.gm.gState != GameManager.GameState.Run)
    {
      return;
    }

    float mouse_X = Input.GetAxis("Mouse X");
    float mouse_Y = Input.GetAxis("Mouse Y");

    mx += mouse_X * rotSpeed * Time.deltaTime;
    my += mouse_Y * rotSpeed * Time.deltaTime;
    my = Mathf.Clamp(my, -90f, 90f);
    transform.eulerAngles = new Vector3(-my, mx, 0);

    /*
    Vector3 dir = new Vector3(-mouse_Y, mouse_X,0);
    transform.eulerAngles += dir * rotSpeed * Time.deltaTime;
    /*
    // x축 회전(상하 회전) -90도 ~ 90도 사이로 제한
    Vector3 rot = transform.eulerAngles;
    rot.x = Mathf.Clamp(rot.x, -90f, 90f);
    transform.eulerAngles = rot;*/
  }
}
