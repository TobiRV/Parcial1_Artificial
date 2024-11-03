using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IFood
{
    public Vector3 Position => transform.position;

    public void Consume()
    {
        Debug.Log("se comio");
        Destroy(gameObject);
    }
}
