using System;
using UnityEngine;
using KasaiFudo.ScreenOrientation;

public class OrientationBootstrap : MonoBehaviour
{
    private void Awake()
    {
        OrientationUpdateService.Initialize();

        AdditionDataKey.Key = "9:16";
    }
}
