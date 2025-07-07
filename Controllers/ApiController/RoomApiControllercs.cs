using Hospital_Test.DAO;
using Hospital_Test.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital_Test.Controllers.Api
{
	[ApiController]
	[Route("api/[controller]")] // Route sẽ là /api/RoomApi
	public class RoomApiController : ControllerBase
	{
		// GET: api/roomapi
		[HttpGet]
		public IActionResult GetAllRooms()
		{
			// Giả định bạn có một bảng tbl_room và DataProvider có thể lấy dữ liệu từ đó
			string query = @"SELECT * FROM dbo.tbl_room";
			var list = DataProvider<Room>.Instance.GetListItemQuery(query);

			if (list == null || !list.Any()) // Kiểm tra nếu danh sách rỗng
			{
				return NotFound("No rooms found."); // Trả về 404 nếu không tìm thấy phòng nào
			}

			return Ok(list); // Trả về danh sách phòng
		}

		// Bạn có thể thêm các phương thức POST, PUT, DELETE khác nếu cần
	}
}