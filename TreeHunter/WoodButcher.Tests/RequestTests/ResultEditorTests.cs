using Request.Models;
using System;
using System.Collections.Generic;
using WoodButcher.Request;
using Xunit;

namespace WoodButcher.Tests.RequestTests
{
    public class ResultEditorTests
    {
        #region Update function

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
                Strasse = "Street2"
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
                PLZ = 1,
                Strasse = "someStreet3"
            };

            _sortInfo = new SortInfo
            {
                PageIndex = 1,
                PageSize = 5,
                SortProperty = "ID",
                FilterProperties = null,
                FilterValue = null,
                SortDirection = SortDirection.Ascending,
            };
        }

        [Fact]
        public void Update_TreeInfoListNotEmptyAndSortInfoContainsOnlyDefaultValues_ShouldBeReturnListSortedAscendingByID()
        {
            // Arrange
            var treeInfoList = new List<TreeInfo> { _treeInfo3, _treeInfo2, _treeInfo1 };

            var resultEditor = new ResultEditor(treeInfoList, _sortInfo);

            // Act
            resultEditor.Update();

            // Asserts
            Assert.Equal(3, treeInfoList.Count);

            Assert.Equal(treeInfoList[0].ID, _treeInfo1.ID);
            Assert.Equal(treeInfoList[1].ID, _treeInfo2.ID);
            Assert.Equal(treeInfoList[2].ID, _treeInfo3.ID);
        }

        #endregion
    }
}
