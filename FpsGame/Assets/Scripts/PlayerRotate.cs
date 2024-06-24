using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
  public float rotSpeed = 200f;

  float mx = 0f;
  //float my = 0f;
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
    //float mouse_Y = Input.GetAxis("Mouse Y");

    mx += mouse_X * rotSpeed * Time.deltaTime;
    //my += mouse_Y * rotSpeed * Time.deltaTime;
    //my = Mathf.Clamp(my, -90f, 90f);
    transform.eulerAngles = new Vector3(0, mx, 0);
  }
}
