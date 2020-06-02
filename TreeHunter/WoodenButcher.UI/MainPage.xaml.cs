using SharedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WoodButcher.Request.Models;
using WoodenButcher.Request;
using WoodenButcher.UI.Models;

namespace WoodenButcher.UI
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        /// <summary>
        /// List with all requested TreeInfos, should be set after the programm starts.
        /// </summary>
        private List<TreeInfo> _results;

        /// <summary>
        /// Index of current viewable page.
        /// </summary>
        private int _currentPage = 1;

        /// <summary>
        /// Viewable count of page results.
        /// </summary>
        private int _pageSize = 10;

        private SortInfo _sortInfo;

        private int _currentResults;

        /* Constructor */
        public MainPage()
        {
            InitializeComponent();

            var requestHandler = new RequestHandler<TreeInfo>();
            _results = requestHandler.GetRequestResult();

            _sortInfo = new SortInfo
            {
                FilterValue = null,
                PageIndex = _currentPage,
                PageSize = _pageSize,
                SortDirection = SortDirection.Ascending,
                SortProperty = "ID"
            };

            UpdateResults(_sortInfo);

            UpdateFellingComboBox();
            UpdateDistrictComboBox();
        }

        private void ButtonClick_SwitchPage(object sender, RoutedEventArgs e)
        {
            var pageBtn = (Button)sender;
            if(pageBtn == LeftClickBtn)
                _currentPage = (_currentPage-1) < 1 ? (_currentResults / _pageSize) : _currentPage -= 1;
            if(pageBtn == RightClickBtn)
                _currentPage = (_currentPage+1) > (_currentResults / _pageSize) ? 1 : _currentPage += 1;

            _sortInfo.PageIndex = _currentPage;
            _sortInfo.PageSize = _pageSize;

            UpdateResults(_sortInfo);
        }

        /// <summary>
        /// Update result ListView and PageLabel with help of page size and index.
        /// </summary>
        private void UpdateResults(SortInfo sortInfo)
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
                ? GetFiltered(sortInfo, _results) 
                : _results.AsEnumerable();

            // Filter by combo box property value.
            filteredResults = sortInfo.FilterProperty == null 
                ? filteredResults 
                : sortInfo.FilterProperty switch
            {
                TreeSortProperty.District => filteredResults.Where(tree => tree.Ortsteil == (string)BezirkComboBox.SelectedItem),
                TreeSortProperty.FellingGround => filteredResults.Where(tree => tree.FaellGrund == (string)FellingReasonComboBox.SelectedItem),
                _ => throw new Exception("TreeSortProperty was not exists!") 
            };

            // Sort the filtered results.
            var sortedResults = sortInfo.SortDirection == SortDirection.Ascending
                ? filteredResults.OrderBy(tree => sortPropertyExpr)
                : filteredResults.OrderByDescending(tree => sortPropertyExpr);

            // Update ListView and paging label dependent of current page index and page size.
            _currentResults = sortedResults.Count();
            var ofPages = _currentResults / _pageSize == 0 && _currentResults > 0 ? 1 : (_currentResults / _pageSize);
            _currentPage = ofPages > _currentPage && _currentPage < 0 ? 1
                : ofPages < _currentPage ? 1
                : _currentResults < 1 ? 1
                : _currentPage;
            //var viewAblePageNumber = _currentPage
            PageLabel.Content = $"{_currentPage}/{ofPages}";
            DataListView.ItemsSource = sortedResults
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize);
        }

        /// <summary>
        /// Prepare Felling Combo box with corresponding values.
        /// </summary>
        private void UpdateFellingComboBox()
        {
            var fellingGrounds = _results.GroupBy(res => res.FaellGrund)
                .Select(group => group.FirstOrDefault().FaellGrund);
            FellingReasonComboBox.ItemsSource = fellingGrounds;
            FellingReasonComboBox.SelectedIndex = fellingGrounds.Count() < 1 ? -1 : 0;
        }

        /// <summary>
        /// Prepare District Combo Box with corresponding values.
        /// </summary>
        private void UpdateDistrictComboBox()
        {
            var districts = _results.GroupBy(res => res.Ortsteil)
                .Select(group => group.FirstOrDefault().Ortsteil);
            BezirkComboBox.ItemsSource = districts;
            BezirkComboBox.SelectedIndex = districts.Count() < 1 ? -1 : 0;
        }

        private void IsChecked_PropertyFilterCheckBox(object sender, RoutedEventArgs e)
        {
            TreeSortProperty? filterProperty = null;
            if(FellingReasonCheckBox == (CheckBox)sender) 
            {
                StreetCheckBox.IsChecked = false;
                filterProperty = TreeSortProperty.FellingGround;
            } else if(StreetCheckBox == (CheckBox)sender)
            {
                FellingReasonCheckBox.IsChecked = false;
                filterProperty = TreeSortProperty.District;
            }

            _sortInfo.FilterProperty = filterProperty;

            UpdateResults(_sortInfo);
        }

        private void Unchecked_PropertyFilterCheckBox(object sender, RoutedEventArgs e)
        {
            _sortInfo.FilterProperty = null;
            _sortInfo.PageIndex = _currentPage;
        
            UpdateResults(_sortInfo);
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

        private void KeyUp_SearchFilter(object sender, KeyEventArgs e)
        {
            var searchBox = (TextBox)sender;

            _sortInfo.FilterValue = searchBox.Text;
            _sortInfo.PageIndex = (_currentResults / _pageSize) > _currentPage ? 1 : _currentPage;

            UpdateResults(_sortInfo);
        }

        private void SelectionChanged_ComboBox(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            _sortInfo.FilterProperty = cb == BezirkComboBox && StreetCheckBox.IsChecked == true 
                ? TreeSortProperty.District 
                : cb == FellingReasonComboBox && FellingReasonCheckBox.IsChecked == true
                ? TreeSortProperty.FellingGround
                : _sortInfo.FilterProperty;
            
            UpdateResults(_sortInfo);
        }
    }
}
