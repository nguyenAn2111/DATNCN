﻿using Microsoft.AspNetCore.Mvc;
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
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Vml;
using Humanizer;
using System.Globalization;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Bibliography;
namespace Hospital_Test.Controllers
{
	public class ManagementController : Controller
	{
		public IActionResult Index()
		{
			return RedirectToAction("Trangchu");
		}

		public IActionResult Trangchu()
		{
			try
			{
				// 1. Tổng số thiết bị
				string totalDevicesQuery = "SELECT COUNT(*) AS Count FROM dbo.tbl_device";
				var totalDevicesResult = DataProvider<Device>.Instance.ExcuteQuery(totalDevicesQuery);
				int totalDevices = totalDevicesResult.Rows.Count > 0 ? Convert.ToInt32(totalDevicesResult.Rows[0]["Count"]) : 0;
				ViewBag.TotalDevices = totalDevices;
				Console.WriteLine($"Total Devices: {totalDevices}");

				// 2. Thiết bị đang sử dụng
				string devicesInUseQuery = "SELECT COUNT(*) AS Count FROM dbo.tbl_device WHERE FK_status_id = '20'";
				var devicesInUseResult = DataProvider<Device>.Instance.ExcuteQuery(devicesInUseQuery);
				int devicesInUse = devicesInUseResult.Rows.Count > 0 ? Convert.ToInt32(devicesInUseResult.Rows[0]["Count"]) : 0;
				ViewBag.DevicesInUse = devicesInUse;
				Console.WriteLine($"Devices In Use: {devicesInUse}");

				// 3. Thiết bị trong kho
				string devicesInStockQuery = "SELECT COUNT(*) AS Count FROM dbo.tbl_device WHERE FK_room_id = 'KHO'";
				var devicesInStockResult = DataProvider<Device>.Instance.ExcuteQuery(devicesInStockQuery);
				int devicesInStock = devicesInStockResult.Rows.Count > 0 ? Convert.ToInt32(devicesInStockResult.Rows[0]["Count"]) : 0;
				ViewBag.DevicesInStock = devicesInStock;
				Console.WriteLine($"Devices In Stock: {devicesInStock}");

				// 4. Lịch sử điều chuyển
				string query_str = @"
			SELECT
				st.*,
				d.device_id,
				d.device_name,
				r_from.room_name AS room_from_name,
				r_to.room_name AS room_to_name
			FROM dbo.tbl_storage st
			LEFT JOIN dbo.tbl_device d ON st.FK_device_id = d.device_id
			LEFT JOIN dbo.tbl_room r_from ON st.FK_room_id_from = r_from.room_id
			LEFT JOIN dbo.tbl_room r_to ON st.FK_room_id_to = r_to.room_id";

				var storage = DataProvider<Storage>.Instance.GetListItemQuery(query_str);
				ViewBag.StorageHistory = storage;
				Console.WriteLine($"Storage History Count: {storage.Count}");

				// 5. Tài chính – tổng chi phí
				string queryDevice = @"
			SELECT d.*, c.contact_finance
			FROM dbo.tbl_device d
			LEFT JOIN dbo.tbl_contact c ON d.FK_contact_id = c.contact_id";
				var devices = DataProvider<Device>.Instance.GetListItemQuery(queryDevice);

				string queryMaintain = @"
			SELECT m.*, c.contact_finance
			FROM dbo.tbl_maintain m
			LEFT JOIN dbo.tbl_contact c ON m.FK_contact_id = c.contact_id";
				var maintain = DataProvider<Maintain>.Instance.GetListItemQuery(queryMaintain);

				string queryRepair = @"
			SELECT r.*, c.contact_finance
			FROM dbo.tbl_repair r
			LEFT JOIN dbo.tbl_contact c ON r.FK_contact_id = c.contact_id";
				var repair = DataProvider<Repair>.Instance.GetListItemQuery(queryRepair);

				long totalCost1 = devices.Sum(d =>
				{
					if (string.IsNullOrWhiteSpace(d.contact_finance)) return 0;
					return long.TryParse(d.contact_finance, out long val) ? val : 0;
				});
				long totalCost2 = maintain.Sum(d =>
				{
					if (string.IsNullOrWhiteSpace(d.contact_finance)) return 0;
					return long.TryParse(d.contact_finance, out long val) ? val : 0;
				});
				long totalCost3 = repair.Sum(d =>
				{
					if (string.IsNullOrWhiteSpace(d.contact_finance)) return 0;
					return long.TryParse(d.contact_finance, out long val) ? val : 0;
				});

				ViewBag.TotalCost1 = totalCost1;
				ViewBag.TotalCost2 = totalCost2;
				ViewBag.TotalCost3 = totalCost3;
				

				// 6. Biểu đồ theo tháng
				string chartQuery = @"
			SELECT MonthYear, SUM(TotalFinance) AS TotalFinance
			FROM (
				SELECT FORMAT(d.device_received_date, 'yyyy-MM') AS MonthYear, ISNULL(c.contact_finance, 0) AS TotalFinance
				FROM dbo.tbl_device d
				LEFT JOIN dbo.tbl_contact c ON d.FK_contact_id = c.contact_id
				WHERE d.device_received_date IS NOT NULL

				UNION ALL

				SELECT FORMAT(m.maintain_date, 'yyyy-MM') AS MonthYear, ISNULL(c.contact_finance, 0) AS TotalFinance
				FROM dbo.tbl_maintain m
				LEFT JOIN dbo.tbl_contact c ON m.FK_contact_id = c.contact_id
				WHERE m.maintain_date IS NOT NULL

				UNION ALL

				SELECT FORMAT(r.repair_date, 'yyyy-MM') AS MonthYear, ISNULL(c.contact_finance, 0) AS TotalFinance
				FROM dbo.tbl_repair r
				LEFT JOIN dbo.tbl_contact c ON r.FK_contact_id = c.contact_id
				WHERE r.repair_date IS NOT NULL
			) AS Combined
			GROUP BY MonthYear
			ORDER BY MonthYear DESC";

				var chartData = DataProvider<Contact>.Instance.GetListItemQueryRaw(chartQuery)
					.Take(5).OrderBy(d => d["MonthYear"]).ToList();

				ViewBag.ChartLabels = chartData.Select(d => d["MonthYear"].ToString()).ToList();
				ViewBag.ChartValues = chartData.Select(d =>
				{
					var val = d["TotalFinance"];
					return val != DBNull.Value ? Convert.ToInt64(val) : 0;
				}).ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine("LỖI: " + ex.ToString());

				ViewBag.TotalDevices = ViewBag.DevicesInUse = ViewBag.DevicesInStock = 0;
				ViewBag.StorageHistory = new List<Storage>();
				ViewBag.ChartLabels = new List<string>();
				ViewBag.ChartValues = new List<int>();
				ViewBag.TotalCost1 = ViewBag.TotalCost2 = ViewBag.TotalCost3 = 0;
			}

			return View("~/Views/Shared/Trangchu.cshtml");
		}

		//-------------Thiết bị---------------------
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
			string query_contact_device = String.Format("Insert into dbo.tbl_contact (contact_id, contact_type, contact_address, contact_finance)" + "Values ({0} , {1} , N'{2}', {3})", tbiContact_id, tbiContact_type, tbiContact_address, tbiContact_finance);
			DataProvider<Contact>.Instance.ExcuteQuery(query_contact_device);

			if (tbiRoom.Trim().ToLower().Contains("kho"))
			{
				tbiStatus = "30";
			}
			else
			{
				tbiStatus = "20";
			}

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
			return RedirectToAction("Thietbi");
		}


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
			sortOrder = sortOrder == null ? "device_received_date" : sortOrder;
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
			CheckAllDevicesMaintenanceStatus();// chuyển từ Thietbi về Baotri
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
			string last4Digits = tbiSeri.Length >= 4 ? tbiSeri.Substring(tbiSeri.Length - 4) : tbiSeri.PadLeft(4, '0');
			string manufacturerNoSpaces = tbiManufacturer.Replace(" ", "");
			string first4Letters = manufacturerNoSpaces.Length >= 4 ? manufacturerNoSpaces.Substring(0, 4) : manufacturerNoSpaces;
			string tbiId = $"{tbiType}-{first4Letters}-{last4Digits}";
			string query = $@"

        UPDATE dbo.tbl_device
        SET 
            device_id='{tbiId}',
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

		public void CheckAllDevicesMaintenanceStatus() //sửa lại cách tính chu kì bằng Tháng
		{
			// Lấy danh sách tất cả thiết bị
			string getAllDevicesQuery = "SELECT device_id, device_maintenance_start, device_maintenance_cycle, FK_status_id FROM dbo.tbl_device";
			var dt = DataProvider<Device>.Instance.ExcuteQuery(getAllDevicesQuery);

			DateTime currentDate = DateTime.Today;

			foreach (DataRow deviceRow in dt.Rows)
			{
				// Kiểm tra null trước khi convert
				if (deviceRow["device_maintenance_start"] != DBNull.Value &&
					deviceRow["device_maintenance_cycle"] != DBNull.Value)
				{
					string deviceId = deviceRow["device_id"].ToString();
					DateTime maintenanceStart = Convert.ToDateTime(deviceRow["device_maintenance_start"]);
					int maintenanceCycle = Convert.ToInt32(deviceRow["device_maintenance_cycle"]);

					// Tính ngày hết hạn (thêm số tháng của chu kỳ vào ngày bắt đầu)
					DateTime expirationDate = maintenanceStart.AddMonths(maintenanceCycle).Date;

					// Nếu ngày hiện tại >= ngày hết hạn và trạng thái chưa phải "00"
					if (currentDate >= expirationDate && deviceRow["FK_status_id"].ToString() == "20")
					{
						string updateStatusQuery = $"UPDATE dbo.tbl_device SET FK_status_id = '00' WHERE device_id = '{deviceId}'";
						DataProvider<Device>.Instance.ExcuteQuery(updateStatusQuery);

						// Có thể thêm log ở đây
						Console.WriteLine($"Đã cập nhật thiết bị {deviceId} sang trạng thái bảo trì");
					}
				}
			}
		}

		public IActionResult Baotri_Suachua()
		{
			return View("~/Views/Shared/Baotri_Suachua.cshtml");
		}
		/// //////////////////////////////////////////////////////////////
		//---------------------------------------Bảo trì--------------------------------
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
		public IActionResult Baotri_Add(string btrmaintainID, string btrdevicesname, string Btrdate, string btrDelivery, int btrDeliPhone, string btrMaintainance, int btrMaintainPhone, int btrFinance, string btrContact, string btrStatus, string btrRoom)
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

			//4. Thêm vào bảng dbo.tbl_contact khi đã có contact_id và địa chỉ, lưu file với contact_type  là "Hợp đồng bảo trì", sủa lại 
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
    d.device_maintenance_start, 
    d.device_maintenance_cycle,
DATEADD(MONTH, d.device_maintenance_cycle, d.device_maintenance_start) AS expiration_date,
    d.FK_status_id, 
    r.room_name, 
    s.status_name
FROM dbo.tbl_device d
LEFT JOIN dbo.tbl_maintain m ON m.FK_device_id = d.device_id
LEFT JOIN dbo.tbl_room r ON d.FK_room_id = r.room_id
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
				.Where(m => m.expiration_date != null && m.expiration_date.Value.Date < today)
				.ToList();

			var comingup = maintain
				.Where(m => m.expiration_date != null &&
							m.expiration_date.Value.Date == today )
							
				.ToList();

			var completed = maintain
				.Where(m => m.maintain_date != null &&
				m.maintain_date.Value.Date <= today.AddDays(10))
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
		public JsonResult Baotri_GetById(string id)
		{
			string query = $@"
               SELECT 
                    m.*, 
                    d.device_id, 
                    d.device_name, 
                    r.room_name, 
                    s.status_name
                FROM dbo.tbl_maintain m
                LEFT JOIN dbo.tbl_device d ON m.FK_device_id = d.device_id
                LEFT JOIN dbo.tbl_room r ON d.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON d.FK_status_id = s.status_id
                WHERE m.maintain_id = '{id}'";
			var maintain = DataProvider<Maintain>.Instance.GetListItemQuery(query).FirstOrDefault();
			return Json(maintain);
		}
		[HttpPost]
		public IActionResult Baotri_Update_Status(string fkStatus, string device_id)
		{
			Device status_update = DataProvider<Device>.Instance.GetListItem("tbl_device").FirstOrDefault(x => x.device_id == device_id);

			if (status_update == null)
			{
				TempData["msg"] = "Không tìm thấy thiết bị!";
				return RedirectToAction("Suachua");
			}

			string status_update_query = String.Format(
				"UPDATE dbo.tbl_device SET FK_status_id = '{0}' WHERE device_id = '{1}'",
				fkStatus.Replace("'", "''"),
				device_id.Replace("'", "''")
			);
			DataProvider<Device>.Instance.ExcuteQuery(status_update_query);
			string status_update_maintain = String.Format(
			  "UPDATE dbo.tbl_maintain SET FK_status_id = '{0}' WHERE FK_device_id = '{1}'",
			  fkStatus.Replace("'", "''"),
			  device_id.Replace("'", "''")
		  );
			DataProvider<Maintain>.Instance.ExcuteQuery(status_update_maintain);
			TempData["msg"] = "Cập nhật trạng thái thành công!";
			return RedirectToAction("Baotri");
		}


		//--------------------------------Sửa chữa---------------------------------------
		public IActionResult Suachua_Add()
		{
			List<Repair> repairs;
			repairs = DataProvider<Repair>.Instance.GetListItem("tbl_repair");
			List<Device> devices;
			devices = DataProvider<Device>.Instance.GetListItem("tbl_device");
			List<Room> rooms;
			rooms = DataProvider<Room>.Instance.GetListItem("tbl_room");
			List<Contact> contacts;
			contacts = DataProvider<Contact>.Instance.GetListItem("tbl_contact");

			RepairDetail repairdetails = new RepairDetail();
			repairdetails.repairs_id = repairs;
			repairdetails.devices_repair = devices;
			repairdetails.rooms_repair = rooms;
			repairdetails.contacts_repair = contacts;

			return View("Suachua", repairdetails);
		}
		[HttpPost]
		public IActionResult Suachua_Add(string schID, string schBroken, string schPriority, string schDate, string schNote, string schImage, int schFinance, string schContact, string schDevice, string schStatus, string schRoom, string schUpdateDate, string schUpdateStatus, string schUpdateNote)
		{


			List<Repair> repairsID = DataProvider<Repair>.Instance.GetListItem("tbl_repair");
			int max_repair = 0;
			foreach (var item in repairsID)
			{
				int currentrepair_Id;
				// Chuyển đổi từ string sang int (cần đảm bảo repair_id có thể chuyển đổi sang int)
				if (int.TryParse(item.repair_id, out currentrepair_Id))
				{
					if (currentrepair_Id > max_repair)
					{
						max_repair = currentrepair_Id;
					}
				}
			}
			string SchID = (max_repair + 1).ToString();

			//Tạo id cho contact
			List<Contact> Contacts_repair = DataProvider<Contact>.Instance.GetListItem("tbl_contact");
			int max_contact = 0;
			foreach (var item in Contacts_repair)
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
			string SchContact_id = (max_contact + 1).ToString();


			// Thêm vào bảng dbo.tbl_contact khi đã có contact_id và địa chỉ, lưu file với contact_type  là "Hợp đồng suachua" 
			string query_contact_repair = String.Format("Insert into dbo.tbl_contact (contact_id, contact_type, contact_address, contact_finance)" + "Values ('{0}' , 3 , N'{1}', {2} )", SchContact_id, schContact, schFinance);
			DataProvider<Contact>.Instance.ExcuteQuery(query_contact_repair);


			DateTime SchUpdateDate;
			if (string.IsNullOrEmpty(schUpdateDate) ||
				!DateTime.TryParseExact(schUpdateDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out SchUpdateDate))
			{
				SchUpdateDate = DateTime.Now;
			}

			//SQL trước đang để schUpdateStatus là int, hiện giờ đã đổi sang string cho đồng bộ
			schUpdateStatus = "Thêm thiết bị sửa chữa";
			schUpdateNote = "Đã thêm thông tin của thiết bị cần sửa chữa";
			schStatus = "11";
			schRoom = "KHO";

			string schBrokenFormat = DateTime.ParseExact(schBroken, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
			string schDateFormat = DateTime.ParseExact(schDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
			string schUpdateDateFormat = SchUpdateDate.ToString("yyyy-MM-dd HH:mm:ss");

			string query = String.Format(
			"INSERT INTO dbo.tbl_repair (repair_id, repair_broken, repair_priority, repair_date, repair_update_date, repair_update_status, repair_note, repair_picture, repair_update_note, FK_room_id, FK_device_id, FK_status_id, FK_contact_id) " +
							"VALUES ('{0}', '{1}',{2}, '{3}', '{4}', N'{5}', N'{6}', '{7}', N'{8}', '{9}', '{10}', '{11}', '{12}')", SchID, schBrokenFormat, schPriority, schDateFormat, schUpdateDateFormat, schUpdateStatus, schNote, schImage, schUpdateNote, schRoom, schDevice, schStatus, SchContact_id);
			DataProvider<Repair>.Instance.ExcuteQuery(query);

			string updateQuery_device = String.Format("UPDATE dbo.tbl_device SET FK_status_id = '{0}' WHERE device_id = '{1}' ", schStatus, schDevice);
			DataProvider<Device>.Instance.ExcuteQuery(updateQuery_device);

			string updateQuery = String.Format("UPDATE dbo.tbl_device SET FK_room_id = '{0}' WHERE device_id = '{1}'", schRoom, schDevice);
			DataProvider<Device>.Instance.ExcuteQuery(updateQuery);

			return RedirectToAction("Suachua");
		}

		[HttpGet]
		public JsonResult Suachua_GetById(string id, string purpose = null)
		{
			string query = $@"
            SELECT
                re.*,
                d.device_id,
                d.device_name,
                r.room_name,
                s.status_name,
                f.contact_finance,
                f.contact_address
            FROM dbo.tbl_repair re
            LEFT JOIN dbo.tbl_device d ON re.FK_device_id = d.device_id
            LEFT JOIN dbo.tbl_room r ON d.FK_room_id = r.room_id
            LEFT JOIN dbo.tbl_status s ON d.FK_status_id = s.status_id
            LEFT JOIN dbo.tbl_contact f ON re.FK_contact_id = f.contact_id
            WHERE re.repair_id = '{id}'";

			var repair = DataProvider<Repair>.Instance.GetListItemQuery(query).FirstOrDefault();

			if (repair == null)
				return Json(new { success = false, message = "Không tìm thấy thông tin sửa chữa" });

			if (purpose == "status_update")
			{
				return Json(new
				{
					success = true,
					repair_id = repair.repair_id,
					repair_update_status = repair.repair_update_status,
					repair_update_note = repair.repair_update_note,
					repair_update_date = repair.repair_update_date,
					fk_status_id = repair.FK_status_id // Sửa thành chữ thường để nhất quán, thêm dòng 
				});
			}

			return Json(repair);
		}


		[HttpGet]
		public IActionResult Suachua_UpdateStatus(string repair_id)
		{
			var repair = DataProvider<Repair>.Instance.GetListItem("tbl_repair")
						 .FirstOrDefault(x => x.repair_id == repair_id);

			if (repair == null)
				return NotFound();

			var viewModel = new RepairPageViewModel
			{
				RepairStatus = repair,
				// Gán các thuộc tính khác nếu cần
			};

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult Suachua_UpdateStatus(string schID, string schstatus, string schnote, DateTime schdate, string totalStatus)
		{
			// Lấy thông tin sửa chữa
			Repair repairs_update = DataProvider<Repair>.Instance.GetListItem("tbl_repair")
				.FirstOrDefault(x => x.repair_id == schID);

			if (repairs_update == null)
			{
				return NotFound();
			}

			// Lấy thông tin thiết bị tương ứng
			var device = DataProvider<Device>.Instance.GetListItem("tbl_device")
				.FirstOrDefault(x => x.device_id == repairs_update.FK_device_id);

			if (device == null)
			{
				return NotFound("Không tìm thấy thiết bị tương ứng");
			}

			// Cập nhật thông tin sửa chữa
			repairs_update.repair_update_status = schstatus;
			repairs_update.repair_update_note = schnote;
			repairs_update.repair_update_date = schdate;
			repairs_update.FK_status_id = totalStatus;

			// Cập nhật trạng thái thiết bị
			device.FK_status_id = totalStatus;

			// Tạo transaction để đảm bảo cả 2 bảng cùng được cập nhật
			string repair_update = String.Format(
				"BEGIN TRANSACTION; " +
				"UPDATE dbo.tbl_repair SET repair_update_date = '{0}', repair_update_note = N'{1}', repair_update_status = N'{2}', FK_status_id = '{3}' WHERE repair_id = '{4}'; " +
				"UPDATE dbo.tbl_device SET FK_status_id = '{3}' WHERE device_id = '{5}'; " +
				"COMMIT;",
				schdate.ToString("yyyy-MM-dd HH:mm:ss"),
				schnote.Replace("'", "''"),
				schstatus.Replace("'", "''"),
				totalStatus.Replace("'", "''"),
				schID.Replace("'", "''"),
				repairs_update.FK_device_id?.Replace("'", "''") ?? ""
			);

			DataProvider<Repair>.Instance.ExcuteQuery(repair_update);

			return RedirectToAction("Suachua");
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

			ItemDisplay<Repair> repairList = new ItemDisplay<Repair>();
			repairList.SortOrder = sortOrder;
			repairList.CurrentSearchField = searchField;
			repairList.CurrentSearchString = searchString;
			repairList.CurrentPage = currentPage;

			string query = @"
                SELECT
                  re.*,
                  d.device_id,
                  d.device_name,
                  r.room_name,
                  s.status_name,
                  f.contact_finance,
                  f.contact_address
                FROM dbo.tbl_repair re
                LEFT JOIN dbo.tbl_device d ON re.FK_device_id = d.device_id
                LEFT JOIN dbo.tbl_room r ON d.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON d.FK_status_id = s.status_id
                LEFT JOIN dbo.tbl_contact f ON re.FK_contact_id = f.contact_id
                WHERE s.status_id LIKE '1%'";
			List<Repair> repair = DataProvider<Repair>.Instance.GetListItemQuery(query);
			repair = Function.Instance.searchItems(repair, repairList);
			repair = Function.Instance.sortItems(repair, repairList.SortOrder);
			repairList.Paging(repair, 10);

			var repairForm = new RepairDetail
			{
				devices_repair = DataProvider<Device>.Instance.GetListItem("tbl_device"),
				rooms_repair = DataProvider<Room>.Instance.GetListItem("tbl_room")
			};

			string repair_id = urlQuery["repair_id"];
			Repair repairStatus = null;
			if (!string.IsNullOrEmpty(repair_id))
			{
				repairStatus = DataProvider<Repair>.Instance.GetListItem("tbl_repair")
					.FirstOrDefault(x => x.repair_id == repair_id);
			}


			var viewModel = new RepairPageViewModel
			{
				RepairList = repairList,
				RepairForm = repairForm,
				RepairStatus = repairStatus
			};
			return View("~/Views/Shared/Suachua.cshtml", viewModel);
		}
		[HttpPost]
		public IActionResult Suachua(String sortOrder, String searchString, String searchField, int currentPage = 1)
		{
			return RedirectToAction("Suachua", new { sort = sortOrder, searchField = searchField, searchString = searchString, page = currentPage });
		}


		//---------------------------------Kho-----------------------------------------------
		public IActionResult Kho()
		{

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

			ItemDisplay<Storage> storageList = new ItemDisplay<Storage>();
			storageList.SortOrder = sortOrder;
			storageList.CurrentSearchField = searchField;
			storageList.CurrentSearchString = searchString;
			storageList.CurrentPage = currentPage;

			string query_str = @"
               SELECT
                st.*,
                d.device_id,
                d.device_name,
                r_from.room_name AS room_from_name,
                r_to.room_name AS room_to_name
            FROM dbo.tbl_storage st
            LEFT JOIN dbo.tbl_device d ON st.FK_device_id = d.device_id
            LEFT JOIN dbo.tbl_room r_from ON st.FK_room_id_from = r_from.room_id
            LEFT JOIN dbo.tbl_room r_to ON st.FK_room_id_to = r_to.room_id";
			List<Storage> storage = DataProvider<Storage>.Instance.GetListItemQuery(query_str);
			storage = Function.Instance.searchItems(storage, storageList);
			storage = Function.Instance.sortItems(storage, storageList.SortOrder);
			storageList.Paging(storage, 10);

			string query = @"
            SELECT 
                d.device_name AS str_device_name,
                COUNT(CASE WHEN d.FK_room_id = 'KHO' THEN 1 END) AS str_quantity
            FROM tbl_device d
            GROUP BY d.device_name";
			var dt = DataProvider<System.Data.DataTable>.Instance.ExcuteQuery(query);

			var list = new List<Storage>();
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (System.Data.DataRow row in dt.Rows)
				{
					list.Add(new Storage
					{
						device_name = row["str_device_name"].ToString(),
						str_quantity = Convert.ToInt32(row["str_quantity"])
					});
				}
			}

			List<Device> allDevices = DataProvider<Device>.Instance.GetListItem("tbl_device");
			List<Room> allRooms = DataProvider<Room>.Instance.GetListItem("tbl_room");

			// Lọc thiết bị để nhập kho (chưa ở kho)
			var devices_import = allDevices
				.Where(d => !string.Equals(d.FK_room_id ?? "", "KHO", StringComparison.OrdinalIgnoreCase))
				.ToList();
			// Lọc thiết bị để xuất kho (đang ở kho)
			var devices_export = allDevices
				.Where(d => string.Equals(d.FK_room_id ?? "", "KHO", StringComparison.OrdinalIgnoreCase))
				.ToList();

			//Phòng
			string Roomquery = @"
               SELECT
                    r.*
                              FROM dbo.tbl_room r";

			ItemDisplay<Room> roomList = new ItemDisplay<Room>();
			roomList.SortOrder = sortOrder;
			roomList.CurrentSearchField = searchField;
			roomList.CurrentSearchString = searchString;
			roomList.CurrentPage = currentPage;
			List<Room> rooms;
			rooms = DataProvider<Room>.Instance.GetListItemQuery(Roomquery);
			rooms = Function.Instance.searchItems(rooms, roomList);
			rooms = Function.Instance.sortItems(rooms, roomList.SortOrder);
			roomList.Paging(rooms, 10);


			var storageforms = new StorageDetail
			{
				devices_import = devices_import,
				devices_export = devices_export,
				devices_all = allDevices,
				rooms_str = allRooms
			};

			var model = new StoragePageViewModel
			{
				StorageList = storageList,
				StorageForm = storageforms,
				DeviceInStockList = list,
				RoomList = roomList // Gán RoomList vào model
			};


			return View("~/Views/Shared/Kho.cshtml", model);
		}
		[HttpPost]
		public IActionResult Kho(String sortOrder, String searchString, String searchField, int currentPage = 1)
		{
			return RedirectToAction("Kho", new { sort = sortOrder, searchField = searchField, searchString = searchString, page = currentPage });
		}

		[HttpPost]
		public IActionResult Kho_Import(int strID, string strDate, string strRoom_from, string strRoom_to, string strDevice, string status_id)
		{
			var allDevices = DataProvider<Device>.Instance.GetListItem("tbl_device");
			var device = allDevices.FirstOrDefault(d => d.device_id == strDevice);
			string room_from = device?.FK_room_id ?? "";

			List<Storage> storages = DataProvider<Storage>.Instance.GetListItem("tbl_storage");
			int maxID = 0;
			foreach (var item in storages)
			{
				int id;
				if (int.TryParse(item.storage_id.ToString(), out id))
				{
					if (id > maxID)
						maxID = id;
				}
			}
			// storage_id mới = maxID + 1
			int newstrID = maxID + 1;

			strRoom_to = "KHO";

			string query = String.Format(
				"INSERT INTO dbo.tbl_storage (storage_date, FK_device_id, FK_room_id_from, FK_room_id_to) " +
					"VALUES ('{0}', '{1}', '{2}', '{3}')", strDate, strDevice, strRoom_from, strRoom_to);
			DataProvider<Storage>.Instance.ExcuteQuery(query);

			status_id = "21";
			string updateStatus_device = String.Format("UPDATE dbo.tbl_device SET FK_status_id = '{0}' WHERE device_id = '{1}' ", status_id, strDevice);
			DataProvider<Device>.Instance.ExcuteQuery(updateStatus_device);


			//cập nhật vị trí
			string updateQuery_device = String.Format("UPDATE dbo.tbl_device SET FK_room_id = '{0}' WHERE device_id = '{1}' ", strRoom_to, strDevice);
			DataProvider<Device>.Instance.ExcuteQuery(updateQuery_device);
			string update_Location_maintain = String.Format("UPDATE dbo.tbl_maintain SET FK_room_id = '{0}' WHERE FK_device_id = '{1}' ", strRoom_to, strDevice);
			DataProvider<Maintain>.Instance.ExcuteQuery(update_Location_maintain);
			string update_Location_repair = String.Format("UPDATE dbo.tbl_repair SET FK_room_id = '{0}' WHERE FK_device_id = '{1}' ", strRoom_to, strDevice);
			DataProvider<Repair>.Instance.ExcuteQuery(update_Location_repair);


			return RedirectToAction("Kho");
		}

		[HttpPost]
		public IActionResult Kho_Export(int estrID, string estrDate, string estrRoom_from, string estrRoom_to, string estrDevice, string status_id)
		{
			List<Storage> storages = DataProvider<Storage>.Instance.GetListItem("tbl_storage");
			int maxID = 0;
			foreach (var item in storages)
			{
				int id;
				if (int.TryParse(item.storage_id.ToString(), out id))
				{
					if (id > maxID)
						maxID = id;
				}
			}
			// storage_id mới = maxID + 1
			int newstrID = maxID + 1;

			estrRoom_from = "KHO";

			string query = String.Format(
				"INSERT INTO dbo.tbl_storage (storage_date, FK_device_id, FK_room_id_from, FK_room_id_to) " +
					"VALUES ('{0}', '{1}', '{2}', '{3}')", estrDate, estrDevice, estrRoom_from, estrRoom_to);
			DataProvider<Storage>.Instance.ExcuteQuery(query);


			//cập nhật vị trí
			string updateQuery_device = String.Format("UPDATE dbo.tbl_device SET FK_room_id = '{0}' WHERE device_id = '{1}' ", estrRoom_to, estrDevice);
			DataProvider<Device>.Instance.ExcuteQuery(updateQuery_device);
			string update_Location_maintain = String.Format("UPDATE dbo.tbl_maintain SET FK_room_id = '{0}' WHERE FK_device_id = '{1}' ", estrRoom_to, estrDevice);
			DataProvider<Maintain>.Instance.ExcuteQuery(update_Location_maintain);
			string update_Location_repair = String.Format("UPDATE dbo.tbl_repair SET FK_room_id = '{0}' WHERE FK_device_id = '{1}' ", estrRoom_to, estrDevice);
			DataProvider<Repair>.Instance.ExcuteQuery(update_Location_repair);

			//cập nhật ngày bảo trì ==> ngày xuất kho 
			string updateQuery_maintaindate = String.Format("UPDATE dbo.tbl_device " + "SET device_maintenance_start = '{0}' " + "WHERE device_id = '{1}' AND FK_status_id = '03'", estrDate, estrDevice);
			DataProvider<Device>.Instance.ExcuteQuery(updateQuery_maintaindate);

			status_id = "20";

			string updateStatus_device = String.Format("UPDATE dbo.tbl_device SET FK_status_id = '{0}' WHERE device_id = '{1}' ", status_id, estrDevice);
			DataProvider<Device>.Instance.ExcuteQuery(updateStatus_device);

			return RedirectToAction("Kho");
		}
		[HttpPost]
		public IActionResult Kho_Transfer(string strDevice, string strRoom_to, string strDate)
		{
			// Lấy vị trí hiện tại của thiết bị
			var allDevices = DataProvider<Device>.Instance.GetListItem("tbl_device");
			var device = allDevices.FirstOrDefault(d => d.device_id == strDevice);
			string room_from = device?.FK_room_id ?? "";

			// Cập nhật thiết bị sang vị trí mới
			string updateQuery = String.Format(
				"UPDATE dbo.tbl_device SET FK_room_id = '{0}' WHERE device_id = '{1}'",
				strRoom_to, strDevice
			);
			DataProvider<Device>.Instance.ExcuteQuery(updateQuery);

			// Lưu lịch sử vào tbl_storage
			string insertHistory = String.Format(
				"INSERT INTO dbo.tbl_storage (storage_date, FK_device_id, FK_room_id_from, FK_room_id_to) " +
				"VALUES ('{0}', '{1}', '{2}', '{3}')", strDate, strDevice, room_from, strRoom_to
			);
			DataProvider<Storage>.Instance.ExcuteQuery(insertHistory);

			return RedirectToAction("Kho");
		}
		public IActionResult Kho_Delete()
		{
			var urlQuery = Request.HttpContext.Request.Query;
			string storage_id_del = urlQuery["storage_id"];
			DataProvider<Storage>.Instance.ExcuteQuery(String.Format("DELETE FROM dbo.tbl_storage WHERE storage_id = {0}", storage_id_del));
			return RedirectToAction("Kho");
		}


 //-------------------------Tài chính - Hợp đồng-----------------------------------------
        //Tài chính hợp đồng
        public IActionResult Taichinh_Hopdong()
		{
			string field, sortOrder, searchField, searchString, page;
			var urlQuery = Request.HttpContext.Request.Query;

			field = urlQuery["field"];
			sortOrder = urlQuery["sort"];
			searchField = urlQuery["searchField"];
			searchString = urlQuery["SearchString"];
			page = urlQuery["page"];

			field ??= "All";
			sortOrder ??= "device_received_date";
			searchField ??= "device_name";
			searchString ??= "";
			page ??= "1";
			int currentPage = Convert.ToInt32(page);

			// === 1. DỮ LIỆU MUA SẮM (DEVICE) ===
			var deviceList = new ItemDisplay<Device>
			{
				SortOrder = sortOrder,
				CurrentSearchField = searchField,
				CurrentSearchString = searchString,
				CurrentPage = currentPage
			};

			string query = @"
        SELECT
            de.*,
            r.room_name,
            s.status_name,
            c.contact_address,
            cf.contact_finance,
            ct.contact_type
        FROM dbo.tbl_device de 
        LEFT JOIN dbo.tbl_room r ON de.FK_room_id = r.room_id
        LEFT JOIN dbo.tbl_status s ON de.FK_status_id = s.status_id
        LEFT JOIN dbo.tbl_contact c ON de.FK_contact_id = c.contact_id
        LEFT JOIN dbo.tbl_contact cf ON de.FK_contact_id = cf.contact_id
        LEFT JOIN dbo.tbl_contact ct ON de.FK_contact_id = ct.contact_id
        WHERE ct.contact_type LIKE '1%'";

			var devices = DataProvider<Device>.Instance.GetListItemQuery(query);
			devices = Function.Instance.searchItems(devices, deviceList);
			devices = Function.Instance.sortItems(devices, deviceList.SortOrder);
			deviceList.Paging(devices, 10);

			var deviceForm = new DeviceDetail
			{
				statuses_device = DataProvider<Status>.Instance.GetListItem("tbl_status"),
				rooms_device = DataProvider<Room>.Instance.GetListItem("tbl_room"),
				contacts_device = DataProvider<Contact>.Instance.GetListItem("tbl_contact")
			};

			// === 2. DỮ LIỆU BẢO TRÌ (MAINTAIN) ===
			var maintainList = new ItemDisplay<Maintain>
			{
				SortOrder = sortOrder,
				CurrentSearchField = searchField,
				CurrentSearchString = searchString,
				CurrentPage = currentPage
			};

			string maintainquery = @"
        SELECT
            m.*,
            id.device_id,
            d.device_name,
            r.room_name,
            s.status_name,
c.contact_address,
            cf.contact_finance
        FROM dbo.tbl_maintain m
        LEFT JOIN dbo.tbl_device id ON m.FK_device_id = id.device_id
        LEFT JOIN dbo.tbl_device d ON m.FK_device_id = d.device_id
        LEFT JOIN dbo.tbl_room r ON m.FK_room_id = r.room_id 
        LEFT JOIN dbo.tbl_status s ON m.FK_status_id = s.status_id
LEFT JOIN dbo.tbl_contact c ON m.FK_contact_id = c.contact_id
        LEFT JOIN dbo.tbl_contact cf ON m.FK_contact_id = cf.contact_id
        WHERE m.FK_status_id = '03'";

			var maintain = DataProvider<Maintain>.Instance.GetListItemQuery(maintainquery);
			maintain = Function.Instance.searchItems(maintain, maintainList);
			maintain = Function.Instance.sortItems(maintain, maintainList.SortOrder);
			maintainList.Paging(maintain, 10);

			var maintainForm = new MaintainDetail
			{
				devices_maintain = DataProvider<Device>.Instance.GetListItem("FK_status_id", "00", "tbl_device"),
				rooms_maintain = DataProvider<Room>.Instance.GetListItem("tbl_room")
			};

			DateTime today = DateTime.Today;
			var overdue = maintain.Where(m => m.maintain_date != null && m.maintain_date.Value.Date < today).ToList();
			var comingup = maintain.Where(m => m.maintain_date != null && m.maintain_date.Value.Date >= today && m.maintain_date.Value.Date <= today.AddDays(2)).ToList();
			var completed = maintain.Where(m => m.maintain_date != null && m.maintain_date.Value.Date > today.AddDays(2) && m.maintain_date.Value.Date <= today.AddDays(30)).ToList();


			// === 3. DỮ LIỆU SỬA CHỮA  ===
			var repairList = new ItemDisplay<Repair>
			{
				SortOrder = sortOrder,
				CurrentSearchField = searchField,
				CurrentSearchString = searchString,
				CurrentPage = currentPage
			};
			string repairquery = @"
               SELECT
                    re.*,
                  id.device_id,
                    d.device_name,
                    r.room_name,
                  s.status_name,
c.contact_address,
                  f.contact_finance
FROM dbo.tbl_repair re
                LEFT JOIN dbo.tbl_device id ON re.FK_device_id = id.device_id
                LEFT JOIN dbo.tbl_device d ON re.FK_device_id = d.device_id
                LEFT JOIN dbo.tbl_room r ON re.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON re.FK_status_id = s.status_id
 LEFT JOIN dbo.tbl_contact c ON re.FK_contact_id = c.contact_id
                LEFT JOIN dbo.tbl_contact f ON re.FK_contact_id = f.contact_id
                 WHERE re.FK_status_id = '13'";
			List<Repair> repair = DataProvider<Repair>.Instance.GetListItemQuery(repairquery);
			repair = Function.Instance.searchItems(repair, repairList);
			repair = Function.Instance.sortItems(repair, repairList.SortOrder);
			repairList.Paging(repair, 10);

			var repairForm = new RepairDetail
			{
				devices_repair = DataProvider<Device>.Instance.GetListItem("tbl_device"),
				rooms_repair = DataProvider<Room>.Instance.GetListItem("tbl_room")
			};


			// === 4. GỘP VÀO MODEL CHUNG ===
			var viewTC_HDModel = new ContactPageViewModel
			{
				DeviceList = deviceList,
				DeviceForm = deviceForm,
				MaintainList = maintainList,
				MaintainForm = maintainForm,
				OverdueList = overdue,
				ComingupList = comingup,
				CompletedList = completed,
				RepairList = repairList,
				RepairForm = repairForm,
			};

			string chartQuery = @"
SELECT MonthYear, SUM(TotalFinance) AS TotalFinance
FROM (
    SELECT 
        FORMAT(d.device_received_date, 'yyyy-MM') AS MonthYear,
        ISNULL(c.contact_finance, 0) AS TotalFinance
    FROM dbo.tbl_device d
    LEFT JOIN dbo.tbl_contact c ON d.FK_contact_id = c.contact_id
    WHERE d.device_received_date IS NOT NULL

    UNION ALL

    SELECT 
        FORMAT(m.maintain_date, 'yyyy-MM') AS MonthYear,
        ISNULL(c.contact_finance, 0) AS TotalFinance
    FROM dbo.tbl_maintain m
    LEFT JOIN dbo.tbl_contact c ON m.FK_contact_id = c.contact_id
    WHERE m.maintain_date IS NOT NULL

    UNION ALL

    SELECT 
        FORMAT(r.repair_date, 'yyyy-MM') AS MonthYear,
        ISNULL(c.contact_finance, 0) AS TotalFinance
    FROM dbo.tbl_repair r
    LEFT JOIN dbo.tbl_contact c ON r.FK_contact_id = c.contact_id
    WHERE r.repair_date IS NOT NULL
) AS Combined
GROUP BY MonthYear
ORDER BY MonthYear DESC";

			var chartData = DataProvider<Contact>.Instance.GetListItemQueryRaw(chartQuery)
				.Take(5)
				.OrderBy(d => d["MonthYear"])
				.ToList();

			ViewBag.ChartLabels = chartData.Select(d => d["MonthYear"].ToString()).ToList();
			ViewBag.ChartValues = chartData.Select(d => Convert.ToInt64(d["TotalFinance"])).ToList();
			return View("~/Views/Shared/Taichinh_Hopdong.cshtml", viewTC_HDModel);

		}

		[HttpPost]
		public JsonResult Phong_Delete(string id)
		{
			try
			{
				string query = $"DELETE FROM dbo.tbl_room WHERE room_id = '{id}'";
				DataProvider<Room>.Instance.ExcuteQuery(query);
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, error = ex.Message });
			}
		}

		[HttpPost]
		public JsonResult Phong_Add(string room_id, string room_type)
		{
			try
			{
				string query = $"INSERT INTO dbo.tbl_room (room_id, room_type, room_name) VALUES ('{room_id}', N'{room_type}','{room_id}')";
				DataProvider<Room>.Instance.ExcuteQuery(query);
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, error = ex.Message });
			}
		}
		public IActionResult Phuluc()
		{
			return View("~/Views/Shared/Phuluc.cshtml");
		}

	}
}