using Request.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WoodButcher.Request
{
    public class ResultEditor
    {
        public List<TreeInfo> TreeInfos { get; set; }

        private SortInfo _sortInfo;

        public ResultEditor(List<TreeInfo> treeInfos, SortInfo sortInfo) 
        {
            TreeInfos = treeInfos;
            _sortInfo = sortInfo;
        }

        public void UpdateResults(SortInfo sortInfo)
        {
            // Gets sort property expression
            Expression<Func<TreeInfo, object>> sortPropertyExpr = sortInfo.SortProperty switch
            {
                "Strasse" => tree => tree.Strasse,
                "ID" => tree => tree.ID,
                _ => throw new Exception("Property is not available")
            };

            // Filter by search input field
            var filteredResults = !string.IsNullOrEmpty(sortInfo.FilterValue)
                ? GetFiltered(sortInfo, TreeInfos)
                : TreeInfos;

        }

        /// <summary>
        /// Gets filtered result by sortInfo filter value.
        /// </summary>
        /// <param name="sortInfo">SortInfo includes filter value which should be checked.</param>
        /// <param name="treeInfos">List of TreeInfo objects to filtering.</param>
        /// <returns>Filtered List with TreeInfo objects.</returns>
        private IEnumerable<TreeInfo> GetFiltered(SortInfo sortInfo, List<TreeInfo> treeInfos)
        {
            var filterValue = sortInfo.FilterValue;
            return treeInfos.Where(tree =>
                tree.ID.ToString().Contains(filterValue) ||
                tree.BaumNummer.Contains(filterValue) ||
                tree.FaellGrund.Contains(filterValue) ||
                tree.Gattung.Contains(filterValue) ||
                tree.HausNummer.Contains(filterValue) ||
                tree.Ortsteil.Contains(filterValue) ||
                tree.Strasse.Contains(filterValue) ||
                tree.PLZ.ToString().Contains(filterValue));
        }
    }
}
