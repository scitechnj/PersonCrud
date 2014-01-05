using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonCrud.Data;
using PersonCrud.Entites;
using PersonCrud.Models;

namespace PersonCrud.Controllers
{
    public class PersonController : Controller
    {
        private PersonCrudDb _personDb = new PersonCrudDb(
            ConfigurationManager.AppSettings["PersonCrudConnectionString"]);

        public ActionResult Index()
        {
            var allPeople = _personDb.Get();
            var viewModel = new PersonsViewModel { Persons = allPeople };
            return View(viewModel);
        }

        public ActionResult Show(int id)
        {
            var person = _personDb.GetById(id);
            var viewModel = new PersonViewModel { Person = person };
            return View(viewModel);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Person person)
        {
            _personDb.Add(person);
            return Redirect("/person/Show?id=" + person.Id);
        }

        public ActionResult Edit(int id)
        {
            var person = _personDb.GetById(id);
            return View(new PersonViewModel { Person = person });
        }

        [HttpPost]
        public ActionResult Update(Person person)
        {
            _personDb.Edit(person);
            return Redirect("/person/show?id=" + person.Id);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _personDb.Delete(id);
            return RedirectToAction("Index");
        }

    }


}
