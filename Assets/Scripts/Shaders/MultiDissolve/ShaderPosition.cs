using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderPosition : MonoBehaviour
{
    public Transform[] positionArray;
    public bool debug = false;

    void Awake()
    {
        if (!debug)
        {
            positionArray = PhaseShift.Bullets.BulletPool.Instance.GetSpaceBullets();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < positionArray.Length; i++) {
            if (!(positionArray[i] == null))
            {
                Shader.SetGlobalVector("_Position" + i, positionArray[i].position);
                Shader.SetGlobalFloat("_Scale" + i, positionArray[i].localScale.x);
                print(positionArray[i].localScale.x);
            }
            else
            {
                Shader.SetGlobalVector("_Position" + i, new Vector3(9000, 9000, 9000));
                Shader.SetGlobalFloat("_Scale" + i, 1.0f);
            }
        }
    }
}