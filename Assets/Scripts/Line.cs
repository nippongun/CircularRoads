using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{   
    [SerializeField]
    public Vector2 p1, p2;
    public float length;

    public Line(Vector2 p1, Vector2 p2) {
        this.p1 = p1;
        this.p2 = p2;
    }
    

    public float GetLenght() {
        return length;
    }
    
}
