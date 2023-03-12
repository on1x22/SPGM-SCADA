using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFacilityEmulator.Model
{    
    public class TopologicalNodeValue
    {
        /*public double? Pgen { get; set; }
        public double? Qgen { get; set; }
        public double? Pcons { get; set; }
        public double? Qcons { get; set; }*/
        public string Description { get; set; }
        [JsonIgnore]
        public double NormalValue { get; set; }
        public double Value { get; set; }
        public int QCode { get; set; }
        public DateTime TimeStamp { get; set; }

        public override string ToString()
        {
            return $"Description={Description}, Value={Value}, QCode={QCode}, TimeStamp={TimeStamp}";
        }
    }
}
