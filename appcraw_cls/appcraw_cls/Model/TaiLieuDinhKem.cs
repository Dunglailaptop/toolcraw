using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appcraw_cls.Model
{
    public class TaiLieuDinhKem
    {
        public int MaSoTaiLieuDinhKem {  get; set; }

        public string MaTaiLieuDinhKem { get; set; }

        public string TenTaiLieu {  get; set; }

        public string checksum { get; set; }

        public string LoaiNoiDung {  get; set; }

        public string DungLuong {  get; set; }

        public string Loai {  get; set; }

        public string TenBacSi {  get; set; }
        
        public NguoiDung DataCapNhat { get; set; }

        public DateTime NgayTao {  get; set; }

        public string image {  get; set; }

    }

    public class NguoiDung
    {
        public DateTime NgayCapNhat { get; set; }
        public DateTime NgayTao { get; set; }

        public string NguoiNhat { get; set; }
        public string NguoiCapNhat { get; set; }

    }
}
