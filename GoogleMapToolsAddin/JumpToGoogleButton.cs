using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    internal class JumpToGoogleButton : Button
    {
        protected override void OnClick()
        {
            QueuedTask.Run(() =>
            {
                //get active MapView
                MapView theView = MapView.Active;

                //get current spatial reference for debugger
                SpatialReference currentSR = theView.Map.SpatialReference;

                //get map center and project to GCS
                MapPoint mapCenter = theView.Extent.Center;
                MapPoint theCoords = (MapPoint)GeometryEngine.Instance.Project(mapCenter, SpatialReferences.WGS84);

                //get map scale
                double theScale = theView.Camera.Scale;

                //calculate google map zoom level equivalent and round to nearest quarter
                double theZoom = Math.Round(
                    4 * (Math.Log(591657550.500000 / (theScale / 2)) / Math.Log(2)), MidpointRounding.ToEven) / 4;

                //message box for debugging
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(string.Format(
                //    "Center X: {0} Center Y: {1} Map Scale: {2} Google Zoom Level: {3}", 
                //    theCoords.X, theCoords.Y, theScale, theZoom), "Results");

                //open in browser
                string googleURL = "https://www.google.com/maps/@";
                string url = string.Format(googleURL+"{0},{1},{2}z", 
                    theCoords.Y, theCoords.X, theZoom);
                Process.Start(url);

            });
        }
    }
}
