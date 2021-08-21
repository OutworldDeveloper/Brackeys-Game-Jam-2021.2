using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class SettingsManager : MonoBehaviour
{

    public const string ManagerName = "Settings Manager";

    public static event Action OnSettingsChanged;
    public static List<SettingsGroup> Groups => _instance._settingsGroups;

    private static SettingsManager _instance;
    private static bool _gameStartedCallbackSended;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnBeforeSceneLoadRuntimeMethod()
    {
        if (_instance)
            return;

        var prefab = Resources.Load<SettingsManager>(ManagerName);
        _instance = Instantiate(prefab);
        DontDestroyOnLoad(_instance);
    }

    public static void SettingsChanged()
    {
        OnSettingsChanged?.Invoke();
    }

    public static void ResetSettings()
    {
        ForEachSetting(setting => setting.Reset());
    }

    public static void ForEachSetting(Action<BaseSetting> action)
    {
        Groups.ForEach(group =>
        {
            group.Settings.ForEach(setting => action.Invoke(setting));
        });
    }

    [SerializeField] private List<SettingsGroup> _settingsGroups = new List<SettingsGroup>();

    private void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }

    private void Start()
    {
        Debug.Log("Settings Manager started");

        if (_gameStartedCallbackSended)
            return;

        ForEachSetting(setting => setting.OnGameStarted());

        _gameStartedCallbackSended = true;
    }

}