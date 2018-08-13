using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBillboard : MonoBehaviour {


    float speed = 1f;
    public Renderer rend;

    [SerializeField]
    protected AudioSource explodeSound;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        rend.material.shader = Shader.Find("Custom/TimedExplosion");
    }

    void Update()
    {
        float pos = Mathf.PingPong(Time.time * speed, 1.1F);
        rend.material.SetFloat("_Progression", pos);

        if(pos >= 1)
        {
            Destroy(this.gameObject);
        }
    }
}
