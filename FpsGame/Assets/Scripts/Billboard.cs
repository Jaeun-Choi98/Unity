using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
  public GameObject target;

  private void Update()
  {
    transform.forward = target.transform.forward;
  }
}
