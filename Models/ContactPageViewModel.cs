namespace Hospital_Test.Models
{
	public class ContactPageViewModel
	{
		public ItemDisplay<Device> DeviceList { get; set; }
		public DeviceDetail DeviceForm { get; set; }
		public ItemDisplay<Maintain> MaintainList { get; set; }
		public MaintainDetail MaintainForm { get; set; }
		public List<Maintain> OverdueList { get; set; }
		public List<Maintain> ComingupList { get; set; }
		public List<Maintain> CompletedList { get; set; }
		public Repair RepairStatus { get; set; }
		public ItemDisplay<Repair> RepairList { get; set; }
		public RepairDetail RepairForm { get; set; }
	}
}
