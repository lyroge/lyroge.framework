using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace lyroge.framework.snapcodes.NOPI
{
    class NOPIDemo
    {
        #region 内部成员
        IList<DataService.Lesson> _lessenList;
        HttpResponse _response;
        #endregion 

        #region 构造函数
        public NOPIDemo()
        {
            this._lessenList = DataService.GetLessonList();
            this._response = new HttpResponse(new StringWriter());
        }
        #endregion

        /// <summary>
        /// 导出内容到excel下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExportToExcel()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();

            //创建一个单元格样式
            var backgroundColorStyle = workbook.CreateCellStyle();
            backgroundColorStyle.FillPattern = NPOI.SS.UserModel.FillPatternType.SOLID_FOREGROUND;
            backgroundColorStyle.FillForegroundColor = HSSFColor.BLUE.index;

            //创建表头行列及赋值样式
            var headTitles = new[] { "StudentCode", "ClassCode", "RoomName" };
            var headRow = sheet.CreateRow(0);
            for (var i = 0; i < 3; i++)
            {
                var cell = headRow.CreateCell(i);
                if (i == 0)
                    cell.CellStyle = backgroundColorStyle;
                cell.SetCellValue(headTitles[i]);
            }

            //创建数据行列
            for (int i = 1; i < this._lessenList.Count + 1; i++)
            {
                var lesson = this._lessenList[i - 1];

                var row = sheet.CreateRow(i);
                if (lesson.StudentCode == "BJ02")
                    row.Height = 50 * 20;

                //设置背景色
                var cellStudentCode = row.CreateCell(0);
                cellStudentCode.CellStyle = backgroundColorStyle;
                cellStudentCode.SetCellValue(lesson.StudentCode);

                row.CreateCell(1).SetCellValue(lesson.ClassCode);
                row.CreateCell(2).SetCellValue(lesson.RoomName);
            }

            //数据写进内存流
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            //销毁对象
            workbook = null;
            sheet = null;
            headRow = null;

            
            //将内存流以附件形式输出到客户端
            var bytes = memoryStream.ToArray();
            this._response.AddHeader("Content-Disposition", "attachment;filename=1.xls");
            this._response.AddHeader("Content-Length", bytes.Length.ToString());
            this._response.OutputStream.Write(bytes, 0, bytes.Length);
            this._response.Flush();
        }

        /// <summary>
        /// 上传excel表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UploadExcel(Stream fileStream)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(fileStream);
            var sheet = workbook.GetSheetAt(0);

            for (var i = 1; i < sheet.LastRowNum + 1; i++)
            {
                var StudentCode = sheet.GetRow(i).Cells[0].StringCellValue;
                var ClassCode = sheet.GetRow(i).Cells[1].StringCellValue;
                var lesson = this._lessenList.Where(l => l.StudentCode == StudentCode && l.ClassCode == ClassCode).FirstOrDefault();
                if (lesson != null)
                    lesson.RoomName = sheet.GetRow(i).Cells[2].StringCellValue;
            }
        }
    }
}
