//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBaseProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class TraitsCats
    {
        public int idTraitCat { get; set; }
        public int idCat { get; set; }
        public int idTrait { get; set; }
    
        public virtual Cats Cats { get; set; }
        public virtual Traits Traits { get; set; }
    }
}
