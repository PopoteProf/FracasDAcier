using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PopoteChatSceneNetManagerAdd : MonoBehaviour
{
    public static PopoteChatSceneNetManagerAdd Instance;
    
    
    [SerializeField] private NetworkManager _networkManager;
    
    private Dictionary<ulong, string> _playersNames = new Dictionary<ulong, string>();
    private Dictionary<ulong, ChatBoxPlayer> _players = new Dictionary<ulong, ChatBoxPlayer>();
    
    void Start() {
        Instance = this;
        _networkManager.OnServerStarted += NetworkManagerOnOnServerStarted;
        _networkManager.OnClientConnectedCallback+= NetworkManagerOnOnClientConnectedCallback;
        _networkManager.OnClientDisconnectCallback += NetworkManagerOnOnClientDisconnectCallback;
        _networkManager.OnServerStopped += NetworkManagerOnOnServerStopped;
    }

    
    public void SumbitPlayerGameObject(ulong playerId, ChatBoxPlayer playerGameObject) {
        if (_players.ContainsKey(playerId)) {
            Debug.LogWarning("Player GameObject already register at id: " + playerId);
            return;
        }
        _players.Add(playerId, playerGameObject);
        ChangePlayerNameList(playerId, playerId.ToString());
        playerGameObject.SetUpRPC(playerId.ToString());
        Debug.Log(" PlayerGameObject Register at id:"+ playerId);
    }

    public void ChangePlayerNameList(ulong id, string name, bool toAdd = true)
    {
        if (toAdd)
        {
            if (_playersNames.ContainsValue(name)) return;
            _playersNames.Add(id, name);
        }
        else
        {
            if (!_playersNames.ContainsValue(name)) return;
            _playersNames.Remove(id);
        }

        List<stringContainer> names = new List<stringContainer>();

        foreach (var item in _playersNames.Values) {
            names.Add(new stringContainer(item));
        }
        foreach (var playerGo in _players.Values) {
            playerGo.UpdatePlayerNameListRPC(names.ToArray());
        }
    }
    
    

    private void NetworkManagerOnOnServerStopped(bool obj) {
        Debug.Log("Server Stopped");
    }

    private void NetworkManagerOnOnClientDisconnectCallback(ulong obj) {
        if (_players.ContainsKey(obj)) {
            _players[obj].OnRemoveFromServerRegister();
            _playersNames.Remove(obj);
            _players.Remove(obj);
        }
        Debug.Log(" ClientDisconnected at id" + obj);
    }

    private void NetworkManagerOnOnClientConnectedCallback(ulong obj) {
        Debug.Log(" ClientConnected at id" + obj);
    }

    private void NetworkManagerOnOnServerStarted() {
        Debug.Log("Server started");
    }

    public void DistributeMessage(ChatBoxMessage message) {
        foreach (var player in _players.Values) {
            player.ShowMessageRPC(message);
        }
    }
    
    
}