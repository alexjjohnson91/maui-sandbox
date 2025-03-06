using Plugin.Maui.OCR;

namespace MauiApp1.ViewModels;

using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Media;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ImageSource capturedImage;

    [ObservableProperty] private String recognizedText;

    public MainPageViewModel(ImageSource capturedImage, String recognizedText)
    {
        _ = InitializeOcr();
        this.capturedImage = capturedImage;
        this.recognizedText = recognizedText;
    }

    private static async Task InitializeOcr()
    {
        await OcrPlugin.Default.InitAsync();
    }

    [RelayCommand]
    public async Task CapturePhotoAsync()
    {
        try
        {
            // var photo = await MediaPicker.Default.CapturePhotoAsync();
            var photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo == null)
                return;

            var localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using (var sourceStream = await photo.OpenReadAsync())
            using (var localFileStream = File.OpenWrite(localFilePath))
            {
                await sourceStream.CopyToAsync(localFileStream);
            }

            byte[] imageAsBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var imageStream = File.OpenRead(localFilePath))
                {
                    await imageStream.CopyToAsync(memoryStream);
                }
                imageAsBytes = memoryStream.ToArray();
            }

            CapturedImage = ImageSource.FromFile(localFilePath);

            await SendToOcr(imageAsBytes);
        }
        catch (Exception ex)
        {
            // Handle error
            Console.WriteLine($"Capture failed: {ex.Message}");
        }
    }

    private async Task SendToOcr(byte[] byteArray)
    {
        try
        {
            var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(byteArray);

            if (!ocrResult.Success)
            {
                RecognizedText = "Could not recognize text";
                return;
            }

            RecognizedText = ocrResult.AllText;
        }
        catch (Exception ex)
        {
            RecognizedText = $"OCR Failed: {ex.Message}";
        }

    }
}
