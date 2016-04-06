namespace Interop.Infrastructure.Models
{
    public class Target
    {
        public int    id { get; set; }                 // "id": 1,
        public int    user { get; set; }               //"user": 1,
        public string type { get; set; }               //"type": "standard", //TODO: Confirm if we have to create this type
        public double latitude { get; set; }           //"latitude": 38.1478,
        public double longitude { get; set; }          //"longitude": -76.4275,
        public char   orientation { get; set; }        //"orientation": "n",
        public string shape { get; set; }              //"shape": "star",
        public string background_color { get; set; }   //"background_color": "orange",
        public char   alphanumeric { get; set; }       //"alphanumeric": "C",
        public string alphanumeric_color { get; set; } //"alphanumeric_color": "black",
        public string description { get; set; }        //"description": null,
        public bool   autonomous { get; set; }         //"autonomous": false,
    }
}
