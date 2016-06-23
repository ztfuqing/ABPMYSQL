/* Used by user and role permission settings. 
 */
appModule.directive('organizationTree', [
      function () {
          return {
              restrict: 'E',
              template: '<div class="organization-tree"></div>',
              scope: {
                  editData: '='
              },
              link: function ($scope, element, attr) {

                  var $tree = $(element).find('.organization-tree');
                  var treeInitializedBefore = false;
                  var inTreeChangeEvent = false;

                  $scope.$watch('editData', function () {
                      if (!$scope.editData) {
                          return;
                      }

                      initializeTree();
                  });

                  function initializeTree() {
                      if (treeInitializedBefore) {
                          $tree.jstree('destroy');
                      }

                      var treeData = _.map($scope.editData.organizations, function (item) {
                          return {
                              id: item.id,
                              parent: item.parentId != 0 ? item.parentId : "#",
                              text: item.displayName,
                              state: {
                                  opened: false,
                                  selected: $scope.editData.selectedId == item.id
                              }
                          };
                      });
                      console.log(treeData);
                      $tree.jstree({
                          'core': {
                              data: treeData
                          }
                      });

                      treeInitializedBefore = true;

                      //$tree.on("changed.jstree", function (e, data) {
                      //    if (!data.node) {
                      //        return;
                      //    }

                      //    var wasInTreeChangeEvent = inTreeChangeEvent;
                      //    if (!wasInTreeChangeEvent) {
                      //        inTreeChangeEvent = true;
                      //    }
                      //});
                  };
              }
          };
      }]);