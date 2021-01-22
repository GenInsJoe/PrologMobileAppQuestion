using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

// 
// SummaryAPI Controller
// Author: Joseph Bjorkman
// Last Edited: 1/22/2021
//

namespace SummaryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SummaryController : ControllerBase
    {
        private const string BASE_URL = "https://5f0ddbee704cdf0016eaea16.mockapi.io/organizations";
        private Organization[] Orgs;
        private const int DELAY = 20; // Value in seconds 
        private readonly ILogger<SummaryController> _logger;
        // For Error Checking
        private const string NF = "Not Found";
        private const string TO = "Timed Out";

        public SummaryController(ILogger<SummaryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get() => 
            GetSummary();

        public string GetSummary()
        {
            List<Summary> Summaries = new List<Summary>();
            string orgJson = Downloader(BASE_URL);
            Orgs = JsonConvert.DeserializeObject<Organization[]>(orgJson);

            foreach (Organization org in Orgs)
            {
                _logger.LogDebug($"Generating Summary for {org.name}");
                Summary orgSum = new Summary(org.id, org.name);

                string orgUserURL = $"/{org.id}/users";

                string userJson = Downloader(BASE_URL + orgUserURL);
                if (userJson == NF)
                {
                    Console.WriteLine($"ERROR: Database does not contain users for {org.name}");
                    continue;
                }
                if (userJson == TO)
                {
                    return "Your requset could not be completed due to too many requests to the server. Please try again.";
                }
                User[] users = JsonConvert.DeserializeObject<User[]>(userJson);
                orgSum.users = new List<UserSummary>();

                foreach (User user in users)
                {
                    _logger.LogDebug($"Pulling Phones for {user.name}");
                    UserSummary userSum = new UserSummary();
                    userSum.id = user.id;
                    userSum.email = user.email;
                    userSum.phoneCount = 0;


                    string userPhoneURL = $"/{user.id}/phones";
                    string phoneJson = Downloader(BASE_URL + orgUserURL + userPhoneURL);
                    if (userJson == NF)
                    {
                        Console.WriteLine($"ERROR: Database does not contain phones for {user.name}");
                        continue;
                    }
                    if (userJson == TO)
                    {
                        return "Your requset could not be completed due to too many requests to the server. Please try again.";
                    }

                    Phone[] phones = JsonConvert.DeserializeObject<Phone[]>(phoneJson);

                    foreach (Phone phone in phones)
                    {
                        userSum.phoneCount++;
                        orgSum.totalCount++;
                        if (phone.Blacklist)
                        {
                            orgSum.blacklistTotal += 1;
                        }
                    }

                    orgSum.users.Add(userSum);
                }

                Summaries.Add(orgSum);
            }

            return JsonConvert.SerializeObject(Summaries);
        }

        // Downloads a JSON string from a URL
        private string Downloader(string URL, int count = 0)
        {
            int delayedSec = count * DELAY;
            if (count == 10)
            {
                _logger.LogError("ERROR: we have waited for {0} seconds ({1} minutes)",
                    delayedSec, delayedSec/60);
                return TO;
            } 
            else if (count > 5)
            {
                _logger.LogWarning("We have waited {0}s and are still not able to download", count*DELAY);
            }
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var data = wc.DownloadString(URL);
                    return data.ToString();
                }
            }
            catch(WebException)
            {
                _logger.LogInformation("Sleeping to let the requests reset");
                Thread.Sleep(DELAY * 1000);
                _logger.LogInformation("Trying the download Again");
                return Downloader(URL, count++);
            }
                        
        }
    }
}
