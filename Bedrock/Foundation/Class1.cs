using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation
{

    /// <summary>
    /// Our main character
    /// </summary>
    public class Rock
    {
        private Random RNG = new Random();
        private Double size;
        private int age;
        /// <summary>
        /// Measured in g/cm^3
        /// </summary>
        private Double density;
        
        public Rock()
        {
            size = RNG.Next(2, 5);
            size = 2 * (Math.Pow(10, 6));
            age = 0;
            density = 2.65;
        }

        public double Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        public Double weight
        {
            get
            {
                return density * size;
            }
        }

        public Double Density
        {
            get
            {
                return density;
            }
            set
            {
                density = value;
            }
        }

        public int Age
        {
            get
            {
                return age;
            }
        }

        /// <summary>
        /// Erodes our rock (diminishes its size)
        /// </summary>
        /// <param name="years"></param>
        public void Erode(int years)
        {
            size = size - (RNG.NextDouble() * (years / 10));
            age = age + years;
        }

        /// <summary>
        /// A good erosion rate is somewhere between 0.0 and 1.0
        /// </summary>
        /// <param name="years"></param>
        /// <param name="erosionRate"></param>
        public void Erode(int years, double erosionRate)
        {
            size = size - (erosionRate * (years / 10));
            age = age + years;
        }
    }

    public class Events
    {
        private Random RNG = new Random();
        
        public string NextEvent()
        {
            return "";
        }
    }

}

