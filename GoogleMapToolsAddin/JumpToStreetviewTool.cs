using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace GoogleMapToolsAddin
{
    internal class JumpToStreetviewTool : MapTool
    {
        protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
        {
            //Handle the event args to get the call to the corresponding async method
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                e.Handled = true; 
        }

        // Get the point in client coordinates relative to the top-left corner of the view
        protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
        {
            return QueuedTask.Run(() =>
            {
                // Convert the clicked point in client coordinates to the corresponding map coordinates.
                MapPoint thePoint = MapView.Active.ClientToMap(e.ClientPoint);

                // Message box for debugging
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(string.Format("X: {0} Y: {1}",
                //   thePoint.X, thePoint.Y), "Map Coordinates");

                // Convert to GCS
                MapPoint theCoords = (MapPoint)GeometryEngine.Instance.Project(thePoint, SpatialReferences.WGS84);

                // Open a web browser
                string googleURL = "https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=";
                string url = string.Format(googleURL + "{0},{1}", theCoords.Y, theCoords.X);
                Process.Start(url);

                // Reset default mouse behavior
                FrameworkApplication.SetCurrentToolAsync("esri_mapping_exploreTool"); 

            });
        }

    }
}
