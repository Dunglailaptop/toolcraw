using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using System.CodeDom.Compiler;
using appcraw_cls.Model;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;


namespace appcraw_cls
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm1()
        {
            InitializeComponent();
           
        }

        private  void GeneratedCode(string filepath, string namefile)
        {
            // Read the content of the text file
            string htmlContent = File.ReadAllText(filepath);

            // Parse the HTML content using HtmlAgilityPack
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // Select all rows with the class 'j-listitem'
            var rows = htmlDoc.DocumentNode.SelectNodes("//tr[@class='j-listitem']");

            var dataList = new List<TaiLieuDinhKem>();
            var dem = 0;
            // Loop through each row and extract the data
            foreach (var row in rows)
            {
                dem++;
                var tooltipTitle = row.SelectSingleNode("td[9]").GetAttributeValue("title", "");
                NguoiDung tooltipDetails = ParseTooltipDetails(tooltipTitle);

                var data = new TaiLieuDinhKem
                {
                    MaSoTaiLieuDinhKem = dem,
                    MaTaiLieuDinhKem = row.SelectSingleNode("td[2]").InnerText.Trim(),
                    TenTaiLieu = row.SelectSingleNode("td[3]").InnerText.Trim(),
                    checksum = row.SelectSingleNode("td[4]").InnerText.Trim(),
                    LoaiNoiDung = row.SelectSingleNode("td[5]").InnerText.Trim(),
                    DungLuong = row.SelectSingleNode("td[6]").InnerText.Trim(),
                    Loai = row.SelectSingleNode("td[7]").InnerText.Trim(),
                    TenBacSi = row.SelectSingleNode("td[8]").InnerText.Trim(),
                    NgayTao = Convert.ToDateTime(row.SelectSingleNode("td[9]").InnerText.Trim()),
                    DataCapNhat = tooltipDetails
                };

                dataList.Add(data);
            }

            // Convert the data list to JSON
            string json = JsonConvert.SerializeObject(dataList, Newtonsoft.Json.Formatting.Indented);
            loaddataingridcontrol(json);

            // Print the JSON data
            Console.WriteLine(json);

            // Print the current directory
            string currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"Saving file to: {currentDirectory}");

            // Save the JSON data to a new text file
            File.WriteAllText(Path.Combine(currentDirectory, namefile), json);
        }

        public void loaddataingridcontrol(string jsonData)
        {
            try
            {
               

                // Chuyển đổi dữ liệu JSON thành danh sách các đối tượng TaiLieuDinhKem
                var dataList = JsonConvert.DeserializeObject<List<TaiLieuDinhKem>>(jsonData);

                // Thiết lập DataSource cho GridControl
                gridControl1.DataSource = dataList;

                // Tùy chỉnh cột hiển thị trong GridView nếu cần thiết
                GridView gridView = (GridView)gridControl1.MainView;
                gridView.Columns["MaTaiLieuDinhKem"].Caption = "Mã Tài Liệu";
                gridView.Columns["TenTaiLieu"].Caption = "Tên Tài Liệu";
                
                // ...
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu: " + ex.Message);
            }
        }


        public  NguoiDung ParseTooltipDetails(string tooltip)
        {
            var details = new Dictionary<string, string>();
            var lines = tooltip.Split(new[] { "<br/>" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ": " }, StringSplitOptions.None);
                if (parts.Length == 2)
                {
                    details[parts[0].Trim()] = parts[1].Trim();
                }
            }

            var nguoiDung = new NguoiDung();

            if (details.ContainsKey("Ngày tạo"))
            {
                nguoiDung.NgayTao = DateTime.ParseExact(details["Ngày tạo"], "dd/MM/yyyy HH:mm", null);
            }

            if (details.ContainsKey("Ngày cập nhật"))
            {
                nguoiDung.NgayCapNhat = DateTime.ParseExact(details["Ngày cập nhật"], "dd/MM/yyyy HH:mm", null);
            }

            if (details.ContainsKey("Người nhập"))
            {
                nguoiDung.NguoiNhat = details["Người nhập"];
            }

            if (details.ContainsKey("Người cập nhật"))
            {
                nguoiDung.NguoiCapNhat = details["Người cập nhật"];
            }

            return nguoiDung;
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text.Length > 0)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        GeneratedCode(filePath, textEdit1.Text);

                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên filename");
            }
        }
    }
}