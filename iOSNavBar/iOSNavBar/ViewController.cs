using System;
using UIKit;
using ActionComponents;

namespace iOSNavBar
{
	public partial class ViewController : UIViewController
	{
		#region Constructors
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		#endregion

		#region Override Methods
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Setup top bar
			NavBar.Top.SuspendUpdates = true;

			var home = NavBar.Top.AddView("House", true);
			home.RequestNewView += (responder) => {
				var view = new UIImageView(MainView.Frame);
				view.Image = UIImage.FromBundle("HomeView");
				return view;
			};

			var barChart = NavBar.Top.AddView("BarChart", true);
			barChart.RequestNewView += (responder) => {
				var view = new UIImageView(MainView.Frame);
				view.Image = UIImage.FromBundle("BarChartView");
				return view;
			};

			var orgChart = NavBar.Top.AddView("OrgChart", true);
			orgChart.RequestNewView += (responder) => {
				var view = new UIImageView(MainView.Frame);
				view.Image = UIImage.FromBundle("OrgChartView");
				return view;
			};

			var ticket = NavBar.Top.AddView("Ticket", true);
			ticket.RequestNewView += (responder) => {
				var view = new UIImageView(MainView.Frame);
				view.Image = UIImage.FromBundle("TicketView");
				return view;
			};

			NavBar.Top.SuspendUpdates = false;

			// Setup bottom bar
			var settings = NavBar.Bottom.AddView("Gear", true);
			settings.RequestNewView += (responder) => {
				var view = new UIImageView(MainView.Frame);
				view.Image = UIImage.FromBundle("SettingsView");
				return view;
			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
		#endregion
	}
}
