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
            List<IMyTerminalBlock> tbs = new List<IMyTerminalBlock>();
            GridTerminalSystem.GetBlocks(tbs);


            tbs.RemoveAll(tb => !MyIni.HasSection(tb.CustomData, "menu"));

            menus = new List<Menu>();

            tbs.ForEach(tb => menus.Add(new Menu(tb, GridTerminalSystem)));


            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Save()
        {
            menus.ForEach(m => m.save());
        }

        public void Main(string argument, UpdateType updateSource)
        {
            menus.ForEach(m => m.run());
        }
    }
}
