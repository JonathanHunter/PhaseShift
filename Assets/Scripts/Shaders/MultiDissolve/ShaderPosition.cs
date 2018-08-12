using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderPosition : MonoBehaviour
{
    public Transform[] positionArray;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < positionArray.Length; i++) {
            if (!(positionArray[i] == null))
            {
                Shader.SetGlobalVector("_Position" + i, positionArray[i].position);
            }
            else
            {
                Shader.SetGlobalVector("_Position" + i, new Vector3(9000, 9000, 9000));
            }
        }
    }
}