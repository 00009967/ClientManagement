using ClientManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly string userApi = "http://ec2-3-125-122-50.eu-central-1.compute.amazonaws.com/api/users/";
        HttpClientHandler _clientHandler = new HttpClientHandler();
        User _oUser = new User();
        List<User> _oUsers = new List<User>();
        public UsersController()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        }
        public async Task<IActionResult> Index()
        {
            var users = await GetAllUsers();
            return View(users);
        }

        // GET: Jokes/ShowSearchForm
        public IActionResult ShowSearchForm()
        {
            return View();
        }

        // POST: Jokes/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            var foundUsers = await SearchUser(SearchPhrase);

            return View("Index", foundUsers);
        }

        public async Task<List<User>> SearchUser(string phrase)
        {
            _oUsers = new List<User>();

            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync(userApi+ "search?phrase=" + phrase))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oUsers = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                }
            }

            return _oUsers;
        }

        public async Task<List<User>> GetAllUsers()
        {
            _oUsers = new List<User>();
            
            using(var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync(userApi))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oUsers = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                }
            }
            
            return _oUsers;
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var user = await GetUserById(Id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        public async Task<User> GetUserById(int? Id)
        {
            _oUser = new User();


            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync(userApi+Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oUser = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            return _oUser;
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone")] User user)
        {
            if (ModelState.IsValid)
            {
                User entity = await AddUser(user);

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }




        public async Task<User> AddUser(User user)
        {
            _oUser = new User();


            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(userApi, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oUser = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            return _oUser;
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var user = await GetUserById(Id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,Email,Phone")] User user)
        {
            if (Id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UpdateUser(Id, user);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw e;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<User> UpdateUser(int Id, User user)
        {
            _oUser = new User();


            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(userApi+Id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oUser = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            return _oUser;
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var user = await GetUserById(Id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var message = await DeleteUser(Id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<string> DeleteUser(int Id)
        {
            string message = "";

            using (var httpClient = new HttpClient(_clientHandler))
            {

                using (var response = await httpClient.DeleteAsync(userApi+Id))
                {
                    message = await response.Content.ReadAsStringAsync();
                }
            }

            return message;
        }
    }
}
