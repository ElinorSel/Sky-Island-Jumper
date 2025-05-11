using UnityEngine;

public class S_CameraMovement : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(1, 1, 1);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
