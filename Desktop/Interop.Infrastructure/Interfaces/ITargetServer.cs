using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Interop.Infrastructure.Interfaces
{
    [ServiceContract]
    public interface ITargetServer
    {
        [OperationContract]
        Response SendTarget(InteropTargetMessage tInfo);
    }

    [MessageContract]
    public class Response
    {
        [MessageHeader]
        public string Message;
    }

    [MessageContract]
    public class InteropTargetMessage
    {
        public enum OperationsTypes { NEW, EDIT, DELETE, TEST };
        public enum Orientations { N, NE, E, SE, S, SW, W, NW };
        public enum Shapes { circle, semicircle, quarter_circle, triangle, square, rectangle, trapezoid, pentagon, hexagon, heptagon, octagon, star, cross };
        public enum Colors { white, black, gray, red, blue, green, yellow, purple, brown, orange };
        public enum targetTypes { standard, qrc, off_axis, emergent };

        [MessageHeader]
        public string TargetID;
        [MessageBodyMember]
        public int InteropID;
        [MessageBodyMember]
        public targetTypes TargetType;
        [MessageBodyMember]
        public OperationsTypes Operation;
        [MessageBodyMember]
        public string TargetName;
        [MessageBodyMember]
        public string ImageSourceName;
        [MessageBodyMember]
        public int PosX;
        [MessageBodyMember]
        public int PosY;
        [MessageBodyMember]
        public double Latitude;
        [MessageBodyMember]
        public double Longitude;
        [MessageBodyMember]
        public Orientations Orientation;
        [MessageBodyMember]
        public Shapes Shape;
        [MessageBodyMember]
        public string Character;
        [MessageBodyMember]
        public Colors BackgroundColor;
        [MessageBodyMember]
        public Colors ForegroundColor;
        [MessageBodyMember]
        public double Area;
        [MessageBodyMember]
        public byte[] Image;
        [MessageBodyMember]
        public bool Autonomous;
        [MessageBodyMember]
        public string Description;
    }
}
