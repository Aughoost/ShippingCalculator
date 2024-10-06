using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace TSBLiving.Models
{
    public class Items
    {
        
        public string Value { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DispatchID { get; set; }
        public string CityofCust { get; set; }
        public string DispatchLoc { get; set; }
        public SelectList _DispatchLocation { get; set; }

        public SelectList DispatchLocation 
        {
            get
            {
                if (_DispatchLocation != null)
              
                    return _DispatchLocation;

                    return new SelectList(GetDispatchLoc(), "DispatchID", "DispatchLoc");
               

            }
            set
            {
                _DispatchLocation = value;
            }

        }

    

        private List<Models.Items> GetDispatchLoc()
        {
            var dispatch  = new List<Items>();
            dispatch.Add(new Items() { DispatchID = 1, DispatchLoc = "AKL"});
            dispatch.Add(new Items() { DispatchID = 2, DispatchLoc = "WLG" });
            dispatch.Add(new Items() { DispatchID = 3, DispatchLoc = "CHCH" });

            return dispatch;

        }

        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string WeightRange { get; set; }
        public string FinalShippingCost{ get; set; }

        public string SubTotalWeight { get; set; }
        public string Location { get; set; }

    }
}