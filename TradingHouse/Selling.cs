//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradingHouse
{
    using System;
    using System.Collections.Generic;
    
    public partial class Selling
    {
        public int id { get; set; }
        public int id_product { get; set; }
        public System.DateTime date { get; set; }
        public double amount { get; set; }
        public Nullable<decimal> cost { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
