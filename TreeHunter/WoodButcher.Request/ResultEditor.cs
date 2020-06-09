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

        public List<TreeInfo> UpdateResults()
        {
            // Gets sort property expression
            Expression<Func<TreeInfo, object>> sortPropertyExpr = _sortInfo.SortProperty switch
            {
                "Strasse" => tree => tree.Strasse,
                "ID" => tree => tree.ID,
                _ => throw new Exception("Property is not available")
            };

            // Filter by search input field
            var filteredResults = !string.IsNullOrEmpty(_sortInfo.FilterValue)
                ? GetFiltered(_sortInfo, TreeInfos)
                : TreeInfos.AsEnumerable();

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

            // Sort the filtered results.
            var sortedResults = _sortInfo.SortDirection == SortDirection.Ascending
                ? filteredResults.OrderBy(tree => sortPropertyExpr)
                : filteredResults.OrderByDescending(tree => sortPropertyExpr);
            return sortedResults.ToList();


            // Update ListView and paging label dependent of current page index and page size.
            //_currentResults = sortedResults.Count();
            //var ofPages = _currentResults / _pageSize == 0 && _currentResults > 0 ? 1 : (_currentResults / _pageSize) + 1;
            //_currentPage = ofPages > _currentPage && _currentPage < 0 ? 1
            //    : ofPages < _currentPage ? 1
            //    : _currentResults < 1 ? 1
            //    : _currentPage;
            //_ofPages = ofPages;
            ////var viewAblePageNumber = _currentPage
            //PageLabel.Content = $"{_currentPage}/{ofPages}";
            //DataListView.ItemsSource = sortedResults
            //    .Skip((_currentPage - 1) * _pageSize)
            //    .Take(_pageSize);
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
