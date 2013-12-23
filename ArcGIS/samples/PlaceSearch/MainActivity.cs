/* 
 * Copyright 2013 ESRI
 *
 * All rights reserved under the copyright laws of the United States
 * and applicable international laws, treaties, and conventions.
 *
 * You may freely redistribute and use this sample code, with or
 * without modification, provided you include the original copyright
 * notice and use restrictions.
 *
 * See the Sample code usage restrictions document for further information.
 *
 */

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Esri.Android.Map;
using Esri.Android.Map.Ags;
using Esri.Core.Geometry;
using Esri.Core.Tasks.Geocode;
using Android.Views.InputMethods;
using System.Collections.Generic;
using Esri.Core.Symbol;
using Esri.Core.Map;

namespace PlaceSearch
{
	[Activity (Label = "PlaceSearch", MainLauncher = true)]
	public class MainActivity : Activity
	{
		// create ArcGIS objects
		MapView _mMapView;
		ArcGISTiledMapServiceLayer _basemap;
		GraphicsLayer _locationLayer;
		Locator _locator;
		LocatorGeocodeResult _geocodeResult;

		Point _mLocation = null;
		// Spatial references used for projecting points
		SpatialReference _wm = SpatialReference.Create(102100);
		SpatialReference _egs = SpatialReference.Create(4326);

		// create UI components
		static ProgressDialog _dialog;
		static Handler _handler;

		// Label instructing input for EditText
		TextView _geocodeLabel;
		// Text box for entering address
		EditText _addressText;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			// create handler to update the UI
			_handler = new Handler();

			// Set the geocodeLabel with instructions
			_geocodeLabel = (TextView) FindViewById(Resource.Id.geocodeLabel);
			_geocodeLabel.Text = GetString(Resource.String.geocode_label);

			// Get the addressText component
			_addressText = (EditText) FindViewById(Resource.Id.addressText);

			// Retrieve the map and initial extent from XML layout
			_mMapView = FindViewById(Resource.Id.map).JavaCast <MapView> ();
			/*			 create a @ArcGISTiledMapServiceLayer */
			_basemap = new ArcGISTiledMapServiceLayer(Resources.GetString(
				Resource.String.basemap_url));
			// Add tiled layer to MapView
			_mMapView.AddLayer(_basemap);
			// Add location layer
			_locationLayer = new GraphicsLayer();
			_mMapView.AddLayer(_locationLayer);

			// get the location service and start reading location
			LocationService locationSrv = _mMapView.LocationService;

			var myLocatioinListener = new MyLocationListener();

			myLocatioinListener.OnLocationChanged += (loc) => {
				if (loc == null)
					return;
				bool zoomToMe = (_mLocation == null);
				_mLocation = new Point (loc.Longitude, loc.Latitude);
				if (zoomToMe) {
					Point p = GeometryEngine.Project (_mLocation, _egs, _wm).JavaCast <Point> ();
					_mMapView.ZoomToResolution (p, 20.0);
				}
			};

			myLocatioinListener.OnProviderDisabled += (provider) => {
				Toast.MakeText (ApplicationContext, "GPS Disabled", ToastLength.Short).Show ();
			};

			myLocatioinListener.OnProviderEnabled += (provider) => {
				Toast.MakeText (ApplicationContext, "GPS Enabled", ToastLength.Short).Show ();
			};

			locationSrv.LocationListener = myLocatioinListener;
			locationSrv.Start();
			locationSrv.AutoPan = false;

			// attribute ESRI logo to map
			_mMapView.SetEsriLogoVisible (true);
			_mMapView.EnableWrapAround (true);
		}

		/*		
	 		* Submit address for place search
	 	*/
		[Java.Interop.Export ("locate")]
		public void Locate(View view)
		{
			// hide virtual keyboard
			InputMethodManager inputManager = (InputMethodManager) GetSystemService(Service.InputMethodService);
			inputManager.HideSoftInputFromWindow(
				CurrentFocus.WindowToken, 0);
			// remove any previous graphics and callouts
			_locationLayer.RemoveAll();

			// obtain address from text box
			string address = _addressText.Text;
			// set parameters to support the find operation for a geocoding service
			setSearchParams(address);
		}

		private async void setSearchParams(string address) {
			try 
			{
				// create Locator parameters from single line address string
				LocatorFindParameters findParams = new LocatorFindParameters(
					address);

				// limit the results to 2
				findParams.MaxLocations = 2;
				// set address spatial reference to match map
				findParams.OutSR = _mMapView.SpatialReference;

				var result = await System.Threading.Tasks.Task.Factory.StartNew ( () => {
					// create results object and set to null
					IList<LocatorGeocodeResult> results = null;
					// set the geocode service
					_locator = Locator.CreateOnlineLocator(Resources.GetString(Resource.String.geocode_url));
					try {

						// pass address to find method to return point representing
						// address
						results = _locator.Find(findParams);
					} catch (Java.Lang.Exception e) {
						e.PrintStackTrace();
					}
					// return the resulting point(s)
					return results;
				});

				if (result == null || result.Count == 0) 
				{
					// update UI with notice that no results were found
					Toast.MakeText(this, "No result found.", ToastLength.Long).Show ();
				}
				else
				{
					// update global result
					_geocodeResult = result[0];
					// show progress dialog while geocoding address
					_dialog = ProgressDialog.Show(_mMapView.Context, "Geocoder", "Searching for address ...");
					// get return geometry from geocode result
					Geometry resultLocGeom = _geocodeResult.Location;
					// create marker symbol to represent location
					SimpleMarkerSymbol resultSymbol = new SimpleMarkerSymbol(
						Android.Graphics.Color.Blue, 20, SimpleMarkerSymbol.STYLE.Circle);
					// create graphic object for resulting location
					Graphic resultLocation = new Graphic(resultLocGeom,
						resultSymbol);
					// add graphic to location layer
					_locationLayer.AddGraphic(resultLocation);

					// create text symbol for return address
					TextSymbol resultAddress = new TextSymbol(12,
						_geocodeResult.Address, Android.Graphics.Color.Black);
					// create offset for text
					resultAddress.SetOffsetX (10);
					resultAddress.SetOffsetY (50);
					// create a graphic object for address text
					Graphic resultText = new Graphic(resultLocGeom, resultAddress);
					// add address text graphic to location graphics layer
					_locationLayer.AddGraphic(resultText);
					// zoom to geocode result

					_mMapView.ZoomToResolution(_geocodeResult.Location, 2);
					// create a runnable to be added to message queue
					_handler.Post(new MyRunnable(_dialog));
				}


			} catch (Java.Lang.Exception e) {
				e.PrintStackTrace();
			}

		}

		public class MyRunnable : Java.Lang.Object, Java.Lang.IRunnable 
		{
			ProgressDialog _dialog;

			public MyRunnable (ProgressDialog dialog)
			{
				_dialog = dialog;
			}

			#region IRunnable implementation

			public void Run ()
			{
				_dialog.Dismiss();
			}

			#endregion
		}

		private class MyLocationListener : Java.Lang.Object, Android.Locations.ILocationListener 
		{
			public event Action <Android.Locations.Location> OnLocationChanged;
			public event Action <string> OnProviderDisabled;
			public event Action <string> OnProviderEnabled;
			public event Action <string, Android.Locations.Availability, Bundle> OnStatusChanged;

			void Android.Locations.ILocationListener.OnLocationChanged (Android.Locations.Location location)
			{
				if (OnLocationChanged != null)
					OnLocationChanged (location);
			}

			void Android.Locations.ILocationListener.OnProviderDisabled (string provider)
			{
				if (OnProviderDisabled != null)
					OnProviderDisabled (provider);
			}
			void Android.Locations.ILocationListener.OnProviderEnabled (string provider)
			{
				if (OnProviderEnabled != null)
					OnProviderEnabled (provider);
			}
			void Android.Locations.ILocationListener.OnStatusChanged (string provider, Android.Locations.Availability status, Bundle extras)
			{
				if (OnStatusChanged != null)
					OnStatusChanged (provider, status, extras);
			}
		}



	}
}


