using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.Models.Parameter;

namespace News.Models
{
    public class ExcelTableModel
    {
        public long STT { get; set; }
        public long STT_NV { get; set; }
        public string SO_CV { get; set; }
        public string LAST_CV { get; set; }
        public string TRANG_THAI { get; set; }
        public string DS_CV { get; set; }
        public string CN_PN { get; set; }

        public string MA_DIEM_THU { get; set; }
        public string DUONG_THU { get; set; }
        public string MA_DT { get; set; }
        public string MA_KH { get; set; }
        public string TEN_NHAN_VIEN { get; set; }
        public string DT_NHAN_VIEN { get; set; }
        public string TEN_DON_VI { get; set; }
        public string TEN_TO_TRUONG { get; set; }
        public string MA_CQ { get; set; }
        public string MA_QLKH { get; set; }
        public string TEN_KH { get; set; }
        public string DIA_CHI { get; set; }
        public string THANG_NO { get; set; }
        public string SO_MAY { get; set; }
        public string DV { get; set; }
        public string MA_NH { get; set; }
        public string MANGUOIXL { get; set; }
        public string MADIEMTHU { get; set; }
        public string SM_LHTT { get; set; }
        public string QUA_HAN { get; set; }
        public string DT_CU { get; set; }
        public string DV_CU { get; set; }
        public string DON_DOC { get; set; }
        public string GIAY_BAO { get; set; }
        public string BIEN_BAN { get; set; }
        public string PHUONG { get; set; }
        public string TEN_PHUONG { get; set; }
        public string TEN_QUAN { get; set; }
        public long PHUONG_SO_LUONG { get; set; }
        public long PHUONG_ID { get; set; }

        public string VT { get; set; }
        public string NV_QLTT { get; set; }
        public string NV_QLDB { get; set; }
        public string DONVI_DB { get; set; }
        public string TOBH_DB { get; set; }

        public string THANG { get; set; }
        public string NAM { get; set; }
        public long TIEN { get; set; }
        public string THONG_BAO { get; set; }
        public long TAM_THU { get; set; }
        public long USD { get; set; }
        public long CON_NO { get; set; }
        public string TRA_SAU { get; set; }
        public long DON_VI_ID { get; set; }
        public long NHAN_VIEN_ID { get; set; }

        public string CHECK_TON_TAI { get; set; }

        public List<ExcelParameters> ListExcelSheets { get; set; }
        public ExcelParameters ExcelInfoSheets { get; set; }
    }
}

