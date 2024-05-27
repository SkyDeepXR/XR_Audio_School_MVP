﻿using DA_Assets.FCU.Drawers.CanvasDrawers;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#pragma warning disable CS0649

namespace DA_Assets.FCU
{
    [Serializable]
    public class ImageTypeSetter : MonoBehaviourBinder<FigmaConverterUnity>
    {
        [SerializeField] List<string> downloadableIds = new List<string>();
        [SerializeField] List<string> generativeIds = new List<string>();
        [SerializeField] List<string> drawableIds = new List<string>();
        [SerializeField] List<string> noneIds = new List<string>();

        public List<string> DownloadableIds => downloadableIds;
        public List<string> GenerativeIds => generativeIds;
        public List<string> DrawableIds => drawableIds;
        public List<string> NoneIds => noneIds;

        public static string HAS_STROKES { get; } = "has strokes";

        public IEnumerator SetImageTypes(List<FObject> fobjects)
        {
            downloadableIds.Clear();
            generativeIds.Clear();
            drawableIds.Clear();
            noneIds.Clear();

            foreach (FObject fobject in fobjects)
            {
                if (fobject.ContainsTag(FcuTag.Image) == false)
                {
                    continue;
                }

                bool isGenerative = IsGenerative(fobject);
                bool isDownloadable = IsDownloadable(fobject);
                bool isDrawable = IsDrawable(fobject);

                if (isGenerative)
                {
                    fobject.Data.FcuImageType = FcuImageType.Generative;
                    generativeIds.Add(fobject.Id);
                }
                else if (isDownloadable)
                {
                    fobject.Data.FcuImageType = FcuImageType.Downloadable;
                    downloadableIds.Add(fobject.Id);
                }
                else if (isDrawable)
                {
                    fobject.Data.FcuImageType = FcuImageType.Drawable;
                    drawableIds.Add(fobject.Id);
                }
                else
                {
                    fobject.Data.FcuImageType = FcuImageType.None;
                    noneIds.Add(fobject.Id);
                }

                monoBeh.Log($"SetImageType | {fobject.Data.NameHierarchy} | {fobject.Data.FcuImageType}", FcuLogType.IsDownloadable);
            }

            monoBeh.Log($"SetImageType | {downloadableIds.Count} | {generativeIds.Count} | {drawableIds.Count} | {noneIds.Count}", FcuLogType.IsDownloadable);

            yield return null;
        }

        private bool IsDrawable(FObject fobject)
        {
            bool result = true;
            string reason = "drawable";

            monoBeh.Log($"{nameof(IsDrawable)} | {result} | {fobject.Data.NameHierarchy} | {reason}", FcuLogType.IsDownloadable);

            return result;
        }

        private bool IsGenerative(FObject fobject)
        {
            bool? result = null;
            string reason = "not generative";

            if (fobject.Data.IsOverlappedByStroke)
            {
                reason = nameof(fobject.Data.IsOverlappedByStroke);
                result = false;
            }
            else if (!fobject.Size.IsSupportedRenderSize(monoBeh, out Vector2Int spriteSize, out Vector2Int _renderSize))
            {
                reason = $"render size is big: {spriteSize} x {_renderSize}";
                result = false;
            }
            else
            {
                FGraphic graphic = fobject.GetGraphic();

                bool hasSupportedColors = graphic.HasFill || graphic.HasStroke;

                if (graphic.HasFill)
                {
                    if (!graphic.GradientFill.IsDefault())
                    {
                        hasSupportedColors = false;
                    }
                }

                if (graphic.HasStroke)
                {
                    if (!graphic.GradientStroke.IsDefault())
                    {
                        hasSupportedColors = false;
                    }
                }

                if (hasSupportedColors)
                {
                    if (fobject.HasActiveProperty(x => x.Strokes))
                    {
                        if (monoBeh.UsingShapes2D())
                        {
                            if (fobject.StrokeAlign == StrokeAlign.CENTER)
                            {
                                reason = HAS_STROKES;
                                result = true;
                            }
                        }
                        else if (fobject.StrokeAlign == StrokeAlign.INSIDE || fobject.StrokeAlign == StrokeAlign.CENTER)
                        {
                            reason = HAS_STROKES;
                            result = true;
                        }
                    }
                }
            }

            fobject.Data.GenerativeReason = reason;

            monoBeh.Log($"{nameof(IsGenerative)} | {result} | {fobject.Data.NameHierarchy} | {reason}", FcuLogType.IsDownloadable);

            return result.ToBoolNullFalse();
        }
        private bool IsDownloadable(FObject fobject)
        {
            bool? result = null;
            string reason = "unknown";

            if (fobject.Data.IsEmpty)
            {
                reason = "IsEmpty";
                result = false;
            }
            else if (fobject.Data.ForceImage)
            {
                reason = "force image";
                result = true;
            }
            else if (fobject.ContainsTag(FcuTag.Vector))
            {
                reason = "vector";
                result = true;
            }
            else if (fobject.IsMask.ToBoolNullFalse())
            {
                reason = "IsMask";
                result = true;
            }
            else if (fobject.HaveUndownloadableTags(out reason))
            {
                reason = "HasUndownloadableTags";
                result = false;
            }
            else if (fobject.IsArcDataFilled())
            {
                reason = "fobject.IsFilled()";
                result = true;
            }
            else if (fobject.HasImageOrGifRef())
            {
                reason = "HasImageOrGifRef";
                result = true;
            }
            else if (fobject.Effects.IsEmpty() == false)
            {
                foreach (Effect effect in fobject.Effects)
                {
                    if (!effect.Visible.ToBoolNullTrue())
                        continue;

                    if (effect.IsShadowType())
                    {
                        if (monoBeh.UsingSpriteRenderer())
                        {
                            reason = "sprite renderer shadow not supported";
                            result = true;
                            break;
                        }
                        else if (monoBeh.Settings.ComponentSettings.ShadowComponent == ShadowComponent.Figma)
                        {
                            reason = "shadowComponent.Figma";
                            result = true;
                            break;
                        }
                    }
                    else
                    {
                        reason = "contains other effects";
                        result = true;
                        break;
                    }
                }
            }

            if (result == null)
            {
                FGraphic graphic = fobject.GetGraphic();

                if (monoBeh.UsingSpriteRenderer() && monoBeh.IsUGUI())
                {
                    if (!fobject.IsRectangle())
                    {
                        reason = "!fobject.IsRectangle()";
                        result = true;
                    }
                    else if (fobject.ContainsRoundedCorners())
                    {
                        reason = "containsRoundedCorners";
                        result = true;
                    }
                    else if (fobject.HasActiveProperty(x => x.Strokes))
                    {
                        reason = HAS_STROKES;
                        result = true;
                    }
                    else if (fobject.Type == NodeType.LINE && fobject.StrokeCap == StrokeCap.ROUND)
                    {
                        reason = "StrokeCap == ROUND";
                        result = true;
                    }
                }
                else if (monoBeh.UsingUnityImage() && monoBeh.IsUGUI())
                {
                    if (!fobject.IsRectangle()/* && !fobject.IsCircle()*/)
                    {
                        reason = "!fobject.IsRectangle() && !fobject.IsCircle()";
                        result = true;
                    }
                    else if (fobject.HasActiveProperty(x => x.Strokes) && fobject.StrokeAlign != StrokeAlign.OUTSIDE)
                    {
                        reason = $"{HAS_STROKES}: {fobject.StrokeAlign}";
                        result = true;
                    }
                    else if (fobject.Type == NodeType.LINE && !fobject.IsSupportedLine())
                    {
                        reason = "unsupported line";
                        result = true;
                    }
                }
                else if (monoBeh.UsingMPUIKit() || monoBeh.UsingJoshPui() || monoBeh.UsingShapes2D() || monoBeh.UsingDttPui() || monoBeh.IsUGUI() == false)
                {
                    if (graphic.HaveDownloadableColors)
                    {
                        reason = nameof(graphic.HaveDownloadableColors);
                        result = true;
                    }
                    else if (fobject.IsRectangle() || fobject.Type == NodeType.ELLIPSE || fobject.Type == NodeType.LINE)
                    {
                        reason = "fobject.IsRectangle() || fobject.Type == NodeType.ELLIPSE || fobject.Type == NodeType.LINE";
                        result = false;
                    }

                    if (fobject.HasActiveProperty(x => x.Strokes))
                    {
                        if (monoBeh.UsingShapes2D())
                        {
                            if (fobject.StrokeAlign == StrokeAlign.CENTER)
                            {
                                reason = HAS_STROKES;
                                result = true;
                            }
                        }
                        else if (fobject.StrokeAlign == StrokeAlign.INSIDE || fobject.StrokeAlign == StrokeAlign.CENTER)
                        {
                            reason = HAS_STROKES;
                            result = true;
                        }
                    }
                }
            }

            fobject.Data.DownloadableReason = reason;

            monoBeh.Log($"{nameof(IsDownloadable)} | {result} | {fobject.Data.NameHierarchy} | {reason}", FcuLogType.IsDownloadable);
            return result.ToBoolNullFalse();
        }
    }
}
