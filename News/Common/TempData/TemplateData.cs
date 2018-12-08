using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using News.Hubs;
using News.Models;
using News.Models.Entities;
using dBASE.NET;

namespace News.Common.TempData
{
    public class TemplateData
    {
        public MenuModel ListMenu = new MenuModel()
        {
            ListMenu = new List<MENU_ENTITY>()
            {
                new MENU_ENTITY(){MENU_PARENT_ID = 0, MENU_ID = 1, MENU_NAME = "Trang chủ", MENU_ICON_MATERIAL = "layers", MENU_PATH = "./Home", STT = 1},

                new MENU_ENTITY(){MENU_PARENT_ID = 0, MENU_ID = 2, MENU_NAME = "Giới thiệu", MENU_ICON_MATERIAL = "layers", MENU_PATH = "./GioiThieu", STT = 2},
                new MENU_ENTITY(){MENU_PARENT_ID = 2, MENU_ID = 6, MENU_NAME = "Về chúng tôi", MENU_ICON_MATERIAL = "layers", MENU_PATH = "./Home/About", STT = 1},
                new MENU_ENTITY(){MENU_PARENT_ID = 2, MENU_ID = 7, MENU_NAME = "Khách hàng tiêu biểu", MENU_ICON_MATERIAL = "layers", MENU_PATH = "./Home/Contact", STT = 2},

                new MENU_ENTITY(){MENU_PARENT_ID = 0, MENU_ID = 3, MENU_NAME = "Dịch vụ", MENU_ICON_MATERIAL = "layers", MENU_PATH = "~/DichVu", STT = 3},
                new MENU_ENTITY(){MENU_PARENT_ID = 3, MENU_ID = 8, MENU_NAME = "Dịch vụ viễn thông", MENU_ICON_MATERIAL = "layers", MENU_PATH = "~/DichVu/DichVuVienThong", STT = 1},
                new MENU_ENTITY(){MENU_PARENT_ID = 3, MENU_ID = 9, MENU_NAME = "Dịch vụ công nghệ thông tin", MENU_ICON_MATERIAL = "layers", MENU_PATH = "~/DichVu/DichVuCntt", STT = 2},

                new MENU_ENTITY(){MENU_PARENT_ID = 0, MENU_ID = 4, MENU_NAME = "Khuyến mại", MENU_ICON_MATERIAL = "layers", MENU_PATH = "~/KhuyenMai", STT = 4},

                new MENU_ENTITY(){MENU_PARENT_ID = 0, MENU_ID = 5, MENU_NAME = "Hỗ trợ", MENU_ICON_MATERIAL = "layers", MENU_PATH = "~/HoTro", STT = 5},

            }
        };
    }
   
}
