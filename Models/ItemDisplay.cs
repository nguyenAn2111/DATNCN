using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test.Models
{
    public class ItemDisplay<T>
    {
        private int pageCount;
        private int pageSize;
        private int currentPage;
        private List<T> items;
        private int itemCount;

        private String field;
        private String sortOrder;
        private String currentSearchString;
        private String currentSearchField;
        private List<String> searchFieldList;

        private string message;
        private bool isMessage;
        private List<String> link;
        private string sessionVar;

        private Dictionary<string, string> nameVar = new Dictionary<string, string>()
          {
              {"Patient_Name", "Tên bệnh nhân" },
              {"Patient_ID", "Căn cước công dân" },
              {"Patient_Gender","Giới tính bệnh nhân"},
              {"Patient_DateofBirth","Ngày sinh bênh nhân"},
              {"Patient_Checkin","Thời gian nhập viện"},
              {"Patient_Checkout","Thời gian ra viện"},
              {"Patient_Symptom","Triệu chứng"},
              {"Patient_Disease","Bệnh lý"},
              {"Patient_Status", "Tình trạng bệnh nhân"},
              {"FK_Staff_Receiver","Nhân viên nhập bệnh án"},
              {"FK_Staff_Main","Bác sĩ mổ chính"},
              {"FK_Room_ID","Mã phòng mổ"},
              {"FK_Finace", "Viện phí" },
              {"Staff_ID", "Mã nhân viên"},
              {"Staff_DateofBirth", "Ngày sinh nhân viên" },
              { "Staff_Name", "Tên nhân viên" },
              {"Staff_Gender", "Giới tính nhân viên" },
              {"Staff_Position", "Vị trí làm việc"},
              {"Staff_Phone", "Số điện thoại" },
              {"Staff_Status", "Tình trạng làm việc" },
              {"FK_Staff_Salary", "Lương nhân viên" },
              {"Room_Type", "Loại phòng" },
              {"Room_ID", "Mã phòng" },
              {"room", "room" },
              {"Room_Name", "Room_Name" },
              {"surgery", "surgery" },
              {"staff", "staff" },
              {"Surgery_ID", "Mã ca mổ" },
              {"Surgery_Time", "Thời gian mổ" },
              {"Surgery_Status", "Tình trạng ca mổ" },
              {"FK_Patient_ID", "Mã bênh nhân" },
          };




        public int PageCount { get => pageCount; set => pageCount = value; }
        public int PageSize { get => pageSize; set => pageSize = value; }
        public int CurrentPage { get => currentPage; set => currentPage = value; }
        public List<T> Items { get => items; set => items = value; }
        public int ItemCount { get => itemCount; set => itemCount = value; }

        public String Field { get => field; set => field = value; }
        public String SortOrder { get => sortOrder; set => sortOrder = value; }
        public String CurrentSearchString { get => currentSearchString; set => currentSearchString = value; }
        public String CurrentSearchField { get => currentSearchField; set => currentSearchField = value; }
        public List<String> SearchFieldList { get => searchFieldList; set => searchFieldList = value; }

        public string Message { get => message; set => message = value; }
        public bool IsMessage { get => isMessage; set => isMessage = value; }
        public string SessionVar { get => sessionVar; set => sessionVar = value; }
        public List<String> Link { get => link; set => link = value; } // Biểu mẫu/Lịch làm việc/Báo lỗi

        public Dictionary<string, string> NameVar { get => nameVar; set => nameVar = value; }
        /*public Dictionary<string, string> UnitVar { get => unitVar; set => unitVar = value; }
        public Dictionary<string, string> ProcedureVar { get => procedureVar; set => procedureVar = value; }
        public Dictionary<string, string> ColorVar { get => procedureColorVar; set => procedureColorVar = value; }
       */
        public static Boolean IsAddMember = false;

        public ItemDisplay()
        {
            this.pageSize = 10;
            this.currentPage = 1;
            this.searchFieldList = new List<String>();
            foreach (var attr in typeof(T).GetProperties().ToList())
            {
                this.searchFieldList.Add(attr.Name.ToString());
            }

            this.isMessage = false;
            this.message = "";
            this.sessionVar = "";
        }



        public void Paging(List<T> members, int pageSize)
        {
            this.items = members;
            this.itemCount = members.Count;
            this.pageSize = pageSize;

            if ((double)((decimal)this.items.Count() % Convert.ToDecimal(this.pageSize)) == 0)
            {
                this.pageCount = this.items.Count() / this.pageSize;
            }
            else
            {
                double page_Count = (double)((decimal)this.items.Count() / Convert.ToDecimal(this.pageSize));
                this.pageCount = (int)Math.Ceiling(page_Count);
            }
        }
    }
}
