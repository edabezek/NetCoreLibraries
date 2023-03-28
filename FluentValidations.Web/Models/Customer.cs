using FluentValidations.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidationApp.Web.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime? BirthDay { get; set; }
        //foreign key
        public IList<Address> Addresses { get; set; }
        public Gender Gender { get; set; }
        //bu metodu dto içindeki bir property'e eşleyeceğiz :
        public string GetFullName() 
        {
            return $"{Name}-{Email}-{Age}";
        }

        //Flattening işlemleri : Customer içindeki bu propery , CustomerDto içindeki propertylere çevireceğiz
        public CreditCard CreditCard { get; set; }
    }
}
