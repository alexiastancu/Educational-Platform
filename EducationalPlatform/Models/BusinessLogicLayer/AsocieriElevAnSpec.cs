//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EducationalPlatform.Models.BusinessLogicLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class AsocieriElevAnSpec
    {
        public int AsocieriElevAnSpecID { get; set; }
        public Nullable<int> ElevID { get; set; }
        public Nullable<int> AnStudiu { get; set; }
        public Nullable<int> SpecializareID { get; set; }
    
        public virtual Elevi Elevi { get; set; }
        public virtual Specializari Specializari { get; set; }
    }
}
