using PdfSharpCore.Pdf;

namespace KegID.DependencyServices
{
    public interface IPdfSave
    {
        void Save(PdfDocument doc, string fileName);
    }
}
