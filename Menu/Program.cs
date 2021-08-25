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
        private List<Menu> menus;

        public Program()
        {
            List<IMyProgrammableBlock> pbs = new List<IMyProgrammableBlock>();
            GridTerminalSystem.GetBlocksOfType(pbs);


            pbs.RemoveAll(pb => !MyIni.HasSection(pb.CustomData, "menu"));

            menus = new List<Menu>();

            pbs.ForEach(pb => menus.Add(new Menu(pb, GridTerminalSystem)));


            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Save()
        {

        }

        public void Main(string argument, UpdateType updateSource)
        {
            menus.ForEach(m => m.run());
        }
    }
}
