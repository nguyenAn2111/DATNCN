using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Threading.Tasks;
using Hospital_Test.Models;
using Hospital_Test.DAO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System.Reflection.Metadata;
using System.Collections;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Configuration;
using DocumentFormat.OpenXml.Office2013.Excel;
using Mono.TextTemplating;

namespace Hospital_Test.Controllers
{
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View("Trangchu");
        }
        public IActionResult Trangchu() {

            return View("~/Views/Shared/Trangchu.cshtml");
        }
        public IActionResult Thietbi() {

            return View("~/Views/Shared/Thietbi.cshtml");
        }
        public IActionResult Baotri_Suachua() {
            return View("~/Views/Shared/Baotri_Suachua.cshtml");
        }


        public IActionResult Baotri_Add()
        {
            List<Device> devices;
            devices = DataProvider<Device>.Instance.GetListItem("FK_status_id", "00", "tbl_device");
            List<Room> rooms;
            rooms = DataProvider<Room>.Instance.GetListItem("tbl_room");

            MaintainDetail maintaindetails = new MaintainDetail();
            maintaindetails.devices = devices;
            maintaindetails.rooms = rooms;

            return RedirectToAction("Baotri");
        }
        [HttpPost]
        public IActionResult Baotri_Add(string btrname, string btrID, string Btrdate, string btrDelivery, int btrDeliPhone, string btrMaintain, int btrMaintainPhone )
        {
            DateTime btrdate = new DateTime();
            btrdate = DateTime.Parse(Btrdate);
            string maintainDate = btrdate.ToString("yyyy-MM-dd");

            //string query = String.Format("Insert into dbo.tbl_Surgery(Surgery_ID, FK_Patient_ID, Surgery_Time, FK_Staff_Main, FK_Room_ID) " +
            //"values({0},{1}, '{2}', {3}, {4} )", surgeryID, pID, surgerytime, surmain, roomID);
            //DataProvider<Staff>.Instance.ExcuteQuery(query);
            return RedirectToAction("Baotri");
        }

        public IActionResult Baotri()
        {
            // Lấy tham số từ query như hiện tại
            string field;
            string sortOrder;
            string searchField;
            string searchString;
            string page;

            var urlQuery = Request.HttpContext.Request.Query;
            field = urlQuery["field"];
            sortOrder = urlQuery["sort"];
            searchField = urlQuery["searchField"];
            searchString = urlQuery["SearchString"];
            page = urlQuery["page"];

            field = field == null ? "All" : field;
            sortOrder = sortOrder == null ? "Name" : sortOrder;
            searchField = searchField == null ? "device_name" : searchField;
            searchString = searchString == null ? "" : searchString;
            page = page == null ? "1" : page;
            int currentPage = Convert.ToInt32(page);

            ItemDisplay<Maintain> maintainList = new ItemDisplay<Maintain>();
            maintainList.SortOrder = sortOrder;
            maintainList.CurrentSearchField = searchField;
            maintainList.CurrentSearchString = searchString;
            maintainList.CurrentPage = currentPage;

            string query = @"
               SELECT
                    m.*,
                    d.device_name,
                    r.room_name,
                    s.status_name
                FROM dbo.tbl_maintain m
                LEFT JOIN dbo.tbl_device d ON m.FK_device_id = d.device_id
                LEFT JOIN dbo.tbl_room r ON m.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON m.FK_status_id = s.status_id
                WHERE s.status_id LIKE '0%'";

            List<Maintain> maintain = DataProvider<Maintain>.Instance.GetListItemQuery(query);
            maintain = Function.Instance.searchItems(maintain, maintainList);
            maintain = Function.Instance.sortItems(maintain, maintainList.SortOrder);
            maintainList.Paging(maintain, 10);

            // Lấy dữ liệu devices và rooms cho form lập kế hoạch
            var maintainForm = new MaintainDetail
            {
                devices = DataProvider<Device>.Instance.GetListItem("FK_status_id", "00", "tbl_device"),
                rooms = DataProvider<Room>.Instance.GetListItem("", "tbl_room")
            };

            // Tạo view model tổng hợp
            var viewModel = new MaintainPageViewModel
            {
                MaintainList = maintainList,
                MaintainForm = maintainForm
            };

            return View("~/Views/Shared/Baotri.cshtml", viewModel);
        }
        [HttpPost]
        public IActionResult Baotri(String sortOrder, String searchString, String searchField, int currentPage = 1)
        {
            return RedirectToAction("Baotri", new { sort = sortOrder, searchField = searchField, searchString = searchString, page = currentPage });
        }
        public IActionResult Suachua()
        {
            // Khởi tạo
            string field;
            string sortOrder;
            string searchField;
            string searchString;
            string page;

            /// Lấy query, không có => đặt mặc định
            var urlQuery = Request.HttpContext.Request.Query;
            field = urlQuery["field"];
            sortOrder = urlQuery["sort"];
            searchField = urlQuery["searchField"];
            searchString = urlQuery["SearchString"];
            page = urlQuery["page"];
            field = field == null ? "All" : field;

            sortOrder = sortOrder == null ? "Name" : sortOrder; ;
            searchField = searchField == null ? "device_name" : searchField;
            searchString = searchString == null ? "" : searchString;
            page = page == null ? "1" : page;
            int currentPage = Convert.ToInt32(page);

            ItemDisplay<Repair> RepairList = new ItemDisplay<Repair>();
            RepairList.SortOrder = sortOrder;
            RepairList.CurrentSearchField = searchField;
            RepairList.CurrentSearchString = searchString;
            RepairList.CurrentPage = currentPage;

            string query = @"
               SELECT
                    re.*,
                    d.device_name,
                    r.room_name,
                    s.status_name
                FROM dbo.tbl_repair re
                LEFT JOIN dbo.tbl_device d ON re.FK_device_id = d.device_id
                LEFT JOIN dbo.tbl_room r ON re.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON re.FK_status_id = s.status_id
                WHERE s.status_id LIKE '1%'";

            List<Repair> Repair;
            Repair = DataProvider<Repair>.Instance.GetListItemQuery(query);
            Repair = Function.Instance.searchItems(Repair, RepairList);
            Repair = Function.Instance.sortItems(Repair, RepairList.SortOrder);
            RepairList.Paging(Repair, 10);

            return View("~/Views/Shared/Suachua.cshtml", RepairList);
        }
        [HttpPost]
        public IActionResult Suachua(String sortOrder, String searchString, String searchField, int currentPage = 1)
        {
            return RedirectToAction("Suachua", new { sort = sortOrder, searchField = searchField, searchString = searchString, page = currentPage });
        }
        public IActionResult Kho()
        {
            // Khởi tạo
            string field;
            string sortOrder;
            string searchField;
            string searchString;
            string page;

            /// Lấy query, không có => đặt mặc định
            var urlQuery = Request.HttpContext.Request.Query;
            field = urlQuery["field"];
            sortOrder = urlQuery["sort"];
            searchField = urlQuery["searchField"];
            searchString = urlQuery["SearchString"];
            page = urlQuery["page"];
            field = field == null ? "All" : field;

            sortOrder = sortOrder == null ? "Name" : sortOrder; ;
            searchField = searchField == null ? "device_name" : searchField;
            searchString = searchString == null ? "" : searchString;
            page = page == null ? "1" : page;
            int currentPage = Convert.ToInt32(page);

            ItemDisplay<Storage> StorageList = new ItemDisplay<Storage>();
            StorageList.SortOrder = sortOrder;
            StorageList.CurrentSearchField = searchField;
            StorageList.CurrentSearchString = searchString;
            StorageList.CurrentPage = currentPage;



            return View("~/Views/Shared/Kho.cshtml", StorageList);
        }
        public IActionResult Taichinh_Hopdong()
        {
            return View("~/Views/Shared/Taichinh_Hopdong.cshtml");
        }
    }

}
