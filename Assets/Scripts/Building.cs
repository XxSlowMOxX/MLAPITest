using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Building : NetworkedBehaviour
{
    public bool selected;
    private Vector2 size = Vector2.zero;
    public GameObject[] buildableUnits;

    public void setSelected(bool sel)
    {
        selected = sel;
        transform.GetChild(0).gameObject.SetActive(sel);
    }

    public Vector2 getSize()
    {
        if(size != Vector2.zero)
        {
            return size;
        }
        else //Calculates Size of Building
        { 
            return GetComponent<SpriteRenderer>().sprite.bounds.size;
        }
    }

    public bool BuildUnit(int index)
    {
        StartCoroutine(spawnTimer(index));
        return true;
    }

    IEnumerator spawnTimer(int index)
    {
        yield return new WaitForSecondsRealtime(buildableUnits[index].GetComponent<Unit>().buildTime);
        if (IsServer)
        {
            SpawnUnit(index);
        }
        else
        {
            InvokeServerRpc(SpawnUnit, index);
        }
    }

    [ServerRPC(RequireOwnership = false)]
    void SpawnUnit(int index)
    {
        Vector3 size3 = getSize();
        GameObject gO = Instantiate(buildableUnits[index], this.transform.position + size3, Quaternion.identity);
        gO.GetComponent<NetworkedObject>().Spawn(null, true);
    }
}
