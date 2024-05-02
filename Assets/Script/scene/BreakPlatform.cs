using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlatform : MonoBehaviour
{
    public enum Status
    {
        None = 0,
        Intact, Break, Change
    }

    public Status status;
    public Status nextStatus;
    private float statusTime;
    private BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        status = Status.Intact;
        nextStatus = Status.None;
        statusTime = 0f;
        _collider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        switch (nextStatus)
        {
            case Status.Intact:
                status = nextStatus;
                nextStatus = Status.None;
                statusTime = 0f;
                _collider.enabled = true;
                break;
            case Status.Break:
                status = nextStatus;
                nextStatus = Status.None;
                statusTime = 0f;
                _collider.enabled = false;
                break;
            case Status.Change:
                status = nextStatus;
                nextStatus = Status.None;
                statusTime = 0f;
                break;
            default:
                break;
        }

        switch (status)
        {
            case Status.Break:
                statusTime += Time.fixedDeltaTime;
                if (statusTime > 5f)
                {
                    nextStatus = Status.Intact;
                }
                break;
            case Status.Change:
                statusTime += Time.fixedDeltaTime;
                if (statusTime > 3f)
                {
                    nextStatus = Status.Break;
                }
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(status == Status.Intact)
        {
            nextStatus = Status.Change;
        }
    }
}
