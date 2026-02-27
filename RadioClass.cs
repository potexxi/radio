using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Text.Json;
using System.Diagnostics.Metrics;
using System.Windows;

namespace Radio
{
    public class RadioClass
    {
        public double MinFrequency { get; } = 84;
        public double MaxFrequency { get; } = 108;

        private double _frequency;
        public double Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                if (value <= MaxFrequency && value >= MinFrequency)
                    _frequency = value;
                else
                    return;
                    //throw new ArgumentException();
            }
        }

        private double _volume;
        public double Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (value < 0)
                    _volume = 0;
                else if (value > 100)
                    _volume = 100;
                else
                    _volume = value;
            }
        }

        private Preset[] _statiomemory;
        public Preset[] StationMemory
        {
            get
            {
                return _statiomemory;
            }
            private set
            {
                _statiomemory = value;
            }
        }

        private double _canvasLeftStart = 40;

        private Canvas canvasFrequency;

        private Line _lineFrequency;

        private Label _labelFrequency;

        public RadioClass(Canvas CanvasFrequency)
        {
            StationMemory = [new Preset(-1, -1, null, 0, null), new Preset(-1, -1, null, 1, null), new Preset(-1, -1, null, 2, null), new Preset(-1, -1, null, 3, null), new Preset(-1, -1, null, 4, null)];
            canvasFrequency = CanvasFrequency;
            Frequency = 84;
            Volume = 15;
            DrawFrequency();
        }

        public RadioClass(Canvas CanvasFrequency, double frequency, double volume): this(CanvasFrequency)
        {
            Frequency = frequency;
            Volume = volume;
            actualizeFrequency();
            ChangeRadioButtonColor();
        }

        private void actualizeFrequency()
        {
            double stepsLeft = Frequency - MinFrequency;
            _labelFrequency.Content = $"{Frequency}MHz";
            Canvas.SetLeft(_lineFrequency, (stepsLeft/2) * _canvasLeftStart + 40);
            Canvas.SetLeft(_labelFrequency, (stepsLeft/2) * _canvasLeftStart + 20);
        }

        public void DrawFrequency()
        {
            _lineFrequency = new Line();
            _labelFrequency = new Label();
            _lineFrequency.Stroke = Brushes.Red;
            _lineFrequency.StrokeThickness = 3;
            _lineFrequency.X1 = 0;
            _lineFrequency.X2 = 0;
            _lineFrequency.Y1 = 15;
            _lineFrequency.Y2 = 90;
            _labelFrequency.Foreground = Brushes.Red;
            _labelFrequency.FontSize = 10;
            _labelFrequency.Margin = new System.Windows.Thickness(0,-5,0,0);
            actualizeFrequency();
            int start = 84;
            for (int i = 0; i < 13; i++)
            {
                Line line = new Line();
                line.X1 = 0;
                line.Y1 = 40;
                line.X2 = 0;
                line.Y2 = 100;
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;
                canvasFrequency.Children.Add(line);
                Canvas.SetLeft(line, _canvasLeftStart * (i+1));

                Label label = new Label();
                label.Content = $"{start + (i*2)}MHz";
                label.FontSize = 10;
                canvasFrequency.Children.Add(label);
                Canvas.SetLeft(label, _canvasLeftStart * i + 20);
                Canvas.SetTop(label, 95);
            }
            canvasFrequency.Children.Add(_lineFrequency);
            canvasFrequency.Children.Add(_labelFrequency);
        }

        public void VolumeUp()
        {
            Volume += 5;
        }

        public void VolumeDown()
        {
            Volume -= 5;
        }

        public void FrequencyUp()
        {
            Frequency += 1;
            actualizeFrequency();
        }

        public void FrequencyDown()
        {
            Frequency -= 1;
            actualizeFrequency();
        }

        public void ChangeRadioButtonColor()
        {
            if (StationMemory == null)
                return;
            foreach(Preset preset in StationMemory)
            {
                if(preset.Volume != -1 && preset.Frequency != 0)
                {
                    if (preset.Name != "notset")
                        preset.radioButton.Content = preset.Name;
                    preset.radioButton.Foreground = Brushes.Green;
                }
            }
        }

        public void LoadStation(int index, RadioButton radioButton, Label volume)
        {
            if (radioButton.Foreground == Brushes.Red)
            {
                MessageBox.Show("Auf diesem Preset wurde keine Sicherung gefunden!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 
            Frequency = StationMemory[index].Frequency;
            Volume = StationMemory[index].Volume;
            ChangeRadioButtonColor();
            actualizeFrequency();
            volume.Content = $"Volume: {this.Volume}%";
        }

        public void SaveStation(int index, RadioButton radioButton, string[] names)
        {
            StationMemory[index].radioButton = radioButton;
            StationMemory[index].Frequency = Frequency;
            StationMemory[index].Volume = Volume;
            StationMemory[index].Name = names[Convert.ToInt32(Frequency) - Convert.ToInt32(MinFrequency)];
            ChangeRadioButtonColor();
            WriteStation(names);
        }

        public void WriteStation(string[] names)
        {
            using (StreamWriter writer = new StreamWriter("station.json", false))
            {
                int counter = 0;
                foreach (Preset preset in StationMemory)
                {
                    string name = "notset";
                    if (preset.Frequency != -1)
                        name = names[Convert.ToInt32(preset.Frequency) - Convert.ToInt32(preset.MinFrequency)];
                    writer.WriteLine("{" + $"\"Index\":{counter},\"Frequency\":{preset.Frequency},\"Volume\":{preset.Volume},\"Name\":\"{name}\"" + "}");
                    counter++;
                }
            }
        }

        public void ReadStation(RadioButton[] radioButtons)
        {
            using(StreamReader reader = new StreamReader("station.json"))
            {
                int counter = 0;
                while (!reader.EndOfStream)
                {
                    StationMemory[counter] = JsonSerializer.Deserialize<Preset>(reader.ReadLine());
                    StationMemory[counter].radioButton = radioButtons[counter];
                    counter++;
                }
            }
        }

        public string ToString()
        {
            return $"Radio: {Frequency} MHz - Volume: {Volume}%";
        }

    }
}
