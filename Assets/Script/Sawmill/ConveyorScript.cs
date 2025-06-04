using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    [Header("Conveyor Properties")]
    public float conveyorSpeed;
    private Vector3 direction;
    public List<GameObject> onBelt = new List<GameObject>();

    public enum directionList
    {
        left, right, front, back
    }

    [Header("Conveyor Direction")]
    public directionList conveyorDirection;

    private void Update()
    {
        ChangeDirection();

        // Iterasi mundur agar bisa hapus item tanpa konflik indeks
        for (int i = onBelt.Count - 1; i >= 0; i--)
        {
            if (onBelt[i] == null)
            {
                onBelt.RemoveAt(i);
                continue;
            }

            Rigidbody rb = onBelt[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = conveyorSpeed * direction * Time.deltaTime;
            }
        }
    }

    private void ChangeDirection()
    {
        switch (conveyorDirection)
        {
            case directionList.left:
                direction = new Vector3(0, 0, 1);
                break;
            case directionList.right:
                direction = new Vector3(0, 0, -1);
                break;
            case directionList.front:
                direction = new Vector3(1, 0, 0);
                break;
            case directionList.back:
                direction = new Vector3(-1, 0, 0);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!onBelt.Contains(collision.gameObject))
        {
            onBelt.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (onBelt.Contains(collision.gameObject))
        {
            onBelt.Remove(collision.gameObject);
        }
    }
}
