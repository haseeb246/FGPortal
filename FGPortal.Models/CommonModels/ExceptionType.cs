using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
    public class ExceptionType
    {
        public int ID { get; set; }

        public int? CustomerID { get; set; }

        public int? CourierID { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string BackgroundColorHex { get; set; }

        public bool System { get; set; }

        public bool Active { get; set; }

        public int Severity { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedWhen { get; set; }

        public int? OverrideTo { get; set; }

        public bool DisplayLegend { get; set; }

        public virtual Courier Courier { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<Exception> Exception { get; set; }

        public virtual ICollection<ExceptionType> ExceptionType1 { get; set; }

        public virtual ExceptionType ExceptionType2 { get; set; }

        public ExceptionType()
        {
            Exception = new HashSet<Exception>();
            ExceptionType1 = new HashSet<ExceptionType>();
        }
    }
}
