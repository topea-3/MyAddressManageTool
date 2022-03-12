namespace MyAddressManageTool.TableManager.Entity
{
    internal class NumberControllTableEntity 
    {
        public string? Id { get; set; }
        public string? NumberName { get; set; }
        public int CurrentNumber { get; set; }
        public int MaxNumber { get; set; }
        public string? PreFix { get; set; }
    }
}
