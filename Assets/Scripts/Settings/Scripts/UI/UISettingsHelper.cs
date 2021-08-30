using UnityEngine;

// Maybe this should be a part of SettingsManager
public struct UISettingsHelper
{

    private readonly SettingsGroupPresenter _groupPresenterPrefab;
    private readonly BaseSettingPresenter[] _presentersPrefabs;

    public UISettingsHelper(SettingsGroupPresenter customGroupPresenter, BaseSettingPresenter[] customPresenters)
    {
        _groupPresenterPrefab = customGroupPresenter;
        _presentersPrefabs = customPresenters;
    }

    public void Populate(Transform parent)
    {
        foreach (var group in SettingsManager.Groups)
        {
            GameObject.Instantiate(_groupPresenterPrefab, parent).Setup(group);
            foreach (var setting in group.Settings)
            {
                var presenterPrefab = FindPresenterPrefabFor(setting);
                GameObject.Instantiate(presenterPrefab, parent).Setup(setting);
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