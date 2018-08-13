using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Material background;

    [SerializeField]
    private float parallaxSpeed = 0.01f;

    private float zPos;

    public enum CameraBehavior { FollowPlayer, ReturnToPlayer, Wait };
    public CameraBehavior myCameraBehavior = CameraBehavior.FollowPlayer;

    void Awake()
    {
        zPos = transform.position.z;
    }
    
    public void SetFollowPlayer()
    {
        myCameraBehavior = CameraBehavior.FollowPlayer;
    }

    public void SetReturnToPlayer()
    {
        myCameraBehavior = CameraBehavior.ReturnToPlayer;
    }

    public void SetWait()
    {
        myCameraBehavior = CameraBehavior.Wait;
    }

    // Update is called once per frame
    void Update () {
        switch (myCameraBehavior)
        {
            case CameraBehavior.FollowPlayer:
                FollowPlayerUpdate();
                break;
            case CameraBehavior.ReturnToPlayer:
                ReturnToPlayerUpdate();
                break;
            case CameraBehavior.Wait:
                WaitUpdate();
                break;
        }
        OverrideZPos();
        UpdateBackgroundParallax();
    }

    void FollowPlayerUpdate()
    {
        transform.position = player.transform.position;
    }

    void ReturnToPlayerUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.5f);
    }

    void WaitUpdate()
    {
        //Do nothing
    }

    void OverrideZPos()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
    }

    void UpdateBackgroundParallax()
    {
        background.SetTextureOffset("_MainTex", new Vector2(transform.position.x * parallaxSpeed, transform.position.y * parallaxSpeed * -1));
    }
}
