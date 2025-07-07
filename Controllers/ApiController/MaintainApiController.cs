using Hospital_Test.DAO;
using Hospital_Test.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital_Test.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintainApiController : ControllerBase
    {
        // GET: api/maintain
        [HttpGet]
        public IActionResult GetAllMaintains()
        {
            string query = @"
                SELECT m.*, d.device_name, d.device_id, r.room_name, s.status_name
                FROM dbo.tbl_maintain m
                LEFT JOIN dbo.tbl_device d ON d.device_id = m.FK_device_id
                LEFT JOIN dbo.tbl_room r ON d.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON d.FK_status_id = s.status_id
                WHERE d.FK_status_id LIKE '0%'";

            var list = DataProvider<Maintain>.Instance.GetListItemQuery(query);
            return Ok(list);
        }

        // GET: api/maintain/{id}
        [HttpGet("{id}")]
        public IActionResult GetMaintainById(string id)
        {
            string query = $@"
                SELECT m.*, d.device_name, r.room_name, s.status_name
                FROM dbo.tbl_maintain m
                LEFT JOIN dbo.tbl_device d ON d.device_id = m.FK_device_id
                LEFT JOIN dbo.tbl_room r ON d.FK_room_id = r.room_id
                LEFT JOIN dbo.tbl_status s ON d.FK_status_id = s.status_id
                WHERE m.maintain_id = '{id}'";

            var result = DataProvider<Maintain>.Instance.GetListItemQuery(query).FirstOrDefault();
            return result == null ? NotFound() : Ok(result);
        }

        // POST: api/maintain
        [HttpPost]
        public IActionResult CreateMaintain([FromBody] MaintainCreateModel maintain)
        {
            if (maintain == null || string.IsNullOrEmpty(maintain.FK_device_id))
                return BadRequest("Thiếu thông tin thiết bị hoặc dữ liệu không hợp lệ");

            // Tạo maintain_id mới
            var existingMaintains = DataProvider<Maintain>.Instance.GetListItem("tbl_maintain");
            int maxMaintainId = existingMaintains.Select(m => int.TryParse(m.maintain_id, out int id) ? id : 0).Max();
            string newMaintainId = (maxMaintainId + 1).ToString();

            // Tạo contact_id mới
            var contacts = DataProvider<Contact>.Instance.GetListItem("tbl_contact");
            int maxContactId = contacts.Select(c => int.TryParse(c.contact_id, out int id) ? id : 0).Max();
            string newContactId = (maxContactId + 1).ToString();

			long financeCost = 0;
			if (!string.IsNullOrEmpty(maintain.contact_finance))
			{
				long.TryParse(maintain.contact_finance, out financeCost);
			}
			// Thêm bản ghi mới vào tbl_contact (Hợp đồng bảo trì = type 2)
			string insertContact = $@"
        INSERT INTO dbo.tbl_contact (contact_id, contact_type, contact_address, contact_finance)
        VALUES ('{newContactId}', 2, N'{maintain.contact_address}', {financeCost})";
			DataProvider<Contact>.Instance.ExcuteQuery(insertContact);

            // Trạng thái mặc định
            string status = "01";
            string room = "KHO";

			// Thêm bản ghi bảo trì
			string insertMaintain = $@"
  INSERT INTO dbo.tbl_maintain (
      maintain_id, maintain_date, maintain_maintenance, maintain_maintenance_phone, 
      maintain_delivery, maintain_delivery_phone, FK_device_id, FK_status_id, 
      FK_room_id, FK_contact_id)
  VALUES (
      '{newMaintainId}', 
      '{maintain.maintain_date:yyyy-MM-dd}', 
      N'{maintain.maintain_maintenance}', 
      '{maintain.maintain_maintenance_phone}', 
      N'{maintain.maintain_delivery}', 
      '{maintain.maintain_delivery_phone}', 
      '{maintain.FK_device_id}', 
      '{status}', 
      '{room}', 
      '{newContactId}'
  )";

			DataProvider<Maintain>.Instance.ExcuteQuery(insertMaintain);

            // Cập nhật thiết bị (trạng thái và vị trí)
            string updateDeviceRoom = $@"
                UPDATE dbo.tbl_device SET FK_room_id = '{room}' WHERE device_id = '{maintain.FK_device_id}'";
            string updateDeviceStatus = $@"
                UPDATE dbo.tbl_device SET FK_status_id = '{status}' WHERE device_id = '{maintain.FK_device_id}'";
            DataProvider<Device>.Instance.ExcuteQuery(updateDeviceRoom);
            DataProvider<Device>.Instance.ExcuteQuery(updateDeviceStatus);

            return Ok(new { success = true, message = "Thêm thông tin bảo trì thành công" });
        }

        // PUT: api/maintain/updatestatus
        [HttpPut("updatestatus")]
        public IActionResult UpdateMaintainStatus([FromBody] MaintainStatusUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.device_id) || string.IsNullOrEmpty(request.status_id))
                return BadRequest("Thiếu mã thiết bị hoặc trạng thái");

            string updateDevice = $@"
                UPDATE dbo.tbl_device SET FK_status_id = '{request.status_id}' WHERE device_id = '{request.device_id}'";
            string updateMaintain = $@"
                UPDATE dbo.tbl_maintain SET FK_status_id = '{request.status_id}' WHERE FK_device_id = '{request.device_id}'";

            DataProvider<Device>.Instance.ExcuteQuery(updateDevice);
            DataProvider<Maintain>.Instance.ExcuteQuery(updateMaintain);

            return Ok(new { success = true, message = "Cập nhật trạng thái bảo trì thành công" });
        }

        // DELETE: api/maintain/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteMaintain(string id)
        {
            try
            {
                string query = $"DELETE FROM dbo.tbl_maintain WHERE maintain_id = '{id}'";
                DataProvider<Maintain>.Instance.ExcuteQuery(query);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        // Class phụ cho PUT status
        public class MaintainStatusUpdateRequest
        {
            public string device_id { get; set; }
            public string status_id { get; set; }
        }
    }
}
