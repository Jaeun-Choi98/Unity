using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

  public float speed = 8;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");

    Vector3 dir = Vector3.right * h + Vector3.up * v;
    //transform.Translate(dir * speed * Time.deltaTime); <- 너무 종속적
    transform.position += dir * speed * Time.deltaTime;
  }
}