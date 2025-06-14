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
using DocumentFormat.OpenXml.Drawing.Charts;
using Azure.Core;
using DocumentFormat.OpenXml.VariantTypes;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Office2010.Excel;


namespace Hospital_Test.Controllers
{
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View("Trangchu");
        }
        public IActionResult Trangchu()
        {

            return View("~/Views/Shared/Trangchu.cshtml");
        }

        //Form them thiet bi
        public IActionResult Thietbi_Add()
        {
            List<Device> devices;
            devices = DataProvider<Device>.Instance.GetListItem("tbl_device");

            List<Status> statuses;
            statuses = DataProvider<Status>.Instance.GetListItem("tbl_status");

            List<Room> rooms;
            rooms = DataProvider<Room>.Instance.GetListItem("tbl_room");

            List<Contact> contacts;
            contacts = DataProvider<Contact>.Instance.GetListItem("tbl_contact");

            // PHẦN CODE CẬP NHẬT ĐỂ XỬ LÝ MAPPING
            // 1. Tạo dữ liệu mapping cho JavaScript (key: room_type, value: danh sách room_id)
            // Giả định class Room của bạn có thuộc tính 'room_type' và 'room_id'
            var roomMappingData = rooms
                .GroupBy(r => r.room_type)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(r => r.room_id).ToList()
                );
            // Chuyển đổi mapping thành chuỗi JSON và gửi qua ViewBag
            ViewBag.RoomMappingJson = JsonSerializer.Serialize(roomMappingData);

            // 2. Tạo một danh sách các loại khoa (room_type) duy nhất để hiển thị
            // Giả định class Room có 'room_type' (mã khoa) và 'room_name' (tên đầy đủ)
            var distinctRoomTypes = rooms
                .GroupBy(r => r.room_type) // Nhóm theo mã khoa để lấy duy nhất
                .Select(g => g.First()) // Lấy phần tử đầu tiên của mỗi nhóm
                .ToList();

            // Gửi danh sách khoa duy nhất này qua ViewBag
            ViewBag.DistinctRoomTypes = distinctRoomTypes;

            DeviceDetail devicedetails = new DeviceDetail();
            devicedetails.devices_id = devices;
            devicedetails.statuses_device = statuses;
            devicedetails.rooms_device = rooms;
            devicedetails.contacts_device = contacts;

            return View("Thietbi", devicedetails);
        }

        [HttpPost]
        public IActionResult Thietbi_Add(string tbiName, string tbiManufacturer, string[] tbiSeri,
        string tbiType, string tbiGroup, int tbiMaintenance_cycle, string tbiMaintenance_start, string tbiStockout_date,
        string tbiCondition, string tbiReceived_date, string tbiNote, int tbiContact_finance, string tbiContact_address, string tbiContact_type, string tbiRoom, string tbiRoom_type, string tbiStatus)
        {

            List<Contact> Contacts_device = DataProvider<Contact>.Instance.GetListItem("tbl_contact");
            int max_contact = 0;
            foreach (var item in Contacts_device)
            {
                int currentId;
                if (int.TryParse(item.contact_id, out currentId))
                {
                    if (currentId > max_contact)
                    {
                        max_contact = currentId;
                    }
                }
            }
            string tbiContact_id = (max_contact + 1).ToString();

					//4. Thêm vào bảng dbo.tbl_contact khi đã có contact_id và địa chỉ, lưu file với contact_type  là "Hợp đồng bảo trì" 
			string query_contact_device = String.Format("Insert into dbo.tbl_contact (contact_id, contact_type, contact_address, contact_finance)" + "Values ('{0}' , N'{1}' , N'{2}', {3} )", tbiContact_id, tbiContact_type, tbiContact_address, tbiContact_finance);
			DataProvider<Contact>.Instance.ExcuteQuery(query_contact_device);
			
			tbiStatus = "20";
            
            
            foreach (var seri in tbiSeri)
            {
                string seriStr = seri.Trim();
                // Lấy 4 số cuối của mã series
                string last4Digits = seriStr.Length >= 4 ? seriStr.Substring(seriStr.Length - 4) : seriStr.PadLeft(4, '0');
				string manufacturerNoSpaces = tbiManufacturer.Replace(" ", "");
				string first4Letters = manufacturerNoSpaces.Length >= 4 ? manufacturerNoSpaces.Substring(0, 4) : manufacturerNoSpaces;
				string tbiID = $"{tbiType}-{first4Letters}-{last4Digits}";

				string stockoutDateSql = string.IsNullOrEmpty(tbiStockout_date) ? "NULL" : $"'{tbiStockout_date}'";


                string query = String.Format(
                    "INSERT INTO dbo.tbl_device (device_id, device_name, device_manufacturer, device_seri, device_type, device_group, device_maintenance_cycle, device_maintenance_start, device_stockout_date, device_condition, device_received_date, device_note, FK_status_id, FK_room_id, FK_contact_id) " +
                    "VALUES ('{0}', N'{1}', N'{2}', N'{3}', '{4}', N'{5}', '{6}', '{7}', {8}, N'{9}', '{10}', N'{11}', '{12}', '{13}', '{14}')",
                    tbiID, tbiName, tbiManufacturer, seriStr, tbiType, tbiGroup, tbiMaintenance_cycle, tbiMaintenance_start, stockoutDateSql, tbiCondition, tbiReceived_date, tbiNote, tbiStatus, tbiRoom, tbiContact_id);

                DataProvider<Device>.Instance.ExcuteQuery(query);
			
			}
            return RedirectToAction("Baotri");
        }
	
		
		//Hien thi danh sach thiet bi
		public IActionResult Thietbi()
        {
            //khởi tạo
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

            ItemDisplay<Device> deviceList = new ItemDisplay<Device>();
            deviceList.SortOrder = sortOrder;
            deviceList.CurrentSearchField = searchField;
            deviceList.CurrentSearchString = searchString;
            deviceList.CurrentPage = currentPage;


            //Mapping cho room_type và room_id
            // Lấy danh sách phòng để chuẩn bị dữ liệu cho dropdown
            List<Room> rooms = DataProvider<Room>.Instance.GetListItem("tbl_room");

            // 1. Tạo dữ liệu mapping cho JavaScript
            var roomMappingData = rooms
                .GroupBy(r => r.room_type)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(r => r.room_id).ToList()
                );
            ViewBag.RoomMappingJson = JsonSerializer.Serialize(roomMappingData);

            // 2. Tạo một danh sách các loại khoa (room_type) duy nhất
            var distinctRoomTypes = rooms
                .GroupBy(r => r.room_type)
                .Select(g => g.First())
                .ToList();
            ViewBag.DistinctRoomTypes = distinctRoomTypes;


            string query = @"
               SELECT
                    de.*,
                    r.room_name,
                    s.status_name,
                    c.contact_address
                FROM dbo.tbl_device de
                LEFT JOIN dbo.tbl_contact c ON de.FK_contact_id = c.contact_id
                LEFT JOIN dbo.tbl_room r ON de.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON de.FK_status_id = s.status_id";


            List<Device> devices;
            devices = DataProvider<Device>.Instance.GetListItemQuery(query);
            devices = Function.Instance.searchItems(devices, deviceList);
            devices = Function.Instance.sortItems(devices, deviceList.SortOrder);
            deviceList.Paging(devices, 10);

            var deviceForm = new DeviceDetail
            {
                statuses_device = DataProvider<Status>.Instance.GetListItem("tbl_status"),
                rooms_device = DataProvider<Room>.Instance.GetListItem("tbl_room"),
                contacts_device = DataProvider<Contact>.Instance.GetListItem("tbl_contact")

            };

            // Tạo view model tổng hợp
            var viewTbiModel = new DevicePageViewModel
            {
                DeviceList = deviceList,
                DeviceForm = deviceForm
            };

            return View("~/Views/Shared/Thietbi.cshtml", viewTbiModel);
        }

        [HttpPost]
        public IActionResult Thietbi(String sortOrder, String searchString, String searchField, int currentPage = 1)
        {
            return RedirectToAction("Thietbi", new { sort = sortOrder, searchField = searchField, searchString = searchString, page = currentPage });
        }

        [HttpGet]
        public JsonResult Thietbi_GetById(string id)
        {
            string query = $@" 
            SELECT d.*,
           r.room_name,
           t.room_type,
           s.status_name,
           c.contact_address,
           ct.contact_type AS contact_type,
           f.contact_finance
       
          FROM dbo.tbl_device d
          LEFT JOIN dbo.tbl_room r ON d.FK_room_id = r.room_id
          LEFT JOIN dbo.tbl_room t ON d.FK_room_id = t.room_id
          LEFT JOIN dbo.tbl_status s ON d.FK_status_id = s.status_id
    
          LEFT JOIN dbo.tbl_contact c ON d.FK_contact_id = c.contact_id
          LEFT JOIN dbo.tbl_contact ct ON d.FK_contact_id = ct.contact_id
          LEFT JOIN dbo.tbl_contact f ON d.FK_contact_id = f.contact_id
          WHERE d.device_id = '{id}'";

            var device = DataProvider<Device>.Instance.GetListItemQuery(query).FirstOrDefault();
            return Json(device);
        }


		[HttpPost]
		public IActionResult Thietbi_Update(string tbiID, string tbiName, string tbiManufacturer, string tbiSeri,
		string tbiType, string tbiGroup, int tbiMaintenance_cycle, string tbiMaintenance_start, string tbiStockout_date,
		string tbiCondition, string tbiReceived_date, string tbiNote, int tbiContact_finance, string tbiContact_address, string tbiContact_type, string tbiContact_id)
		{
			string query = $@"

        UPDATE dbo.tbl_device
        SET 
            device_id='{tbiID}',
            device_name = N'{tbiName}',
            device_manufacturer = N'{tbiManufacturer}',
            device_seri = '{tbiSeri}',
            device_type = '{tbiType}',
            device_group = N'{tbiGroup}',
            device_maintenance_start = '{tbiMaintenance_start}',
            device_maintenance_cycle = {tbiMaintenance_cycle},
            device_stockout_date = '{tbiStockout_date}',
            device_condition = N'{tbiCondition}',
            device_received_date = '{tbiReceived_date}',
            device_note = N'{tbiNote}'

         WHERE device_id = '{tbiID}'";
            DataProvider<Device>.Instance.ExcuteQuery(query);

			//string queryContact = $@"
   //          UPDATE dbo.tbl_contact
   //          SET
   //          contact_type = N'{tbiContact_type}',
   //          contact_finance = {tbiContact_finance}
   //          WHERE contact_id = '{tbiContact_id}'";

			//DataProvider<Contact>.Instance.ExcuteQuery(queryContact);

			return RedirectToAction("Thietbi");
		}

		[HttpPost]
		public JsonResult Thietbi_Delete(string id)
		{
			try
			{
				string query = $"DELETE FROM dbo.tbl_device WHERE device_id = '{id}'";
				DataProvider<Device>.Instance.ExcuteQuery(query);
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, error = ex.Message });
			}
		}

		/// //////////////////////////////////////////////////////////////
		public IActionResult Baotri_Add()
        {
            List<Maintain> maintains;
            maintains = DataProvider<Maintain>.Instance.GetListItem("tbl_maintain");
            List<Device> devices;
            devices = DataProvider<Device>.Instance.GetListItem("FK_status_id", "00", "tbl_device");
            List<Room> rooms;
            rooms = DataProvider<Room>.Instance.GetListItem("tbl_room");
            List<Contact> contacts;
            contacts = DataProvider<Contact>.Instance.GetListItem("tbl_contact");

            MaintainDetail maintaindetails = new MaintainDetail();
            maintaindetails.maintains_id = maintains;
            maintaindetails.devices_maintain = devices;
            maintaindetails.rooms_maintain = rooms;
            maintaindetails.contacts_maintain = contacts;

            return View("Baotri", maintaindetails);

        }

        [HttpPost]
        public IActionResult Baotri_Add(string btrmaintainID, string btrdevicesname, string Btrdate, string btrDelivery, int btrDeliPhone, string btrMaintainance, int btrMaintainPhone, int btrFinance, int btrContact, string btrStatus, string btrRoom)
        {

            // 2. đoạn này là tự động cộng id cho maintain_id bằng cách lấy id lớn nhất rồi cộng thêm "1" 
            List<Maintain> maintainid = DataProvider<Maintain>.Instance.GetListItem("tbl_maintain");
            int max_maintain = 0;
            foreach (var item in maintainid)
            {
                int currentId;
                if (int.TryParse(item.maintain_id, out currentId))
                {
                    if (currentId > max_maintain)
                    {
                        max_maintain = currentId;
                    }
                }
            }
            string BtrmaintainID = (max_maintain + 1).ToString();

            // 3. đoạn này cũng là tự động cộng id cho contact_id bằng cách lấy id lớn nhất rồi cộng thêm "1" 
            List<Contact> Contacts_maintain = DataProvider<Contact>.Instance.GetListItem("tbl_contact");
            int max_contact = 0;
            foreach (var item in Contacts_maintain)
            {
                int currentId;
                if (int.TryParse(item.contact_id, out currentId))
                {
                    if (currentId > max_contact)
                    {
                        max_contact = currentId;
                    }
                }
            }
            string BtrContact_id = (max_contact + 1).ToString();

            //4. Thêm vào bảng dbo.tbl_contact khi đã có contact_id và địa chỉ, lưu file với contact_type  là "Hợp đồng bảo trì" 
            string query_contact_maintain = String.Format("Insert into dbo.tbl_contact (contact_id, contact_type, contact_address, contact_finance)" + "Values ('{0}' , 2 , N'{1}', {2} )", BtrContact_id, btrContact, btrFinance);
            DataProvider<Contact>.Instance.ExcuteQuery(query_contact_maintain);


            btrStatus = "01";
            btrRoom = "KHO";

            string query = String.Format("Insert into dbo.tbl_maintain (maintain_id, maintain_date, maintain_maintenance, maintain_maintenance_phone, maintain_delivery, maintain_delivery_phone, FK_device_id, FK_status_id, FK_room_id, FK_contact_id )"
            + "values('{0}', '{1}', N'{2}', {3}, N'{4}', {5}, '{6}', '{7}', N'{8}', {9})", BtrmaintainID, Btrdate, btrMaintainance, btrMaintainPhone, btrDelivery, btrDeliPhone, btrdevicesname, btrStatus, btrRoom, BtrContact_id);
            DataProvider<Maintain>.Instance.ExcuteQuery(query);

            string updateQuery = String.Format("UPDATE dbo.tbl_device SET FK_room_id = '{0}' WHERE device_id = '{1}'", btrRoom, btrdevicesname);
            DataProvider<Device>.Instance.ExcuteQuery(updateQuery);

            string updateQuery_device = String.Format("UPDATE dbo.tbl_device SET FK_status_id = '{0}' WHERE device_id = '{1}' ", btrStatus, btrdevicesname);
            DataProvider<Device>.Instance.ExcuteQuery(updateQuery_device);

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
                    d.device_id,
                    d.device_name,
                    r.room_name,
                    s.status_name
                FROM dbo.tbl_device d
                LEFT JOIN dbo.tbl_maintain m ON m.FK_device_id = d.device_id
                LEFT JOIN dbo.tbl_room r ON d.FK_room_id = r.room_id  -- join qua device thay vì maintain
                LEFT JOIN dbo.tbl_status s ON d.FK_status_id = s.status_id
                WHERE d.FK_status_id LIKE '0%'";
            List<Maintain> maintain = DataProvider<Maintain>.Instance.GetListItemQuery(query);

            maintain = Function.Instance.searchItems(maintain, maintainList);
            maintain = Function.Instance.sortItems(maintain, maintainList.SortOrder);
            maintainList.Paging(maintain, 10);

            var maintainForm = new MaintainDetail
            {
                devices_maintain = DataProvider<Device>.Instance.GetListItem("FK_status_id", "00", "tbl_device"),
                rooms_maintain = DataProvider<Room>.Instance.GetListItem("tbl_room")
            };

            DateTime today = DateTime.Today;
            var overdue = maintain
                .Where(m => m.maintain_date != null && m.maintain_date.Value.Date < today)
                .ToList();

            var comingup = maintain
                .Where(m => m.maintain_date != null && m.maintain_date.Value.Date >= today && m.maintain_date.Value.Date <= today.AddDays(2))
                .ToList();

            var completed = maintain
                .Where(m => m.maintain_date != null && m.maintain_date.Value.Date > today.AddDays(2) && m.maintain_date.Value.Date <= today.AddDays(30))
                .ToList();

            // Tạo view model tổng hợp
            var viewModel = new MaintainPageViewModel
            {
                MaintainList = maintainList,
                MaintainForm = maintainForm,
                OverdueList = overdue,
                ComingupList = comingup,
                CompletedList = completed
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
