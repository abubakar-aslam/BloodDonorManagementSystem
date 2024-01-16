using Data_Access_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.GraphQL
{
    internal class BloodDonorType : ObjectType<Donor>
    {
        protected override void Configure(IObjectTypeDescriptor<Donor> descriptor)
        {
            descriptor.Field(b => b.DonorId).Type<IdType>();
            descriptor.Field(b => b.DonorName).Type<StringType>();
            descriptor.Field(b => b.DonorAge).Type<IntType>();
            descriptor.Field(b => b.BloodType).Type<StringType>();
            descriptor.Field(b => b.ContactNumber).Type<StringType>();
            descriptor.Field(b => b.EmailAddress).Type<StringType>();
            descriptor.Field(b => b.Address).Type<StringType>();
            // Add other fields as needed
        }
    }
}
  