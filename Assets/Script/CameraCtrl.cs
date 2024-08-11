using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    public GameObject player;
    public enum Status
    {
        None = 0,
        Idle, Move
    }
    private Status status;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y + 3.7775f,
            transform.position.z);
        status = Status.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y + 3.7775f,
            transform.position.z);
    }

    void setPlayer(GameObject p)
    {
        player = p;
    }
}
