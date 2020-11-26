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

        private async void Fibonacci_Click(object sender, RoutedEventArgs e)
        {
            await ControllerMatematica.Instance.Fibonacci(int.Parse(ReqB.Text.ToString()));
        }
        private async void Factorial_Click(object sender, RoutedEventArgs e)
        {
            await ControllerMatematica.Instance.Factorial(int.Parse(ReqB.Text.ToString()));
        }

        private async void Membri_Click(object sender, RoutedEventArgs e)
        {
            await ControllerMember.Instance.Members();

        }

        private async void Read_Click(object sender, RoutedEventArgs e)
        {
            await ControllerPersona.Instance.Read(Convert.ToInt32(idpersonBox.Text));
        }

        private async void ReadAll_Click(object sender, RoutedEventArgs e)
        {
            await ControllerPersona.Instance.ReadAll();
        }

        private async void Insert_Click(object sender, RoutedEventArgs e)
        {
            await ControllerPersona.Instance.Insert(CreateUpdatePersona);
        }
        private ZyzzyvagRPC.Services.PersonagRPC CreateUpdatePersona => new ZyzzyvagRPC.Services.PersonagRPC
        {
            Id = idpersonBox.Text!=string.Empty?Convert.ToInt32(idpersonBox.Text):0,
            Nome =nameBox.Text,
            Cognome = surNameBox.Text,
            Eta = Convert.ToInt32(etaBox.Text),
            HaMacchina = haMachinaBox.IsChecked.Value
        };
        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            await ControllerPersona.Instance.Update(CreateUpdatePersona);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            await ControllerPersona.Instance.Delete(Convert.ToInt32(idpersonBox.Text));
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
