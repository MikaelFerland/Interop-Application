using System.Collections.Generic;
using System.Linq;

namespace Interop.Infrastructure.Models
{
    public class Target : System.IComparable
    {
        static TargetEqualityComparer TargetEqC = new TargetEqualityComparer();

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


        public static bool ScrambledEquals(IEnumerable<Target> list1, IEnumerable<Target> list2)
        {
            return ScrambledEquals<Target>(list1, list2, TargetEqC);
        }

        private static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2, IEqualityComparer<T> comparer)
        {

            var cnt = new Dictionary<T, int>(comparer);
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        internal class TargetEqualityComparer : IEqualityComparer<Target>
        {
            public bool Equals(Target b1, Target b2)
            {
                if (b2 == null && b1 == null)
                    return true;
                else if (b1 == null | b2 == null)
                    return false;
                else if (b1.user == b2.user &
                         b1.type == b2.type &
                         b1.shape == b2.shape &
                         b1.orientation == b2.orientation &
                         b1.longitude == b2.longitude &
                         b1.latitude == b2.latitude &
                         b1.id == b2.id &
                         b1.description == b2.description &
                         b1.background_color == b2.background_color &
                         b1.autonomous == b2.autonomous &
                         b1.alphanumeric == b1.alphanumeric &
                         b1.alphanumeric_color == b2.alphanumeric_color)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(Target target)
            {
                int hCode = target.id;

                return hCode.GetHashCode();
            }
        }
    }
}
