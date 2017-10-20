using System;
using System.Threading.Tasks;
using Provider.Models;

namespace Provider.Interface
{
    public interface IMedia
    {
		Task<bool> Initialize();

		bool IsCameraAvailable { get; }
		bool IsTakePhotoSupported { get; }
		bool IsPickPhotoSupported { get; }

		/// <summary>
		/// Picks a photo from the default gallery
		/// </summary>
		/// <returns>Media file or null if canceled</returns>
		Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null);

		/// <summary>
		/// Take a photo async with specified options
		/// </summary>
		/// <param name="options">Camera Media Options</param>
		/// <returns>Media file of photo or null if canceled</returns>
		Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options);

    }
}
