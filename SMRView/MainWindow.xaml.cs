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
        private TextBoxTraceListener _textBoxListener { get; set; }
        private TextBoxTraceListener _textBoxListenerMember { get; set; }
        private TextBoxTraceListener _textBoxListenerPerson { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _textBoxListener = new TextBoxTraceListener(ResultB);
            _textBoxListenerMember = new TextBoxTraceListener(membri);
            _textBoxListenerPerson = new TextBoxTraceListener(person); 
            Trace.Listeners.Add(_textBoxListener);
            Trace.Listeners.Add(_textBoxListenerMember);
            Trace.Listeners.Add(_textBoxListenerPerson );

            
        }

        private async void fibonacci_Click(object sender, RoutedEventArgs e)
        {
            await ControllerMatematica.Instance.Fibonacci(int.Parse(ReqB.Text.ToString()));
        }
        private async void factorial_Click(object sender, RoutedEventArgs e)
        {
            await ControllerMatematica.Instance.Factorial(int.Parse(ReqB.Text.ToString()));
        }

        private async void membri_Click(object sender, RoutedEventArgs e)
        {
            await ControllerMember.Instance.Members();

        }

        private void read_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private async void readAll_Click(object sender, RoutedEventArgs e)
        {
            await ControllerPersona.Instance.ReadAll();
        }

        private async void insert_Click(object sender, RoutedEventArgs e)
        {
            await ControllerPersona.Instance.Insert(new ZyzzyvagRPC.Services.PersonagRPC { 
            
            Nome="Giovvanni",
            Cognome = "Mormone",
            Eta = -44,
            HaMacchina = true
            });
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

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
