using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

public class WordCreator
{

    private Microsoft.Office.Interop.Word.Application _wordApplication;

    private Microsoft.Office.Interop.Word.Document _wordDocument;

    public void CreateAWord()
    {
        //实例化word应用对象 
        this._wordApplication = new Microsoft.Office.Interop.Word.ApplicationClass();
        Object myNothing = System.Reflection.Missing.Value;

        this._wordDocument = this._wordApplication.Documents.Add(ref myNothing, ref myNothing, ref myNothing, ref myNothing);
    }

    public void SetPageHeader(string pPageHeader)
    {
        //添加页眉 
        this._wordApplication.ActiveWindow.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdOutlineView;
        this._wordApplication.ActiveWindow.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekPrimaryHeader;
        this._wordApplication.ActiveWindow.ActivePane.Selection.InsertAfter(pPageHeader);
        //设置中间对齐 
        this._wordApplication.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //跳出页眉设置 
        this._wordApplication.ActiveWindow.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;
    }
    /// 
    /// 插入文字 

    public void InsertText(string pText, int pFontSize, Microsoft.Office.Interop.Word.WdColor pFontColor, int pFontBold, Microsoft.Office.Interop.Word.WdParagraphAlignment ptextAlignment)
    {
        //设置字体样式以及方向 
        this._wordApplication.Application.Selection.Font.Size = pFontSize;
        this._wordApplication.Application.Selection.Font.Bold = pFontBold;
        this._wordApplication.Application.Selection.Font.Color = pFontColor;
        this._wordApplication.Application.Selection.ParagraphFormat.Alignment = ptextAlignment;
        this._wordApplication.Application.Selection.TypeText(pText);
    }


    /// 
    /// 换行 
    /// 
    public void NewLine()
    {

        this._wordApplication.Application.Selection.TypeParagraph();
    }

    public void InsertPicture(string pPictureFileName)
    {
        object myNothing = System.Reflection.Missing.Value;
        //图片居中显示 
        this._wordApplication.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        this._wordApplication.Application.Selection.InlineShapes.AddPicture(pPictureFileName, ref myNothing, ref myNothing, ref myNothing);
    }
    /// 
    /// 保存文件 

    public void SaveWord(string pFileName)
    {
        string sPath = Application.StartupPath;
        //string fileOutFJ = sPath + @"\PCL\" + this.listView1.SelectedItems[i].SubItems[0].Text.ToString() + "FJ" + ".prn";//追加的纸
        object fileName = sPath+@"\附加页\"+pFileName;//移动到指定位置
        object myNothing = System.Reflection.Missing.Value;
        object myFileName = pFileName;
        object myWordFormatDocument = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
        object myLockd = false;
        object myPassword = "";
        object myAddto = true;
        try
        {
            this._wordDocument.SaveAs(ref fileName, ref myWordFormatDocument, ref myLockd, ref myPassword, ref myAddto, ref myPassword,
            ref myLockd, ref myLockd, ref myLockd, ref myLockd, ref myNothing, ref myNothing, ref myNothing,
            ref myNothing, ref myNothing, ref myNothing);
            this._wordDocument.Close();
            this._wordApplication.Quit();
            _wordApplication = null;
            _wordDocument = null;
        }
        catch(Exception ex)
        {
           // throw new Exception("导出word文档失败!");
            MessageBox.Show(ex.Message);
        }
        
    }
    
　　
　　
} 

