using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] private GameObject _townHall = null;
    [SerializeField] private GameObject _worker = null;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        GameObject townHallInstance = Instantiate(_townHall, conn.identity.transform.position, Quaternion.identity);
        GameObject workerInstance = Instantiate(_worker, conn.identity.transform.position - Vector3.one, Quaternion.identity);

        NetworkServer.Spawn(townHallInstance, conn);
        NetworkServer.Spawn(workerInstance, conn);
    }
}
