using BudgetCalculatorFrontEnd.Models;
using BudgetCalculatorFrontEnd.Models.Domain;
using BudgetCalculatorFrontEnd.Models.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace BudgetCalculatorFront_End.Controllers
{
    public class CategoryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Category(int id)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {cookie.Value}");

            var response = httpClient
                .GetAsync($"http://localhost:64310/api/Category/View/{id}")
                .Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var categories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data);

                return View(categories);
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.id = id;
            return View();
        }

        //Create Category
        [HttpPost]
        public ActionResult Create(int id, CreateEditCategoryViewModel model)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Index");
            }

            var token = cookie.Value;

            var url = "http://localhost:64310/api/Category/Create";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization",
               $"Bearer {token}");


            var name = model.Name;
            var description = model.Description;
            model.DateCreated = DateTime.Now;

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", name));
            parameters.Add(new KeyValuePair<string, string>("Description", description));
            parameters.Add(new KeyValuePair<string, string>("HouseholdId", id.ToString()));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            var data1 = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<Categories>(data1);
            return RedirectToAction("Category", "Category", new { id = id });
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {cookie.Value}");

            string url = $"http://localhost:64310/api/Category/GetCategory/{id}";
            var response = httpClient
                .GetAsync(url)
                .Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<CategoryViewModel>(data);

                var editViewModel = new CreateEditCategoryViewModel();
                editViewModel.Name = result.Name;
                editViewModel.Description = result.Description;
                ViewBag.id = id;
                return View(editViewModel);
            }
            else
            {
                return RedirectToAction("Category", new { id = id });
            }
        }

        [HttpPost]
        public ActionResult Edit(int? id, CreateEditCategoryViewModel model)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var parameters = new List<KeyValuePair<string, string>>();

            parameters.Add(
                new KeyValuePair<string, string>("Name", model.Name));
            parameters.Add(
                new KeyValuePair<string, string>("Description", model.Description));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {cookie.Value}");

            var response = httpClient
                .PostAsync($"http://localhost:64310/api/Category/Edit/{id}",
                    encodedParameters)
                .Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                string url = $"http://localhost:64310/api/Category/GetCategory/{id}";
                var response1 = httpClient
                    .GetAsync(url)
                    .Result;
                var data = response1.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<CategoryViewModel>(data);
                return RedirectToAction("Category", "Category", new { id = result.HouseHoldId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var errors = JsonConvert.DeserializeObject<BudgetCalculatorFrontEnd.Models.Domain.Error>(data);

                foreach (var key in errors.ModelState)
                {
                    foreach (var error in key.Value)
                    {
                        ModelState.AddModelError(key.Key, error);
                    }
                }
                ViewBag.id = id;
                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return View("Error");
            }
            else
            {
                //Create a log for the error message
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View(model);
            }
        }


        public ActionResult Delete(int id)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {cookie.Value}");

            var response = httpClient
                .PostAsync($"http://localhost:64310/api/Category/Delete/{id}",
                   null)
                .Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Category", new { id = id });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var errors = JsonConvert.DeserializeObject<Error>(data);

                foreach (var key in errors.ModelState)
                {
                    foreach (var error in key.Value)
                    {
                        ModelState.AddModelError(key.Key, error);
                    }
                }
                ViewBag.Message = errors.Message;
                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["Message"] = "It looks like this Category was deleted";
                return RedirectToAction("Category");
            }
            else
            {
                //Create a log for the error message
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
        }
    }

}
