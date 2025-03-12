using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationSetup : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SetLanguage());
    }

    IEnumerator SetLanguage()
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
    }
}
