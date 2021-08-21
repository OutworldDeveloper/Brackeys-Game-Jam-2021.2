using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSettingPresenter : MonoBehaviour
{
    public abstract Type TargetType { get; }
    public abstract void Setup(BaseSetting setting);

}