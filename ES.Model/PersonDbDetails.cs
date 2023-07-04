﻿using System.ComponentModel.DataAnnotations;

namespace ES.Model
{
    public class PersonDbDetails
    {
        [Key]
        public Guid personId { get; set; }
        public int? weight { get; set; }
        public byte? fromFloor { get; set; } // we are using byte beacuse the floor number will not be too big , so we are using byte
        public byte? toFloor { get; set; }
    }
}