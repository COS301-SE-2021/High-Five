using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using High5SDK;

namespace analysis_engine
{
    public class LabelDrawingTool : DrawingTool
    {
        public override Data Process(Data data)
        {
            var image = data.Frame.Image;
            var count = 1;
            var outputs = data.Meta;
            foreach (var output in outputs.Cast<BoxCoordinateData>())
            {
                var textPoint = new Point(image.Width / 445, count*6*image.Height / 80);
                CvInvoke.PutText(image, output.Purpose, textPoint, FontFace.HersheyTriplex, 2.0, new Bgr(Color.SpringGreen).MCvScalar, 5);
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