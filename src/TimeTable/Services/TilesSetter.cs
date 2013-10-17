using Microsoft.Phone.Shell;
using System;
using System.Linq;

namespace TimeTable.Services
{
    public static class TilesSetter
    {
        private static readonly Version TargetVersion = new Version(7, 10, 8858);

        private static bool IsTargetVersion
        {
            get { return Environment.OSVersion.Version >= TargetVersion; }
        }

        public static void SetTiles()
        {
            if (IsTargetVersion)
            {
                // Get the new FlipTileData type.
                var flipTileDataType = Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone");

                // Get the ShellTile type so we can call the new version of "Update" that takes the new Tile templates.
                var shellTileType = Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");

                // Loop through any existing Tiles that are pinned to Start.
                var tileToUpdate = ShellTile.ActiveTiles.First();

                // Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                var UpdateTileData = flipTileDataType.GetConstructor(new Type[] {}).Invoke(null);

                // Set the properties. 
                SetProperty(UpdateTileData, "Title", "");
                SetProperty(UpdateTileData, "SmallBackgroundImage", new Uri("Images/SmallTile.png", UriKind.Relative));
                SetProperty(UpdateTileData, "BackgroundImage", new Uri("Images/MediumTile.png", UriKind.Relative));
                SetProperty(UpdateTileData, "WideBackgroundImage", new Uri("Images/WideTile.png", UriKind.Relative));

                // Invoke the new version of ShellTile.Update.
                shellTileType.GetMethod("Update").Invoke(tileToUpdate, new[] {UpdateTileData});
            }
        }

        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new[] {value});
        }
    }
}