using Data_Access_Layer.Entities;

namespace Data_Access_Layer.Interfaces
{
    public interface IDonorRepository
    {
        void Update(Donor donor);
        void DeleteDonor(Donor donor);
        void Insert(Donor student);
        List<Donor> GetDonorList(string serach);
        Donor GetDonorById(int id);
        bool IsEmailExist(string emailAddress);
        string EmailCheck(int id);
        //////////////////
        ///
        public void AddUser(Credentials credential);

        public bool GetCredential(Credentials credential);

        public bool IsUserNameExist(string userName);

        //////////////
        ///

        public void AddInventory(BloodInventoryStore AddInventory);

        public List<BloodInventoryStore> GetInventoryList(string search);

        public BloodInventoryStore GetInventryById(int id);

        public void DeleteInventoryRecord(BloodInventoryStore inventory);

        public void UpdateInventoryRecord(BloodInventoryStore inventory);

        public (Dictionary<string, int>, Dictionary<string, int>) GetGraphData();
    }
}
