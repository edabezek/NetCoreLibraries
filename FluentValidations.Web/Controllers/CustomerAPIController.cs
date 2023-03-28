using AutoMapper;
using FluentValidation;
using FluentValidationApp.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidations.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<Customer> _customerValidator;
        //private readonly IMapper _mapper;
        public CustomerAPIController(AppDbContext context, IValidator<Customer> customerValidator)
        {
            _customerValidator = customerValidator;
            _context = context;
            //_mapper = mapper;
        }
        //[Route("MappingOrnek")]
        //[HttpGet]
        //public IActionResult MappingOrnek()
        //{
        //    Customer customer = new Customer { Id = 1, Name = "Fatih", Email = "fcakiroglu@outlook.com", Age = 23, CreditCard = new CreditCard { Number = "1234", ValidDate = DateTime.Now } };

        //    return Ok(_mapper.Map<CustomerDto>(customer));
        //}

        // GET: api/CustomerAPI
        //[HttpGet]
        //public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
        //{
        //    List<Customer> customers = await _context.Customers.ToListAsync();
        //    //return _mapper.Map<List<CustomerDto>>(customers);
        //}

        // GET: api/CustomerAPI
        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            List<Customer> customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        // GET: api/CustomerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/CustomerAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CustomerAPI
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            var result = _customerValidator.Validate(customer);

            if (!result.IsValid)
            {
                //BadRequest : client tarafından gönderilen model hatalı tekrar gönder demek için yazdık
                //result üzerinden hataları alacağız , yani sadece property ismi ve onun için yazdığımız hata gelecek.
                return BadRequest(result.Errors.Select(x => new { property = x.PropertyName, error = x.ErrorMessage }));
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/CustomerAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
