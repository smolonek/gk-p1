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
        /// <summary>
        /// Co ma zostać narysowane. 0 - linia, 1 - prostokąt, 2 - okrąg
        /// </summary>
        private int DRAW_MODE = 0;
        /// <summary>
        /// Tryb pracy. 0 - rysowanie, 1 - zmiana położenia, 2 - zmiana rozmiaru
        /// </summary>
        private int MODE = 0;
        double x, y;
        Point coords;
        Point figureStart;
        Point figureEnd;
        Point movecoords;
        Rectangle rectMove = new Rectangle();
        Rectangle rectMovePrev = new Rectangle();
        //Line line = new Line();
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
            if (e.ButtonState == MouseButtonState.Pressed && MODE == 0 && doesRectExist == false)
            {
                figureStart = e.GetPosition(this);
            }
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && MODE == 0 && doesRectExist == false)
            {
                if(DRAW_MODE == 0)
                {

                }
                else if (DRAW_MODE == 1)
                {
                    if (canvas.Children.Contains(rectMovePrev))
                        canvas.Children.Remove(rectMovePrev);
                    coords = e.GetPosition(this);
                    rectMove.Width = Math.Abs(coords.X - figureStart.X);
                    rectMove.Height = Math.Abs(coords.Y - figureStart.Y);
                    rectMove.StrokeDashArray = new DoubleCollection() { 2 };
                    rectMove.Stroke = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(rectMove, figureStart.X);
                    Canvas.SetTop(rectMove, figureStart.Y);
                    rectMovePrev = rectMove;
                    canvas.Children.Add(rectMove);
                }
                else
                {

                }

            }
        }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Released && MODE == 0 && doesRectExist == false)
            {
                figureEnd.X = e.GetPosition(this).X;
                figureEnd.Y = e.GetPosition(this).Y;
                if(DRAW_MODE == 0)
                {
                    Line line = new Line();
                    line.X1 = figureStart.X;
                    line.Y1 = figureStart.Y;
                    line.X2 = figureEnd.X;
                    line.Y2 = figureEnd.Y;
                    line.Stroke = new SolidColorBrush(Colors.Red);
                    line.StrokeThickness = 2;
                    line.MouseLeftButtonDown += line_MouseLeftButtonDown;
                    line.MouseLeftButtonUp += line_MouseLeftButtonUp;
                    line.MouseMove += line_MouseMove;
                    canvas.Children.Add(line);
                }
                else if(DRAW_MODE == 1)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = Math.Abs(figureEnd.X - figureStart.X);
                    rectangle.Height = Math.Abs(figureEnd.Y - figureStart.Y);
                    rectangle.Stroke = new SolidColorBrush(Colors.Red);
                    rectangle.StrokeThickness = 2;
                    Canvas.SetLeft(rectangle, figureStart.X);
                    Canvas.SetTop(rectangle, figureStart.Y);
                    rectangle.Fill = new SolidColorBrush(Colors.Blue);
                    rectangle.Focusable = true;
                    rectangle.Name = "rectangle" + rectNumber++;
                    rectangle.KeyUp += new KeyEventHandler(rectangle_keydown);
                    rectangle.MouseLeftButtonDown += rect_MouseLeftButtonDown;
                    rectangle.MouseLeftButtonUp += rect_MouseLeftButtonUp;
                    rectangle.MouseMove += rect_MouseMove;
                    canvas.Children.Add(rectangle);
                    FocusManager.SetFocusedElement(canvas, canvas.Children[canvas.Children.Count - 1]);
                    canvas.Children.Remove(rectMovePrev);
                    //doesRectExist = true;
                }
                else
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Height = 2 * GetDistance(figureStart, e.GetPosition(this));
                    ellipse.Width = ellipse.Height;
                    ellipse.StrokeThickness = 2;
                    ellipse.Stroke = new SolidColorBrush(Colors.Green);
                    ellipse.MouseLeftButtonDown += ellipse_MouseLeftButtonDown;
                    ellipse.MouseLeftButtonUp += ellipse_MouseLeftButtonUp;
                    ellipse.MouseMove += ellipse_MouseMove;
                    Canvas.SetLeft(ellipse, figureStart.X - ellipse.Width/2);
                    Canvas.SetTop(ellipse, figureStart.Y - ellipse.Width / 2);
                    canvas.Children.Add(ellipse);
                }
            }
        }
        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            movecoords = e.GetPosition(this);
            //MessageBox.Show(movecoords.ToString());
        }

        private void ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            var rect = (Ellipse)sender;
            if (e.LeftButton == MouseButtonState.Pressed && MODE == 1)
            {
                // get the position of the mouse relative to the Canvas
                var mousePos = e.GetPosition(canvas);
                //MessageBox.Show(movecoords.ToString());
                // center the rect on the mouse
                double left = mousePos.X - (rect.ActualWidth / 2);
                double top = mousePos.Y - (rect.ActualHeight / 2);
                Canvas.SetLeft(rect, left);
                Canvas.SetTop(rect, top);
            }
        }

        private void ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Ellipse rect = (Ellipse)sender;
            if (!rect.IsMouseCaptured) return;
            Point endpos = e.GetPosition(canvas);
            canvas.Children.Remove(rect);
            Canvas.SetLeft(rect, endpos.X);
            Canvas.SetTop(rect, endpos.Y);
            //canvas.Children.Add(rect);
        }
        private void Line_Click(object sender, RoutedEventArgs e)
        {
            DRAW_MODE = 0;
        }

        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            DRAW_MODE = 1;
        }

        private void Circle_Click(object sender, RoutedEventArgs e)
        {
            DRAW_MODE = 2;
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
        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2));
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            MODE = 0;
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            MODE = 1;
        }

        private void Resize_Click(object sender, RoutedEventArgs e)
        {
            MODE = 2;
        }
        private void line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            movecoords = e.GetPosition(this);
            //MessageBox.Show(movecoords.ToString());
        }

        private void line_MouseMove(object sender, MouseEventArgs e)
        {
            var rect = (Line)sender;
            if (e.LeftButton == MouseButtonState.Pressed && MODE == 1)
            {
                // get the position of the mouse relative to the Canvas
                var mousePos = e.GetPosition(canvas);
                //MessageBox.Show(movecoords.ToString());
                // center the rect on the mouse
                //MessageBox.Show(rect.ActualHeight.ToString());
                double left = mousePos.X - (rect.X1 / 2);
                double top = mousePos.Y - (rect.Y1 / 2);
                rect.X1 = mousePos.X;
                rect.Y1 = mousePos.Y;
                rect.X2 = rect.X2 + (rect.X2 - rect.X1);
                //rect.Y2 = rect.Y2 + (rect.Y2 - rect.Y1);
                //Canvas.SetLeft(rect, left);
                //Canvas.SetTop(rect, top);
            }
        }

        private void line_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Line rect = (Line)sender;
            if (!rect.IsMouseCaptured) return;
            Point endpos = e.GetPosition(canvas);
            canvas.Children.Remove(rect);
            Canvas.SetLeft(rect, endpos.X);
            Canvas.SetTop(rect, endpos.Y);
            //canvas.Children.Add(rect);
        }
        private void rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            movecoords = e.GetPosition(this);
            //MessageBox.Show(movecoords.ToString());
        }

        private void rect_MouseMove(object sender, MouseEventArgs e)
        {
            var rect = (Rectangle)sender;
            if (e.LeftButton == MouseButtonState.Pressed && MODE == 1)
            {
                // get the position of the mouse relative to the Canvas
                var mousePos = e.GetPosition(canvas);
                //MessageBox.Show(movecoords.ToString());
                // center the rect on the mouse
                double left = mousePos.X - (rect.ActualWidth / 2);
                double top = mousePos.Y - (rect.ActualHeight / 2);
                Canvas.SetLeft(rect, left);
                Canvas.SetTop(rect, top);
            }
        }

        private void rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            if (!rect.IsMouseCaptured) return;
            Point endpos = e.GetPosition(canvas);
            canvas.Children.Remove(rect);
            Canvas.SetLeft(rect, endpos.X);
            Canvas.SetTop(rect, endpos.Y);
            //canvas.Children.Add(rect);
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
