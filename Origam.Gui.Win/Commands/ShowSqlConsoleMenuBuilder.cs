﻿#region license
/*
Copyright 2005 - 2020 Advantage Solutions, s. r. o.

This file is part of ORIGAM (http://www.origam.org).

ORIGAM is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

ORIGAM is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with ORIGAM. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using Origam.UI;
using Origam.Workbench.Services.CoreServices;
using System;
using System.Collections.Generic;

namespace Origam.Gui.Win.Commands
{
    public class ShowSqlConsoleMenuBuilder : ISubmenuBuilder
    {
        public bool LateBound => true;

        public AsMenuCommand[] BuildSubmenu(object owner)
        {
			var list = new List<AsMenuCommand>();
			OrigamSettings settings = ConfigurationManager.GetActiveConfiguration();
            if (settings != null)
            {
				foreach (var platform in settings.GetAllPlatform())
				{
					CreateMenuItem(list, platform);
				}
			}
			return list.ToArray();
		}

        public bool HasItems()
        {
			OrigamSettings settings = ConfigurationManager.GetActiveConfiguration();
			return settings != null;
        }

		private void MenuItemClick(object sender, EventArgs e)
		{
			AsMenuCommand cmd = sender as AsMenuCommand;
			cmd.Command.Run();
		}

		private void CreateMenuItem(List<AsMenuCommand> list, Platform platform)
		{
			SqlConsoleParameters parameters = new SqlConsoleParameters
			{
				Platform = platform
			};
			ShowSqlConsole command = new ShowSqlConsole(parameters);
			AsMenuCommand menuItem = new AsMenuCommand(platform.Name, command);
			menuItem.Click += new EventHandler(MenuItemClick);
			list.Add(menuItem);
		}
	}
}