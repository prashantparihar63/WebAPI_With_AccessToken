//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAPI_With_Token.DAL
{
    using System;
    
    public partial class sp_GetEmployee_Result
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Gender { get; set; }
        public Nullable<long> DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> CityId { get; set; }
        public string CityName { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
