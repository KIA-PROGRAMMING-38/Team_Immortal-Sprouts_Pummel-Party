using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Start()
    {
        Managers.UI.PopUI<TitleUI>(parent : Managers.UI.RootTransform);
    }

}
