using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using netco;
using System.Net;

public class LoginController : MonoBehaviour {
    public Text networkState;
    public Text LogText;
    public InputField ipInputField;
    string networkStateText;
    string LogTextContent = "";
    string ip = "172.16.0.102";


    TCPClient client;

    private void Awake() {
        MainThreadHandler.Init();
    }

    private void Start() {
        InitClient();
    }

    void InitClient() {
        if (client != null) {
            // close
        }
        client = new TCPClient();
        client.OnNetworkStateChanged += OnNetworkStateChanged;

        IPAddress ipAddress = IPAddress.Parse(ip);
        Debug.Log(ip + " => " + ipAddress);
        client.Connect(ipAddress, 9000, () => {
            AppendLog("连接成功");
            MainThreadHandler.Invoke(() => {
                StartLogin();
            });
        });
    }

    void OnNetworkStateChanged(NetworkState state) {
        networkStateText = "网络状态改变" + state;
        if (state == NetworkState.Error || state == NetworkState.Closed) {
            client = null;
        }
        MainThreadHandler.Invoke(() => {
            UpdateUI();
        });
    }

    private void UpdateUI() {
        networkState.text = networkStateText;
        LogText.text = LogTextContent;
    }

    public void StartLogin() {
        LogTextContent = "";

        if (client == null) {
            return;
        }
        client.Request("Example.Login", "hello".StringToBytes(), (r) => {
            var msg = r.BytesToStringUTF8();
            Debug.Log("收到: " + msg);
            client.Request("Example.Test", r, (rr) => { });

            MainThreadHandler.Invoke(() => {
                AppendLog(msg);
            });
        });
    }

    void AppendLog(object msg) {
        LogTextContent += msg != null ? msg.ToString() : "" + "\n";
        MainThreadHandler.Invoke(() => {
            UpdateUI();
        });
    }

    public void ChangeIP() {
        ip = ipInputField.text;
        InitClient();
    }
}
