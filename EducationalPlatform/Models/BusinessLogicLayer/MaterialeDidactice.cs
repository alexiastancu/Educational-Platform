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
    
    public partial class MaterialeDidactice
    {
        public int MaterialID { get; set; }
        public Nullable<int> MaterieID { get; set; }
        public string NumeMateriale { get; set; }
        public string TipFisier { get; set; }
    
        public virtual Materii Materii { get; set; }
    }
}