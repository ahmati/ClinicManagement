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

                string fontPath = "wwwroot\\fonts\\DejaVuSans.ttf";
                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

                // Add title
                Paragraph title = new Paragraph("KARTELA DENTARE")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(16)
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
                    //.SetBold()
                    );

                document.Add(new Paragraph($"Emri i pacientit: {patient.Name} {patient.Surname}    Data e Lindjes: {patient.BornDate.ToString("dd/mm/yyyy")}    Seksi: {patient.Gender}"));
                document.Add(new Paragraph($"Emri i Prindërit/Shoqëruesit per (Minorenet): {patient.ParentName}"));
                document.Add(new Paragraph($"Adresa: {patient.Address}    Tel: {patient.PhoneNo}"));
                document.Add(new Paragraph($"Puna tanishme: {patient.Profession}          Adresa: {patient.JobAddress}"));

                document.Add(new Paragraph());

                // Add medical history section
                document.Add(new Paragraph("HISTORIA MJEKSORE")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    //.SetBold()
                    );

                var hasIllnes = patient.MedicalData.HasIllness  ? "Po" : "Jo";

                var isUnderTreatment = (bool)patient.MedicalData.IsUnderTreatment ? "Po" : "Jo";
                document.Add(new Paragraph($"Vuani nga ndonje semundje? {hasIllnes}          Nese po, cilat? {patient.MedicalData.IllnessDetails}"));
                document.Add(new Paragraph($"A jeni nen kujdesin e mjekut? {isUnderTreatment}          Emri i mjekut tuaj? {patient.MedicalData.DoctorsName}"));
                document.Add(new Paragraph($"Shkruani mjekimin (Medikamentin) qe merrni: {patient.MedicalData.MedicationsTreatment}"));
                document.Add(new Paragraph());

                document.Add(new Paragraph("Cilat nga problemet e meposhtme keni? (KUJDES! Rikujtoni semundjen tuaj)"));

                var medaicalData = patient.MedicalData;

                Table medicalDataTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();

                string isChecked = "☑";
                string unChecked = "☐";

                string semundjeGjaku = medaicalData.SemundjeGjaku ? isChecked : unChecked ;
                string tensionILarte = medaicalData.TensionILarte   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{semundjeGjaku} Semundje gjaku")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{tensionILarte} Tension i lartë")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string tensionIUlet = medaicalData.TensionIUlet   ? isChecked : unChecked ;
                string nderhyrjeKirugjikaleNeZemer = medaicalData.NderhyrjeKirugjikaleNeZemer   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{tensionIUlet} Tension i ulet")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{nderhyrjeKirugjikaleNeZemer} NderhyrjeKirugjikale ne zemer")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string problemeZemre = medaicalData.ProblemeZemre   ? isChecked : unChecked ;
                string etheRaumatizmale = medaicalData.EtheRaumatizmale   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{problemeZemre} Probleme zemre")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{etheRaumatizmale} Ethe Raumatizmale")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string glaucoma = medaicalData.Glaucoma   ? isChecked : unChecked ;
                string diabet = medaicalData.Diabet   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{glaucoma} Glaucoma")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{diabet} Diabet")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string alergjiNgaLlastikuDorezave = medaicalData.AlergjiNgaLlastikuDorezave   ? isChecked : unChecked ;
                string alergjiNgaMedikamentet = medaicalData.AlergjiNgaMedikamentet   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{alergjiNgaLlastikuDorezave} Alergji nga llastiku Dorezave")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{alergjiNgaMedikamentet} Alergji nga medikamentet")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string alergjiNgaMetalet = medaicalData.AlergjiNgaMetalet   ? isChecked : unChecked ;
                string semundjeMendore = medaicalData.SemundjeMendore   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{alergjiNgaMetalet} Alergji nga metalet")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{semundjeMendore} Semundje Mendore")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string Epilepsi = medaicalData.Epilepsi   ? isChecked : unChecked ;
                string PerdoruesDroge = medaicalData.PerdoruesDroge   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{Epilepsi} Epilepsi")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{PerdoruesDroge} Perdorues droge")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string AzemBronkiale = medaicalData.AzemBronkiale   ? isChecked : unChecked ;
                string Turbekuloze = medaicalData.Turbekuloze   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{AzemBronkiale} Azem Bronkiale")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{Turbekuloze} Turbekuloze")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string MjekimTumori = medaicalData.MjekimTumori   ? isChecked : unChecked ;
                string Shtatzen = medaicalData.Shtatzen   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{MjekimTumori} Mjekim Tumori")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{Shtatzen} Shtatzen")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                
                string AIDS = medaicalData.AIDS   ? isChecked : unChecked ;
                string SemundjeMelcie = medaicalData.SemundjeMelcie   ? isChecked : unChecked ;

                // Add a row with left-aligned and right-aligned text
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{AIDS} AIDS")).SetFont(font).SetBorder(Border.NO_BORDER));
                medicalDataTable.AddCell(new Cell().Add(new Paragraph($"{SemundjeMelcie} Semundje Melcie")).SetFont(font).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));

                document.Add(medicalDataTable);

                document.Add(new Paragraph($"Cila eshte arsyeja qe jeni paraqitur ne klinike? {medaicalData.ArsyejaEParaqitjesNeKlinike}"));

                document.Add(new Paragraph("\nFirma e Pacientit / Prindit: _____________________________"));

                document.Add(new Paragraph());
                document.Add(new Paragraph());

                // Adding a table for medical data
                var table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 20, 20, 20, 20 }))
                                                .UseAllAvailableWidth()
                                                .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                table.AddCell(new Cell().Add(new Paragraph("Dhembi")));
                table.AddCell(new Cell().Add(new Paragraph("Diagnoza")));
                table.AddCell(new Cell().Add(new Paragraph("Trajtimi")));
                table.AddCell(new Cell().Add(new Paragraph("Data")));
                table.AddCell(new Cell().Add(new Paragraph("Pagesa")));

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


                // Finish the document
                document.Close();


                return new FileDto
                {
                    File = memoryStream.ToArray(),
                    Name = patient.Name + patient.Surname + "_Kartela.pdf",
                };
            }
        }
    }
}
