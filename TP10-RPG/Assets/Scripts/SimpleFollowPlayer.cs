using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float distanceY = 10;
    [SerializeField] float distanceZ = 15;

    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + distanceY, player.transform.position.z - distanceZ);
        transform.LookAt(player.transform);
    }

}
