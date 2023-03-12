using Newtonsoft.Json;

namespace PowerFacilityEmulator.Model
{
    public class TopologicalNode
    {
        public int Number { get; set; }
        //public TopologicalNodeValues TopologicalNodeValues { get; set; }
        public List<TopologicalNodeValue> TopologicalNodeValues { get; set; }
    }
}
