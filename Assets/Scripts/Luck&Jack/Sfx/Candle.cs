using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{

    [SerializeField] private GameObject glowObject;

    public void EnableGlowing(bool b) => glowObject.SetActive(b);
  

}