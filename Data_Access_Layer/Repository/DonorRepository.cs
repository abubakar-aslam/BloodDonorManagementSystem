using Data_Access_Layer.DataContext;
using Data_Access_Layer.Entities;
using Data_Access_Layer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Drawing;
using System.Linq;

namespace Data_Access_Layer.Repository
{
    public class DonorRepository : IDonorRepository
    {
        private AppDbContext dbContext;
        public DonorRepository(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }
        public void DeleteDonor(Donor donor)
        {
            var existingDonor = dbContext.Donors.Find(donor.DonorId);
            if (existingDonor != null)
            {
                dbContext.Entry(existingDonor).State = EntityState.Detached;
            }
            dbContext.Donors.Remove(donor);
        }
        public Donor GetDonorById(int id)
        {
            var donor = (from s in dbContext.Donors
                         where s.DonorId == id
                         select s).FirstOrDefault();
            return donor;
        }
        public List<Donor> GetDonorList(string search)
        {
            var donorList = (from s in dbContext.Donors
                             orderby s.DonorId descending
                             select s);
            if (!String.IsNullOrEmpty(search))
            {
                donorList = (from s in dbContext.Donors
                             where s.DonorAge.ToString().Contains(search) ||
                                    s.DonorName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                                    s.BloodType.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                                    s.Address.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                                    s.EmailAddress.ToLower().Trim().Contains(search.ToLower().Trim())
                             orderby s.DonorId descending
                             select s);
            }
            return donorList.ToList();
        }
        public void Insert(Donor donor)
        {
            dbContext.Donors.Add(donor);
        }
        public bool IsEmailExist(string emailAddress)
        {
            return dbContext.Donors.Any(s => s.EmailAddress == emailAddress);
        }
        public string EmailCheck(int id)
        {
            return (from s in dbContext.Donors
                    where s.DonorId == id
                    select s.EmailAddress).FirstOrDefault();
        }
        public void Update(Donor donor)
        {
            var existingDonor = dbContext.Donors.Find(donor.DonorId);
            if (existingDonor != null)
            {
                dbContext.Entry(existingDonor).State = EntityState.Detached;
            }
            dbContext.Entry(donor).State = EntityState.Modified;
        }

        /////////////////////
        ////////////////
        ///

        public void AddUser(Credentials credential)
        {
            dbContext.Credentials.Add(credential);
            // dbContext.Credentials.Add(password);
        }

        public bool GetCredential(Credentials credential)
        {
            var status = (from s in dbContext.Credentials
                          where s.UserName.ToLower().Trim() == credential.UserName.ToLower().Trim() &&
                                s.Password == credential.Password
                          select s);
            if (status.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsUserNameExist(string userName)
        {
            return dbContext.Credentials.Any(s => s.UserName == userName);
        }
        /////////////
        ///

        public void AddInventory(BloodInventoryStore AddInventory)
        {
            dbContext.BloodInventoryStore.Add(AddInventory);
        }

        /////////////////
        /////////
        ///

        public List<BloodInventoryStore> GetInventoryList(string search)
        {
            var inventoryList = (from s in dbContext.BloodInventoryStore
                                 orderby s.Id descending
                                 select s);
            if (!String.IsNullOrEmpty(search))
            {
                inventoryList = (from s in dbContext.BloodInventoryStore
                                 where s.Quantity.ToString().Contains(search) ||
                                       s.BloodTypeName.ToLower().Trim().Contains(search.ToLower().Trim())
                                 orderby s.Id descending
                                 select s);
            }
            return inventoryList.ToList();
        }


        public BloodInventoryStore GetInventryById(int id)
        {
            var inventory = (from s in dbContext.BloodInventoryStore
                             where s.Id == id
                             select s).FirstOrDefault();
            return inventory;
        }

        public void DeleteInventoryRecord(BloodInventoryStore inventory) 
        {
            var existingInventory = dbContext.BloodInventoryStore.Find(inventory.Id);
            if (existingInventory != null)
            {
                dbContext.Entry(existingInventory).State = EntityState.Detached;
            }
            dbContext.BloodInventoryStore.Remove(inventory);
        }

        ///////////////
        ///

        public void UpdateInventoryRecord(BloodInventoryStore inventory) 
        {
            var existingInventory = dbContext.BloodInventoryStore.Find(inventory.Id);
            if (existingInventory != null)
            {
                dbContext.Entry(existingInventory).State = EntityState.Detached;
            }
            dbContext.Entry(inventory).State = EntityState.Modified;
        }

        /////////////
        ///Graph data 
        ///

        public (Dictionary<string, int>, Dictionary<string, int>) GetGraphData()
        {
            var donorCounts = dbContext.Donors
                 .GroupBy(s => s.BloodType)
                 .ToDictionary(group => group.Key, group => group.Count());

            var invetoryCounts = dbContext.BloodInventoryStore
                 .GroupBy(s => s.BloodTypeName)
                 .ToDictionary(group => group.Key, group => group.Sum(item => item.Quantity));

            return (donorCounts, invetoryCounts);
        }


    }
}
