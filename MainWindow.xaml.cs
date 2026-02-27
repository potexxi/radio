using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace Radio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RadioClass radio;
        MediaPlayer player;
        string[] sender =
        {
            "https://edge72.radio.antennevorarlberg.at/av-live",
            "https://orf-live.ors-shoutcast.at/fm4-q2a",
            "https://frontend.streamonkey.net/radio886-rotweissrock/stream/mp3",
            "https://frontend.streamonkey.net/antoesterreich-live/stream/mp3",
            "http://stream.rockantenne.de/rockantenne/stream/mp3",
            "https://lounge877fmlagos-atunwadigital.streamguys1.com/lounge877fmlagos",
            "https://oesterreichradio.stream.laut.fm/oesterreichradio",
            "https://live.u1-radio.at/u1-tirol-live/",
            "https://orf-live.ors-shoutcast.at/stm-q2a",
            "https://stream.liferadio.tirol/live/mp3-192/",
            "http://radioklassikstephansdom.ice.infomaniak.ch/radioklassikstephansdom.mp3",
            "https://live.stream.radioarabella.de/radioarabella-goldenoldies/stream/mp3",
            "https://stream01.superfly.fm/live-mp3",
            "http://live.radiomaria.at/rma",
            "https://rs8.stream24.net/radio-soundportal.mp3",
            "https://secureonair.krone.at/kronehit-hd.mp3",
            "https://bass-high.rautemusik.fm/listen",
            "https://rock-high.rautemusik.fm/",
            "https://happy-high.rautemusik.fm/",
            "http://stream.104.6rtl.com/rtl-live/mp3-192/play.m3u",
            "http://stream.antenne.de/antenne/stream/mp3",
            "http://live.antenne.at/ak",
            "https://frontend.streamonkey.net/antsalzburg-live/stream/mp3",
            "https://hallefm.stream.laut.fm/hallefm",
            "https://orf-live.ors-shoutcast.at/oe3-q2a"
        };

        static public string[] names =
        {
            "Antenne Vorarlberg",
            "FM4",
            "Radio 88.6",
            "Antenne Österreich",
            "Rockantenne",
            "LoungeFM",
            "Österreich Radio",
            "Tirol U1",
            "Radio Steiermark",
            "Life Radio Tirol",
            "radio klassik Stephansdom",
            "Arabella Golden Oldies",
            "Radio Superfly",
            "Radio Maria Österreich",
            "Soundportal",
            "Kronehit",
            "Rautemusik BASS",
            "Rautemusik ROCK",
            "Rautemusik HAPPY",
            "RTL-Live",
            "Antenne Bayern",
            "Antenne Kärnten",
            "Antenne Salzburg",
            "Halle FM",
            "Hitradio Ö3"
        };

        public MainWindow()
        {
            InitializeComponent();
            Canvas canvas = CanvasFrequenz;
            radio = new RadioClass(canvas);
            RadioButton[] radioButtons = { Preset0, Preset1, Preset2, Preset3, Preset4 };
            radio.ReadStation(radioButtons);
            radio.ChangeRadioButtonColor();
            radio.ChangeRadioButtonColor();
            player = new MediaPlayer();
            PlayRadio();
        }

        private void ResetRadioButtons()
        {
            
        }

        private void PlayRadio()
        {
            player.Volume = radio.Volume / 100;
            player.Open(new Uri(sender[Convert.ToInt32(radio.Frequency) - Convert.ToInt32(radio.MinFrequency)]));
            LabelName.Content = names[Convert.ToInt32(radio.Frequency) - Convert.ToInt32(radio.MinFrequency)];
            player.Position = TimeSpan.Zero;
            player.Play();
        }

        private void ButtonVolumeMinus_Click(object sender, RoutedEventArgs e)
        {
            radio.VolumeDown();
            player.Volume = radio.Volume / 100;
            LabelVolume.Content = $"Volume: {radio.Volume}%";
        }

        private void ButtonVolumePlus_Click(object sender, RoutedEventArgs e)
        {
            radio.VolumeUp();
            player.Volume = radio.Volume / 100;
            LabelVolume.Content = $"Volume: {radio.Volume}%";
        }

        private void ButtonFrequenzMinus_Click(object sender, RoutedEventArgs e)
        {
            radio.FrequencyDown();
            player.Stop();
            PlayRadio();
        }

        private void ButtonFrequenzPlus_Click(object sender, RoutedEventArgs e)
        {
            radio.FrequencyUp();
            player.Stop();
            PlayRadio();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string name = names[Convert.ToInt32(radio.Frequency) - Convert.ToInt32(radio.MinFrequency)];
            if (Preset0.IsChecked == true)
                radio.SaveStation(0, Preset0, names);
            else if (Preset1.IsChecked == true)
                radio.SaveStation(1, Preset1, names);
            else if (Preset2.IsChecked == true)
                radio.SaveStation(2, Preset2, names);
            else if (Preset3.IsChecked == true)
                radio.SaveStation(3, Preset3, names);
            else if (Preset4.IsChecked == true)
                radio.SaveStation(4, Preset4, names);
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            if (Preset0.IsChecked == true)
                radio.LoadStation(0, Preset0, LabelVolume);
            else if (Preset1.IsChecked == true)
                radio.LoadStation(1, Preset1, LabelVolume);
            else if (Preset2.IsChecked == true)
                radio.LoadStation(2, Preset2, LabelVolume);
            else if (Preset3.IsChecked == true)
                radio.LoadStation(3, Preset3, LabelVolume);
            else if (Preset4.IsChecked == true)
                radio.LoadStation(4, Preset4, LabelVolume);
            PlayRadio();
        }
    }
}