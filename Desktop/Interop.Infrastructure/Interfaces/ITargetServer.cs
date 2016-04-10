using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Interop.Infrastructure.Interfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITargetServer" in both code and config file together.
    [ServiceContract]
    public interface ITargetServer
    {
        [OperationContract]
        Response SendTarget(InteropTargetMessage tInfo);
    }

    [MessageContract]
    public class Response
    {
        private string _message;

        [MessageHeader]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }

    [MessageContract]
    public class InteropTargetMessage
    {
        private int m_ID;
        private InteropTargetIntel m_TargetInfo = new InteropTargetIntel();
        private string m_operation;

        [MessageHeader]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        [MessageBodyMember]
        public string Operation
        {
            get { return m_operation; }
            set { m_operation = value; }
        }

        [MessageBodyMember]
        public InteropTargetIntel TargetInfo
        {
            get { return m_TargetInfo; }
            set { m_TargetInfo = value; }
        }
    }


    [DataContract]
    public class InteropTargetIntel
    {
        private string _targetName;
        private string _imageSourceName;
        private string _targetID;
        private string _parentID;
        private int _posX;
        private int _posY;
        private double _latitude;
        private double _longitude;
        private int _orientation;
        private string _shape;
        private string _char;
        private string _backColor;
        private string _foreColor;
        private double _area;
        private byte[] _image;

        [DataMember]
        public string TargetName
        {
            get { return _targetName; }
            set { _targetName = value; }
        }

        [DataMember]
        public string ImageSourceName
        {
            get { return _imageSourceName; }
            set { _imageSourceName = value; }
        }

        [DataMember]
        public string TargetID
        {
            get { return _targetID; }
            set { _targetID = value; }
        }

        [DataMember]
        public string ParentID
        {
            get { return _parentID; }
            set { _parentID = value; }
        }

        [DataMember]
        public int PosX
        {
            get { return _posX; }
            set { _posX = value; }
        }

        [DataMember]
        public int PosY
        {
            get { return _posY; }
            set { _posY = value; }
        }

        [DataMember]
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        [DataMember]
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        [DataMember]
        public int Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        [DataMember]
        public string Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        [DataMember]
        public string Character
        {
            get { return _char; }
            set { _char = value; }
        }

        [DataMember]
        public string BackgroundColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        [DataMember]
        public string ForegroundColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        [DataMember]
        public double Area
        {
            get { return _area; }
            set { _area = value; }
        }

        [DataMember]
        public byte[] Image
        {
            get { return _image; }
            set { _image = value; }
        }
    }
}
