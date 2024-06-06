using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
  public Material bgMaterial;

  public float scrollSpeed = 0.2f;

  Vector2 dir;
  // Start is called before the first frame update
  void Start()
  {
    dir = Vector2.up; 
  }

  // Update is called once per frame
  void Update()
  {
    bgMaterial.mainTextureOffset += dir * scrollSpeed * Time.deltaTime;
  }
}
