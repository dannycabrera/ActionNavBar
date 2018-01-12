using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

namespace Android.Views 
{
	/// <summary>
	/// This example Android View class takes the ID of an image from Resource.Drawable.my-image-name and custom draws
	/// the image in the center of the View.
	/// </summary>
	/// <remarks>It also shows the best practice of caching drawing of the view to improve both performance and 
	/// memory usage on an Android device.</remarks>
	public class DrawImageView : View
	{
		#region Private Variables
		private int _imageID = 0;
		private Bitmap _imageCache;
		#endregion

		#region Computed Properties
		/// <summary>
		/// Gets or sets the ID of the image displayed in this view.
		/// </summary>
		/// <value>The ID of the image to display</value>
		/// <remarks>Can be set from Resource.Drawable.my-image-name given that the named image has been
		/// included in the Android project in Resources/drawable.</remarks>
		public int imageID {
			get{ return _imageID; }
			set{
				// Save the new image ID
				_imageID = value;

				// Force the view to redisplay itself
				Redraw ();
			}
		}
		#endregion 

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Android.Views.DrawImageView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public DrawImageView(Context context)
			: base(context)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Android.Views.DrawImageView"/> class and sets the default
		/// image to display
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="imageID">The ID of the initial image to display.</param>
		/// <remarks>The imageID can be set from Resource.Drawable.my-image-name given that the named image has been
		/// included in the Android project in Resources/drawable.</remarks>
		public DrawImageView(Context context, int imageID)
			: base(context)
		{
			// Save default image ID
			this._imageID = imageID;
		}
		#endregion 

		#region Public Methods
		/// <summary>
		/// Forces the View to dump any previous image cache and fully redraw itself
		/// </summary>
		public void Redraw(){

			//Clear any existing image cache
			if (_imageCache!=null) {
				_imageCache.Dispose ();
				_imageCache=null;
			}

			//Force this View to redraw
			this.Invalidate ();

		}
		#endregion 

		#region Private Methods
		/// <summary>
		/// Populates the image cache for this view containing the image from <c>imageID</c> drawn
		/// into the center of the View
		/// </summary>
		/// <returns>The image cache.</returns>
		private Bitmap PopulateImageCache(){

			// Create a temporary canvas
			var canvas=new Canvas();

			// Create bitmap storage and assign to canvas
			var viewBitmap=Bitmap.CreateBitmap (this.Width,this.Height,Bitmap.Config.Argb8888);
			canvas.SetBitmap (viewBitmap);

			// Draw Image
			if (_imageID!=0) {
				// Load image bitmap from resources
				Bitmap bitmap=BitmapFactory.DecodeResource(Resources,_imageID);

				// Get the image's height and width
				var h=bitmap.Height;
				var w=bitmap.Width;

				// Calculate the center position
				var l=((this.Width/2)-(w/2));
				var t=((this.Height/2)-(h/2));

				// Draw bitmap into canvas
				canvas.DrawBitmap (bitmap,null,new Rect(l,t,l+w,t+h),null);
			}

			// Return the cache with the image drawn into its center
			return viewBitmap;
		}
		#endregion

		#region Override Methods
		/// <summary>
		/// Overrides the Draw event for the given view and allows you to
		/// custom draw its contents.
		/// </summary>
		/// <param name="canvas">The View's Canvas.</param>
		protected override void OnDraw (Canvas canvas)
		{
			// Call the base drawing routine first
			base.OnDraw (canvas);

			// Restoring image from cache?
			if (_imageCache==null) _imageCache=PopulateImageCache();

			// Draw cached image to canvas
			canvas.DrawBitmap (_imageCache,0,0,null);

		}
		#endregion 
	}
}

