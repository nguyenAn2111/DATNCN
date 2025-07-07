using Hospital_Test.DAO;
using Hospital_Test.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System;

namespace Hospital_Test.Controllers.ApiController
{
	[Route("api/[controller]")]
	[ApiController]
	public class DeviceApiController : ControllerBase
	{
		// GET: api/device: OK
		[HttpGet]
		public IActionResult GetAllDevices()
		{
			string query = @"
				SELECT d.*, s.status_name, r.room_name, c.contact_address
				FROM dbo.tbl_device d
				LEFT JOIN dbo.tbl_status s ON s.status_id = d.FK_status_id
				LEFT JOIN dbo.tbl_room r ON r.room_id = d.FK_room_id
				LEFT JOIN dbo.tbl_contact c ON c.contact_id = d.FK_contact_id";

			var devices = DataProvider<Device>.Instance.GetListItemQuery(query);
			return Ok(devices);
		}

		// GET: api/device/{id}: OK
		[HttpGet("{id}")]
		public IActionResult GetDeviceById(string id)
		{
			string query = $@"
				SELECT d.*, s.status_name, r.room_name, c.contact_address
				FROM dbo.tbl_device d
				LEFT JOIN dbo.tbl_status s ON s.status_id = d.FK_status_id
				LEFT JOIN dbo.tbl_room r ON r.room_id = d.FK_room_id
				LEFT JOIN dbo.tbl_contact c ON c.contact_id = d.FK_contact_id
				WHERE d.device_id = '{id}'";

			var device = DataProvider<Device>.Instance.GetListItemQuery(query).FirstOrDefault();
			if (device == null)
				return NotFound();

			return Ok(device);
		}

		// POST: api/device
		[HttpPost]
		public IActionResult CreateDevice([FromBody] DeviceCreateModel device)
		{
			if (device == null)
				return BadRequest();

			string fkContactValue = device.FK_contact_id == null ? "NULL" : $"'{device.FK_contact_id}'";
			string fkRoomValue = device.FK_room_id == null ? "NULL" : $"'{device.FK_room_id}'";
			string maintenanceStart = device.device_maintenance_start?.ToString("yyyy-MM-dd") ?? "NULL";
			string receivedDate = device.device_received_date?.ToString("yyyy-MM-dd") ?? "NULL";

			string query = $@"
	INSERT INTO dbo.tbl_device (
		device_id, device_name, device_manufacturer, device_seri, device_type, device_group, 
		device_maintenance_start, device_maintenance_cycle, device_condition, 
		device_received_date, device_note, FK_status_id, FK_room_id, FK_contact_id)
	VALUES (
		'{device.device_id}', N'{device.device_name}', N'{device.device_manufacturer}', 
		N'{device.device_seri}', '{device.device_type}', N'{device.device_group}', 
		{(device.device_maintenance_start == null ? "NULL" : $"'{maintenanceStart}'")}, 
		{device.device_maintenance_cycle ?? 0}, 
		N'{device.device_condition}', 
		{(device.device_received_date == null ? "NULL" : $"'{receivedDate}'")}, 
		N'{device.device_note}', 
		'{device.FK_status_id}', {fkRoomValue}, {fkContactValue})";

			DataProvider<Device>.Instance.ExcuteQuery(query);

			return Ok(new { success = true, message = "Thêm thiết bị thành công" });
		}



		[HttpGet("/api/room")]
		public IActionResult GetAllRooms()
		{
			string query = "SELECT room_id, room_name, room_type FROM dbo.tbl_room";
			var rooms = DataProvider<Room>.Instance.GetListItemQuery(query);
			return Ok(rooms);
		}

		// PUT: api/device/{id}
		[HttpPut("{id}")]
		public IActionResult UpdateDevice(string id, [FromBody] Device device)
		{
			if (device == null || id != device.device_id)
				return BadRequest();

			string query = $@"
				UPDATE dbo.tbl_device SET
					device_name = N'{device.device_name}',
					device_manufacturer = N'{device.device_manufacturer}',
					device_seri = N'{device.device_seri}',
					device_type = '{device.device_type}',
					device_group = N'{device.device_group}',
					device_maintenance_start = '{device.device_maintenance_start}',
					device_maintenance_cycle = {device.device_maintenance_cycle},
					device_stockout_date = '{device.device_stockout_date}',
					device_condition = N'{device.device_condition}',
					device_received_date = '{device.device_received_date}',
					device_note = N'{device.device_note}',
					FK_status_id = '{device.FK_status_id}',
					FK_room_id = '{device.FK_room_id}',
					FK_contact_id = '{device.FK_contact_id}'
				WHERE device_id = '{id}'";

			DataProvider<Device>.Instance.ExcuteQuery(query);
			return Ok(new { success = true, message = "Cập nhật thiết bị thành công" });
		}

		// DELETE: api/device/{id}
		[HttpDelete("{id}")]
		public IActionResult DeleteDevice(string id)
		{
			try
			{
				string query = $"DELETE FROM dbo.tbl_device WHERE device_id = '{id}'";
				DataProvider<Device>.Instance.ExcuteQuery(query);
				return Ok(new { success = true, message = "Xóa thiết bị thành công" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { success = false, error = ex.Message });
			}
		}
	}
}
