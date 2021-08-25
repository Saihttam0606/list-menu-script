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
    partial class Program : MyGridProgram
    {
        List<IMyTimerBlock> options;

        public Program()
        {
            options = new List<IMyTimerBlock>();


            MyIni ini = new MyIni();
            MyIniParseResult result;
            if (!ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());

            int count = ini.Get("menu","count").ToInt32();

            for (int i = 0; i < count; i++)
            {
                options.Add(GridTerminalSystem.GetBlockWithName(ini.Get("options", "" + i).ToString()) as IMyTimerBlock);
            }
        }

        public void Save()
        {
            
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & UpdateType.Script) != 0)
            {
                options.ElementAt(int.Parse(argument)).Trigger();
            }
        }
    }
}
