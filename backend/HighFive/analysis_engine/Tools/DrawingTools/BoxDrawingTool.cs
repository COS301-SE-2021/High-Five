using System;
using System.Drawing;
using System.Linq;//TODO Check this import
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace analysis_engine
{
    public class BoxDrawingTool : DrawingTool
    {
        public override Data Process(Data data)
        {
            var image = data.Frame.Image;
            var count = 1;
            var outputs = data.Meta;
            foreach (var output in outputs.Cast<BoxCoordinateData>())
            {
                for (var i = 0; i < output.Classes.Count; i++)
                {
                    var box = new Rectangle(Convert.ToInt32(output.Boxes[i * 4]),
                        Convert.ToInt32(output.Boxes[i * 4 + 1]), 
                        Convert.ToInt32(output.Boxes[i * 4 + 2]),
                        Convert.ToInt32(output.Boxes[i * 4 + 3]));
                    var point = new Point(Convert.ToInt32(output.Boxes[i * 4]),
                        Convert.ToInt32(output.Boxes[i * 4 + 1] - 10.0 * image.Height / 2286.0));
                    CvInvoke.Rectangle(image, box, new Bgr(Color.Red).MCvScalar, 5, LineType.Filled);
                    CvInvoke.PutText(image, output.Classes[i].ToUpper(), point, FontFace.HersheyTriplex, 2.0, new Bgr(Color.Red).MCvScalar, 5);
                }

                var textPoint = new Point(image.Width / 445, count*6*image.Height / 229);
                CvInvoke.PutText(image, "Vehicle Count: "+output.Classes.Count, textPoint, FontFace.HersheyTriplex, 2.0, new Bgr(Color.Red).MCvScalar, 5);
                count++;
            }

            data.Frame.Image = image;
            return data;
        }

        public override void Init()
        {
            
        }
    }
}