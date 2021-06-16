using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    float zoomSpeed = 3f;

    float minZoom = 5f;
    float maxZoom = 15f;
    float currentZoom = 10;
    public float pitch = 2f;

    public float yawSpeed = 100f;
    private float yawInput = 0f;
    private void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        if (Input.GetKey(KeyCode.E))
        {
            yawInput -= yawSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            yawInput += yawSpeed * Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        transform.position = target.position + offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, yawInput);
    }
    /*[SerializeField] GameObject player;
    [SerializeField] float distanceY = 10;
    [SerializeField] float distanceZ = 15;

    public float speed = 5;
    public float radius = 2;
    public float angle = 0;

    public float rotationAngle = 0;

    Vector3 v3;
    private void Start()
    {
        v3 = player.transform.position;
        v3.y += distanceY;
    }
    private void LateUpdate()
    {
        // transform.position = new Vector3(player.transform.position.x, player.transform.position.y + distanceY, player.transform.position.z - distanceZ);

        if (Input.GetKey(KeyCode.E))
        {
            v3 = player.transform.position;
            v3.y += distanceY;
            angle += speed * Time.deltaTime;
            v3.x += radius * Mathf.Cos(angle);
            v3.z += radius * Mathf.Sin(angle);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            v3 = player.transform.position;
            v3.y += distanceY;
            angle -= speed * Time.deltaTime;
            v3.x -= radius * Mathf.Cos(angle);
            v3.z -= radius * Mathf.Sin(angle);            
        }
        transform.position = v3;
        transform.LookAt(player.transform);
    }

        */
}
