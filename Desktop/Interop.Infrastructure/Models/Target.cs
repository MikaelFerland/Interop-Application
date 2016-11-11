namespace Interop.Infrastructure.Models
{
    public class Target : System.IComparable
    {
        public int    id { get; set; }                 // "id": 1,
        public int    user { get; set; }               //"user": 1,
        public string type { get; set; }               //"type": "standard", //TODO: Confirm if we have to create this type
        public double latitude { get; set; }           //"latitude": 38.1478,
        public double longitude { get; set; }          //"longitude": -76.4275,
        public string orientation { get; set; }        //"orientation": "n",
        public string shape { get; set; }              //"shape": "star",
        public string background_color { get; set; }   //"background_color": "orange",
        public string alphanumeric { get; set; }       //"alphanumeric": "C",
        public string alphanumeric_color { get; set; } //"alphanumeric_color": "black",
        public string description { get; set; }        //"description": null,
        public bool   autonomous { get; set; }         //"autonomous": false,

        public int CompareTo(object target)
        {
            if (this == null && target == null)
                return 1;
            else if (this == null | target == null)
                return -1;
            else if (this.user == ((Target)target).user &
                     this.type == ((Target)target).type &
                     this.shape == ((Target)target).shape &
                     this.orientation == ((Target)target).orientation &
                     this.longitude == ((Target)target).longitude &
                     this.latitude == ((Target)target).latitude &
                     this.id == ((Target)target).id &
                     this.description == ((Target)target).description &
                     this.background_color == ((Target)target).background_color &
                     this.autonomous == ((Target)target).autonomous &
                     this.alphanumeric == ((Target)target).alphanumeric &
                     this.alphanumeric_color == ((Target)target).alphanumeric_color)
                return 1;
            else
                return -1;
        }
    }
}
