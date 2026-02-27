using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;

namespace Radio
{
    public class Preset
    {
        public string Name {  get; set; }

        public int Index {  get; set; }

        public double MinFrequency { get; } = 84;

        public double MaxFrequency { get; } = 108;

        public RadioButton radioButton;

        private double _frequency;

        public double Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                _frequency = value;
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
                _volume = value;
            }
        }

        public Preset() { }

        public Preset(double frequency, double volume, RadioButton radioButton, int index, string name)
        {
            Frequency = frequency;
            Volume = volume;
            this.radioButton = radioButton;
            Index = index;
            Name = name;
        }
    }
}
