using UnityEngine;
using Zenject;

[RequireComponent(typeof(JackShiner))]
public class JackCustomizationApplier : MonoBehaviour
{

    [Inject] private JackCustomizaton _playerProfile;
    [SerializeField] private Transform _headBone;

    private JackShiner _jackShiner;
    private HatVisuals _currentHat;

    private void Awake()
    {
        _jackShiner = GetComponent<JackShiner>();
    }

    private void OnEnable()
    {
        _playerProfile.HatEquiped += OnHatEquiped;
        _jackShiner.LightStateChanged += OnLightStateChanged;
    }

    private void OnDisable()
    {
        _playerProfile.HatEquiped -= OnHatEquiped;
        _jackShiner.LightStateChanged -= OnLightStateChanged;
    }

    private void Start()
    {
        var hat = _playerProfile.EquipedHat;
        OnHatEquiped(hat);
    }

    private void OnHatEquiped(Hat hat)
    {
        if (_currentHat)
        {
            Destroy(_currentHat.gameObject);
        }
        _currentHat = Instantiate(hat.Prefab, _headBone);
        // TODO: Check if should be activated instantly
    }

    private void OnLightStateChanged(bool state)
    {
        if (state)
        {
            _currentHat.StartShining();
        }
        else
        {
            _currentHat.StopShining();
        }
    }

}