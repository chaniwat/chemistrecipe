using System;
using UnityEngine;

namespace chemistrecipe
{
    public static class ChemistRecipeApp
    {
        public static void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
