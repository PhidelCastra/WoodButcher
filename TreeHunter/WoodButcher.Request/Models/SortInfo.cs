using System;
using System.Collections.Generic;
using System.Text;
using WoodButcher.Request;

namespace Request.Models
{
    public class SortInfo
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool SortAscending { get; set; } = true;

        public string FilterValue { get; set; }

        public string SortProperty { get; set; }

        public Dictionary<TreeSortProperty, string> FilterProperties { get; set; } 
    }
}
