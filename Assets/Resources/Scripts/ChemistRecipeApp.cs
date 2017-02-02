using System;
using UnityEngine;

namespace ChemistRecipe
{
    public static class ChemistRecipeApp
    {
        public static bool isPlaying
        {
            get
            {
                return Application.isPlaying;
            }
        }

        public static bool isEditing
        {
            get
            {
                #if UNITY_EDITOR
                if (Application.isEditor && !Application.isPlaying)
                {
                    return true;
                }
                #endif

                return false;
            }
        }

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
