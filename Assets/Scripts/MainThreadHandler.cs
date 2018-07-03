using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadHandler : MonoBehaviour {
    static MainThreadHandler _Instance;
    static MainThreadHandler Instance {
        get {
            if (_Instance == null) {
                _Instance = new GameObject("MainThreadHandler").AddComponent<MainThreadHandler>();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }
    List<Action> actions = new List<Action>();
    public static void Init() {
        Instance.actions.Clear();
        Invoke(() => { });
    }
    public static void Invoke(Action callback) {
        lock(Instance.actions) {
            Instance.actions.Add(callback);
        }
    }

    private void Update() {
        List<Action> invokes = new List<Action>();
        lock (Instance.actions) {
            invokes.AddRange(actions.ToArray());
            actions.Clear();
        }

        foreach (var item in invokes) {
            try {
                item.Invoke();
            } catch (Exception e) {
                Debug.LogError(e);
            }
        }
    }
}
