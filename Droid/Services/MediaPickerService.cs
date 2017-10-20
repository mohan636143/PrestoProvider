﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Support.V4.App;
using Provider.Droid.Activities;
using Provider.Droid.Services;
using Provider.Interface;
using Provider.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaPickerService))]
namespace Provider.Droid.Services
{
    public class MediaPickerService : IMedia
    {
        [Android.Runtime.Preserve(AllMembers = true)]
		public MediaPickerService()
		{

			this.context = Android.App.Application.Context;
			IsCameraAvailable = context.PackageManager.HasSystemFeature(PackageManager.FeatureCamera);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Gingerbread)
				IsCameraAvailable |= context.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFront);
		}

		
		public Task<bool> Initialize() => Task.FromResult(true);
		public bool IsCameraAvailable { get; }
		public bool IsTakePhotoSupported => true;
		public bool IsPickPhotoSupported => true;

		public Intent GetPickPhotoUI()
		{
			int id = GetRequestId();
			return CreateMediaIntent(id, "image/*", Intent.ActionPick, null, tasked: false);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public Android.Content.Intent GetTakePhotoUI(StoreCameraMediaOptions options)
		{
			if (!IsCameraAvailable)
				throw new NotSupportedException();

			VerifyOptions(options);

			int id = GetRequestId();
			return CreateMediaIntent(id, "image/*", MediaStore.ActionImageCapture, options, tasked: false);
		}

		public async Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null)
		{
			if (!(await RequestStoragePermission()))
			{
				return null;
			}
			var media = await TakeMediaAsync("image/*", Intent.ActionPick, null);

			if (options == null)
				return media;

			//check to see if we picked a file, and if so then try to fix orientation and resize
			if (!string.IsNullOrWhiteSpace(media?.Path))
			{
				try
				{
					await FixOrientationAndResizeAsync(media.Path, options.PhotoSize, options.CompressionQuality, options.CustomPhotoSize);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Unable to check orientation: " + ex);
				}
			}

			return media;
		}


		/// <summary>
		/// Take a photo async with specified options
		/// </summary>
		/// <param name="options">Camera Media Options</param>
		/// <returns>Media file of photo or null if canceled</returns>
		public async Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
		{
			if (!IsCameraAvailable)
				throw new NotSupportedException();

			if (!(await RequestStoragePermission()))
			{
				return null;
			}

			VerifyOptions(options);

			var media = await TakeMediaAsync("image/*", MediaStore.ActionImageCapture, options);

			if (string.IsNullOrWhiteSpace(media?.Path))
				return media;

			if (options.SaveToAlbum)
			{
				try
				{
					var fileName = System.IO.Path.GetFileName(media.Path);
					var publicUri = MediaPickerActivity.GetOutputMediaFile(context, options.Directory ?? "temp", fileName, true, true);
					using (System.IO.Stream input = File.OpenRead(media.Path))
					using (System.IO.Stream output = File.Create(publicUri.Path))
						input.CopyTo(output);

					media.AlbumPath = publicUri.Path;

					var f = new Java.IO.File(publicUri.Path);

					//MediaStore.Images.Media.InsertImage(context.ContentResolver,
					//    f.AbsolutePath, f.Name, null);

					try
					{
						Android.Media.MediaScannerConnection.ScanFile(context, new[] { f.AbsolutePath }, null, context as MediaPickerActivity);

						ContentValues values = new ContentValues();
						values.Put(MediaStore.Images.Media.InterfaceConsts.Title, System.IO.Path.GetFileNameWithoutExtension(f.AbsolutePath));
						values.Put(MediaStore.Images.Media.InterfaceConsts.Description, string.Empty);
						values.Put(MediaStore.Images.Media.InterfaceConsts.DateTaken, Java.Lang.JavaSystem.CurrentTimeMillis());
						values.Put(MediaStore.Images.ImageColumns.BucketId, f.ToString().ToLowerInvariant().GetHashCode());
						values.Put(MediaStore.Images.ImageColumns.BucketDisplayName, f.Name.ToLowerInvariant());
						values.Put("_data", f.AbsolutePath);

						var cr = context.ContentResolver;
						cr.Insert(MediaStore.Images.Media.ExternalContentUri, values);
					}
					catch (Exception ex1)
					{
						Console.WriteLine("Unable to save to scan file: " + ex1);
					}

					var contentUri = Android.Net.Uri.FromFile(f);
					var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile, contentUri);
					context.SendBroadcast(mediaScanIntent);
				}
				catch (Exception ex2)
				{
					Console.WriteLine("Unable to save to gallery: " + ex2);
				}
			}

			//check to see if we need to rotate if success

			try
			{
				await FixOrientationAndResizeAsync(media.Path, options.PhotoSize, options.CompressionQuality, options.CustomPhotoSize);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Unable to check orientation: " + ex);
			}


			return media;
		}
		private readonly Context context;
		private int requestId;
		private TaskCompletionSource<MediaFile> completionSource;
		int REQUEST_CODE_ASK_PERMISSIONS = 123;

		async Task<bool> RequestStoragePermission()
		{
			//We always have permission on anything lower than marshmallow.

			if ((int)Build.VERSION.SdkInt < 23)
				return true;

			const string permission = Manifest.Permission.Camera;
			if (ActivityCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.Camera) != (int)Permission.Granted)
			{
				return false;
			}

			return true;
		}

		public void ShowAlert(string str)
		{
			AlertDialog.Builder alert = new AlertDialog.Builder(Forms.Context);
			alert.SetTitle(str);
			alert.SetPositiveButton("OK", (senderAlert, args) => {
				// write your own set of instructions
			});

			//run the alert in UI thread to display in the screen
			alert.Show();
		}

		const string IllegalCharacters = "[|\\?*<\":>/']";
		private void VerifyOptions(StoreMediaOptions options)
		{
			if (options == null)
				throw new ArgumentNullException("options");
			if (System.IO.Path.IsPathRooted(options.Directory))
				throw new ArgumentException("options.Directory must be a relative path", "options");

			if (!string.IsNullOrWhiteSpace(options.Name))
				options.Name = Regex.Replace(options.Name, IllegalCharacters, string.Empty).Replace(@"\", string.Empty);


			if (!string.IsNullOrWhiteSpace(options.Directory))
				options.Directory = Regex.Replace(options.Directory, IllegalCharacters, string.Empty).Replace(@"\", string.Empty);
		}

		private Intent CreateMediaIntent(int id, string type, string action, StoreMediaOptions options, bool tasked = true)
		{
			Intent pickerIntent = new Intent(this.context, typeof(MediaPickerActivity));
			pickerIntent.PutExtra(MediaPickerActivity.ExtraId, id);
			pickerIntent.PutExtra(MediaPickerActivity.ExtraType, type);
			pickerIntent.PutExtra(MediaPickerActivity.ExtraAction, action);
			pickerIntent.PutExtra(MediaPickerActivity.ExtraTasked, tasked);

			if (options != null)
			{
				pickerIntent.PutExtra(MediaPickerActivity.ExtraPath, options.Directory);
				pickerIntent.PutExtra(MediaStore.Images.ImageColumns.Title, options.Name);

				var cameraOptions = (options as StoreCameraMediaOptions);
				if (cameraOptions != null)
				{
					if (cameraOptions.DefaultCamera == CameraDevice.Front)
					{
						pickerIntent.PutExtra("android.intent.extras.CAMERA_FACING", 1);
					}
					pickerIntent.PutExtra(MediaPickerActivity.ExtraSaveToAlbum, cameraOptions.SaveToAlbum);
				}
			}
			//pickerIntent.SetFlags(ActivityFlags.ClearTop);
			pickerIntent.SetFlags(ActivityFlags.NewTask);
			return pickerIntent;
		}

		private int GetRequestId()
		{
			int id = this.requestId;
			if (this.requestId == Int32.MaxValue)
				this.requestId = 0;
			else
				this.requestId++;

			return id;
		}

		private Task<MediaFile> TakeMediaAsync(string type, string action, StoreMediaOptions options)
		{
			int id = GetRequestId();

			var ntcs = new TaskCompletionSource<MediaFile>(id);
			if (Interlocked.CompareExchange(ref this.completionSource, ntcs, null) != null)
				throw new InvalidOperationException("Only one operation can be active at a time");

			this.context.StartActivity(CreateMediaIntent(id, type, action, options));

			EventHandler<MediaPickedEventArgs> handler = null;
			handler = (s, e) => {
				var tcs = Interlocked.Exchange(ref this.completionSource, null);

				MediaPickerActivity.MediaPicked -= handler;

				if (e.RequestId != id)
				{
					return;
				}
				if (e.IsCanceled)
				{
					if (tcs != null)
					{
						tcs.SetResult(null);
					}
				}
				else if (e.Error != null)
				{
					tcs.SetException(e.Error);
				}
				else
				{
					tcs.SetResult(e.Media);
				}
			};

			MediaPickerActivity.MediaPicked += handler;

			return completionSource.Task;
		}

		/// <summary>
		///  Rotate an image if required and saves it back to disk.
		/// </summary>
		/// <param name="filePath">The file image path</param>
		/// <param name="photoSize">Photo size to go to.</param>
		/// <returns>True if rotation or compression occured, else false</returns>
		public Task<bool> FixOrientationAndResizeAsync(string filePath, PhotoSize photoSize, int quality, int customPhotoSize)
		{
			if (string.IsNullOrWhiteSpace(filePath))
				return Task.FromResult(false);

			try
			{
				return Task.Run(() => {
					try
					{
						//First decode to just get dimensions
						var options = new BitmapFactory.Options
						{
							InJustDecodeBounds = true
						};

						//already on background task
						BitmapFactory.DecodeFile(filePath, options);

						var rotation = GetRotation(filePath);

						// if we don't need to rotate, aren't resizing, and aren't adjusting quality then simply return
						if (rotation == 0 && photoSize == PhotoSize.Full && quality == 100)
							return false;

						var percent = 1.0f;
						switch (photoSize)
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
								percent = (float)customPhotoSize / 100f;
								break;
						}


						var finalWidth = (int)(options.OutWidth * percent);
						var finalHeight = (int)(options.OutHeight * percent);

						//calculate sample size
						options.InSampleSize = CalculateInSampleSize(options, finalWidth, finalHeight);

						//turn off decode
						options.InJustDecodeBounds = false;


						//this now will return the requested width/height from file, so no longer need to scale
						var originalImage = BitmapFactory.DecodeFile(filePath, options);

						if (finalWidth != originalImage.Width || finalHeight != originalImage.Height)
						{
							originalImage = Bitmap.CreateScaledBitmap(originalImage, finalWidth, finalHeight, true);
						}
						//if we need to rotate then go for it.
						//then compresse it if needed
						if (rotation != 0)
						{
							var matrix = new Matrix();
							matrix.PostRotate(rotation);
							using (var rotatedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true))
							{

								//always need to compress to save back to disk
								using (var stream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite))
								{
									rotatedImage.Compress(Bitmap.CompressFormat.Jpeg, quality, stream);
									stream.Close();
								}
								rotatedImage.Recycle();
							}
							originalImage.Recycle();
							originalImage.Dispose();
							// Dispose of the Java side bitmap.
							GC.Collect();
							return true;
						}



						//always need to compress to save back to disk
						using (var stream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite))
						{
							originalImage.Compress(Bitmap.CompressFormat.Jpeg, quality, stream);
							stream.Close();
						}



						originalImage.Recycle();
						originalImage.Dispose();
						// Dispose of the Java side bitmap.
						GC.Collect();
						return true;

					}
					catch (Exception ex)
					{
#if DEBUG
						throw ex;
#else
                        return false;
#endif
					}
				});
			}
			catch (Exception ex)
			{
#if DEBUG
				throw ex;
#else
                return Task.FromResult(false);
#endif
			}

		}

		public int CalculateInSampleSize(
			BitmapFactory.Options options, int reqWidth, int reqHeight)
		{
			// Raw height and width of image
			var height = options.OutHeight;
			var width = options.OutWidth;
			var inSampleSize = 1;

			if (height > reqHeight || width > reqWidth)
			{

				var halfHeight = height / 2;
				var halfWidth = width / 2;

				// Calculate the largest inSampleSize value that is a power of 2 and keeps both
				// height and width larger than the requested height and width.
				while ((halfHeight / inSampleSize) >= reqHeight
						&& (halfWidth / inSampleSize) >= reqWidth)
				{
					inSampleSize *= 2;
				}
			}

			return inSampleSize;
		}

		/// <summary>
		/// Resize Image Async
		/// </summary>
		/// <param name="filePath">The file image path</param>
		/// <param name="photoSize">Photo size to go to.</param>
		/// <returns>True if rotation or compression occured, else false</returns>
		public Task<bool> ResizeAsync(string filePath, PhotoSize photoSize, int quality, int customPhotoSize)
		{
			if (string.IsNullOrWhiteSpace(filePath))
				return Task.FromResult(false);

			try
			{
				return Task.Run(() => {
					try
					{


						if (photoSize == PhotoSize.Full)
							return false;

						var percent = 1.0f;
						switch (photoSize)
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
								percent = (float)customPhotoSize / 100f;
								break;
						}


						//First decode to just get dimensions
						var options = new BitmapFactory.Options
						{
							InJustDecodeBounds = true
						};

						//already on background task
						BitmapFactory.DecodeFile(filePath, options);

						var finalWidth = (int)(options.OutWidth * percent);
						var finalHeight = (int)(options.OutHeight * percent);

						//calculate sample size
						options.InSampleSize = CalculateInSampleSize(options, finalWidth, finalHeight);

						//turn off decode
						options.InJustDecodeBounds = false;


						//this now will return the requested width/height from file, so no longer need to scale
						using (var originalImage = BitmapFactory.DecodeFile(filePath, options))
						{

							//always need to compress to save back to disk
							using (var stream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite))
							{

								originalImage.Compress(Bitmap.CompressFormat.Jpeg, quality, stream);
								stream.Close();
							}

							originalImage.Recycle();

							// Dispose of the Java side bitmap.
							GC.Collect();
							return true;
						}
					}
					catch (Exception ex)
					{
#if DEBUG
						throw ex;
#else
                        return false;
#endif
					}
				});
			}
			catch (Exception ex)
			{
#if DEBUG
				throw ex;
#else
                return Task.FromResult(false);
#endif
			}
		}


		static int GetRotation(string filePath)
		{
			try
			{
				using (var ei = new ExifInterface(filePath))
				{
					var orientation = (Orientation)ei.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Normal);

					switch (orientation)
					{
						case Orientation.Rotate90:
							return 90;
						case Orientation.Rotate180:
							return 180;
						case Orientation.Rotate270:
							return 270;
						default:
							return 0;
					}
				}
			}
			catch (Exception ex)
			{
#if DEBUG
				throw ex;
#else
                return 0;
#endif
			}
		}

	}
}
