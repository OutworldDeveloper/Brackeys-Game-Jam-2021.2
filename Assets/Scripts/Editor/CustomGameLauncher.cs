using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace EditorUtilities
{

    // This class is needed so we don't have to manually open the first scene.
    // It also saves and restores scene setup when we leave the play mode.
    [InitializeOnLoad]
    public static class CustomGameLauncher
    {

        private const string MainKey = "ShouldRestoreScenes";
        private const string AmountKey = "ScenesAmount";
        private const string SceneKey = "Scene";

        static CustomGameLauncher()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        [MenuItem("Play/Launch")]
        public static void LaunchGame()
        {
            var currentSceneSetup = EditorSceneManager.GetSceneManagerSetup();

            if (HasUnsavedScenes())
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            EditorPrefs.SetBool(MainKey, true);
            EditorPrefs.SetInt(AmountKey, currentSceneSetup.Length);

            for (int i = 0; i < currentSceneSetup.Length; i++)
            {
                var sceneSetup = currentSceneSetup[i];
                EditorPrefs.SetString($"{SceneKey}{i}", sceneSetup.path);
            }

            var scene = EditorBuildSettings.scenes[0];
            EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
            EditorApplication.EnterPlaymode();
        }

        private static bool HasUnsavedScenes()
        {
            bool hasUnsavedScenes = false;
            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);
                if (scene.isDirty)
                {
                    hasUnsavedScenes = true;
                }
            }
            return hasUnsavedScenes;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            // A hacky way of "overriding" the Play button functionality
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                LaunchGame();
                return;
            }

            // Restoring scenes setup
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                if (EditorPrefs.GetBool(MainKey) == false)
                    return;

                var amount = EditorPrefs.GetInt(AmountKey);
                var setup = new SceneSetup[amount];

                for (int i = 0; i < amount; i++)
                {
                    setup[i] = new SceneSetup()
                    {
                        path = EditorPrefs.GetString($"{SceneKey}{i}"),
                        isActive = true,
                        isLoaded = true,
                        isSubScene = false,
                    };
                }

                EditorSceneManager.RestoreSceneManagerSetup(setup);

                // We don't want our changes to be persistent between editor sessions
                EditorPrefs.DeleteKey(MainKey);
                EditorPrefs.DeleteKey(AmountKey);
                EditorPrefs.DeleteKey(SceneKey);
            }
        }
    }

}