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
        DrawDefaultInspector();
        if (GUILayout.Button("Build City"))
        {
           
            var buildings = city.GetComponentsInChildren<Transform>();
            foreach (var building in buildings)
            {
                if (building.name.Contains("Cube"))
                {
                    var tempPos = building.localPosition;
                    tempPos.y = 610.0485f;
                    building.localPosition = tempPos;


                    var random = Random.Range(0, 420);
                    building.localPosition -= new Vector3(0, random, 0);


                    var meshRender = building.gameObject.GetComponent<MeshRenderer>();
                    var material = meshRender.sharedMaterials;
                     material[0] = city.buildingMat;

                    var newMat = new Material(Shader.Find("Standard"));
                    newMat.SetFloat("_Glossiness", 0.3f);
                    newMat.SetFloat("_Metallic", 0.43f);
                    // Debug.Log(material[0]);
                    newMat.color = Random.ColorHSV(0f, 1f, .5f, 1f, 0f, 1f);
                    material[0] = newMat;
                   // material[0].SetFloat("_Glossiness", 0.43f);
                    //"_Glossiness"
                   // material[1] = city.glassMat;
                    //material[2] = city.glowMat;
                    meshRender.sharedMaterials = material;

                    // material = meshRender.sharedMaterials;
                    //material[0].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

                }
            }
        }
    }
}
