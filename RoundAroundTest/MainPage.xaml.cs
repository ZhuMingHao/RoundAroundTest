using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RoundAroundTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        // Called when the image is ready to be rendered. ActualHeight and ActualWidth are known.
        private void Image_Opened(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Image img = sender as Image;
            RotateTransform rt = img.RenderTransform as RotateTransform;
            rt.CenterX = img.ActualWidth / 2;
            rt.CenterY = img.ActualHeight / 2;
        }

        // Called when rotate by touch.
        private void Left_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.LeftRotateTransform.Angle += e.Delta.Rotation;
        }

        // Called when rotate by mousewheel.
        private void Left_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            bool shift = (e.KeyModifiers & VirtualKeyModifiers.Shift) == VirtualKeyModifiers.Shift;
            bool ctrl = (e.KeyModifiers & VirtualKeyModifiers.Control) == VirtualKeyModifiers.Control;

            if (shift && ctrl)
            {
                var delta = PointerPoint.GetCurrentPoint(e.Pointer.PointerId).Properties.MouseWheelDelta;
                // With my mouse, delta is a multiple of 30.
                this.LeftRotateTransform.Angle -= delta / 15;
            }
        }

        // Called by touch and mouse.
        private void Right_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            // Uncomment to remove inertia.
            //if (e.IsInertial)
            //{
            //    return;
            //}

            // Alternatively, use Triangle Cosines Law.
            // It uses just one trigonometric function (Acos), but you first need to calculate the lengths of all sides.

            var x = this.RightRotateTransform.CenterX - e.Position.X;
            var y = this.RightRotateTransform.CenterY - e.Position.Y;

            double a1 = Math.Atan(y / x);
            double a2 = Math.Atan((e.Delta.Translation.Y - y) / (x - e.Delta.Translation.X));

            this.RightRotateTransform.Angle += a1 - a2;
        }
    }
}
