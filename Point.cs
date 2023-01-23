using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KP_lab1_Framework4._8._1
{
    [SoapInclude(typeof(Point3D))]
    [XmlInclude(typeof(Point3D))]
    [Serializable]
    public class Point : IComparable<object>
    {
        [SoapElement(DataType = "X")]
        public int X { get; set; }
        [SoapElement(DataType = "Y")]
        public int Y { get; set; }
        public static Random rnd = new Random();

        public virtual double Metric()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }

        public override string ToString()
        {
            return string.Format($"({X}, {Y})");
        }
        public Point()
        {
            X = rnd.Next(10);
            Y = rnd.Next(10);
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int CompareTo(object obj)
        {
            Point p = (Point)obj;
            return (int)(Metric() - p.Metric());
        }
    }
    [SoapInclude(typeof(Point3D))]
    [XmlInclude(typeof(Point3D))]
    [Serializable]
    public class Point3D : Point
    {
        [SoapElement(DataType = "Z")]
        public int Z { get; set; }
        public Point3D() : base()
        {
            Z = rnd.Next(10);
        }
        public Point3D(int x, int y, int z) : base(x, y)
        {
            Z = z;
        }
        public override double Metric()
        {
            return Math.Sqrt(Math.Pow(X, X) + Math.Pow(Y, Y) + Math.Pow(Z, Z));
        }
        public override string ToString()
        {
            return string.Format($"({X}, {Y}, {Z})");
        }
    }
}
