using BudgetCalculatorFrontEnd.Models;
using BudgetCalculatorFrontEnd.Models.Domain;
using BudgetCalculatorFrontEnd.Models.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace BudgetCalculatorFront_End.Controllers
{
    public class TransactionController : Controller
    {
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Transaction(int id, int houseHoldId)
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
                .GetAsync($"http://localhost:64310/api/Transaction/View/{id}")
                .Result;

            var data = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var transaction = JsonConvert.DeserializeObject<List<TransactionViewModel>>(data);
                ViewBag.houseHoldId = houseHoldId;
                return View(transaction);
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Create(int id, int bankAccountId)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {cookie.Value}");

            string url = $"http://localhost:64310/api/Category/HouseHoldCategories/{id}";
            var response = httpClient
                .GetAsync(url)
                .Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data);

                var editViewModel = new CreateTransactionViewModel();

                ViewBag.id = id;
                editViewModel.Category = new SelectList(result, nameof(CategoryViewModel.Id), nameof(CategoryViewModel.Name));
                return View(editViewModel);
            }

            return View();
        }

        //Create Category
        [HttpPost]
        public ActionResult Create(int id, int bankAccountId, CreateTransactionViewModel model)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                var httpClient1 = new HttpClient();

                httpClient1.DefaultRequestHeaders.Add("Authorization",
                    $"Bearer {cookie.Value}");

                string url1 = $"http://localhost:64310/api/Category/HouseHoldCategories/{id}";
                var response1 = httpClient1
                    .GetAsync(url1)
                    .Result;
                var data1 = response1.Content.ReadAsStringAsync().Result;

                var result1 = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data1);
                model.Category = new SelectList(result1, nameof(CategoryViewModel.Id), nameof(CategoryViewModel.Name));
                return View(model);
            }

            var token = cookie.Value;

            //Url and parameters to post
            var url = "http://localhost:64310/api/Transaction/Create";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization",
               $"Bearer {token}");

            //var data = httpClient.GetStringAsync(url).Result;

            var title = model.Title;
            var description = model.Description;
            var category = model.CategoryId.ToString();
            var amount = model.Amount.ToString();
            var date = model.TransactionDate.ToString(); ;
            //model.DateCreated = DateTime.Now;
            //Parameters list with KeyValue pair
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Title", title));
            parameters.Add(new KeyValuePair<string, string>("Description", description));
            parameters.Add(new KeyValuePair<string, string>("BankAccountId", bankAccountId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("HouseHoldId", id.ToString()));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", category));
            parameters.Add(new KeyValuePair<string, string>("Amount", amount));
            parameters.Add(new KeyValuePair<string, string>("TransactionDate", date));

            //Encoding the parameters before sending to the API
            var encodedParameters = new FormUrlEncodedContent(parameters);

            //Calling the API and storing the response
            var response = httpClient.PostAsync(url, encodedParameters).Result;

            //Read the response
            var data = response.Content.ReadAsStringAsync().Result;

            //Convert the data back into an object
            var result = JsonConvert.DeserializeObject<Transaction>(data);

            return RedirectToAction("BankAccount", "BankAccount", new { id = id });
        }


        [HttpGet]
        public ActionResult Edit(int? id, int houseHoldId)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {cookie.Value}");

            string url = $"http://localhost:64310/api/Transaction/Transaction/{id}";
            var response = httpClient
                .GetAsync(url)
                .Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<TransactionViewModel>(data);

                var editViewModel = new EditTransactionViewModel();

                string url1 = $"http://localhost:64310/api/Category/HouseHoldCategories/{houseHoldId}";
                var response1 = httpClient
                    .GetAsync(url1)
                    .Result;
                var data1 = response1.Content.ReadAsStringAsync().Result;

                var result1 = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data1);
                editViewModel.Title = result.Title;
                editViewModel.Description = result.Description;
                editViewModel.TransactionDate = result.TransactionDate;
                editViewModel.Void = result.IsVoided;
                editViewModel.Amount = result.Amount;
                ViewBag.id = id;
                ViewBag.bankAccountId = result.BankAccountId;
                editViewModel.Category = new SelectList(result1, nameof(CategoryViewModel.Id), nameof(CategoryViewModel.Name));
                ViewBag.houseHoldId = houseHoldId;
                return View(editViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit(int? id, int houseHoldId, EditTransactionViewModel model)
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
                new KeyValuePair<string, string>("Title", model.Title));
            parameters.Add(
                new KeyValuePair<string, string>("Description", model.Description));
            parameters.Add(
                new KeyValuePair<string, string>("TransactionDate", model.TransactionDate.ToString()));
            parameters.Add(
                new KeyValuePair<string, string>("Amount", model.Amount.ToString()));
            parameters.Add(
                new KeyValuePair<string, string>("CategoryId", model.CategoryId.ToString()));
            parameters.Add(
                new KeyValuePair<string, string>("IsVoided", model.Void.ToString().ToLower()));


            var encodedParameters = new FormUrlEncodedContent(parameters);

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {cookie.Value}");

            var response = httpClient
                .PostAsync($"http://localhost:64310/api/Transaction/Edit/{id}",
                    encodedParameters)
                .Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Transaction", "Transaction", new { id = model.BankAccountId, houseHoldId = houseHoldId });
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
                string url1 = $"http://localhost:64310/api/Category/HouseHoldCategories/{houseHoldId}";
                var response1 = httpClient
                    .GetAsync(url1)
                    .Result;
                var data1 = response1.Content.ReadAsStringAsync().Result;
                var result1 = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data1);
                model.Category = new SelectList(result1, nameof(CategoryViewModel.Id), nameof(CategoryViewModel.Name));
                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
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
                .PostAsync($"http://localhost:64310/api/Transaction/Delete/{id}",
                   null)
                .Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Transaction", new { id = id });
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
                TempData["Message"] = "It looks like this Transaction was deleted";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Create a log for the error message
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
        }


        public ActionResult Void(int id)
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
                .PostAsync($"http://localhost:64310/api/Transaction/Void/{id}",
                   null)
                .Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Transaction");
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
                TempData["Message"] = "It looks like this Transaction was deleted";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View("Error");
            }
        }
    }
}