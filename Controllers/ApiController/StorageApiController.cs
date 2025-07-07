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
    public class StorageApiController : ControllerBase
    {
        // GET: api/storage 
        [HttpGet]
        public IActionResult GetAllStorage()
        {
            string query = @"
                SELECT st.*, d.device_name, 
                       r_from.room_name AS room_from_name, 
                       r_to.room_name AS room_to_name
                FROM dbo.tbl_storage st
                LEFT JOIN dbo.tbl_device d ON st.FK_device_id = d.device_id
                LEFT JOIN dbo.tbl_room r_from ON st.FK_room_id_from = r_from.room_id
                LEFT JOIN dbo.tbl_room r_to ON st.FK_room_id_to = r_to.room_id";

            var list = DataProvider<Storage>.Instance.GetListItemQuery(query);
            return Ok(list);
        }

        // POST: api/storage/import
        [HttpPost("import")]
        public IActionResult ImportDevice([FromBody] StorageRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.device_id))
                return BadRequest("Thiếu thiết bị");

            var device = DataProvider<Device>.Instance.GetListItem("tbl_device")
                             .FirstOrDefault(d => d.device_id == req.device_id);
            string room_from = device?.FK_room_id ?? "";
            string room_to = "KHO";
            string status_id = "21";

            // Ghi log import
            string insertStorage = $@"
                INSERT INTO dbo.tbl_storage (storage_date, FK_device_id, FK_room_id_from, FK_room_id_to)
                VALUES ('{req.date}', '{req.device_id}', '{room_from}', '{room_to}')";
            DataProvider<Storage>.Instance.ExcuteQuery(insertStorage);

            // Cập nhật trạng thái & vị trí
            string updateDeviceStatus = $"UPDATE dbo.tbl_device SET FK_status_id = '{status_id}' WHERE device_id = '{req.device_id}'";
            string updateDeviceRoom = $"UPDATE dbo.tbl_device SET FK_room_id = '{room_to}' WHERE device_id = '{req.device_id}'";
            string updateMaintainRoom = $"UPDATE dbo.tbl_maintain SET FK_room_id = '{room_to}' WHERE FK_device_id = '{req.device_id}'";
            string updateRepairRoom = $"UPDATE dbo.tbl_repair SET FK_room_id = '{room_to}' WHERE FK_device_id = '{req.device_id}'";

            DataProvider<Device>.Instance.ExcuteQuery(updateDeviceStatus);
            DataProvider<Device>.Instance.ExcuteQuery(updateDeviceRoom);
            DataProvider<Maintain>.Instance.ExcuteQuery(updateMaintainRoom);
            DataProvider<Repair>.Instance.ExcuteQuery(updateRepairRoom);

            return Ok(new { success = true, message = "Thiết bị đã nhập kho" });
        }

        // POST: api/storage/export
        [HttpPost("export")]
        public IActionResult ExportDevice([FromBody] StorageRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.device_id) || string.IsNullOrEmpty(req.room_to))
                return BadRequest("Thiếu thông tin thiết bị");

            string room_from = "KHO";
            string status_id = "20";

            // Ghi log export
            string insertStorage = $@"
                INSERT INTO dbo.tbl_storage (storage_date, FK_device_id, FK_room_id_from, FK_room_id_to)
                VALUES ('{req.date}', '{req.device_id}', '{room_from}', '{req.room_to}')";
            DataProvider<Storage>.Instance.ExcuteQuery(insertStorage);

            // Cập nhật vị trí thiết bị
            string updateRoom = $"UPDATE dbo.tbl_device SET FK_room_id = '{req.room_to}' WHERE device_id = '{req.device_id}'";
            string updateMaintain = $"UPDATE dbo.tbl_maintain SET FK_room_id = '{req.room_to}' WHERE FK_device_id = '{req.device_id}'";
            string updateRepair = $"UPDATE dbo.tbl_repair SET FK_room_id = '{req.room_to}' WHERE FK_device_id = '{req.device_id}'";
            DataProvider<Device>.Instance.ExcuteQuery(updateRoom);
            DataProvider<Maintain>.Instance.ExcuteQuery(updateMaintain);
            DataProvider<Repair>.Instance.ExcuteQuery(updateRepair);

            // Nếu đang bảo trì, cập nhật ngày bắt đầu bảo trì
            string updateMaintainStart = $@"
                UPDATE dbo.tbl_device 
                SET device_maintenance_start = '{req.date}' 
                WHERE device_id = '{req.device_id}' AND FK_status_id = '03'";
            DataProvider<Device>.Instance.ExcuteQuery(updateMaintainStart);

            // Cập nhật trạng thái thiết bị
            string updateStatus = $"UPDATE dbo.tbl_device SET FK_status_id = '{status_id}' WHERE device_id = '{req.device_id}'";
            DataProvider<Device>.Instance.ExcuteQuery(updateStatus);

            return Ok(new { success = true, message = "Thiết bị đã xuất kho" });
        }

        // POST: api/storage/transfer
        [HttpPost("transfer")]
        public IActionResult TransferDevice([FromBody] StorageRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.device_id) || string.IsNullOrEmpty(req.room_to))
                return BadRequest("Thiếu thông tin thiết bị hoặc phòng");

            var device = DataProvider<Device>.Instance.GetListItem("tbl_device")
                             .FirstOrDefault(d => d.device_id == req.device_id);
            string room_from = device?.FK_room_id ?? "";

            // Cập nhật vị trí thiết bị
            string updateRoom = $"UPDATE dbo.tbl_device SET FK_room_id = '{req.room_to}' WHERE device_id = '{req.device_id}'";
            DataProvider<Device>.Instance.ExcuteQuery(updateRoom);

            // Lưu lịch sử vào tbl_storage
            string insertStorage = $@"
                INSERT INTO dbo.tbl_storage (storage_date, FK_device_id, FK_room_id_from, FK_room_id_to)
                VALUES ('{req.date}', '{req.device_id}', '{room_from}', '{req.room_to}')";
            DataProvider<Storage>.Instance.ExcuteQuery(insertStorage);

            return Ok(new { success = true, message = "Thiết bị đã được điều chuyển" });
        }

        // DELETE: api/storage/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteStorage(int id)
        {
            try
            {
                string query = $"DELETE FROM dbo.tbl_storage WHERE storage_id = {id}";
                DataProvider<Storage>.Instance.ExcuteQuery(query);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        // DTO Request
        public class StorageRequest
        {
            public string device_id { get; set; }
            public string room_to { get; set; }
            public string date { get; set; } // yyyy-MM-dd
        }
    }
}
