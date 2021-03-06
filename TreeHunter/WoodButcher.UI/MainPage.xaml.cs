﻿using Request.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WoodButcher.Request;

namespace WoodButcher.UI
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

        private int _ofPages;

        /// <summary>
        /// Viewable count of page results.
        /// </summary>
        private int _pageSize = 10;

        private SortInfo _sortInfo;

        private int _currentResults;

        private ResultEditor _resultEditor;

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
                SortAscending = true,
                SortProperty = "ID"
            };

            _resultEditor = new ResultEditor(_results, _sortInfo);

            UpdateResults();

            UpdateFellingComboBox();
            UpdateDistrictComboBox();
        }

        public void Click_ColumnHeader(object sender, RoutedEventArgs e)
        {
            var columnHeader = e.OriginalSource as GridViewColumnHeader;
            var bindingPropertyName = ((System.Windows.Data.Binding)columnHeader.Column.DisplayMemberBinding).Path.Path;

            _sortInfo.SortProperty = bindingPropertyName;
            _sortInfo.SortAscending = !_sortInfo.SortAscending;

            UpdateResults();
        }

        private void ButtonClick_SwitchPage(object sender, RoutedEventArgs e)
        {
            var pageBtn = (Button)sender;
            if (pageBtn == LeftClickBtn)
                _currentPage = (_currentPage - 1) < 1 ? _ofPages : _currentPage -= 1;
            if (pageBtn == RightClickBtn)
                _currentPage = (_currentPage + 1) > _ofPages ? 1 : _currentPage += 1;

            _sortInfo.PageIndex = _currentPage;
            _sortInfo.PageSize = _pageSize;

            UpdateResults();
        }

        /// <summary>
        /// Update result ListView and PageLabel with help of page size and index.
        /// </summary>
        private void UpdateResults()
        {
            // Filter and sort the filtered results.
            var sortedResults = _resultEditor.GetPreparedResults(); 
                
            // Update ListView and paging label dependent of current page index and page size.
            _currentResults = sortedResults.Count();
            var ofPages = _currentResults / _pageSize == 0 && _currentResults > 0 ? 1 : (_currentResults / _pageSize) + 1;
            _currentPage = ofPages > _currentPage && _currentPage < 0 ? 1
                : ofPages < _currentPage ? 1
                : _currentResults < 1 ? 1
                : _currentPage;
            _ofPages = ofPages;
            //var viewAblePageNumber = _currentPage
            PageLabel.Content = $"{_currentPage}/{ofPages}";
            DataListView.ItemsSource = sortedResults
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize);
        }

        /// <summary>
        /// Prepare Felling Combo box with corresponding start values.
        /// </summary>
        private void UpdateFellingComboBox()
        {
            var fellingGrounds = _results.GroupBy(res => res.FaellGrund)
                .Select(group => group.FirstOrDefault().FaellGrund);
            FellingReasonComboBox.ItemsSource = fellingGrounds;
            FellingReasonComboBox.SelectedIndex = fellingGrounds.Count() < 1 ? -1 : 0;
        }

        /// <summary>
        /// Prepare District Combo Box with corresponding start values.
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
            UpdateCheckboxValues();
            
            UpdateResults();
        }

        private void UpdateCheckboxValues()
        {
            var sortProps = new Dictionary<TreeSortProperty, string>();
            if(FellingReasonCheckBox.IsChecked == true)
            {
                sortProps.Add(TreeSortProperty.FellingGround, (string)FellingReasonComboBox.SelectedItem);
            }
            if(StreetCheckBox.IsChecked == true)
            {
                sortProps.Add(TreeSortProperty.District, (string)BezirkComboBox.SelectedItem);
            }

            _sortInfo.FilterProperties = sortProps;
        }

        private void Unchecked_PropertyFilterCheckBox(object sender, RoutedEventArgs e)
        {
            UpdateCheckboxValues();

            _sortInfo.PageIndex = _currentPage;

            UpdateResults();
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

            UpdateResults();
        }

        private void SelectionChanged_ComboBox(object sender, SelectionChangedEventArgs e)
        {
            UpdateCheckboxValues();
            
            UpdateResults();
        }
    }
}
