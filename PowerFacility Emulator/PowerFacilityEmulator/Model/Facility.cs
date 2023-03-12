using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerFacilityEmulator.Model
{
    internal class Facility
    {


        // Строка с данными должна иметь следующий формат:
        // "object:{substationName} pgen:12.34 qgen:3.45 pcons:45,67 qcons:67.80\n"
        // Символ \n свидетельствует об окончании


        /*// Пока вот такая структура:
        //JSON:
        {
            FacilityIPEndPoint:"192.160.0.5",
            FacilityName:"Substation 1",
            TopologicalNodes:
            [
	            {

                    TopologicalNumber:1,
                    Values:
	                {

                        pgen:12.34,
                        qgen:3.45,
                        pcons:null,
                        qcons:null

                    }
                },
	            {
                    TopologicalNumber: 2,
	                Values:
                    {
                        pgen: null,
	                    qgen: null,
		                pcons: 45.67,
		                qcons: 67.80

                    }
                }
            ]
        }*/

        // json v1
        //{FacilityIPEndPoint:"192.160.0.5", FacilityName:"Substation 1", TopologicalNodes:[{TopologicalNumber:1, Values:{pgen:12.34, qgen:3.45, pcons:null, qcons:null}},{TopologicalNumber: 2,Values:{pgen: null, qgen: null, pcons: 45.67, qcons: 67.80}}]}
        // json v2
        //{"FacilityIPEndPoint": "192.160.0.5:1234","ServerIPEndPoint": "127.0.0.10:8888","FacilityName": "Substation 1","TopologicalNodes": [{"Number": 1,"TopologicalNodeValues": [{"Description": "Pgen","Value": 12.34,"QCode": 268435458,"TimeStamp": "2019-07-04T13:33:04.015Z"},{"Description": "Qgen","Value": 3.45,"QCode": 268435458,"TimeStamp": "2019-07-04T13:33:04.002Z"}]},{"Number": 2,"TopologicalNodeValues": [{"Description": "Pcons","Value": 45.67,"QCode": 268435458,"TimeStamp": "2019-07-04T13:33:03.969Z"},{"Description": "Qcons","Value": 67.80,"QCode": 268435458,"TimeStamp": "2019-07-04T13:33:03.002Z"}]}]}

        // C#:
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public string FacilityIPEndPoint { get; set; }
        public string ServerIPEndPoint { get; set; }
        public string FacilityName { get; set; }
        public List<TopologicalNode> TopologicalNodes { get; set; }    
        
        internal Facility() { }
        internal Facility(string[] args)
        {
            LoadInitialDataFromJsonFile(args);
        }

        private void LoadInitialDataFromJsonFile(string[] args)
        {
            try
            {
                if (args.Length != 0)
                {
                    string inputPath = args[0];
                    using StreamReader sr = new StreamReader(inputPath);
                    string json = sr.ReadToEnd();
                    Facility facility = JsonConvert.DeserializeObject<Facility>(json);

                    FacilityIPEndPoint = facility.FacilityIPEndPoint;
                    ServerIPEndPoint = facility.ServerIPEndPoint;
                    FacilityName = facility.FacilityName;
                    TopologicalNodes = facility.TopologicalNodes;
                    foreach (var node in TopologicalNodes) 
                    {
                        foreach(var nodeValue in node.TopologicalNodeValues) 
                        {
                            nodeValue.NormalValue = nodeValue.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }

}
