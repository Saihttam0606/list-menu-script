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
            private IMyProgrammableBlock pb;
            private IMyCockpit console;
            private IMyTextSurface textSurface;

            private int row;
            private bool rowBool;
            private int rowCount;

            private bool canSelect;

            private List<string> options;

            public Menu(IMyProgrammableBlock pb, IMyGridTerminalSystem gridTerminalSystem)
            {
                this.pb = pb;

                MyIni ini = new MyIni();
                MyIniParseResult result;
                if (!ini.TryParse(pb.CustomData, out result))
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

                options = new List<string>();
                for (int i = 0; i < rowCount; i++)
                {
                    options.Add(ini.Get("menu", "option" + i).ToString());
                }

                row = 0;

                rowBool = true;
                canSelect = true;

                run();
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
                    pb.TryRun("" + row);
                }
                rowBool = console.MoveIndicator.Z == 0;
                canSelect = console.RollIndicator == 0;

                if (!rowBool)
                {
                    string text = "";

                    for (int i = 0; i < rowCount; i++)
                    {
                        if (i == row)
                        {
                            text += "[x]";
                        }
                        else
                        {
                            text += "[ ]";
                        }
                        text += options.ElementAt(i) + "\n";

                    }

                    textSurface.WriteText(text);
                }
            }
        }
    }
}
