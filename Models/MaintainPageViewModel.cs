namespace Hospital_Test.Models
{
    public class MaintainPageViewModel
    {
        public ItemDisplay<Maintain> MaintainList { get; set; }
        public MaintainDetail MaintainForm { get; set; }
        public List<Maintain> OverdueList { get; set; }
        public List<Maintain> ComingupList { get; set; }
        public List<Maintain> CompletedList { get; set; }
    }
}