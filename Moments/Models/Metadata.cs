//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Moments.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Metadata
    {
        public int Id { get; set; }
        public string DC_Title { get; set; }
        public string DC_Description { get; set; }
        public int DC_Creator { get; set; }
        public string DC_Publisher { get; set; }
        public string DC_Keywords { get; set; }
        public string DC_Type { get; set; }
    
        public virtual Login Login { get; set; }
    }
}
