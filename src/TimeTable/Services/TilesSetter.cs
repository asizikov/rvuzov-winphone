﻿using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTable.Services
{
    public static class TilesSetter
    {
        private static readonly Version targetVersion = new Version(7, 10, 8858);
        private static bool isTargetVersion { get { return Environment.OSVersion.Version >= targetVersion;} }

        public static void SetTiles()
        {
            ShellTile appTile = ShellTile.ActiveTiles.First();
            if (isTargetVersion)
            {
                // Get the new FlipTileData type.
                Type flipTileDataType = Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone");

                // Get the ShellTile type so we can call the new version of "Update" that takes the new Tile templates.
                Type shellTileType = Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");

                // Loop through any existing Tiles that are pinned to Start.
                var tileToUpdate = ShellTile.ActiveTiles.First();

                // Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                var UpdateTileData = flipTileDataType.GetConstructor(new Type[] { }).Invoke(null);

                // Set the properties. 
                SetProperty(UpdateTileData, "Title", "");
                SetProperty(UpdateTileData, "SmallBackgroundImage", new Uri("Images/SmallTile.png", UriKind.Relative));
                SetProperty(UpdateTileData, "BackgroundImage", new Uri("Images/MediumTile.png", UriKind.Relative));
                SetProperty(UpdateTileData, "WideBackgroundImage", new Uri("Images/WideTile.png", UriKind.Relative));

                // Invoke the new version of ShellTile.Update.
                shellTileType.GetMethod("Update").Invoke(tileToUpdate, new Object[] { UpdateTileData });
            }
            else
            {
                StandardTileData newTile = new StandardTileData
                {
                    Title = "",
                    BackgroundImage = new Uri("Images/MediumTile.png", UriKind.Relative),
                };
                appTile.Update(newTile);
            }
        }

        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
        }
    }
}
