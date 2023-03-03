// 사용한 함수 참고 : https://doc-api.photonengine.com/en/fusion/current/interface_fusion_1_1_i_network_runner_callbacks.html

using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private bool _mouseButton0;
    private bool _mouseButton1;

    private void Update()
    {
        _mouseButton0 = _mouseButton0 || Input.GetMouseButton(0);
        _mouseButton1 = _mouseButton1 || Input.GetMouseButton(1);
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    // NetworkRunner가 연결됨
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    // NetworkRunner의 연결이 끊김
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    // NetworkRunner의 사용자 입력을 인식함
    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        if (_mouseButton0)
            data.buttons |= NetworkInputData.MOUSEBUTTON1;
        _mouseButton0 = false;

        if (_mouseButton1)
            data.buttons |= NetworkInputData.MOUSEBUTTON2;
        _mouseButton1 = false;

        input.Set(data);
    }

    // NetworkRunner의 사용자 입력을 잃음
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { Debug.Log("OnInputMissing()" ); }
    
    // NetworkRunner가 셧다운됨
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { Debug.Log("OnShutdown()"); }
    
    // NetworkRunner가 서버나 호스트에 연결성공
    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("OnConnectedToServer()"); }

    // NetworkRunner가 서버나 호스트에서 연결을 끊음
    public void OnDisconnectedFromServer(NetworkRunner runner) { Debug.Log("OnDisconnectedFromServer()"); }

    // NetworkRunner가 리모트 클라이언트로부터 요청을 받음
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Debug.Log("OnConnectRequest()"); }

    // NetworkRunner가 서버나 호스트에 연결실패
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { Debug.Log("OnConnectFailed()"); }

    // 리모트 클라이언트로부터 수동으로 전달된 메시지가 수신됨
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { Debug.Log("OnUserSimulationMessage()"); }

    // 세션이 업데이트됨
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { Debug.Log("OnSessionListUpdated()"); }

    // 인증 절차가 인증 서버에서 응답을 반환
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { Debug.Log("OnCustomAuthenticationResponse()"); }

    // 호스트 마이그레이션 프로세스가 시작
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { Debug.Log("OnHostMigration()"); }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { Debug.Log("OnReliableDataReceived()"); }

    public void OnSceneLoadDone(NetworkRunner runner) { Debug.Log("OnSceneLoadDone()"); }

    public void OnSceneLoadStart(NetworkRunner runner) { Debug.Log("OnSceneLoadStart()"); }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}
