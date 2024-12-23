//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BDWPF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Donation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Donation()
        {
            this.Report = new HashSet<Report>();
        }
    
        public int DonationID { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> DonorID { get; set; }
        public Nullable<int> BeneficiaryID { get; set; }
        public Nullable<int> ProgramID { get; set; }
    
        public virtual Beneficiary Beneficiary { get; set; }
        public virtual Donor Donor { get; set; }
        public virtual Program Program { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Report { get; set; }
    }
}
