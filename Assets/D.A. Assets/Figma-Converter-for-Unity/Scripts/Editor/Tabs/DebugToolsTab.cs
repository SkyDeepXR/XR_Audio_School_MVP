using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DA_Assets.FCU
{
    internal class DebugToolsTab : ScriptableObjectBinder<FcuSettingsWindow, FigmaConverterUnity>
    {

        private Editor fcuConfigEditor;

        public override void Init()
        {
            fcuConfigEditor = Editor.CreateEditor(FcuConfig.Instance);
        }

        public void Draw()
        {
            gui.SectionHeader(FcuLocKey.label_debug_tools.Localize());
            gui.Space15();

            monoBeh.Settings.DebugSettings.DebugMode = gui.Toggle(
                new GUIContent(FcuLocKey.label_debug_mode.Localize(), FcuLocKey.tooltip_debug_mode.Localize()),
                monoBeh.Settings.DebugSettings.DebugMode);

            if (monoBeh.Settings.DebugSettings.DebugMode)
            {
                monoBeh.Settings.DebugSettings.LogDefault = gui.Toggle(new GUIContent(FcuLocKey.label_log_default.Localize()),
                    monoBeh.Settings.DebugSettings.LogDefault);

                monoBeh.Settings.DebugSettings.LogSetTag = gui.Toggle(new GUIContent(FcuLocKey.label_log_set_tag.Localize()),
                    monoBeh.Settings.DebugSettings.LogSetTag);

                monoBeh.Settings.DebugSettings.LogIsDownloadable = gui.Toggle(new GUIContent(FcuLocKey.label_log_downloadable.Localize()),
                    monoBeh.Settings.DebugSettings.LogIsDownloadable);

                monoBeh.Settings.DebugSettings.LogTransform = gui.Toggle(new GUIContent(FcuLocKey.label_log_transform.Localize()),
                    monoBeh.Settings.DebugSettings.LogTransform);

                monoBeh.Settings.DebugSettings.LogGameObjectDrawer = gui.Toggle(new GUIContent(FcuLocKey.label_log_go_drawer.Localize()),
                    monoBeh.Settings.DebugSettings.LogGameObjectDrawer);

                monoBeh.Settings.DebugSettings.LogComponentDrawer = gui.Toggle(new GUIContent(FcuLocKey.label_log_component_drawer.Localize()),
                    monoBeh.Settings.DebugSettings.LogComponentDrawer);

                monoBeh.Settings.DebugSettings.LogHashGenerator = gui.Toggle(new GUIContent(FcuLocKey.label_log_hash_generator_drawer.Localize()),
                     monoBeh.Settings.DebugSettings.LogHashGenerator);
            }

            gui.Space15();

            if (gui.OutlineButton("Open logs folder"))
            {
                FcuConfig.LogPath.OpenFolderInOS();
            }

            gui.Space15();

            if (gui.OutlineButton("Open cache folder"))
            {
                FcuConfig.CachePath.OpenFolderInOS();
            }

            gui.Space15();

            if (gui.OutlineButton("Open backup folder"))
            {
                SceneBackuper.GetBackupsPath().OpenFolderInOS();
                /*  List<string> sceneBackupPath = SceneBackuper.GetSceneBackupPath();
                  string _libraryPath = SceneBackuper.GetLibraryPath(sceneBackupPath);
                  _libraryPath = _libraryPath.Replace('/', '\\');

                  _libraryPath.OpenFolderInOS();*/
            }

            gui.Space15();

            if (gui.OutlineButton("Test Button"))
            {
            var p =    ConvertSvgPolygonToPoints("M85.5 0L159.545 99.75L11.4548 99.75L85.5 0Z");
                foreach (var item in p)
                {
                    Debug.Log(item);
                }

                Vector3[] ConvertSvgPolygonToPoints(string svgPolygon)
                {
                    // Видаляємо початкову 'M' та кінцеву 'Z'
                    svgPolygon = svgPolygon.Trim('M', 'Z').Trim();

                    // Розділяємо координати
                    string[] coordinatePairs = svgPolygon.Split(new[] { 'L' }, System.StringSplitOptions.RemoveEmptyEntries);

                    // Створюємо список для точок
                    List<Vector3> points = new List<Vector3>();

                    // Заповнюємо список точок
                    foreach (string pair in coordinatePairs)
                    {
                        string[] coordinates = pair.Trim().Split(' ');
                        if (coordinates.Length == 2)
                        {
                            float x = float.Parse(coordinates[0], CultureInfo.InvariantCulture);
                            float y = float.Parse(coordinates[1], CultureInfo.InvariantCulture);
                            points.Add(new Vector3(x, -y, 0)); // Інвертуємо координати по осі Y
                        }
                    }

                    return points.ToArray();
                }
            }

            if (monoBeh.Settings.DebugSettings.DebugMode)
            {
                gui.Space30();

                fcuConfigEditor.OnInspectorGUI();

                gui.Space30();
                scriptableObject.Inspector.DrawBaseOnInspectorGUI();
            }
        }
    }
}