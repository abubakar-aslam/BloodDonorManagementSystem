using Azure;
using Data_Access_Layer.Entities;
using Data_Access_Layer;
using StudentProjectMVC.Exception;
using System.Text.RegularExpressions;
using X.PagedList;
using AutoMapper;
using Business_Logic_Layer.Models;
using System.Collections.Generic;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace Business_Logic_Layer
{
    public class DonorBLL
    {
        private readonly Data_Access_Layer.DonorDAL _data_Access_Layer;
        private Mapper _DonorMapper;
        public DonorBLL(Data_Access_Layer.DonorDAL data_Access_Layer)
        {
            _data_Access_Layer = data_Access_Layer;
            var _configDonor = new MapperConfiguration(cfg => cfg.CreateMap<Donor, DonorModel>().ReverseMap());
            _DonorMapper = new Mapper(_configDonor);
        }
        public bool EmailStatus(string emailAddress, int id)
        {
            var email = _data_Access_Layer.EmailCheck(id);
            if (email != null)
            {
                if (email.ToLower() == emailAddress.ToLower())
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public void Dispose()
        {
            _data_Access_Layer.Dispose();
        }
        public DonorModel GetDonorById(int id)
        {
            var donor = _data_Access_Layer.GetDonorById(id);
            DonorModel donorModel = _DonorMapper.Map<Donor, DonorModel>(donor);
            return donorModel;
        }
        public void DeleteRecord(DonorModel donorModel)
        {
            Donor donor = _DonorMapper.Map<DonorModel, Donor>(donorModel);
            _data_Access_Layer.DeleteRecord(donor);
        }
        public List<DonorModel> GetDonors(string search)
        {
            var donorList = _data_Access_Layer.GetDonors(search);
            List<DonorModel> donorModelList = _DonorMapper.Map<List<Donor>, List<DonorModel>>(donorList);
            return donorModelList.ToList();
        }
        public string AddRecord(string age, string name, string bloodtype, string phoneNumber, string emailAddress, string address)
        {
            List<Donor> donorRecord;
            string message;
            List<DonorModel> donor = new List<DonorModel>();
            donor.Add(new DonorModel { DonorName = name, DonorAge = Convert.ToInt32(age), BloodType = bloodtype, ContactNumber = phoneNumber, EmailAddress = emailAddress, Address = address });
            bool emailExist = _data_Access_Layer.IsEmailExist(emailAddress);
            if (emailExist == false)
            {
                donorRecord = _DonorMapper.Map<List<DonorModel>, List<Donor>>(donor);
                foreach (var donorItem in donorRecord)
                {
                    _data_Access_Layer.Insert(donorItem);
                }
                _data_Access_Layer.Save();
                message = "Donor Added Successfully!";
                return message;
            }
            else
            {
                message = "Email Already Exist!";
                return message;
            }
        }
        public string UpdateRecord(DonorModel donorModelList, string emailAddress)
        {
            string message;
            Donor donorList;
            donorModelList.EmailAddress = emailAddress;
            donorList = _DonorMapper.Map<DonorModel, Donor>(donorModelList);
            _data_Access_Layer.Update(donorList);
            _data_Access_Layer.Save();
            message = "Donor Updated Successfully!";
            return message;
        }
        public string EditRecord(int id, string name, string age, string bloodType, string phoneNumber,string emailAddress, string address, DonorModel donorModelList)
        {
            string message;
            donorModelList.DonorName = name;
            donorModelList.DonorAge = Convert.ToInt32(age);
            donorModelList.BloodType = bloodType;
            donorModelList.ContactNumber = phoneNumber;
            donorModelList.Address = address;
            if (EmailStatus(emailAddress, id))
            {
                return UpdateRecord(donorModelList, emailAddress);
            }
            else
            {
                bool checkEmail = _data_Access_Layer.IsEmailExist(emailAddress);
                if (checkEmail == false)
                {
                    return UpdateRecord(donorModelList, emailAddress);
                }
                else
                {
                    message = "Email Already Exist!";
                    return message;
                }
            }
        }

        ////////////////////////////////////////////////////////
        //////////////////////////////////////////
        ///

        //public static bool ValidateEmail(string email)
        //{
        //    string reg_pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

        //    Regex regex = new Regex(reg_pattern);

        //    return regex.IsMatch(email);
        //}

        public string UserSignUp(string userName, string password)
        {
            string message;
            if (password != null && userName != null)
            {
                bool status = _data_Access_Layer.IsUserNameExist(userName);
                if (status == false)
                {
                    Credentials credential = new Credentials { UserName = userName, Password = password };
                    _data_Access_Layer.AddUser(credential);
                    _data_Access_Layer.Save();
                    message = "Account Created Successfully! Please Sign In";
                    return message;
                }
                else
                {
                    message = "User Name Already Exist";
                    return message;
                }
            }
            else 
            {
                message = "There is Some Error Please Try Again";
                return message;
            }
        }

        /////////////////////////
        ///////////////
        ///

        public string UserSignIn(Credentials credential)
        {
            if (credential.Password != null && credential.UserName != null)
            {
                string message;
                bool status = _data_Access_Layer.GetCredential(credential);
                if (status == true)
                {
                    message = "Sign In Successfully!";
                    return message;
                }
                else
                {
                    message = "Please Enter Valid User Name and Password";
                    return message;
                }
            }
            else 
            {
                return "Some Issue Please Try Again";
            }
        }

        //////////////////
        ////////////////////
        ////////////
        ///

        public string AddInventory(string bloodType, int quantity)
        {
            string message;
            if (bloodType != null && quantity > 0)
            {
                BloodInventoryStore inventory = new BloodInventoryStore {BloodTypeName = bloodType, Quantity = quantity };
                _data_Access_Layer.AddInventory(inventory);
                _data_Access_Layer.Save();
                message = "Record Added Successfully!";
                return message;
            }
            else
            {
               message = "Error, Please Try Again";
               return message;
            }
           
        }
        ///////////
        ///

        public List<BloodInventoryStore> GetInventory(string search)
        {
            var inventoryList = _data_Access_Layer.GetInventory(search);
            //List<DonorModel> donorModelList = _DonorMapper.Map<List<Donor>, List<DonorModel>>(donorList);
            return inventoryList.ToList();
        }


        public BloodInventoryStore GetInventoryById(int id)
        {
            var inventory = _data_Access_Layer.GetInventryById(id);
            //DonorModel donorModel = _DonorMapper.Map<Donor, DonorModel>(donor);
            return inventory;
        }

        public void DeleteInventory(BloodInventoryStore inventory)
        {
            //Donor donor = _DonorMapper.Map<DonorModel, Donor>(donorModel);
            _data_Access_Layer.DeleteInventory(inventory);
        }


        public string UpdateInventoryRecord(string bloodTypeName, int quantity, BloodInventoryStore inventory)
        {
            string message;

            inventory.BloodTypeName = bloodTypeName; 
            inventory.Quantity = quantity;
            if (inventory != null)
            {
                _data_Access_Layer.UpdateInventoryRecord(inventory);
                _data_Access_Layer.Save();
                 message = "Record Updated Successfully!";
            }
            else 
            {
                message = "Record Not Found Please Try Again!";
            }
            return message;
        }
        ///////////////
        ///  Graph Data
        ///  
        public (Dictionary<string, int>, Dictionary<string, int>) GetGraphData()
        {
            return _data_Access_Layer.GetGraphData();
        }


    }
}