using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class Menu
        {
            private IMyCockpit console;
            private IMyTextSurface textSurface;
            private IMyTerminalBlock iniLocation;

            List<IMyTimerBlock> options;

            private int row;
            private bool rowBool;
            private int rowCount;

            private int showableRows;
            private int start;

            private bool canSelect;

            private List<string> optionNames;

            public Menu(IMyTerminalBlock tb, IMyGridTerminalSystem gridTerminalSystem)
            {
                iniLocation = tb;

                MyIni ini = new MyIni();
                MyIniParseResult result;
                if (!ini.TryParse(tb.CustomData, out result))
                    throw new Exception(result.ToString());

                int surfaceNumber = 0;
                if (ini.ContainsKey("menu", "surface"))
                {
                    surfaceNumber = ini.Get("menu", "surface").ToInt32();
                }

                rowCount = ini.Get("menu", "count").ToInt32();

                console = gridTerminalSystem.GetBlockWithName(ini.Get("menu", "control").ToString()) as IMyCockpit;
                textSurface = (gridTerminalSystem.GetBlockWithName(ini.Get("menu", "screen").ToString()) as IMyTextSurfaceProvider).GetSurface(surfaceNumber);

                textSurface.ContentType = ContentType.TEXT_AND_IMAGE;
                textSurface.Font = "Monospace";

                optionNames = new List<string>();
                for (int i = 0; i < rowCount; i++)
                {
                    optionNames.Add(ini.Get("menu", "option" + i).ToString());
                }

                options = new List<IMyTimerBlock>();
                for (int i = 0; i < rowCount; i++)
                {
                    options.Add(gridTerminalSystem.GetBlockWithName(ini.Get("options", "" + i).ToString()) as IMyTimerBlock);
                }

                row = 0;
                if (ini.ContainsKey("menu", "selected"))
                {
                    row = ini.Get("menu", "selected").ToInt32();
                }

                showableRows = (int)(textSurface.SurfaceSize.Y / textSurface.MeasureStringInPixels(new StringBuilder("|"), textSurface.Font, textSurface.FontSize).Y);

                //Temporary fix for IMyTextSurface.SurfaceSize (probably) not being correct
                if (surfaceNumber != 0)
                    showableRows *= 2;

                if (ini.ContainsKey("menu", "rows"))
                {
                    showableRows = ini.Get("menu", "rows").ToInt32();
                }

                start = 0;
                if (ini.ContainsKey("menu", "start"))
                {
                    start = ini.Get("menu", "start").ToInt32();
                }

                rowBool = true;
                canSelect = true;

                updateText();
            }

            public void save()
            {
                MyIni ini = new MyIni();
                MyIniParseResult result;
                if (!ini.TryParse(iniLocation.CustomData, out result))
                    throw new Exception(result.ToString());

                ini.Set("menu", "selected", row);
                ini.Set("menu", "start", start);
                iniLocation.CustomData = ini.ToString();
            }

            public void run()
            {
                if (rowBool && console.MoveIndicator.Z > 0 && row < rowCount - 1)
                {
                    row++;
                }
                else if (rowBool && console.MoveIndicator.Z < 0 && row > 0)
                {
                    row--;
                }
                else if (canSelect && console.RollIndicator > 0)
                {
                    options.ElementAt(row).Trigger();
                }
                rowBool = console.MoveIndicator.Z == 0;
                canSelect = console.RollIndicator == 0;

                if (!rowBool)
                {
                    updateText();
                }
            }

            public void updateText()
            {
                string text = "";
                int length = rowCount;
                bool down = false;

                if (rowCount > showableRows)
                {
                    length = showableRows;
                }
                else
                {
                    start = 0;
                }

                if (row < start)
                {
                    start = row;
                }

                if (start > 0)
                {
                    length -= 1;
                    text += "/\\\n";
                }

                if (start + length < rowCount)
                {
                    length -= 1;
                    down = true;
                }

                if (row >= start + length)
                {
                    start = row - (length - 1);
                }

                for (int i = start; i < start + length; i++)
                {
                    if (i == row)
                    {
                        text += "[x]";
                    }
                    else
                    {
                        text += "[ ]";
                    }
                    text += optionNames.ElementAt(i) + "\n";
                }

                if (down)
                {
                    text += "\\/";
                }

                textSurface.WriteText(text);
            }
        }
    }
}
