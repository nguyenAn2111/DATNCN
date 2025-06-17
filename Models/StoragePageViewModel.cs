using Hospital_Test.Models;

public class StoragePageViewModel
{
    public ItemDisplay<Storage> StorageList { get; set; }
    public StorageDetail StorageForm { get; set; }
    public List<Storage> DeviceInStockList { get; set; } 
}
