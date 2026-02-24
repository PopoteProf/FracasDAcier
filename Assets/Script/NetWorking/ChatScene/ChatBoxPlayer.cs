using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ChatBoxPlayer : NetworkBehaviour
{
    [SerializeField] private bool _SendToAll;
    [SerializeField] private ulong _playerTarget ;
    [SerializeField] private  string _textToSend;
    
    public override void OnNetworkSpawn() {
        Debug.Log("OnNetworkSpawn with id" + this.OwnerClientId);
        if (IsHost || IsServer) {
            PopoteChatSceneNetManagerAdd.Instance.SumbitPlayerGameObject(OwnerClientId, this);
        }
    }

    [ContextMenu("Send")]
    public void SendMessage()
    {
        DisplayMessageRPC(_playerTarget, _textToSend);
    }

    [Rpc(SendTo.Everyone)]
    public void DisplayMessageRPC(ulong playerTarget, string textToSend) {
        if (NetworkManager.LocalClientId == _playerTarget) {
            Debug.Log(" The Message is :"+textToSend );
        }
    }
    
    [Rpc(SendTo.Everyone)]
    public void UpdatePlayerNameListRPC(stringContainer[] names) {
        if (!IsOwner) return;
        ChatBoxManager.Instance.SetupDropdownTargets(names);
    }
    

    public void OnRemoveFromServerRegister() { }

    [Rpc(SendTo.ClientsAndHost)]
    public void SetUpRPC(string id) {
        if (!IsOwner) return;
        Debug.Log("SetUpRPC with id:"+ id);
        ChatBoxManager.Instance.gameObject.SetActive(true);
        ChatBoxManager.Instance.SetCurrentUser(id);
        ChatBoxManager.Instance.OnMessageSend += InstanceOnOnMessageSendRPC;
    }

    
    private void InstanceOnOnMessageSendRPC(object sender, ChatBoxMessage message)
    {
        SendMessageToServerRPC(message);
    }
    [Rpc(SendTo.Server)]
    private void SendMessageToServerRPC(ChatBoxMessage message)
    {
        PopoteChatSceneNetManagerAdd.Instance.DistributeMessage(message);
    }

    [Rpc(SendTo.Owner)]
    public void ShowMessageRPC(ChatBoxMessage message) {
        ChatBoxManager.Instance.DisplayMessage(message);
    }
}