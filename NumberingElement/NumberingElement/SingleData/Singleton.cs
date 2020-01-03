using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleData
{
    public class Singleton
    {
        public static Singleton Instance { get; set; }

        private RevitData revitData;
        public RevitData RevitData
        {
            get
            {
                if (revitData == null) revitData = new RevitData();
                return revitData;
            }
        }

        private ModelData modelData;
        public ModelData ModelData
        {
            get
            {
                if (modelData == null) modelData = new ModelData();
                return modelData;
            }
        }
        private FormData formData;
        public FormData FormData
        {
            get
            {
                if(formData == null)
                {
                    formData = new FormData();
                }
                return formData;
            }
        }
    }
}
