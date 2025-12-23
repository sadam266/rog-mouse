namespace GHelper.UI
{
    public static class ControlHelper
    {
        public static void ApplyTheme(Control control)
        {
            foreach (Control child in control.Controls)
            {
                if (child is Panel || child is Label || child is CheckBox || child is PictureBox)
                {
                    child.BackColor = RForm.formBack;
                    child.ForeColor = RForm.foreMain;
                }

                if (child is RButton button)
                {
                    button.BackColor = button.Secondary ? RForm.buttonSecond : RForm.buttonMain;
                    button.ForeColor = RForm.foreMain;
                }

                if (child is RComboBox combo)
                {
                    combo.BackColor = RForm.buttonMain;
                    combo.ForeColor = RForm.foreMain;
                }

                ApplyTheme(child);
            }
        }

        public static void Resize(Control control, float scale = 1.0f) { }
        public static void Adjust(Control control, bool force = false) { }
        public static Image TintImage(Image image, Color color)
        {
            Bitmap bmp = new Bitmap(image);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixel = bmp.GetPixel(x, y);
                    if (pixel.A > 0)
                    {
                        bmp.SetPixel(x, y, Color.FromArgb(pixel.A, color));
                    }
                }
            }
            return bmp;
        }
    }
}
