﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClient.DTO
{
    public class Organization
    {
        public string Name { get; set; }
        public string OrganizationNumber { get; set; }
        public string Type { get; set; }
        public DateTime? LastChanged { get; set; }
        public DateTime? LastConfirmed { get; set; }
        public OfficialContactsResult OfficialContacts { get; set; }
        public Links _links { get; set; }
    }
}
