using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject obj;
    public Transform[] pos;

    private int randomNum;

    private void Start()
    {
        ObjectSpawn();
    }

    private void ObjectSpawn()
    {
        randomNum = Random.Range(0, pos.Length);

        if (obj != null) {
            var item = Instantiate(obj, pos[randomNum].position, pos[randomNum].rotation);
            item.transform.parent = pos[randomNum].transform;
        }
    }
}
