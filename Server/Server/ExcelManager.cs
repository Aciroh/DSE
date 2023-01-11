using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace Server
{
    public class ExcelManager
    {
        String path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName + @"/Resources/Excel.xlsx";
        List<IXLWorksheet> sheets = new List<IXLWorksheet>();
        XLWorkbook workbook;
        

        internal ExcelManager()
        {
            workbook = new XLWorkbook();
            CreateSheet("Generation 1");
        }
        internal void CreateSheet(String sheetname)
        {
                var worksheet = workbook.Worksheets.Add(sheetname);
                sheets.Add(worksheet);
                worksheet.Cell("A1").Value = "Config ID";
                worksheet.Cell("B1").Value = "Config Name";
                worksheet.Cell("C1").Value = "Superscalar";
                worksheet.Cell("D1").Value = "Rename";
                worksheet.Cell("E1").Value = "Reorder";
                worksheet.Cell("F1").Value = "RSB Architecture";
                worksheet.Cell("G1").Value = "RS Per RSB";
                worksheet.Cell("H1").Value = "Speculative";
                worksheet.Cell("I1").Value = "Speculation Accuracy";
                worksheet.Cell("J1").Value = "Separate Dispatch";
                worksheet.Cell("K1").Value = "Integer";
                worksheet.Cell("L1").Value = "Floating";
                worksheet.Cell("M1").Value = "Branch";
                worksheet.Cell("N1").Value = "Memory";
                worksheet.Cell("O1").Value = "Memory Architecture";
                worksheet.Cell("P1").Value = "L1 Data Hitrate";
                worksheet.Cell("Q1").Value = "L1 Code Hitrate";
                worksheet.Cell("R1").Value = "L2 Hitrate";
                worksheet.Cell("S1").Value = "IPC";
                worksheet.Cell("T1").Value = "Power";
                worksheet.Cell("U1").Value = "Front";
                worksheet.Cell("V1").Value = "Crowding Distance";
                workbook.SaveAs(path);
        }
        internal void AddConfiguration(Simulation config)
        {
            var worksheet = sheets.Last();
            int cellNR = 2;
            while (true)
            {
                if (worksheet.Cell("A" + cellNR).Value.IsBlank)
                {
                    worksheet.Cell("A" + cellNR).Value = config.Config.ConfigID;
                    worksheet.Cell("B" + cellNR).Value = config.Config.ConfigName;
                    worksheet.Cell("C" + cellNR).Value = config.Config.Superscalar;
                    worksheet.Cell("D" + cellNR).Value = config.Config.Rename;
                    worksheet.Cell("E" + cellNR).Value = config.Config.Reorder;
                    worksheet.Cell("F" + cellNR).Value = config.Config.RsbArchitecture;
                    worksheet.Cell("G" + cellNR).Value = config.Config.RsPerRsb;
                    worksheet.Cell("H" + cellNR).Value = config.Config.Speculative;
                    worksheet.Cell("I" + cellNR).Value = config.Config.SpeculationAccuracy;
                    worksheet.Cell("J" + cellNR).Value = config.Config.SeparateDispatch;
                    worksheet.Cell("K" + cellNR).Value = config.Config.Integer;
                    worksheet.Cell("L" + cellNR).Value = config.Config.Floating;
                    worksheet.Cell("M" + cellNR).Value = config.Config.Branch;
                    worksheet.Cell("N" + cellNR).Value = config.Config.Memory;
                    worksheet.Cell("O" + cellNR).Value = config.Config.MemoryArchitecture;
                    worksheet.Cell("P" + cellNR).Value = config.Config.L1DataHitrate;
                    worksheet.Cell("Q" + cellNR).Value = config.Config.L1CodeHitrate;
                    worksheet.Cell("R" + cellNR).Value = config.Config.L2Hitrate;
                    workbook.Save();
                    return;
                }
                else if (worksheet.Cell("A" + cellNR).Value.ToString() == config.Config.ConfigID.ToString())
                {
                    if(config.Output != null)
                    {
                        worksheet.Cell("S" + cellNR).Value = config.Output[0];
                        worksheet.Cell("T" + cellNR).Value = config.Output[1];
                        if(config.FrontNumber != null)
                        {
                            worksheet.Cell("U" + cellNR).Value = config.FrontNumber;
                            if(config.CrowdingDistance != null)
                            {
                                worksheet.Cell("V" + cellNR).Value = config.CrowdingDistance;
                            }
                        }
                        workbook.Save();
                        return;
                    }
                }
                cellNR++;
            }
        }
    }
}
