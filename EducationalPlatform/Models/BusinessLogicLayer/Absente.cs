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
    
    public partial class Absente
    {
        public int AbsentaID { get; set; }
        public Nullable<int> ElevID { get; set; }
        public Nullable<int> MaterieID { get; set; }
        public Nullable<int> Semestru { get; set; }
        public Nullable<bool> Motivare { get; set; }
        public Nullable<System.DateTime> data_absenta { get; set; }
    
        public virtual Elevi Elevi { get; set; }
        public virtual Materii Materii { get; set; }
    }
}
