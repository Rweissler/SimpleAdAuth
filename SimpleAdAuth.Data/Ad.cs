﻿using System;

namespace SimpleAdAuth.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

    }
}
