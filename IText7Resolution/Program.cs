using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

internal class Program
{
    public const float PAGE_MARGIN_CM = .5f;
    public const float IMAGE_MARGIN_CM = .5f;
    public const float CM_TO_POINTS_FACTOR = 28.35f;

    private static async Task Main(string[] args)
    {
        using MemoryStream memoryStream = new MemoryStream();
        PdfWriter writer = new PdfWriter(memoryStream, new WriterProperties().SetCompressionLevel(0));
        PdfDocument pdfDoc = new PdfDocument(writer);
        Document document = new Document(pdfDoc);
        float pageMargin = CalcCentimeterToPoints(PAGE_MARGIN_CM);
        document.SetMargins(pageMargin, pageMargin, pageMargin, pageMargin);

        PageSize pageSize = PageSize.A4;
        pdfDoc.SetDefaultPageSize(pageSize);
        string imagePath = "SchulzLF_0.png";

        float imageMargin = CalcCentimeterToPoints(IMAGE_MARGIN_CM);
        float nutzenHeight = (pageSize.GetHeight() / 2) - (2 * imageMargin);
        float imageWidth = pageSize.GetWidth() - (2 * imageMargin);
        float imageHeight = nutzenHeight;

        ImageData imageData = ImageDataFactory.Create(imagePath);
        Image image = new Image(imageData);
        
        
        var tmpWidth = image.GetImageWidth();
        var tmpHeight = image.GetImageHeight();
        //image.SetAutoScaleHeight(false);
        //image.SetAutoScaleWidth(false);
        image.ScaleToFit(imageWidth, imageHeight);
        document.Add(image);
        image.SetFixedPosition(imageMargin, imageMargin);



        document.Add(image);
        document.Close();
        byte[] pdfBytes = memoryStream.ToArray();

        await File.WriteAllBytesAsync("SchulzLF_0.pdf", pdfBytes);

    }

    private static float CalcCentimeterToPoints(float cm)
    {
        return (float)(cm * CM_TO_POINTS_FACTOR);
    }
}