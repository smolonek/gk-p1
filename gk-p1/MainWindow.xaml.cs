using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gk_p1
{

    ///  TODO:
    ///  obsluga ujemnych rozmiarow
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        double x, y;
        Point coords;
        Point rectStart;
        Point rectEnd;
        Rectangle rectMove = new Rectangle();
        Rectangle rectMovePrev = new Rectangle();
        private bool IsShiftKey { get; set; }
        bool doesRectExist = false;
        int rectNumber = 0;
        public MainWindow()
        {
            if (Debugger.IsAttached)
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && doesRectExist == false)
            {
                rectStart = e.GetPosition(this);
                x_value.Content = rectStart.X;
                y_value.Content = rectStart.Y;
            }
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && doesRectExist == false)
            {
                if (canvas.Children.Contains(rectMovePrev))
                    canvas.Children.Remove(rectMovePrev);
                coords = e.GetPosition(this);
                x_value_end.Content = coords.X;
                y_value_end.Content = coords.Y;
                rectMove.Width = Math.Abs(coords.X - rectStart.X);
                rectMove.Height = Math.Abs(coords.Y - rectStart.Y);
                rectMove.StrokeDashArray = new DoubleCollection() { 2 };
                rectMove.Stroke = new SolidColorBrush(Colors.Black);
                Canvas.SetLeft(rectMove, rectStart.X);
                Canvas.SetTop(rectMove, rectStart.Y);
                rectMovePrev = rectMove;
                canvas.Children.Add(rectMove);

            }
        }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Released && doesRectExist == false)
            {
                rectEnd.X = e.GetPosition(this).X;
                rectEnd.Y = e.GetPosition(this).Y;
                x_value_end.Content = rectEnd.X;
                y_value_end.Content = rectEnd.Y;
                Rectangle rectangle = new Rectangle();
                rectangle.Width = Math.Abs(rectEnd.X - rectStart.X);
                rectangle.Height = Math.Abs(rectEnd.Y - rectStart.Y);
                rectangle.Stroke = new SolidColorBrush(Colors.Red);
                rectangle.StrokeThickness = 2;
                Canvas.SetLeft(rectangle, rectStart.X);
                Canvas.SetTop(rectangle, rectStart.Y);
                rectangle.Fill = new SolidColorBrush(Colors.Blue);
                rectangle.Focusable = true;
                rectangle.Name = "rectangle" + rectNumber++;
                rectangle.KeyUp += new KeyEventHandler(rectangle_keydown);
                canvas.Children.Add(rectangle);
                FocusManager.SetFocusedElement(canvas, canvas.Children[canvas.Children.Count - 1]);
                canvas.Children.Remove(rectMovePrev);
                //doesRectExist = true;
            }
        }

        private void rectangle_keydown(object sender, KeyEventArgs e)
        {
            UIElement rectUI = canvas.Children[canvas.Children.Count - 1];
            Rectangle rect = (Rectangle)rectUI;
            IsShiftKey = Keyboard.Modifiers == ModifierKeys.Shift ? true : false;
            if (IsShiftKey)
            {
                if(e.Key == Key.Right || e.Key == Key.Up)
                {
                    rect.Width = rect.Width + 5;
                    rect.Height = rect.Height + 5;
                    rectUI = rect;
                }                
                else if (e.Key == Key.Left || e.Key == Key.Down)
                {
                    rect.Width = rect.Width - 5;
                    rect.Height = rect.Height - 5;
                    rectUI = rect;
                }
            }
            else
            {
                if (e.Key == Key.Left)
                {
                    Canvas.SetLeft(rectUI, Canvas.GetLeft(rectUI) - 10);
                }
                else if (e.Key == Key.Up)
                {
                    Canvas.SetTop(rectUI, Canvas.GetTop(rectUI) - 10);
                }
                else if (e.Key == Key.Right)
                {
                    Canvas.SetLeft(rectUI, Canvas.GetLeft(rectUI) + 10);
                }
                else if (e.Key == Key.Down)
                {
                    Canvas.SetTop(rectUI, Canvas.GetTop(rectUI) + 10);
                }
            }
        }
        //private void Canvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if(e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        //Line line = new Line();
        //        //line.Stroke = SystemColors.WindowFrameBrush;
        //        //line.X1 = coords.X;
        //        //line.Y1 = coords.Y;
        //        //line.X2 = e.GetPosition(this).X;
        //        //line.Y2 = e.GetPosition(this).Y;
        //        //Rectangle rectangle = new Rectangle();
        //        //rectangle.Stroke = new SolidColorBrush(Colors.Red);
        //        //Canvas.SetLeft(rectangle, e.GetPosition(this).X);
        //        //Canvas.SetTop(rectangle, e.GetPosition(this).Y);
        //        //coords = e.GetPosition(this);
        //        //x_value.Content = coords.X;
        //        //y_value.Content = coords.Y;
        //        //canvas.Children.Add(rectangle);
        //    }
        //}

        //private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    ////currentPoint = e.GetPosition(this);
        //    //if (e.ButtonState == MouseButtonState.Pressed)
        //    //    coords = e.GetPosition(this);
        //    //x_value.Content = coords.X;
        //    //y_value.Content = coords.Y;
        //    //Rectangle rectangle;
        //    //rectangle = new Rectangle
        //    //{
        //    //    Stroke = new SolidColorBrush(Colors.Red),
        //    //    Width = 200,
        //    //    Height = 100
        //    //};

        //    //Canvas.SetLeft(rectangle, 0);
        //    //Canvas.SetRight(rectangle, 0);
        //    //canvas.Children.Add(rectangle);
        //}
    }
}
