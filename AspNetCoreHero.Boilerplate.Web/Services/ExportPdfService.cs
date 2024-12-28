using Application.Interfaces.Shared;
using AspNetCoreHero.Boilerplate.Application.Interfaces.Contexts;
using AspNetCoreHero.Boilerplate.Domain.Entities;
using AutoMapper;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using Microsoft.AspNetCore.Mvc.Razor;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using iText.Forms.Form.Element;
using iText.Layout.Borders;
using iText.Kernel.Font;
using Application.DTOs.File;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;

namespace Web.Services
{
    public class ExportPdfService : IExportPdfService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public ExportPdfService(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<FileDto> GeneratePdf(int patientId)
        {
            var patient = await _dbContext.EntitySet<Patient>()
                                                       .Include(x => x.MedicalData)
                                                       .Include(x => x.PatientTreatments)
                                                       .Where(x => x.Id == patientId)
                                                       .FirstOrDefaultAsync();

            using (var memoryStream = new MemoryStream())
            {
                // Create a PDF writer
                PdfWriter writer = new PdfWriter(memoryStream);

                // Create a PDF document
                PdfDocument pdf = new PdfDocument(writer);

                // Create a document to add layout elements
                Document document = new Document(pdf);

                document.SetMargins(40, 40, 40, 40);

                string fontPath = "wwwroot\\fonts\\DejaVuSans.ttf";
                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

                // Add title
                Paragraph title = new Paragraph("KARTELA DENTARE")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(16)
                    .SetUnderline()
                    //.SetBold()
                    ;
                document.Add(title);

                document.Add(new Paragraph($"Data: {DateTime.UtcNow.ToString("dd/mm/yyyy")}")
                    .SetTextAlignment(TextAlignment.RIGHT));

                // Add clinic info
                document.Add(new Paragraph("Nr.i Licenses: 434")
                    .SetTextAlignment(TextAlignment.LEFT));
                document.Add(new Paragraph("Klinika Dentare: \"SKARRA DENTAL\"   Dr. Bilbil Skarra")
                    .SetTextAlignment(TextAlignment.LEFT));
                document.Add(new Paragraph("Adresa: Rruga \"Ali Demi\"  \n Cel: 0685512899 / 0696800593")
                    .SetTextAlignment(TextAlignment.LEFT));

                document.Add(new Paragraph());

                // Add patient general info section
                document.Add(new Paragraph("GJENERALITETET E PACIENTIT")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetUnderline()
                    //.SetBold()
                    );

                document.Add(new Paragraph($"Emri i pacientit: {patient.Name} {patient.Surname}                Data e Lindjes: {patient.BornDate.ToString("dd/mm/yyyy")}    Seksi: {patient.Gender}"));
                document.Add(new Paragraph($"Emri i Prindërit/Shoqëruesit per (Minorenet): {patient.ParentName}"));
                document.Add(new Paragraph($"Adresa: {patient.Address}"));
                document.Add(new Paragraph($"Tel: {patient.PhoneNo}"));
                document.Add(new Paragraph($"Puna tanishme: {patient.Profession}                    Adresa: {patient.JobAddress}"));

                document.Add(new Paragraph());

                // Add medical history section
                document.Add(new Paragraph("HISTORIA MJEKSORE")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetUnderline()
                    //.SetBold()
                    );

                var hasIllnes = patient.MedicalData.HasIllness  ? "Po" : "Jo";

                var isUnderTreatment = patient.MedicalData.IsUnderTreatment ? "Po" : "Jo";
                 
                Table generalMedicalDataTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();
                generalMedicalDataTable.AddCell(new Cell().Add(new Paragraph($"Vuani nga ndonje semundje? {hasIllnes}")).SetFont(font).SetBorder(Border.NO_BORDER));
                generalMedicalDataTable.AddCell(new Cell().Add(new Paragraph($"Nese po, cilat? {patient.MedicalData.IllnessDetails}")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                generalMedicalDataTable.AddCell(new Cell().Add(new Paragraph($"A jeni nen kujdesin e mjekut? {isUnderTreatment} ")).SetFont(font).SetBorder(Border.NO_BORDER));
                generalMedicalDataTable.AddCell(new Cell().Add(new Paragraph($"Emri i mjekut tuaj? {patient.MedicalData.DoctorsName}")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                document.Add(generalMedicalDataTable);

                document.Add(new Paragraph($"Shkruani mjekimin (Medikamentin) qe merrni: {patient.MedicalData.MedicationsTreatment}"));
                document.Add(new Paragraph());

                document.Add(new Paragraph("Cilat nga problemet e meposhtme keni? (KUJDES! Rikujtoni semundjen tuaj)"));

                var medicalData = patient.MedicalData;

                Table medicalDataTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();

                string isChecked = "☑";
                string unChecked = "☐";

                string semundjeGjaku = medicalData.SemundjeGjaku ? isChecked : unChecked ;
                string tensionILarte = medicalData.TensionILarte   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{semundjeGjaku} Semundje gjaku")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{tensionILarte} Tension i lartë")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string tensionIUlet = medicalData.TensionIUlet   ? isChecked : unChecked ;
                string nderhyrjeKirugjikaleNeZemer = medicalData.NderhyrjeKirugjikaleNeZemer   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{tensionIUlet} Tension i ulet")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{nderhyrjeKirugjikaleNeZemer} NderhyrjeKirugjikale ne zemer")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string problemeZemre = medicalData.ProblemeZemre   ? isChecked : unChecked ;
                string etheRaumatizmale = medicalData.EtheRaumatizmale   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{problemeZemre} Probleme zemre")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{etheRaumatizmale} Ethe Raumatizmale")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string glaucoma = medicalData.Glaucoma   ? isChecked : unChecked ;
                string diabet = medicalData.Diabet   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{glaucoma} Glaucoma")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{diabet} Diabet")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string alergjiNgaLlastikuDorezave = medicalData.AlergjiNgaLlastikuDorezave   ? isChecked : unChecked ;
                string alergjiNgaMedikamentet = medicalData.AlergjiNgaMedikamentet   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{alergjiNgaLlastikuDorezave} Alergji nga llastiku Dorezave")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{alergjiNgaMedikamentet} Alergji nga medikamentet")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string alergjiNgaMetalet = medicalData.AlergjiNgaMetalet   ? isChecked : unChecked ;
                string semundjeMendore = medicalData.SemundjeMendore   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{alergjiNgaMetalet} Alergji nga metalet")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{semundjeMendore} Semundje Mendore")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string Epilepsi = medicalData.Epilepsi   ? isChecked : unChecked ;
                string PerdoruesDroge = medicalData.PerdoruesDroge   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{Epilepsi} Epilepsi")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{PerdoruesDroge} Perdorues droge")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string AzemBronkiale = medicalData.AzemBronkiale   ? isChecked : unChecked ;
                string Turbekuloze = medicalData.Turbekuloze   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{AzemBronkiale} Azem Bronkiale")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{Turbekuloze} Turbekuloze")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string MjekimTumori = medicalData.MjekimTumori   ? isChecked : unChecked ;
                string Shtatzen = medicalData.Shtatzen   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{MjekimTumori} Mjekim Tumori")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{Shtatzen} Shtatzen")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string AIDS = medicalData.AIDS   ? isChecked : unChecked ;
                string SemundjeMelcie = medicalData.SemundjeMelcie   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{AIDS} AIDS")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{SemundjeMelcie} Semundje Melcie")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));

                document.Add(medicalDataTable);

                document.Add(new Paragraph($"Cila eshte arsyeja qe jeni paraqitur ne klinike? {medicalData.ArsyejaEParaqitjesNeKlinike}"));

                document.Add(new Paragraph("\nFirma e Pacientit / Prindit: _____________________________"));

                document.Add(new Paragraph());
                document.Add(new Paragraph());

                // Adding a table for medical data
                var table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 20, 20, 20, 20 }))
                                                .UseAllAvailableWidth()
                                                .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                table.AddCell(new Cell().SetHeight(40).Add(new Paragraph("Dhembi")));
                table.AddCell(new Cell().SetHeight(40).Add(new Paragraph("Diagnoza")));
                table.AddCell(new Cell().SetHeight(40).Add(new Paragraph("Trajtimi")));
                table.AddCell(new Cell().SetHeight(40).Add(new Paragraph("Data")));
                table.AddCell(new Cell().SetHeight(40).Add(new Paragraph("Pagesa")));

                if (patient.PatientTreatments is not null && patient.PatientTreatments.Any())
                {
                    foreach(var treatment in patient.PatientTreatments)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(treatment.Tooth.ToString())));
                        table.AddCell(new Cell().Add(new Paragraph(treatment.Diagnosis ?? "")));
                        table.AddCell(new Cell().Add(new Paragraph(treatment.Treatment ?? "")));
                        table.AddCell(new Cell().Add(new Paragraph(treatment.DateOfIntervention.ToString("dd/mm/yyyy") ?? "")));
                        table.AddCell(new Cell().Add(new Paragraph(treatment.Payment ?? "") ));
                    }
                }

                document.Add(table);

                PdfDocument pdfDoc = document.GetPdfDocument();

                int totalPages = pdf.GetNumberOfPages();


                // Add borders dynamically for all pages

                    //for (int pageNum = 1; pageNum <= totalPages; pageNum++)
                    //{
                    //    DrawBorder(pdfDoc.GetPage(pageNum));
                    //}


                // Finish the document
                document.Close();


                return new FileDto
                {
                    File = memoryStream.ToArray(),
                    Name = patient.Name + patient.Surname + "_Kartela.pdf",
                };
            }

        }
        private static void DrawBorder(PdfPage page)
        {
            // Get the page size
            Rectangle pageRect = new Rectangle(0, 0, 595, 842); // Default to A4 size (in points)
            // Get PdfCanvas to draw on the page
            PdfCanvas canvas = new PdfCanvas(page);

            // Set border properties
            canvas.SetLineWidth(2); // Border width
            canvas.SetStrokeColor(iText.Kernel.Colors.ColorConstants.BLACK).SetLineWidth(5).Rectangle(pageRect);

            canvas.Stroke();
        }
    }
}
