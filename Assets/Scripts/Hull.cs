using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull : MonoBehaviour
{
    List<Line> lineList;

    public Hull(Line line){
        lineList = new List<Line>();
        lineList.Add(line);
    }

    float CalculateAngle(Line l1, Line l2) {
        return 1;
    }

    void DeleteLine() { 
    
    }

    void CreateLine(Vector2 p1, Vector2 p2) {
        lineList.Add(new Line(p1, p2));
    }
}
