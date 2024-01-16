using Data_Access_Layer.Entities;
using Data_Access_Layer.Interfaces;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;

namespace Data_Access_Layer
{
    public class DonorDAL
    {
        
        private readonly IUnitOfWork uowContext;
        public DonorDAL(IUnitOfWork _uowContext)
        {
            uowContext = _uowContext;
        }
        public Donor GetDonorById(int id)
        {
            return uowContext.DonorRepository.GetDonorById(id);
        }
        public void DeleteRecord(Donor donor)
        {
            uowContext.DonorRepository.DeleteDonor(donor);
            uowContext.Save();
        }
        public List<Donor> GetDonors(string search)
        {
            return uowContext.DonorRepository.GetDonorList(search);
        }
        public bool IsEmailExist(string emailAddress)
        {
            return uowContext.DonorRepository.IsEmailExist(emailAddress);
        }
        public void Insert(Donor donor)
        {
            uowContext.DonorRepository.Insert(donor);
        }
        public void Save()
        {
            uowContext.Save();
        }
        public void Update(Donor donorList)
        {
            uowContext.DonorRepository.Update(donorList);
        }
        public string EmailCheck(int id)
        {
            return uowContext.DonorRepository.EmailCheck(id);
        }
        public void Dispose()
        {
            uowContext.Dispose();
        }

        //////////////////////
        ///////////////
        ///

        public void AddUser(Credentials credential)
        {
            uowContext.DonorRepository.AddUser(credential);
        }
        public bool GetCredential(Credentials credential) 
        {
            return uowContext.DonorRepository.GetCredential(credential);
        }
        public bool IsUserNameExist(string userName) 
        {
            return uowContext.DonorRepository.IsUserNameExist(userName);
        }

        //////////////
        /////////
        ///

        public void AddInventory(BloodInventoryStore AddInventory) 
        {
            uowContext.DonorRepository.AddInventory(AddInventory);
        }

        public List<BloodInventoryStore> GetInventory(string search)
        {
            return uowContext.DonorRepository.GetInventoryList(search);
        }

        public BloodInventoryStore GetInventryById(int id)
        {
            return uowContext.DonorRepository.GetInventryById(id);
        }

         public void DeleteInventory(BloodInventoryStore inventory)
        {
            uowContext.DonorRepository.DeleteInventoryRecord(inventory);
            uowContext.Save();
        }

        public void UpdateInventoryRecord(BloodInventoryStore inventory)
        {
            uowContext.DonorRepository.UpdateInventoryRecord(inventory);
        }

        /////////////////
        ///
        public (Dictionary<string, int>, Dictionary<string, int>) GetGraphData()
        {
            return uowContext.DonorRepository.GetGraphData();
        }
    }
}