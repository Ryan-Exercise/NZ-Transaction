using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Vensa.Api.Entities
{
    public partial class Transaction : EntityBase
    {
        public long ConsumerId { get; set; }
        public DateTime DateTime { get; set; }
        public long TransactionType { get; set; }
        public long TransactionMethod { get; set; }
        public decimal Value { get; set; }
    }
}
