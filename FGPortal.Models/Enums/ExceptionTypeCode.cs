using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models.Enums
{
    public enum ExceptionTypeCode
    {
        [Display(Name = "late")]
        Exception_Late,
        [Display(Name = "late-1")]
        Exception_Late1,
        [Display(Name = "late-2")]
        Exception_Late2,
        [Display(Name = "late-3")]
        Exception_Late3,
        [Display(Name = "missed-stop")]
        Exception_Missed_Stop,
        [Display(Name = "out-of-sequence")]
        Exception_Out_Of_Sequence,
        [Display(Name = "missing-pod")]
        Exception_Missing_POD,
        [Display(Name = "real")]
        Exception_REAL,

    }
}
