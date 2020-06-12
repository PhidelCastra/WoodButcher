using Request.Models;
using System;
using System.Collections.Generic;
using WoodButcher.Request;
using Xunit;

namespace WoodButcher.XUnitTests.RequestTests
{
    public class ResultEditorTests : IDisposable
    {
        /// <summary>
        /// Default TreeInfos for tests.
        /// </summary>
        private TreeInfo _treeInfo1;
        private TreeInfo _treeInfo2;
        private TreeInfo _treeInfo3;

        /// <summary>
        /// SortInfo for testing, should have some default values.
        /// </summary>
        private SortInfo _sortInfo;

        /* Constructor includes preconditions. */
        public ResultEditorTests()
        {
            _treeInfo1 = new TreeInfo
            {
                ID = 1,
                BaumNummer = "1A",
                Datum = "1.1.2011",
                FaellGrund = "FellingReason1",
                Gattung = "TreeType1",
                HausNummer = "1",
                Ortsteil = "some location1",
                PLZ = 1,
                Strasse = "someStreet1",
            };
            _treeInfo2 = new TreeInfo
            {
                ID = 2,
                BaumNummer = "2B",
                Datum = "2.1.2011",
                FaellGrund = "FellingReason2",
                Gattung = "TreeType2",
                HausNummer = "2",
                Ortsteil = "Some location2",
                PLZ = 2,
                Strasse = "someStreet2"
            };
            _treeInfo3 = new TreeInfo
            {
                ID = 3,
                BaumNummer = "3C",
                Datum = "3.1.2011",
                FaellGrund = "FellingReason3",
                Gattung = "TreeType3",
                HausNummer = "3",
                Ortsteil = "some location3",
                PLZ = 3,
                Strasse = "someStreet3"
            };

            _sortInfo = new SortInfo
            {
                PageIndex = 1,
                PageSize = 5,
                SortProperty = "ID",
                FilterProperties = null,
                FilterValue = null,
                SortAscending = true,
            };
        }

        public void Dispose()
        {
            _sortInfo = null;
            _treeInfo1 = null;
            _treeInfo2 = null;
            _treeInfo3 = null;
        }

        #region GetPreparedResults -function
        [Fact]
        public void GetPreparedResults_TreeInfoListNotEmptyAndSortInfoContainsOnlyDefaultValues_ShouldBeReturnListSortedAscendingByID()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Asserts
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].ID, _treeInfo1.ID);
            Assert.Equal(results[1].ID, _treeInfo2.ID);
            Assert.Equal(results[2].ID, _treeInfo3.ID);
        }

        [Fact]
        public void GetPreparedResults_ChangeSortDirectionInSortInfoToDescending_ShouldBeReturnListSortedDescendingByID()
        {
            // Arrange 
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            _sortInfo.SortAscending = false;

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].ID, _treeInfo3.ID);
            Assert.Equal(results[1].ID, _treeInfo2.ID);
            Assert.Equal(results[2].ID, _treeInfo1.ID);
        }

        [Fact]
        public void GetPreparedResults_SetSortInfoSortPropertyToPLZ_ShouldBeReturnListSortedAscendingByPLZ()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            _sortInfo.SortProperty = "PLZ";

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].PLZ, _treeInfo1.PLZ);
            Assert.Equal(results[1].PLZ, _treeInfo2.PLZ);
            Assert.Equal(results[2].PLZ, _treeInfo3.PLZ);
        }

        [Fact]
        public void GetPreparedResults_SetSortInfoSortPropertyToOrtsteil_ShouldBeReturnListSortedAscendingByOrtsteil()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            _sortInfo.SortProperty = "Ortsteil";

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].Ortsteil, _treeInfo1.Ortsteil);
            Assert.Equal(results[1].Ortsteil, _treeInfo2.Ortsteil);
            Assert.Equal(results[2].Ortsteil, _treeInfo3.Ortsteil);
        }

        [Fact]
        public void GetPreparedResults_SetSortInfoSortProperty_ShouldBeReturnListSortedAscendingByStrasse()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            _sortInfo.SortProperty = "Strasse";

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].Strasse, _treeInfo1.Strasse);
            Assert.Equal(results[1].Strasse, _treeInfo2.Strasse);
            Assert.Equal(results[2].Strasse, _treeInfo3.Strasse);
        }

        [Fact]
        public void GetPreparedResults_SetSortInfoSortProperty_ShouldBeReturnListSortedAscendingByHausNummer()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            _sortInfo.SortProperty = "HausNummer";

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].HausNummer, _treeInfo1.HausNummer);
            Assert.Equal(results[1].HausNummer, _treeInfo2.HausNummer);
            Assert.Equal(results[2].HausNummer, _treeInfo3.HausNummer);
        }

        [Fact]
        public void GetPreparedResults_SetSortInfoSortProperty_ShouldBeReturnListSortedAscendingByBaumNummer()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            _sortInfo.SortProperty = "BaumNummer";

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].BaumNummer, _treeInfo1.BaumNummer);
            Assert.Equal(results[1].BaumNummer, _treeInfo2.BaumNummer);
            Assert.Equal(results[2].BaumNummer, _treeInfo3.BaumNummer);
        }

        [Fact]
        public void GetPreparedResults_SetSortInfoSortProperty_ShouldBeReturnListSortedAscendingByGattung()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            _sortInfo.SortProperty = "Gattung";

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].Gattung, _treeInfo1.Gattung);
            Assert.Equal(results[1].Gattung, _treeInfo2.Gattung);
            Assert.Equal(results[2].Gattung, _treeInfo3.Gattung);
        }

        [Fact]
        public void GetPreparedResults_SetSortInfoSortProperty_ShouldBeReturnListSortedAscendingByFaellGrund()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            _sortInfo.SortProperty = "FaellGrund";

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].FaellGrund, _treeInfo1.FaellGrund);
            Assert.Equal(results[1].FaellGrund, _treeInfo2.FaellGrund);
            Assert.Equal(results[2].FaellGrund, _treeInfo3.FaellGrund);
        }

        [Fact]
        public void GetPreparedResults_SetSortInfoSortProperty_ShouldBeReturnListSortedAscendingByDatum()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo2, _treeInfo3, _treeInfo1 };

            _sortInfo.SortProperty = "Datum";

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Equal(3, results.Count);

            Assert.Equal(results[0].Datum, _treeInfo1.Datum);
            Assert.Equal(results[1].Datum, _treeInfo2.Datum);
            Assert.Equal(results[2].Datum, _treeInfo3.Datum);
        }

        [Fact]
        public void GetPreparedResults_PrepareSortInfoFilterDict_ShouldReturnListFilteredByDistrictValue()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo1, _treeInfo2, _treeInfo3 };

            _sortInfo.FilterProperties = new Dictionary<TreeSortProperty, string> { { TreeSortProperty.District, "Some location2" } };

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Single(results);

            Assert.Equal(results[0], _treeInfo2);
        }

        [Fact]
        public void GetPreparedResults_PrepareSortInfoFilterDict_ShouldReturnListFilteredByFellingReasonValue()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo1, _treeInfo2, _treeInfo3 };

            _sortInfo.FilterProperties = new Dictionary<TreeSortProperty, string> { { TreeSortProperty.FellingGround, "FellingReason3" } };

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            var results = resultEditor.GetPreparedResults();

            // Assert
            Assert.Single(results);

            Assert.Equal(results[0], _treeInfo3);
        }
        #endregion
    }
}
