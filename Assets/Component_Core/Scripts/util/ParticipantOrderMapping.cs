using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticipantOrderMapping {
    private Dictionary<ulong, ParticipantOrder> _clientToOrder;
    private Dictionary<ParticipantOrder, ulong> _orderToClient;
    private Dictionary<ParticipantOrder, ConnectionAndSpawning.SpawnType> _orderToSpawnType;
    private Dictionary<ParticipantOrder, ConnectionAndSpawning.JoinType> _orderToJoinType;

    private bool initDone = false;

    private void LogErrorNotInit() {
        Debug.LogError("Class not correctly initialized!");
    }

    public ParticipantOrderMapping() {
        _orderToClient = new Dictionary<ParticipantOrder, ulong>();
        _clientToOrder = new Dictionary<ulong, ParticipantOrder>();
        _orderToSpawnType = new Dictionary<ParticipantOrder, ConnectionAndSpawning.SpawnType>();
        _orderToJoinType = new Dictionary<ParticipantOrder, ConnectionAndSpawning.JoinType>();
        initDone = true;
    }

    public bool AddParticipant(ParticipantOrder po, ulong id,ConnectionAndSpawning.SpawnType st, ConnectionAndSpawning.JoinType jt) {
        if (!initDone) {
            LogErrorNotInit();
            return false;
        }

        if (!_orderToClient.ContainsKey(po)) {
            _orderToClient.Add(po, id);
            _clientToOrder.Add(id, po);
            _orderToSpawnType.Add(po, st);
            _orderToJoinType.Add(po, jt);

            return true;
        }

        return false;
    }

    public bool RemoveParticipant(ulong id) {
        var outVal = GetOrder(id, out var or);
        if (outVal && _orderToClient.ContainsKey(or) && _clientToOrder.ContainsKey(id)) {
            _orderToClient.Remove(or);
            _clientToOrder.Remove(id);
            _orderToSpawnType.Remove(or);
            _orderToJoinType.Remove(or);
        }

        return outVal;
    }


    public bool CheckOrder(ParticipantOrder or) {
        if (!initDone) {
            LogErrorNotInit();
            return false;
        }

        return _orderToClient.ContainsKey(or);
    }

    public bool CheckClientID(ulong id) {
        if (!initDone) {
            LogErrorNotInit();
            return false;
        }

        return _clientToOrder.ContainsKey(id);
    }

    public bool GetSpawnType(ulong clientId,out ConnectionAndSpawning.SpawnType st) {
        if (!initDone ||  GetOrder(clientId, out ParticipantOrder po)) {
            LogErrorNotInit();
            st = ConnectionAndSpawning.SpawnType.NONE;
            return false;
        }

       
        return GetSpawnType(po, out st);
    }
    public bool GetSpawnType(ParticipantOrder or,out ConnectionAndSpawning.SpawnType st) {
        if (!initDone || !_orderToSpawnType.ContainsKey(or)) {
            LogErrorNotInit();
            st = ConnectionAndSpawning.SpawnType.NONE;
            return false;
        }

        st = _orderToSpawnType[or];
        return true;
    }
    public bool GetJoinType(ulong clientId,out ConnectionAndSpawning.JoinType st) {
        if (!initDone ||  GetOrder(clientId, out ParticipantOrder po)) {
            LogErrorNotInit();
            st = ConnectionAndSpawning.JoinType.SCREEN;
            return false;
        }
        return GetJoinType(po, out st);
    }
    public bool GetJoinType(ParticipantOrder or,out ConnectionAndSpawning.JoinType st) {
        if (!initDone || !_orderToSpawnType.ContainsKey(or)) {
            LogErrorNotInit();
            st = ConnectionAndSpawning.JoinType.SCREEN;
            return false;
        }

        st = _orderToJoinType[or];
        return true;
    }

    /*
     *
     * Returns Success state if found, out
     */
    public bool GetClientID(ParticipantOrder or, out ulong outVal) {
        if (!initDone) LogErrorNotInit();

        if (CheckOrder(or)) {
            outVal = _orderToClient[or];
            return true;
        }

        outVal = 0;
        return false;
    }

    public bool GetOrder(ulong id, out ParticipantOrder outVal) {
        if (!initDone) {
            LogErrorNotInit();
            outVal = ParticipantOrder.None;
            return false;
        }

        if (CheckClientID(id)) {
            outVal = _clientToOrder[id];
            return true;
        }

        outVal = ParticipantOrder.None;
        return false;
    }

    public int GetParticipantCount() {
        if (!initDone) {
            LogErrorNotInit();
            return -1;
        }

        if (_clientToOrder.Count == _orderToClient.Count) return _clientToOrder.Count;

        Debug.LogError(
            "Our Participant Connection has become inconsistent. This is bad. Please restart and tell david!");
        return -1;
    }

    public ParticipantOrder[] GetAllConnectedParticipants() {
        if ( !initDone) LogErrorNotInit();

        return _orderToClient.Keys.ToArray();
    }

    public ulong[] GetAllConnectedClients() {
        if ( !initDone) LogErrorNotInit();

        return _clientToOrder.Keys.ToArray();
    }
}