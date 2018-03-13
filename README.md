# About Action Nav Bar

**Action Nav Bar** is a left-side, icon based, customizable navigation strip and view controller for tablet based iOS and Android devices that supports several different types of user definable buttons such as:

* **View** – Handles hiding and showing an attached view.
* **AutoDisposingView** – Handles hiding and showing an attached view and removes the view from memory when it loses focus.
* **Tool** – A button that has a user defined action when touched.
* **Notification** – A non-touchable icon displayed in the bar.

**Action Nav Bar** has three separate regions that you can add buttons to: **Top**, **Middle** and **Bottom**. **Action Nav Bar** automatically handles the spacing and placement of buttons within the regions and responds to `UIInterfaceOrientation` events with smoothly animated transitions.

**Note**: While **Action Nav Bar** will work on small screen devices such as iPhones, iPod Touches and Android phones, it was designed for tablet devices as may not provide the best User Experience in these situations.

# Running the Samples

Before you can successfully build and run these sample apps, you'll need to download and install the [Action Components Trial](http://appracatappra.com/products/action-components/) version from the Appracatappra website or have purchased and installed a licensed version of the components.

Next, open either the iOS or the Android version of the sample in Visual Studio and expand the **Resources** folder:

![](Images/Intro01.png)

If the `ActionComponents` entry is grayed-out with a red X (as shown in the image above), right-click on it and select **Delete**. Double-Click the **Resources** folder to open the **Edit References** dialog and select the **.Net Assembly** tab:

![](Images/Intro02.png)

Click the **Browse** button, navigate to where you installed the **Action Components** trial or licensed version and locate the appropriate `ActionComponents.ddl` (for either iOS or Android) and click the **OK** button. The sample will now be ready to run.

# Features

**Action Nav Bar** features a fully documented API with full comments for every element. **Action Nav Bar** is fully customizable with user definable appearances for every element of its UI and its interface is built from vectors to be fully resolution independent.

# iOS Example

**Action Nav Bar** was designed to make adding it to an iOS project super easy. Open your `.xib` file in Xcode, insert a `UIView`, make it 80 pixels wide and pin it to the left hand side of the screen. Then change its Class to `ACNavBar`, next switch to the Editor view in Xcode and create an outlet for your `ACNavBar` called “navBar”.

Switch back to Visual Studio and quickly add buttons and views to your **Action Nav Bar** for it to control:

```csharp
using ActionComponents;
...

public override void ViewDidLoad ()
{
    UIImageView homeView = null;
    ACNavBarButton warning = null, delete = null, ticket = null;

    base.ViewDidLoad ();

    //---------------------------------------------
    // NavBar created from a UIView set to ACNavBar
    // in Xcode and saved to the .xib file
    //---------------------------------------------
    // Adjust the appearance of the NavBar
    navBar.appearance.border = UIColor.Gray;

    // Create a new view from code
    // Adjust view to match current interface orientation
    switch (UIApplication.SharedApplication.StatusBarOrientation) {
    case UIInterfaceOrientation.LandscapeLeft:
    case UIInterfaceOrientation.LandscapeRight:
        homeView = new UIImageView(new RectangleF(0,0,1024,748));
        homeView.Image=UIImage.FromFile("homeview.png");
        break;
    case UIInterfaceOrientation.Portrait:
    case UIInterfaceOrientation.PortraitUpsideDown:
        homeView = new UIImageView(new RectangleF(0,0,748,1004));
        homeView.Image=UIImage.FromFile("homeview-portrait.png");
        break;
    }

    //---------------------------------------------
    // Add buttons to the top of the bar
    //---------------------------------------------
    // The first button added to the top collection will automatically be selected
    ACNavBarButton home = navBar.top.AddButton (UIImage.FromFile ("Icons/house.png"), true, false);

    // Wire up request for this button's view
    home.RequestNewView += responder => {
        // Attaching a view to a button will automatically display it under the NavBar
        home.attachedView = homeView;
    };

    //Add an action to the home button
    home.Touched += responder => {
        // Hide warning notification in NavBar
        if (warning != null) warning.Hidden = true;

        // Disable the delete button
        if (delete != null) delete.Enabled = false;
    };

    // Request that the initial view being controlled by the NavBar be displayed
    navBar.DisplayDefaultView ();

    navBar.top.AddAutoDisposingButton (UIImage.FromFile ("Icons/bar-chart.png"), true, false).RequestNewView += responder => {
        // Build new view from a .xib file and attach it to the button it will automatically
        // be displayed under the NavBar
        responder.attachedView = BarChartView.Factory (this);
    };

    navBar.top.AddButton (UIImage.FromFile ("Icons/orgchart.png"), true, false).RequestNewView += responder => {
        responder.attachedView = OrgChartView.Factory (this);
    };

    ticket = navBar.top.AddButton (UIImage.FromFile ("Icons/ticket.png"), true, false);

    ticket.RequestNewView += responder => {
        responder.attachedView = TicketView.Factory (this);
    };

    ticket.Touched += responder => {
        // Enable the delete button
        if (delete != null) delete.Enabled = true;
    };

    //--------------------------------------------
    // Add buttons to the middle of the bar
    //--------------------------------------------
    navBar.middle.AddTool (UIImage.FromFile ("Icons/printer.png"), true, false).Touched += responder => {
        // Display Alert Dialog Box
        using (var alert = new UIAlertView ("NavBar", "Sorry but printing is not available at this time.", null, "OK", null)) {
            alert.Show ();    
        }

        // Display warning notification in NavBar
        if (warning != null) warning.Hidden = false;
    };

    delete = navBar.middle.AddTool (UIImage.FromFile ("Icons/trash.png"), false, false);
    delete.Touched += responder => {
        // Verify that the user really wants to stop downloading information
        var alert = new UIAlertView ("NavBar", "Delete Item?", null, "Cancel", "OK");
        // Wireup events
        alert.CancelButtonIndex = 0;
        alert.Clicked += (sender, buttonArgs) => {
            // Did the user verify termination?
            if (buttonArgs.ButtonIndex == 1) {
                // Yes
                delete.Enabled = false;
            }
        };

        // Display dialog
        alert.Show ();
    };

    //-----------------------------------------
    // Add buttons to the bottom of the bar
    //-----------------------------------------
    warning = navBar.bottom.AddNotification (UIImage.FromFile ("Icons/warning.png"), null, true);
    navBar.bottom.AddButton (UIImage.FromFile ("Icons/gear.png"), true, false).RequestNewView += responder => {
        responder.attachedView = SettingsView.Factory (this, navBar);
    };
}
```

**NOTE**: **NavBars** and the `UIViews` that they control can be completely created in C# code without using `.storyboard` or `.xib` files.

# Android Example

**Action Nav Bar** was designed to make adding it to an Android project super easy. Open your `Main.axml` file in Visual Studio, insert a `View`, make it 80 pixels wide and pin it to the left hand side of the screen. Then change its Class to `ACNavBar` then edit it’s Activity and make it look like the following:

```csharp
using ActionComponents;
...

[Activity (Label = "NavBarTestAndroid", MainLauncher = true)]
public class Activity1 : Activity
{
    #region Constants
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
        navBar = FindViewById (Resource.Id.navBar);
        viewHome = FindViewById (Resource.Id.viewHome);

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
            // Bring view into existence
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
            dialogMessage="Sorry but printing is not supported at this time";
            ShowDialog (DialogLongMessage);

            // Display warning notification in NavBar
            if (warning != null) warning.Hidden = false;
        };

        delete = navBar.middle.AddTool (Resource.Drawable.trash,false,false);

        delete.Touched += (responder) => {
            // Inform user (dialogMessage defined as a global variable)
            dialogMessage="Are you sure you what to delete the item?";
            ShowDialog (DialogLongMessageOkCancel);
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
            showHideNavBar = FindViewById (Resource.Id.showHideButton);

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

    protected override Dialog OnCreateDialog (int id)
    {
        Dialog alert = null;
        AlertDialog.Builder builder;

        base.OnCreateDialog (id);

        // Build requested dialog type
        switch (id){
        case DialogLongMessage:
            builder = new AlertDialog.Builder(this);
            builder.SetIcon (Android.Resource.Attribute.AlertDialogIcon);
            builder.SetTitle ("NavBar");
            builder.SetMessage(dialogMessage);
            builder.SetPositiveButton ("OK", delegate(object sender, DialogClickEventArgs e) {
                // Ignore for now
            });
            alert=builder.Create ();
            break;
        case DialogLongMessageOkCancel:
            builder = new AlertDialog.Builder(this);
            builder.SetIcon (Android.Resource.Attribute.AlertDialogIcon);
            builder.SetTitle ("NavBar");
            builder.SetMessage(dialogMessage);
            builder.SetPositiveButton ("OK", delegate(object sender, DialogClickEventArgs e) {
                // Ignore for now
            });
            builder.SetNegativeButton ("Cancel", delegate(object sender, DialogClickEventArgs e) {
                // Ignore for now
            });
            alert=builder.Create ();
            break;
        }

        // Return dialog
        return alert;
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
```

# Trial Version

The Trial version of **Action Nav Bar** is fully functional however the bar background is watermarked. The fully licensed version removes this watermark.
