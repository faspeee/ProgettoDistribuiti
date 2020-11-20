using SMRView.Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SMRView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MainWindowViewModel view;
        private TextBoxTraceListener _textBoxListener { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _textBoxListener = new TextBoxTraceListener(ResultB);
            Trace.Listeners.Add(_textBoxListener);
            view = new Program().Main();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await view.Add(int.Parse(ReqB.Text.ToString()));
        } 
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await view.Members();

        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await view.Factorial(int.Parse(ReqB.Text.ToString()));
        }
        public class TextBoxTraceListener : TraceListener
        {
            private TextBox _target;
            private StringSendDelegate _invokeWrite;
            public TextBoxTraceListener(TextBox target)
            {
                _target = target;
                _invokeWrite = new StringSendDelegate(SendString);
            }
            public override void Write(string message)
            { _target.Dispatcher.Invoke(_invokeWrite, new object[] { message }); }
            public override void WriteLine(string message)
            { _target.Dispatcher.Invoke(_invokeWrite, new object[] { message + Environment.NewLine }); }
            private delegate void StringSendDelegate(string message);
            private void SendString(string message)
            { _target.AppendText(message); }
        }
    }
}
