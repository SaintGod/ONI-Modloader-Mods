﻿namespace MaterialColor.Helpers
{
    using Extensions;
    using System;
    using UnityEngine;

    public static class ColorHelper
    {
        public static readonly Color DefaultColorOffset = new Color(0, 0, 0, 0);
        public static readonly Color InvalidColorOffset = new Color(1, 0, 1, 0);

        public static readonly Color DefaultTileColor = DefaultColorOffset.ToTileColor(true);
        public static readonly Color InvalidTileColor = InvalidColorOffset.ToTileColor(true);

        public static Color?[] TileColors;

        public static Color GetTileMaterialColor(int cellIndex)
        {
            if (Grid.IsValidCell(cellIndex) && MaterialHelper.CellIndexToElement(cellIndex, out Element element))
            {
                return element.id.ToMaterialColor(out Color materialColor)
                    ? materialColor
                    : (Color)element.substance.overlayColour;
            }
            else return InvalidTileColor;
        }

        /// <summary>
        /// Tries to get material color for given component, if not possible fallsback to to substance.overlayColour, then to ColorHelper.DefaultColor
        /// </summary>
        /// <param name="outColor">on true outputs color offset, otherwise fallsback to substance.overlayColour, then to ColorHelper.DefaultColor</param>
        public static bool GetComponentMaterialColor(Component component, out Color outColor)
        {
            if (State.ConfiguratorState.Enabled)
            {
                PrimaryElement primaryElement = component.GetComponent<PrimaryElement>();

                if (primaryElement != null)
                {
                    SimHashes material = primaryElement.ElementID;

                    bool materialColorResult = material.ToMaterialColor(out outColor);

                    if (!materialColorResult)
                    {
                        outColor = primaryElement.Element.substance.overlayColour;
                        if (State.ConfiguratorState.ShowMissingElementColorInfos)
                        {
                            Debug.Log($"Missing color for material: {material}, while coloring building: {component.GetComponent<BuildingComplete>()}");
                        }
                    }

                    return materialColorResult;
                }
            }

            outColor = DefaultColorOffset;
            return false;
        }
    }
}