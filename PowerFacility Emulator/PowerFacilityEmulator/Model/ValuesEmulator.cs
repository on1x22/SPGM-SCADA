using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFacilityEmulator.Model
{
    internal class ValuesEmulator
    {
        Facility _facility;
        static Facility initialFacility;
        public ValuesEmulator(Facility facility)
        {
            _facility = facility;
            //newFacility = _facility.
        }

        internal async Task<Facility> GenerateValuesAcync()
        {
            foreach(TopologicalNode node in _facility.TopologicalNodes)
            {
                foreach(TopologicalNodeValue nodeValue in node.TopologicalNodeValues)
                {
                    nodeValue.Value = GetRandomValue(nodeValue.NormalValue);
                    nodeValue.TimeStamp = DateTime.Now;
                }
                /*node.TopologicalNodeValues.Pgen = GetRandomValue2(node.TopologicalNodeValues.Pgen);
                node.TopologicalNodeValues.Qgen = GetRandomValue2(node.TopologicalNodeValues.Qgen);
                node.TopologicalNodeValues.Pcons = GetRandomValue2(node.TopologicalNodeValues.Pcons);
                node.TopologicalNodeValues.Qcons = GetRandomValue2(node.TopologicalNodeValues.Qcons);*/
            }
            await Task.Delay(1000);
            return _facility;
        }

        private static double GetRandomValue(double normalValue)
        {
            Random rnd = new Random();
            int probability = rnd.Next(0, 100);
            double dop = 0;
            if (probability == 99)
            {
                dop = 10 - Convert.ToDouble(rnd.Next(0, 2000)) / 100;
                return normalValue + dop;
            }
            dop = 10 - Convert.ToDouble(rnd.Next(0, 1200)) / 100;
            return normalValue + dop;
            /*if (normalValue != null)
            {
                //int oldValue = Convert.ToInt32(doubleValue);
                Random rnd = new Random();
                int probability = rnd.Next(0, 100);
                double dop = 0;
                if (probability == 99)
                {
                    dop = 10 - Convert.ToDouble(rnd.Next(0, 2000)) / 100;
                    return normalValue + dop;
                }
                dop = 10 - Convert.ToDouble(rnd.Next(0, 1200)) / 100;
                return normalValue + dop;
            }
            return null;*/
        }

        private double? GetRandomValue2(double? doubleValue, int i)
        {
            if (doubleValue != null)
            {

            }
            return null;
        }
    }
}
