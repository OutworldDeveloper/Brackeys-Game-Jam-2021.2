using System;
using UnityEngine;
using Zenject;

public class JackCustomizaton : MonoBehaviour
{

    public event Action<Hat> HatEquiped;
    public Hat EquipedHat { get; private set; }
    public Hat DefaultHat => _defaultHat;

    [Inject] private DataContainer _saveData;
    [Inject] private SavingSystem _savingSystem;
    [Inject] private UnlockablesManager _unlockablesManager;

    [SerializeField] private Hat _defaultHat;

    private void Start()
    {
        if (_saveData.TryGetData("EquipedHat", out string data))
        {
            var hat = _unlockablesManager.GetHatByName(data);
            if (hat)
            {
                Equip(hat);
                return;
            }
        }

        Equip(_defaultHat);
    }

    public void Equip(Hat hat)
    {
        EquipedHat = hat;
        HatEquiped?.Invoke(hat);
        _saveData.SetData("EquipedHat", EquipedHat.name);
        _savingSystem.SaveOverride(_saveData);
    }

}