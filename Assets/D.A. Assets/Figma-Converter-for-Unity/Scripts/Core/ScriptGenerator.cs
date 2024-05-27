using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.FCU.UI;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU
{
    [Serializable]
    public class ScriptGenerator : MonoBehaviourBinder<FigmaConverterUnity>
    {
        private static string _baseClass = @"
using System;
using UnityEngine;
using UnityEngine.UI;

#if TextMeshPro
using TMPro;
#endif

#if DABUTTON_EXISTS
using DA_Assets.DAB;
#endif

namespace {0}
{{
    public class {1} : MonoBehaviour
    {{
{2}
    }}
}}
";
        public IEnumerator Generate()
        {
            List<FObject> buttons = monoBeh.CanvasDrawer.ButtonDrawer.Buttons;
            List<FObject> inputFields = monoBeh.CanvasDrawer.InputFieldDrawer.InputFields;
            List<FObject> texts = monoBeh.CanvasDrawer.TextDrawer.Texts;

            List<FObject> allItems = new List<FObject>();

            if (!buttons.IsEmpty())
            {
                allItems.AddRange(buttons);
            }

            if (!inputFields.IsEmpty())
            {
                allItems.AddRange(inputFields);
            }

            if (!texts.IsEmpty())
            {
                texts = texts.Where(x => !x.Data.Parent.IsDefault() && !x.Data.Parent.ContainsTag(FcuTag.InputField)).ToList();
                allItems.AddRange(texts);
            }

            var grouped = allItems
                .GroupBy(item => item.Data.RootFrame)
                .Select(group => new
                {
                    RootFrame = group.Key,
                    FObjects = group.ToList()
                });

            foreach (var group in grouped)
            {
                SyncData rootFrame = group.RootFrame;
                string className = rootFrame.FieldName;

                string fields = GetFields(group.FObjects);

                string script = string.Format(_baseClass, monoBeh.Settings.ScriptGeneratorSettings.Namespace, className, fields);

                string folderPath = Path.Combine(Application.dataPath, monoBeh.Settings.ScriptGeneratorSettings.OutputPath);
                Directory.CreateDirectory(folderPath);
             
                string filePath = Path.Combine(folderPath, $"{className}.cs");
                File.WriteAllText(filePath, script.ToString());

#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            yield return null;
        }

        private static string GetFields(List<FObject> fobjects)
        {
            StringBuilder elemsSb = new StringBuilder();
            StringBuilder labelsSb = new StringBuilder();

            foreach (FObject fobject in fobjects)
            {
                try
                {
                    string fieldName = fobject.Data.FieldName;/*fobject.Data.NewName.GetFieldName(); */

                    if (fobject.Data.GameObject.TryGetComponent(out Text c1))
                    {
                        labelsSb.AppendLine($"        [SerializeField] Text {fieldName};");
                    }
                    else if (fobject.Data.GameObject.TryGetComponent(out TMP_Text c2))
                    {
                        labelsSb.AppendLine($"        [SerializeField] TMP_Text {fieldName};");
                    }
                    else if (fobject.Data.GameObject.TryGetComponent(out Button c3))
                    {
                        labelsSb.AppendLine($"        [SerializeField] Button {fieldName};");
                    }
                    else if (fobject.Data.GameObject.TryGetComponent(out FcuButton c4))
                    {
                        labelsSb.AppendLine($"        [SerializeField] FcuButton {fieldName};");
                    }
                 /*   else if (fobject.Data.GameObject.TryGetComponent(out DAButton c5))
                    {
                        labelsSb.AppendLine($"        [SerializeField] DAButton {fieldName};");
                    }*/
                    else if (fobject.Data.GameObject.TryGetComponent(out InputField c6))
                    {
                        labelsSb.AppendLine($"        [SerializeField] InputField {fieldName};");
                    }
                    else if (fobject.Data.GameObject.TryGetComponent(out TMP_InputField c7))
                    {
                        labelsSb.AppendLine($"        [SerializeField] TMP_InputField {fieldName};");
                    }
                    else
                    {
                        labelsSb.AppendLine($"        [SerializeField] GameObject {fieldName};");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }

            }

            return $"{elemsSb}\n{labelsSb}";
        }
    }

    public struct ScriptData
    {
        public FObject FObject { get; set; }
        public Type ComponentType { get; set; }

        public ScriptData(FObject fobject, Type type)
        {
            this.FObject = fobject;
            this.ComponentType = type;
        }
    }
}