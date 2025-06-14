namespace Hospital_Test.Models
{
    public class RepairPageViewModel
    {
        public Repair RepairStatus { get; set; }
        public ItemDisplay<Repair> RepairList { get; set; }
        public RepairDetail RepairForm { get; set; }
    }
}
