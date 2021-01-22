using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// 
// SummaryAPI Data Objects
// Author: Joseph Bjorkman
// Last Edited: 1/18/2021
//

namespace SummaryAPI
{
    public class Summary
    {
        public int id { get; set; }
        public string name { get; set; }
        public int blacklistTotal { get; set; }
        public int totalCount { get; set; }
        public List<UserSummary> users { get; set; }

        public Summary(int i, string n)
        {
            id = i;
            name = n;
            blacklistTotal = 0;
            totalCount = 0;
        }
    }

    public class UserSummary
    {
        public int id { get; set; }
        public string email { get; set; }
        public int phoneCount { get; set; }
    }
    public class Organization
    {
        public int id { get; set; }
        public string createdAt { get; set; }
        public string name { get; set; }

    }

    public class User
    {
        public int id { get; set; }
        public int organizationId { get; set; }
        public string name { get; set; }
        public string createdAt { get; set; }
        public string email { get; set; }
    }

    public class Phone
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string createdAt { get; set; }
        public string IMEI { get; set; }
        public bool Blacklist { get; set; }
    }
}
