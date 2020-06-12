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
        /// <summary>
        /// List with all TreeInfo -objects.
        /// </summary>
        public List<TreeInfo> TreeInfos { get; set; }

        /// <summary>
        /// SortInfo includes informations of sort and filter process for a list with TreeInfos.
        /// </summary>
        private SortInfo _sortInfo;

        /* Constructor */
        public ResultEditor(List<TreeInfo> treeInfos, SortInfo sortInfo) 
        {
            TreeInfos = treeInfos;
            _sortInfo = sortInfo;
        }

        /// <summary>
        /// Prepare a filtered, sorted TreeInfo List of this.TreeInfos, dependent of the sort info -values.
        /// </summary>
        /// <returns>The filtered, sorted List with tree infos.</returns>
        public List<TreeInfo> GetPreparedResults()
        {
            // Filter by search input field
            var filteredResults = !string.IsNullOrEmpty(_sortInfo.FilterValue)
                ? GetFiltered(_sortInfo, TreeInfos).AsQueryable()
                : TreeInfos.AsQueryable();

            if (_sortInfo.FilterProperties != null)
                _sortInfo.FilterProperties.ToList().ForEach(kV =>
                {
                    filteredResults = kV.Key switch
                    {
                        TreeSortProperty.FellingGround => filteredResults.Where(tree => tree.FaellGrund == kV.Value),
                        TreeSortProperty.District => filteredResults.Where(tree => tree.Ortsteil == kV.Value),
                        _ => throw new Exception("TreeSortProperty was not exist!")
                    };
                });
            
            // Gets sort property expression
            Expression<Func<TreeInfo, object>> sortPropertyExpr = _sortInfo.SortProperty switch
            {
                "ID" => tree => tree.ID,
                "PLZ" => tree => tree.PLZ,
                "Strasse" => tree => tree.Strasse,
                "HausNummer" => tree => tree.HausNummer,
                "BaumNummer" => tree => tree.BaumNummer,
                "Gattung" => tree => tree.Gattung,
                "FaellGrund" => tree => tree.FaellGrund,
                "Ortsteil" => tree => tree.Ortsteil,
                "Datum" => tree => tree.Datum,
                _ => throw new Exception("Property is not available")
            };

            // Sort the filtered results.
            var sortedResults = _sortInfo.SortAscending == true
                ? filteredResults.OrderBy(sortPropertyExpr)
                : filteredResults.OrderByDescending(sortPropertyExpr);
            return sortedResults.ToList();
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
