// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace iOSNavBar
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIView MainView { get; set; }

		[Outlet]
		ActionComponents.ACNavBar NavBar { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (NavBar != null) {
				NavBar.Dispose ();
				NavBar = null;
			}

			if (MainView != null) {
				MainView.Dispose ();
				MainView = null;
			}
		}
	}
}
