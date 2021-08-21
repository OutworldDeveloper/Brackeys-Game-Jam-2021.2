using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SettingsUI : MonoBehaviour
{

    [SerializeField] private SettingsGroupPresenter _groupPresenterPrefab;
    [SerializeField] private BaseSettingPresenter[] _presentersPrefabs;
    [SerializeField] private Transform _parent;

    private void Start()
    {
        foreach (var group in SettingsManager.Groups)
        {
            Instantiate(_groupPresenterPrefab, _parent).Setup(group);
            foreach (var setting in group.Settings)
            {
                var presenterPrefab = FindPresenterPrefabFor(setting);
                Instantiate(presenterPrefab, _parent).Setup(setting);
            }
        }
    }

    private BaseSettingPresenter FindPresenterPrefabFor(BaseSetting setting)
    {
        foreach (var presenterPrefab in _presentersPrefabs)
        {
            if (presenterPrefab.TargetType == setting.GetType())
            {
                return presenterPrefab;
            }
        }

        foreach (var presenterPrefab in _presentersPrefabs)
        {
            if (setting.GetType().IsSubclassOf(presenterPrefab.TargetType))
            {
                return presenterPrefab;
            }
        }

        return null;
    }

}