using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content.PM;

using ActionComponents;


namespace NavBarTestAndroid
{
	[Activity (Label = "NavBarTestAndroid", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Landscape)]
	public class Activity1 : Activity
	{
		#region Constantants
		const int DialogLongMessage=1;
		const int DialogLongMessageOkCancel=2;
		#endregion 

		#region Private Variables
		private ACNavBar navBar;
		private ImageView viewHome;
		private View viewBarChart, viewOrgChart, viewTicket, viewSettings;
		private Button showHideNavBar = null;
		private ACNavBarButton warning = null, delete = null, ticket = null;
		private string dialogMessage="";
		#endregion 

		#region Override Methods
		protected override void OnCreate (Bundle bundle)
		{

			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Gain Access to all views and controls in our layout
			navBar = FindViewById<ACNavBar> (Resource.Id.navBar);
			viewHome = FindViewById<ImageView> (Resource.Id.viewHome);

			//Are we rehydrating after a state change?
			if (bundle!=null) {
				//Yes, attempt to restore the previously selected NavBar button
				navBar.rehydrationId = bundle.GetInt ("SelectedButton");
				navBar.Hidden = bundle.GetBoolean("Hidden");
			}

			// Style bar
			navBar.appearance.border = Color.LightGray;

			//---------------------------------------------
			// Add buttons to the top of the bar
			//---------------------------------------------
			// The first button added to the top collection will automatically be selected
			ACNavBarButton home = navBar.top.AddButton(Resource.Drawable.house,true,false);

			// Wire up request for this button's view
			home.RequestNewView += responder => {
				// Attach view to the button
				responder.attachedView=viewHome;
			};
			
			// Add an action to the home button
			home.Touched += responder => {
				// Hide warning notification in NavBar
				if (warning != null) warning.Hidden = true;
				
				// Disable the delete button
				if (delete != null) delete.Enabled = false;
			};

			// Add Bar Chart
			navBar.top.AddAutoDisposingButton (Resource.Drawable.barchart,true,false).RequestNewView += (responder) => {
				// Bring view into existance
				viewBarChart = (View)LayoutInflater.Inflate (Resource.Layout.ViewBarChart,null);

				// Attach view to the button
				responder.attachedView = viewBarChart;
			};

			// Add Org Chart
			navBar.top.AddAutoDisposingButton (Resource.Drawable.orgchart,true,false).RequestNewView += (responder) => {
				// Bring view into existance
				viewOrgChart = (View)LayoutInflater.Inflate (Resource.Layout.ViewOrgChart,null);

				// Attach view to the button
				responder.attachedView=viewOrgChart;
			};

			// Add ticket
			ticket = navBar.top.AddAutoDisposingButton (Resource.Drawable.ticket,true,false);

			ticket.RequestNewView += responder => {
				// Bring view into existance
				viewTicket = (View)LayoutInflater.Inflate (Resource.Layout.ViewTIcket,null);

				// Attach view to the button
				responder.attachedView=viewTicket;
			};
			
			ticket.Touched += responder => {
				// Enable the delete button
				if (delete != null) delete.Enabled = true;
			};

			//--------------------------------------------
			// Add buttons to the middle of the bar
			//--------------------------------------------
			navBar.middle.AddTool (Resource.Drawable.printer,true,false).Touched+= (responder) => {
				// Inform user (dialogMessage defined as a global variable)
				ACAlert.ShowAlertOK(this, "Nav Bar", "Sorry but printing is not supported at this time.");

				// Display warning notification in NavBar
				if (warning != null) warning.Hidden = false;
			};

			delete = navBar.middle.AddTool (Resource.Drawable.trash,false,false);

			delete.Touched += (responder) => {
				// Inform user (dialogMessage defined as a global variable)
				ACAlert.ShowAlertOK(this, "Nav Bar", "Are you sure you what to delete the item?");
			};

			//-----------------------------------------
			// Add buttons to the bottom of the bar
			//-----------------------------------------
			warning = navBar.bottom.AddNotification (Resource.Drawable.warning, null, true);
			navBar.bottom.AddAutoDisposingButton (Resource.Drawable.gear, true, false).RequestNewView += responder => {
				// Bring view into existance
				viewSettings = (View)LayoutInflater.Inflate (Resource.Layout.ViewSettings,null);

				// Attach view to the button
				responder.attachedView=viewSettings;

				// grab show/hide button
				showHideNavBar = FindViewById<Button> (Resource.Id.showHideButton);

				//-----------------------------------------
				// Wireup button action
				//-----------------------------------------
				if (showHideNavBar!=null) {
					showHideNavBar.Click += (sender, e) => {
						//Is the NavBar visible?
						navBar.Hidden=(!navBar.Hidden);
					};
				}
			};

		}

		protected override void OnStart ()
		{
			base.OnStart ();

			//-----------------------------------------
			// Ask the Nav Bar to display the first view
			//-----------------------------------------
			navBar.DisplayDefaultView();
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			//Save the NavBar's selected button before the state change
			outState.PutInt("SelectedButton",navBar.SelectedButtonId ());
			outState.PutBoolean("Hidden",navBar.Hidden);

			base.OnSaveInstanceState (outState);
		}
		#endregion
	}
}


