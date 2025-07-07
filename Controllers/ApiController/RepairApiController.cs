using Hospital_Test.DAO;
using Hospital_Test.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Hospital_Test.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairApiController : ControllerBase
    {
        // GET: api/repair
        [HttpGet]
        public IActionResult GetAllRepairs()
        {
			string query = @"
    SELECT r.*, d.device_name, r2.room_name, s.status_name
    FROM dbo.tbl_repair r
    LEFT JOIN dbo.tbl_device d ON d.device_id = r.FK_device_id
    LEFT JOIN dbo.tbl_room r2 ON d.FK_room_id = r2.room_id
    LEFT JOIN dbo.tbl_status s ON s.status_id = r.FK_status_id
WHERE d.FK_status_id LIKE '1%' ";

			var list = DataProvider<Repair>.Instance.GetListItemQuery(query);
			return Ok(list);
        }

        // GET: api/repair/{id}
        [HttpGet("{id}")]
        public IActionResult GetRepairById(string id)
        {
			string query = $@"
     SELECT re.*, d.device_name, r.room_name, s.status_name, f.contact_finance, f.contact_address
     FROM dbo.tbl_repair re
     LEFT JOIN dbo.tbl_device d ON re.FK_device_id = d.device_id
     LEFT JOIN dbo.tbl_room r ON d.FK_room_id = r.room_id
     LEFT JOIN dbo.tbl_status s ON d.FK_status_id = s.status_id
     LEFT JOIN dbo.tbl_contact f ON re.FK_contact_id = f.contact_id
     WHERE re.repair_id = '{id}'";



			var repair = DataProvider<Repair>.Instance.GetListItemQuery(query).FirstOrDefault();
            if (repair == null)
                return NotFound();

            return Ok(repair);
        }

        // POST: api/repair
        [HttpPost]
        public IActionResult CreateRepair([FromBody] RepairCreateModel repair)
        {
            if (repair == null || string.IsNullOrEmpty(repair.FK_device_id))
                return BadRequest("Thiếu thông tin thiết bị");

            // Auto ID for repair
            var repairs = DataProvider<Repair>.Instance.GetListItem("tbl_repair");
            int maxRepairId = repairs.Select(r => int.TryParse(r.repair_id, out int id) ? id : 0).Max();
            string newRepairId = (maxRepairId + 1).ToString();

            // Auto ID for contact
            var contacts = DataProvider<Contact>.Instance.GetListItem("tbl_contact");
            int maxContactId = contacts.Select(c => int.TryParse(c.contact_id, out int id) ? id : 0).Max();
            string newContactId = (maxContactId + 1).ToString();

            // Thêm contact mới (type = 3: Hợp đồng sửa chữa)
            string insertContact = $@"
                INSERT INTO dbo.tbl_contact (contact_id, contact_type, contact_address, contact_finance)
                VALUES ('{newContactId}', 3, N'{repair.contact_address}', {repair.contact_finance})";
            DataProvider<Contact>.Instance.ExcuteQuery(insertContact);

            // Mặc định
            repair.FK_status_id = "11"; // Đang sửa chữa
            repair.FK_room_id = "KHO";
            repair.repair_update_status = "Thêm thiết bị sửa chữa";
            repair.repair_update_note = "Đã thêm thông tin của thiết bị cần sửa chữa";
            repair.repair_update_date = DateTime.Now;

			// Format ngày
			//Cần fix về kiểu dữ liệu datetime hay datetime?
			string broken = repair.repair_broken.ToString("yyyy-MM-dd");
			string date = repair.repair_date.ToString("yyyy-MM-dd");
			string updated = (repair.repair_update_date ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");

			// Insert repair
			string insertRepair = $@"
                INSERT INTO dbo.tbl_repair (
                    repair_id, repair_broken, repair_priority, repair_date, repair_update_date,
                    repair_update_status, repair_note, repair_picture, repair_update_note,
                    FK_room_id, FK_device_id, FK_status_id, FK_contact_id)
                VALUES (
                    '{newRepairId}', '{broken}', {repair.repair_priority}, '{date}', '{updated}',
                    N'{repair.repair_update_status}', N'{repair.repair_note}', '{repair.repair_picture}',
                    N'{repair.repair_update_note}', '{repair.FK_room_id}', '{repair.FK_device_id}',
                    '{repair.FK_status_id}', '{newContactId}')";

            DataProvider<Repair>.Instance.ExcuteQuery(insertRepair);

            // Update thiết bị
            string updateDeviceStatus = $"UPDATE dbo.tbl_device SET FK_status_id = '{repair.FK_status_id}' WHERE device_id = '{repair.FK_device_id}'";
            string updateDeviceRoom = $"UPDATE dbo.tbl_device SET FK_room_id = '{repair.FK_room_id}' WHERE device_id = '{repair.FK_device_id}'";
            DataProvider<Device>.Instance.ExcuteQuery(updateDeviceStatus);
            DataProvider<Device>.Instance.ExcuteQuery(updateDeviceRoom);

            return Ok(new { success = true, message = "Thêm thông tin sửa chữa thành công" });
        }

        // PUT: api/repair/updatestatus
        [HttpPut("updatestatus")]
        public IActionResult UpdateRepairStatus([FromBody] RepairStatusUpdateRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.repair_id) || string.IsNullOrEmpty(req.total_status))
                return BadRequest("Thiếu thông tin cập nhật");

            var repair = DataProvider<Repair>.Instance.GetListItem("tbl_repair").FirstOrDefault(r => r.repair_id == req.repair_id);
            var device = DataProvider<Device>.Instance.GetListItem("tbl_device").FirstOrDefault(d => d.device_id == repair?.FK_device_id);

            if (repair == null || device == null)
                return NotFound("Không tìm thấy bản ghi sửa chữa hoặc thiết bị");

            string updateSql = $@"
                BEGIN TRANSACTION;
                UPDATE dbo.tbl_repair 
                SET 
                    repair_update_date = '{req.schdate:yyyy-MM-dd HH:mm:ss}', 
                    repair_update_note = N'{req.schnote.Replace("'", "''")}', 
                    repair_update_status = N'{req.schstatus.Replace("'", "''")}', 
                    FK_status_id = '{req.total_status.Replace("'", "''")}' 
                WHERE repair_id = '{req.repair_id.Replace("'", "''")}';

                UPDATE dbo.tbl_device 
                SET FK_status_id = '{req.total_status.Replace("'", "''")}' 
                WHERE device_id = '{repair.FK_device_id?.Replace("'", "''")}';
                COMMIT;
            ";

            DataProvider<Repair>.Instance.ExcuteQuery(updateSql);
            return Ok(new { success = true, message = "Cập nhật trạng thái sửa chữa thành công" });
        }

        // DELETE: api/repair/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteRepair(string id)
        {
            try
            {
                string query = $"DELETE FROM dbo.tbl_repair WHERE repair_id = '{id}'";
                DataProvider<Repair>.Instance.ExcuteQuery(query);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        // DTO cho cập nhật trạng thái
        public class RepairStatusUpdateRequest
        {
            public string repair_id { get; set; }
            public string schstatus { get; set; }
            public string schnote { get; set; }
            public DateTime schdate { get; set; }
            public string total_status { get; set; }
        }
    }
}
