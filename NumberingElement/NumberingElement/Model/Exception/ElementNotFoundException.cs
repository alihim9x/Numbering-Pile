using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Exception
{
    class ElementNotFoundException : System.Exception
    {
        public override string Message  {get {return "Đối tượng không tồn tại";} } // Ghi đè thuộc tính Message của System.Exception
    }
    
}
