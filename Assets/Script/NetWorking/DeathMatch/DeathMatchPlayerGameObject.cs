using Unity.Netcode;

public class DeathMatchPlayerGameObject : NetworkBehaviour {

    private void Start() {
        SetUpMeck();
    }

    private void SetUpMeck() {
        DeathMatchGameManager.Instance.SpawnNewMeck(this);
    }
}