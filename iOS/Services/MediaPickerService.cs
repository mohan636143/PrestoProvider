using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssetsLibrary;
using CoreGraphics;
using Foundation;
using Provider.Interface;
using Provider.Models;
using UIKit;
using NSAction = global::System.Action;

namespace Provider.iOS.Services
{
	public class MediaPickerService : IMedia
	{
		/// <summary>
		/// Color of the status bar
		/// </summary>
		public static UIStatusBarStyle StatusBarStyle { get; set; }

		///<inheritdoc/>
		public Task<bool> Initialize() => Task.FromResult(true);

		/// <summary>
		/// Implementation
		/// </summary>
		public MediaPickerService()
		{
			StatusBarStyle = UIApplication.SharedApplication.StatusBarStyle;
			IsCameraAvailable = UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera);

			var availableCameraMedia = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.Camera) ?? new string[0];
			var avaialbleLibraryMedia = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary) ?? new string[0];

			foreach (string type in availableCameraMedia.Concat(avaialbleLibraryMedia))
			{
			    if (type == TypeImage)
					IsTakePhotoSupported = IsPickPhotoSupported = true;
			}
		}

		public bool IsCameraAvailable { get; }
		public bool IsTakePhotoSupported { get; }
		public bool IsPickPhotoSupported { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public MediaPickerController GetPickPhotoUI()
		{
			if (!IsPickPhotoSupported)
				throw new NotSupportedException();

			var d = new MediaPickerDelegate(null, UIImagePickerControllerSourceType.PhotoLibrary, null);
			return SetupController(d, UIImagePickerControllerSourceType.PhotoLibrary, TypeImage);
		}

		/// <summary>
		/// Picks a photo from the default gallery
		/// </summary>
		/// <returns>Media file or null if canceled</returns>
		public Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null)
		{
			if (!IsPickPhotoSupported)
				throw new NotSupportedException();

			CheckPhotoUsageDescription();

			var cameraOptions = new StoreCameraMediaOptions
			{
				PhotoSize = options?.PhotoSize ?? PhotoSize.Full,
				CompressionQuality = options?.CompressionQuality ?? 100
			};

			return GetMediaAsync(UIImagePickerControllerSourceType.PhotoLibrary, TypeImage, cameraOptions);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public MediaPickerController GetTakePhotoUI(StoreCameraMediaOptions options)
		{
			if (!IsTakePhotoSupported)
				throw new NotSupportedException();
			if (!IsCameraAvailable)
				throw new NotSupportedException();

			VerifyCameraOptions(options);

			var d = new MediaPickerDelegate(null, UIImagePickerControllerSourceType.PhotoLibrary, options);
			return SetupController(d, UIImagePickerControllerSourceType.Camera, TypeImage, options);
		}

		/// <summary>
		/// Take a photo async with specified options
		/// </summary>
		/// <param name="options">Camera Media Options</param>
		/// <returns>Media file of photo or null if canceled</returns>
		public Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
		{
			if (!IsTakePhotoSupported)
				throw new NotSupportedException();
			if (!IsCameraAvailable)
				throw new NotSupportedException();

			CheckCameraUsageDescription();

			VerifyCameraOptions(options);

			return GetMediaAsync(UIImagePickerControllerSourceType.Camera, TypeImage, options);
		}

		private UIPopoverController popover;
		private UIImagePickerControllerDelegate pickerDelegate;
		/// <summary>
		/// image type
		/// </summary>
		public const string TypeImage = "public.image";
		/// <summary>
		/// movie type
		/// </summary>
		public const string TypeMovie = "public.movie";

		private void VerifyOptions(StoreMediaOptions options)
		{
			if (options == null)
				throw new ArgumentNullException("options");
			if (options.Directory != null && Path.IsPathRooted(options.Directory))
				throw new ArgumentException("options.Directory must be a relative path", "options");
		}

		private void VerifyCameraOptions(StoreCameraMediaOptions options)
		{
			VerifyOptions(options);
			if (!Enum.IsDefined(typeof(CameraDevice), options.DefaultCamera))
				throw new ArgumentException("options.Camera is not a member of CameraDevice");
		}

		private static MediaPickerController SetupController(MediaPickerDelegate mpDelegate, UIImagePickerControllerSourceType sourceType, string mediaType, StoreCameraMediaOptions options = null)
		{
			var picker = new MediaPickerController(mpDelegate);
			picker.MediaTypes = new[] { mediaType };
			picker.SourceType = sourceType;

			if (sourceType == UIImagePickerControllerSourceType.Camera)
			{
				picker.CameraDevice = GetUICameraDevice(options.DefaultCamera);
				picker.AllowsEditing = options?.AllowCropping ?? false;

				if (options.OverlayViewProvider != null)
				{
					var overlay = options.OverlayViewProvider();
					if (overlay is UIView)
					{
						picker.CameraOverlayView = overlay as UIView;
					}
				}
				if (mediaType == TypeImage)
				{
					picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
				}
			}

			return picker;
		}

		private Task<MediaFile> GetMediaAsync(UIImagePickerControllerSourceType sourceType, string mediaType, StoreCameraMediaOptions options = null)
		{
			UIWindow window = UIApplication.SharedApplication.KeyWindow;
			if (window == null)
				throw new InvalidOperationException("There's no current active window");

			UIViewController viewController = window.RootViewController;

			if (viewController == null)
			{
				window = UIApplication.SharedApplication.Windows.OrderByDescending(w => w.WindowLevel).FirstOrDefault(w => w.RootViewController != null);
				if (window == null)
					throw new InvalidOperationException("Could not find current view controller");
				else
					viewController = window.RootViewController;
			}

			while (viewController.PresentedViewController != null)
				viewController = viewController.PresentedViewController;

			MediaPickerDelegate ndelegate = new MediaPickerDelegate(viewController, sourceType, options);
			var od = Interlocked.CompareExchange(ref pickerDelegate, ndelegate, null);
			if (od != null)
				throw new InvalidOperationException("Only one operation can be active at at time");

			var picker = SetupController(ndelegate, sourceType, mediaType, options);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad && sourceType == UIImagePickerControllerSourceType.PhotoLibrary)
			{
				ndelegate.Popover = new UIPopoverController(picker);
				ndelegate.Popover.Delegate = new MediaPickerPopoverDelegate(ndelegate, picker);
				ndelegate.DisplayPopover();
			}
			else
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
				{
					picker.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
				}
				viewController.PresentViewController(picker, true, null);
			}

			return ndelegate.Task.ContinueWith(t => {
				if (popover != null)
				{
					popover.Dispose();
					popover = null;
				}

				Interlocked.Exchange(ref pickerDelegate, null);
				return t;
			}).Unwrap();
		}

		private static UIImagePickerControllerCameraDevice GetUICameraDevice(CameraDevice device)
		{
			switch (device)
			{
				case CameraDevice.Front:
					return UIImagePickerControllerCameraDevice.Front;
				case CameraDevice.Rear:
					return UIImagePickerControllerCameraDevice.Rear;
				default:
					throw new NotSupportedException();
			}
		}


		void CheckCameraUsageDescription()
		{
			var info = NSBundle.MainBundle.InfoDictionary;

			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				if (!info.ContainsKey(new NSString("NSCameraUsageDescription")))
					throw new UnauthorizedAccessException("On iOS 10 and higher you must set NSCameraUsageDescription in your Info.plist file to enable Authorization Requests for Camera access!");
			}
		}

		void CheckPhotoUsageDescription()
		{
			var info = NSBundle.MainBundle.InfoDictionary;

			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				if (!info.ContainsKey(new NSString("NSPhotoLibraryUsageDescription")))
					throw new UnauthorizedAccessException("On iOS 10 and higher you must set NSPhotoLibraryUsageDescription in your Info.plist file to enable Authorization Requests for Photo Library access!");
			}
		}
	}

	public sealed class MediaPickerController : UIImagePickerController
	{
		internal MediaPickerController(MediaPickerDelegate mpDelegate)
		{
			base.Delegate = mpDelegate;
		}

		/// <summary>
		/// Deleage
		/// </summary>
		public override NSObject Delegate
		{
			get { return base.Delegate; }
			set
			{
				if (value == null)
					base.Delegate = value;
				else
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets result of picker
		/// </summary>
		/// <returns></returns>
		public Task<MediaFile> GetResultAsync() =>
			((MediaPickerDelegate)Delegate).Task;
	}

	internal class MediaPickerDelegate : UIImagePickerControllerDelegate
	{
		internal MediaPickerDelegate(UIViewController viewController, UIImagePickerControllerSourceType sourceType, StoreCameraMediaOptions options)
		{
			this.viewController = viewController;
			this.source = sourceType;
			this.options = options ?? new StoreCameraMediaOptions();

			if (viewController != null)
			{
				UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
				observer = NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, DidRotate);
			}
		}

		public UIPopoverController Popover
		{
			get;
			set;
		}

		public UIView View
		{
			get { return viewController.View; }
		}

		public Task<MediaFile> Task
		{
			get { return tcs.Task; }
		}

		public override async void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
		{
			RemoveOrientationChangeObserverAndNotifications();

			MediaFile mediaFile;
			switch ((NSString)info[UIImagePickerController.MediaType])
			{
                case MediaPickerService.TypeImage:
					mediaFile = await GetPictureMediaFile(info);
					break;

				default:
					throw new NotSupportedException();
			}

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
			{
				UIApplication.SharedApplication.SetStatusBarStyle(MediaPickerService.StatusBarStyle, false);
			}

			Dismiss(picker, () => {


				tcs.TrySetResult(mediaFile);
			});
		}

		public override void Canceled(UIImagePickerController picker)
		{
			RemoveOrientationChangeObserverAndNotifications();

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
			{
				UIApplication.SharedApplication.SetStatusBarStyle(MediaPickerService.StatusBarStyle, false);
			}

			Dismiss(picker, () => {


				tcs.SetResult(null);
			});
		}

		public void DisplayPopover(bool hideFirst = false)
		{
			if (Popover == null)
				return;

			var swidth = UIScreen.MainScreen.Bounds.Width;
			var sheight = UIScreen.MainScreen.Bounds.Height;

			nfloat width = 400;
			nfloat height = 300;


			if (orientation == null)
			{
				if (IsValidInterfaceOrientation(UIDevice.CurrentDevice.Orientation))
					orientation = UIDevice.CurrentDevice.Orientation;
				else
					orientation = GetDeviceOrientation(this.viewController.InterfaceOrientation);
			}

			nfloat x, y;
			if (orientation == UIDeviceOrientation.LandscapeLeft || orientation == UIDeviceOrientation.LandscapeRight)
			{
				y = (swidth / 2) - (height / 2);
				x = (sheight / 2) - (width / 2);
			}
			else
			{
				x = (swidth / 2) - (width / 2);
				y = (sheight / 2) - (height / 2);
			}

			if (hideFirst && Popover.PopoverVisible)
				Popover.Dismiss(animated: false);

			Popover.PresentFromRect(new CGRect(x, y, width, height), View, 0, animated: true);
		}

		private UIDeviceOrientation? orientation;
		private NSObject observer;
		private readonly UIViewController viewController;
		private readonly UIImagePickerControllerSourceType source;
		private TaskCompletionSource<MediaFile> tcs = new TaskCompletionSource<MediaFile>();
		private readonly StoreCameraMediaOptions options;

		private bool IsCaptured
		{
			get { return source == UIImagePickerControllerSourceType.Camera; }
		}

		private void Dismiss(UIImagePickerController picker, NSAction onDismiss)
		{
			if (viewController == null)
			{
				onDismiss();
				tcs = new TaskCompletionSource<MediaFile>();
			}
			else
			{
				if (Popover != null)
				{
					Popover.Dismiss(animated: true);
					Popover.Dispose();
					Popover = null;

					onDismiss();
				}
				else
				{
					picker.DismissViewController(true, onDismiss);
					picker.Dispose();
				}
			}
		}

		private void RemoveOrientationChangeObserverAndNotifications()
		{
			if (viewController != null)
			{
				UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();
				NSNotificationCenter.DefaultCenter.RemoveObserver(observer);
				observer.Dispose();
			}
		}

		private void DidRotate(NSNotification notice)
		{
			UIDevice device = (UIDevice)notice.Object;
			if (!IsValidInterfaceOrientation(device.Orientation) || Popover == null)
				return;
			if (orientation.HasValue && IsSameOrientationKind(orientation.Value, device.Orientation))
				return;

			if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
			{
				if (!GetShouldRotate6(device.Orientation))
					return;
			}
			else if (!GetShouldRotate(device.Orientation))
				return;

			UIDeviceOrientation? co = orientation;
			orientation = device.Orientation;

			if (co == null)
				return;

			DisplayPopover(hideFirst: true);
		}

		private bool GetShouldRotate(UIDeviceOrientation orientation)
		{
			UIInterfaceOrientation iorientation = UIInterfaceOrientation.Portrait;
			switch (orientation)
			{
				case UIDeviceOrientation.LandscapeLeft:
					iorientation = UIInterfaceOrientation.LandscapeLeft;
					break;

				case UIDeviceOrientation.LandscapeRight:
					iorientation = UIInterfaceOrientation.LandscapeRight;
					break;

				case UIDeviceOrientation.Portrait:
					iorientation = UIInterfaceOrientation.Portrait;
					break;

				case UIDeviceOrientation.PortraitUpsideDown:
					iorientation = UIInterfaceOrientation.PortraitUpsideDown;
					break;

				default: return false;
			}

			return viewController.ShouldAutorotateToInterfaceOrientation(iorientation);
		}

		private bool GetShouldRotate6(UIDeviceOrientation orientation)
		{
			if (!viewController.ShouldAutorotate())
				return false;

			UIInterfaceOrientationMask mask = UIInterfaceOrientationMask.Portrait;
			switch (orientation)
			{
				case UIDeviceOrientation.LandscapeLeft:
					mask = UIInterfaceOrientationMask.LandscapeLeft;
					break;

				case UIDeviceOrientation.LandscapeRight:
					mask = UIInterfaceOrientationMask.LandscapeRight;
					break;

				case UIDeviceOrientation.Portrait:
					mask = UIInterfaceOrientationMask.Portrait;
					break;

				case UIDeviceOrientation.PortraitUpsideDown:
					mask = UIInterfaceOrientationMask.PortraitUpsideDown;
					break;

				default: return false;
			}

			return viewController.GetSupportedInterfaceOrientations().HasFlag(mask);
		}

		private async Task<MediaFile> GetPictureMediaFile(NSDictionary info)
		{
			var image = (UIImage)info[UIImagePickerController.EditedImage];
			if (image == null)
				image = (UIImage)info[UIImagePickerController.OriginalImage];

			var meta = info[UIImagePickerController.MediaMetadata] as NSDictionary;


            string path = GetOutputPath(MediaPickerService.TypeImage,
				options.Directory ?? ((IsCaptured) ? String.Empty : "temp"),
				options.Name);

			var cgImage = image.CGImage;

			if (options.PhotoSize != PhotoSize.Full)
			{
				try
				{
					var percent = 1.0f;
					switch (options.PhotoSize)
					{
						case PhotoSize.Large:
							percent = .75f;
							break;
						case PhotoSize.Medium:
							percent = .5f;
							break;
						case PhotoSize.Small:
							percent = .25f;
							break;
						case PhotoSize.Custom:
							percent = (float)options.CustomPhotoSize / 100f;
							break;
					}

					//calculate new size
					var width = (image.CGImage.Width * percent);
					var height = (image.CGImage.Height * percent);

					//begin resizing image
					image = image.ResizeImageWithAspectRatio(width, height);

				}
				catch (Exception ex)
				{
					Console.WriteLine($"Unable to compress image: {ex}");
				}
			}

			//iOS quality is 0.0-1.0
			var quality = (options.CompressionQuality / 100f);
			image.AsJPEG(quality).Save(path, true);

			Action<bool> dispose = null;
			string aPath = null;
			if (source != UIImagePickerControllerSourceType.Camera)
			{
				dispose = d => File.Delete(path);

				//try to get the album path's url
				var url = (NSUrl)info[UIImagePickerController.ReferenceUrl];
				aPath = url?.AbsoluteString;
			}
			else
			{
				if (this.options.SaveToAlbum)
				{
					try
					{
						var library = new ALAssetsLibrary();
						var albumSave = await library.WriteImageToSavedPhotosAlbumAsync(cgImage, meta);
						aPath = albumSave.AbsoluteString;
					}
					catch (Exception ex)
					{
						Console.WriteLine("unable to save to album:" + ex);
					}
				}

			}

			return new MediaFile(path, () => File.OpenRead(path), dispose: dispose, albumPath: aPath);
		}



		private async Task<MediaFile> GetMovieMediaFile(NSDictionary info)
		{
			NSUrl url = (NSUrl)info[UIImagePickerController.MediaURL];

            string path = GetOutputPath(MediaPickerService.TypeMovie,
					  options.Directory ?? ((IsCaptured) ? String.Empty : "temp"),
					  options.Name ?? Path.GetFileName(url.Path));

			File.Move(url.Path, path);

			string aPath = null;
			Action<bool> dispose = null;
			if (source != UIImagePickerControllerSourceType.Camera)
			{
				dispose = d => File.Delete(path);
				//try to get the album path's url
				var url2 = (NSUrl)info[UIImagePickerController.ReferenceUrl];
				aPath = url2?.AbsoluteString;
			}
			else
			{
				if (this.options.SaveToAlbum)
				{
					try
					{
						var library = new ALAssetsLibrary();
						var albumSave = await library.WriteVideoToSavedPhotosAlbumAsync(new NSUrl(path));
						aPath = albumSave.AbsoluteString;
					}
					catch (Exception ex)
					{
						Console.WriteLine("unable to save to album:" + ex);
					}
				}
			}

			return new MediaFile(path, () => File.OpenRead(path), dispose: dispose, albumPath: aPath);
		}

		private static string GetUniquePath(string type, string path, string name)
		{
            bool isPhoto = (type == MediaPickerService.TypeImage);
			string ext = Path.GetExtension(name);
			if (ext == String.Empty)
				ext = ((isPhoto) ? ".jpg" : ".mp4");

			name = Path.GetFileNameWithoutExtension(name);

			string nname = name + ext;
			int i = 1;
			while (File.Exists(Path.Combine(path, nname)))
				nname = name + "_" + (i++) + ext;

			return Path.Combine(path, nname);
		}

		private static string GetOutputPath(string type, string path, string name)
		{
			path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);
			Directory.CreateDirectory(path);

			if (String.IsNullOrWhiteSpace(name))
			{
				string timestamp = DateTime.Now.ToString("yyyMMdd_HHmmss");
                if (type == MediaPickerService.TypeImage)
					name = "IMG_" + timestamp + ".jpg";
				else
					name = "VID_" + timestamp + ".mp4";
			}

			return Path.Combine(path, GetUniquePath(type, path, name));
		}

		private static bool IsValidInterfaceOrientation(UIDeviceOrientation self)
		{
			return (self != UIDeviceOrientation.FaceUp && self != UIDeviceOrientation.FaceDown && self != UIDeviceOrientation.Unknown);
		}

		private static bool IsSameOrientationKind(UIDeviceOrientation o1, UIDeviceOrientation o2)
		{
			if (o1 == UIDeviceOrientation.FaceDown || o1 == UIDeviceOrientation.FaceUp)
				return (o2 == UIDeviceOrientation.FaceDown || o2 == UIDeviceOrientation.FaceUp);
			if (o1 == UIDeviceOrientation.LandscapeLeft || o1 == UIDeviceOrientation.LandscapeRight)
				return (o2 == UIDeviceOrientation.LandscapeLeft || o2 == UIDeviceOrientation.LandscapeRight);
			if (o1 == UIDeviceOrientation.Portrait || o1 == UIDeviceOrientation.PortraitUpsideDown)
				return (o2 == UIDeviceOrientation.Portrait || o2 == UIDeviceOrientation.PortraitUpsideDown);

			return false;
		}

		private static UIDeviceOrientation GetDeviceOrientation(UIInterfaceOrientation self)
		{
			switch (self)
			{
				case UIInterfaceOrientation.LandscapeLeft:
					return UIDeviceOrientation.LandscapeLeft;
				case UIInterfaceOrientation.LandscapeRight:
					return UIDeviceOrientation.LandscapeRight;
				case UIInterfaceOrientation.Portrait:
					return UIDeviceOrientation.Portrait;
				case UIInterfaceOrientation.PortraitUpsideDown:
					return UIDeviceOrientation.PortraitUpsideDown;
				default:
					throw new InvalidOperationException();
			}
		}
	}

	public static class UIImageExtensions
	{
		/// <summary>
		/// Resize image to maximum size
		/// keeping the aspect ratio
		/// </summary>
		public static UIImage ResizeImageWithAspectRatio(this UIImage sourceImage, float maxWidth, float maxHeight)
		{
			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1) return sourceImage;
			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;
			UIGraphics.BeginImageContext(new CGSize(width, height));
			sourceImage.Draw(new CGRect(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage;
		}

		/// <summary>
		/// Resize image, but ignore the aspect ratio
		/// </summary>
		/// <param name="sourceImage"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static UIImage ResizeImage(this UIImage sourceImage, float width, float height)
		{
			UIGraphics.BeginImageContext(new SizeF(width, height));
			sourceImage.Draw(new RectangleF(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage;
		}

		/// <summary>
		/// Crop image to specitic size and at specific coordinates
		/// </summary>
		/// <param name="sourceImage"></param>
		/// <param name="crop_x"></param>
		/// <param name="crop_y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static UIImage CropImage(this UIImage sourceImage, int crop_x, int crop_y, int width, int height)
		{
			var imgSize = sourceImage.Size;
			UIGraphics.BeginImageContext(new SizeF(width, height));
			var context = UIGraphics.GetCurrentContext();
			var clippedRect = new RectangleF(0, 0, width, height);
			context.ClipToRect(clippedRect);
			var drawRect = new CGRect(-crop_x, -crop_y, imgSize.Width, imgSize.Height);
			sourceImage.Draw(drawRect);
			var modifiedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return modifiedImage;
		}
	}
	internal class MediaPickerPopoverDelegate : UIPopoverControllerDelegate
	{
		internal MediaPickerPopoverDelegate(MediaPickerDelegate pickerDelegate, UIImagePickerController picker)
		{
			this.pickerDelegate = pickerDelegate;
			this.picker = picker;
		}

		public override bool ShouldDismiss(UIPopoverController popoverController) => true;

		public override void DidDismiss(UIPopoverController popoverController) =>
			pickerDelegate.Canceled(picker);

		private readonly MediaPickerDelegate pickerDelegate;
		private readonly UIImagePickerController picker;
	}
}
