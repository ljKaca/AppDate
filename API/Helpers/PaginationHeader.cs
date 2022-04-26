using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int curentPage, int itemsPerPage, int totalItems, int totlalPages)
        {
            CurentPage = curentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotlalPages = totlalPages;
        }

        public int CurentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotlalPages { get; set; }


    }
}
