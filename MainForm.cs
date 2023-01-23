using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

namespace KP_lab1_Framework4._8._1
{
    public partial class MainForm : Form
    {
        private Point[] points = null;
        public MainForm()
        {
            InitializeComponent();
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            points = new Point[5];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = rnd.Next(3) % 2 == 0 ? new Point() : new Point3D();
            }
            listBox1.DataSource = points;
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (points == null)
            {
                return;
            }
            Array.Sort(points);
            listBox1.DataSource = null;
            listBox1.DataSource = points;
        }

        private void btnSerialize_Click(object sender, EventArgs e)
        {
            if (points != null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "SOAP|*.soap|XML|*.xml|JSON|*.json|Binary|*.bin";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                using (FileStream fs = new FileStream(dlg.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    switch (Path.GetExtension(dlg.FileName))
                    {
                        case ".bin":
                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(fs, points);
                            break;
                        case ".json":
                            string final = JsonConvert.SerializeObject(points);
                            byte[] wr = Encoding.UTF8.GetBytes(final);
                            fs.Write(wr, 0, wr.Length);
                            break;
                        case ".xml":
                            XmlSerializer xml = new XmlSerializer(points.GetType());
                            xml.Serialize(fs, points);
                            break;
                        case ".soap":
                            SoapFormatter sf = new SoapFormatter();
                            sf.Serialize(fs, points);
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Сериализация невозможна, не заполены точки");
            }
        }

        private void btnDeserialize_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SOAP|*.soap|XML|*.xml|JSON|*.json|Binary|*.bin";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
            {
                switch (Path.GetExtension(dlg.FileName))
                {
                    case ".bin":
                        BinaryFormatter bf = new BinaryFormatter();
                        points = (Point[])bf.Deserialize(fs);
                        break;
                    case ".soap":
                        SoapFormatter sf = new SoapFormatter();
                        points = (Point[])sf.Deserialize(fs);
                        break;
                    case ".xml":
                        XmlSerializer xf = new XmlSerializer(typeof(Point[]), new[] { typeof(Point3D) });
                        points = (Point[])xf.Deserialize(fs);
                        break;
                    case ".json":
                        JsonSerializer jf = new JsonSerializer();
                        using (StreamReader r = new StreamReader(fs))
                            points = (Point[])jf.Deserialize(r, typeof(Point[]));
                        break;

                }
            }
            listBox1.DataSource = null;
            listBox1.DataSource = points;
        }
    }
}
