using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Media.SpeechRecognition;
using System.Diagnostics;
using Windows.Media.SpeechSynthesis;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace ChatterBot
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SpeechSynthesizer sp = new SpeechSynthesizer();
        SpeechSynthesisStream st;
        SpeechRecognizer Recognizer = new SpeechRecognizer();
        SpeechRecognitionResult res;
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                await listenForWakeupCall();
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }

            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async Task listenForWakeupCall()//a simulation of microphone button for starting to listen to the user!
        {
            String[] vals = { "start listening" };
            Recognizer.Constraints.Clear();
            Recognizer.Constraints.Add(new SpeechRecognitionListConstraint(vals, "strt"));
            await Recognizer.CompileConstraintsAsync();
            while (true)
            {
                res = await Recognizer.RecognizeAsync();
                if ((res.Status == SpeechRecognitionResultStatus.Success)) {
                    infoTextBox.Text = res.Text;
                    if (res.Text.ToLower().Equals("start listening")) {
                        break;
                    }
                }
            }
            await StartListening();
        }

        private async Task StartListening()//this method analyses the user's spoken data and sends it to analyse it and responds accordingly!
        {
            String[] vals = { "start listening", "exit"};
            Recognizer.Constraints.Clear();
            Recognizer.Constraints.Add(new SpeechRecognitionListConstraint(vals, "strt"));
            await Recognizer.CompileConstraintsAsync();
            res = await Recognizer.RecognizeAsync();
            while (!res.Text.ToLower().Equals("exit"))
            {
                mediaElement.Play();
                res = await Recognizer.RecognizeAsync();
                respond(res.Text.ToLower());
            }
            Application.Current.Exit();
        }

        private void respond(string q)//method to respond as per the user's query 'q'.
        {

        }
        private async void speakIt(String speak) {//speaks aloud the text specified by string 'speak'!
            st = await sp.SynthesizeTextToStreamAsync(speak);
            MediaElement media = new MediaElement();
            media.SetSource(st, st.ContentType);
            media.Play();
        }

    }
}
