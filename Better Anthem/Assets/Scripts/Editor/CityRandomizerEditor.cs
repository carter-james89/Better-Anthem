using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CityRandomizer))]
public class CityRandomizerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        CityRandomizer city = (CityRandomizer)target;

        if (GUILayout.Button("Build City"))
        {
            var buildings = city.GetComponentsInChildren<Transform>();
            foreach (var building in buildings)
            {
                if (building.name.Contains("Cube"))
                {
                    var tempPos = building.localPosition;
                    tempPos.y = 0;
                    building.localPosition = tempPos;
                    building.transform.localPosition -= new Vector3(0,Random.Range(0,380),0);
                }
            }
        }
    }
}
