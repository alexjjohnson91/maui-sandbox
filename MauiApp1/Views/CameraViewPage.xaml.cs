using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace MauiApp1
{
    public partial class CameraViewPage : ContentPage
    {
        public CameraViewPage()
        {
            InitializeComponent();
        }

        private void MyCamera_MediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            if (Dispatcher.IsDispatchRequired)
            {
                Dispatcher.Dispatch((() => MyImage.Source = ImageSource.FromStream(() => e.Media)));
                return;
            }
            MyImage.Source = ImageSource.FromStream(() => e.Media);
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await MyCamera.CaptureImage(CancellationToken.None);
            MyImage.IsVisible = true;
            Retake.IsVisible = true;
            MyCamera.IsVisible = false;
            Button.IsVisible = false;
        }

        private void Retake_Clicked(object sender, EventArgs e)
        {
            MyImage.IsVisible = false;
            Retake.IsVisible = false;
            MyCamera.IsVisible = true;
            Button.IsVisible = true;
        }
    }
}
