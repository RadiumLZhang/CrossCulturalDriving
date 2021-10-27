using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class NetworkCommandLine : MonoBehaviour {
    private NetworkManager netManager;
    private ConnectionAndSpawing connectionAndSpawing;


    public bool EditorRunAsServer;

    void Start() {
        netManager = NetworkManager.Singleton;
        connectionAndSpawing = ConnectionAndSpawing.Singleton;
        SetupAndStart();
    }

    private void Update() { }

    private void SetupAndStart() {
        if (Application.isEditor) {
            if (EditorRunAsServer) {
                BroadCastParticipantOrder(ParticipantOrder.A);
                connectionAndSpawing.StartAsHost();
            }
            else {
                BroadCastParticipantOrder(ParticipantOrder.A);
                connectionAndSpawing.StartAsClient();
            }

            SetlanguagePrivate("English");
            // netManager.StartHost();
            return;
        }

        Screen.SetResolution(1280, 720, false);
        var args = GetCommandlineArgs();


        if (args.TryGetValue("-po", out string participantOrderString)) {
            switch (participantOrderString) {
                case "A":
                case "a":
                    BroadCastParticipantOrder(ParticipantOrder.A);
                    break;
                case "B":
                case "b":
                    BroadCastParticipantOrder(ParticipantOrder.B);
                    break;
                case "C":
                case "c":
                    BroadCastParticipantOrder(ParticipantOrder.C);
                    break;
                case "D":
                case "d":
                    BroadCastParticipantOrder(ParticipantOrder.D);
                    break;
                case "E":
                case "e":
                    BroadCastParticipantOrder(ParticipantOrder.E);
                    break;
                case "F":
                case "f":
                    BroadCastParticipantOrder(ParticipantOrder.F);
                    break;
                default:
                    BroadCastParticipantOrder(ParticipantOrder.None);
                    break;
            }
        }

        if (args.TryGetValue("-mlapi", out string mlapiValue)) {
            switch (mlapiValue) {
                case "server":
                    netManager.StartServer();
                    break;
                case "host":
                    connectionAndSpawing.StartAsHost();
                    break;
                case "client":
                    connectionAndSpawing.StartAsClient();
                    break;
            }
        }

        if (args.TryGetValue("-language", out string lang)) { SetlanguagePrivate(lang); }
    }

    private void BroadCastParticipantOrder(ParticipantOrder or) {
        connectionAndSpawing.SetParticipantOrder(or);
        FindObjectOfType<LocalVRPlayer>().SetParticipantOrder(or);
    }

    private void SetlanguagePrivate(LanguageSelect lang) { FindObjectOfType<LocalVRPlayer>().Setlanguage(lang); }

    private Dictionary<string, string> GetCommandlineArgs() {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i) {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-")) {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }

        return argDictionary;
    }
}