using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float width = 30;
    public float height = 20;
    public float depth = 0;

   
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void ShiftPositionOnBounds(Transform t)
    {
        Vector3 pos = t.position;
        float w = width / 2;
        float h = height / 2;
        float d = depth / 2;

        if (pos.y > h) pos.y= -h;
        if (pos.y < -h) pos.y = h;
        if (pos.x > w) pos.x = -w;
        if (pos.x < -w) pos.x = w;
        if (pos.z > d) pos.z = -d;
        if (pos.z < -d) pos.z = d;

        t.position = pos;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector3(0, 1.706192f,0), Vector3.right * width + Vector3.up * height + Vector3.forward * depth);

    }

}
