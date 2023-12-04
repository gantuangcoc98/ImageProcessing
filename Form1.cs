using System.Transactions;
using System.Windows.Forms;

namespace Image_Processing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Show result
                DialogResult result = openFileDialog.ShowDialog();

                // Check if the user selected a file
                if (result == DialogResult.OK)
                {
                    // Get the selected file's path
                    string filePath = openFileDialog.FileName;

                    // Load the image into a Bitmap
                    Bitmap bitmap = new Bitmap(filePath);

                    // Display the image in the PictureBox
                    selectedPicBox.Image = bitmap;
                }
            }
        }

        private void selectedPicBox_Click(object sender, EventArgs e)
        {

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Bitmap selectedBitmap = (Bitmap)selectedPicBox.Image;
            Bitmap resultBitmap = new Bitmap(selectedBitmap.Width, selectedBitmap.Height);

            for (int x = 0; x < selectedBitmap.Width; x++)
            {
                for (int y = 0; y < selectedBitmap.Height; y++)
                {
                    Color color = selectedBitmap.GetPixel(x, y);
                    resultBitmap.SetPixel(x, y, color);
                }
            }

            resultPicBox.Image = resultBitmap;
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap selectedBitmap = (Bitmap)selectedPicBox.Image;
            Bitmap resultBitmap = new Bitmap(selectedBitmap.Width, selectedBitmap.Height);

            for (int x = 0; x < selectedBitmap.Width; x++)
            {
                for (int y = 0; y < selectedBitmap.Height; y++)
                {
                    Color color = selectedBitmap.GetPixel(x, y);
                    int greyscale = (int)(color.R + color.G + color.B) / 3;
                    Color grey = Color.FromArgb(greyscale, greyscale, greyscale);

                    resultBitmap.SetPixel(x, y, grey);
                }
            }

            resultPicBox.Image = resultBitmap;
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap selectedBitmap = (Bitmap)selectedPicBox.Image;
            Bitmap resultBitmap = new Bitmap(selectedBitmap.Width, selectedBitmap.Height);

            for (int x = 0; x < selectedBitmap.Width; x++)
            {
                for (int y = 0; y < selectedBitmap.Height; y++)
                {
                    Color color = selectedBitmap.GetPixel(x, y);
                    Color invertedColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);

                    resultBitmap.SetPixel(x, y, invertedColor);
                }
            }

            resultPicBox.Image = resultBitmap;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap selectedBitmap = (Bitmap)selectedPicBox.Image;

            // get first a greyscaled bitmap
            Bitmap greyscaledBitmap = new Bitmap(selectedBitmap.Width, selectedBitmap.Height);
            for (int x = 0; x < selectedBitmap.Width; x++)
            {
                for (int y = 0; y < selectedBitmap.Height; y++)
                {
                    Color color = selectedBitmap.GetPixel(x, y);
                    int greyscale = (int)(color.R + color.G + color.B) / 3;
                    Color grey = Color.FromArgb(greyscale, greyscale, greyscale);
                    greyscaledBitmap.SetPixel(x, y, grey);
                }
            }

            // create an array for the histogram and store the greyScaleValue
            int[] histogram = new int[256];
            for (int x = 0; x < greyscaledBitmap.Width; x++)
            {
                for (int y = 0; y < greyscaledBitmap.Height; y++)
                {
                    Color color = greyscaledBitmap.GetPixel(x, y);
                    int greyScaleValue = color.R;
                    histogram[greyScaleValue]++;
                }
            }

            // plot the histogram
            int maxCount = histogram[0];
            for (int i = 0; i < histogram.Length; i++) // finding the highest value of greyscale as a height
            {
                if (histogram[i] > maxCount)
                    maxCount = histogram[i];
            }

            Bitmap histogramBitmap = new Bitmap(256, maxCount);
            using (Graphics g = Graphics.FromImage(histogramBitmap))
            {
                g.Clear(Color.White);

                for (int i = 0; i < histogram.Length; i++)
                {
                    int barHeight = (int)((double)histogram[i] / maxCount * (maxCount - 1));
                    g.DrawLine(Pens.Black, i, maxCount - 1, i, maxCount - 1 - barHeight);
                }
            }

            resultPicBox.Image = histogramBitmap;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {


            Bitmap resultBitmap = (Bitmap)resultPicBox.Image;

            if (resultBitmap != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Bitmap Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png|All Files|*.*";
                    saveFileDialog.Title = "untitled";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        resultBitmap.Save(filePath);
                    }
                }
            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Bitmap selectedBitmap = (Bitmap)selectedPicBox.Image;
           Bitmap resultBitmap = new Bitmap(selectedBitmap.Width, selectedBitmap.Height);

           for (int x = 0; x < selectedBitmap.Width; x++)
            {
                for (int y = 0; y < selectedBitmap.Height; y++)
                {
                    Color color = selectedBitmap.GetPixel(x, y);
                    int greyscale = (int)(color.R + color.G + color.B) / 3;

                    Color gray = Color.FromArgb(greyscale, greyscale, greyscale);
                    int sepiaR = (int)(0.393 * gray.R + 0.769 * gray.G + 0.189 * gray.B);
                    int sepiaG = (int)(0.349 * gray.R + 0.686 * gray.G + 0.168 * gray.B);
                    int sepiaB = (int)(0.272 * gray.R + 0.534 * gray.G + 0.131 * gray.B);

                    // Ensure values are in the valid range [0, 255]
                    sepiaR = Math.Max(0, Math.Min(255, sepiaR));
                    sepiaG = Math.Max(0, Math.Min(255, sepiaG));
                    sepiaB = Math.Max(0, Math.Min(255, sepiaB));

                    Color sepiaColor = Color.FromArgb(sepiaR, sepiaG, sepiaB);
                    resultBitmap.SetPixel(x, y, sepiaColor);
                }
            }

           resultPicBox.Image = resultBitmap;
        }
    }
}