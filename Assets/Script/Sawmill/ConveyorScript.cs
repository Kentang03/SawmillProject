using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    [Header("Conveyor Properties")]
    public float conveyorSpeed;
    private Vector3 direction;
    public List<GameObject> onBelt;

    public enum directionList
    {
        left,right,front,back
    }

    [Header("ConveyorDirection")]
    public directionList conveyorDirection;

    private void Update()
    {
        ChangeDirection();

        for (int i = 0; i < onBelt.Count; i++)
        {
            onBelt[i].GetComponent<Rigidbody>().velocity = conveyorSpeed * direction * Time.deltaTime;
        }
    }

    private void ChangeDirection()
    {
        switch(conveyorDirection)
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
        onBelt.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        onBelt.Remove(collision.gameObject);
    }
}
