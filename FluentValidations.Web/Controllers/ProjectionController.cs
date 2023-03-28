using AutoMapper;
using FluentValidations.Web.DTO;
using FluentValidations.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidations.Web.Controllers
{
    public class ProjectionController : Controller
    {
        private readonly IMapper _mapper;
        public ProjectionController(IMapper mapper)
        {
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(EventDateDto eventDateDto)
        {
            EventDate eventDate = _mapper.Map<EventDate>(eventDateDto);
            ViewBag.date = eventDate.Date.ToShortDateString();
            return View();
        }
    }
}
