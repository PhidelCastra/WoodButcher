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

        public SortDirection SortDirection { get; set; }

        public string FilterValue { get; set; }

        public TreeSortProperty? FilterProperty { get; set; }

        public string SortProperty { get; set; }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
