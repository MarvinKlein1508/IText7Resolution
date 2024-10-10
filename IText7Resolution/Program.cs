using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
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


        float imageMargin = CalcCentimeterToPoints(IMAGE_MARGIN_CM);
        float imageWidth = pageSize.GetWidth() - (2 * imageMargin);
        float imageHeight = (pageSize.GetHeight() / 2) - (2 * imageMargin);


        var xObject = CreateReusableBlock(pdfDoc, imageWidth, imageHeight);
        Image label = new Image(xObject).SetAutoScale(true);


        document.Add(label);
        label.SetFixedPosition(imageMargin, imageMargin);
        document.Add(label);



        document.Close();
        byte[] pdfBytes = memoryStream.ToArray();

        await File.WriteAllBytesAsync("SchulzLF_0.pdf", pdfBytes);

    }

    private static PdfFormXObject CreateReusableBlock(PdfDocument pdf, float width, float height)
    {
        // Create a PdfFormXObject 
        Rectangle rectangle = new Rectangle(width, height);
        PdfFormXObject xObject = new PdfFormXObject(rectangle);

        // Create PdfCanvas for PdfFormXObject
        PdfCanvas canvas = new PdfCanvas(xObject, pdf);

        string imagePath = "SchulzLF_0.png";
        ImageData imageData = ImageDataFactory.Create(imagePath);

        canvas.AddImageFittedIntoRectangle(imageData, rectangle, true);


        // close the canvas
        canvas.Release();

        return xObject;
    }
    private static float CalcCentimeterToPoints(float cm)
    {
        return (float)(cm * CM_TO_POINTS_FACTOR);
    }
}