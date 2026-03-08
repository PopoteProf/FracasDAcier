using Unity.Netcode;
using UnityEngine;

public class DeathMatchGameManager : NetworkBehaviour
{
    public static DeathMatchGameManager Instance;
    [SerializeField]private Transform[] _spawnPoints;
    [SerializeField] private NetPhysicControllerFoot _prfMeck;

    private void Awake() {
        Instance = this;
    }

    public void SpawnNewMeck(DeathMatchPlayerGameObject player) {
        if (IsHost || IsServer) {
            Transform spawnPoint = GetSpawnPoint();
            NetPhysicControllerFoot meck = Instantiate(_prfMeck, spawnPoint.position, spawnPoint.rotation);
            meck.GetComponent<NetworkObject>().SpawnWithOwnership(player.OwnerClientId);
        }
    }

    private Transform GetSpawnPoint() {
        return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
    }
    
}