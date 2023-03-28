using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidations.Web.DTO
{
    public class CustomerDto
    {
        //aynı kullanmak istersek : 
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Email { get; set; }
        //public int Age { get; set; }

        //farklı kullanmak istersek : 
        public int Id { get; set; }
        public string Isim { get; set; }
        public string Eposta { get; set; }
        public int Yas { get; set; }
        //Customer'da GetFullName den dönen değeri FullName içine atacağız
        public string FullName { get; set; }
        //Flattening : CreditCard içindeki Number'ı al diyoruz.Automapper bunu algılayacak
        public string CreditCardNumber { get; set; }
        public DateTime CreditCardValidDate { get; set; }
    }
}
