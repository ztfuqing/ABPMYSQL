/* Used by user and role permission settings. 
 */
appModule.directive('userTree', [
      function () {
          return {
              restrict: 'E',
              template: '<div class="user-tree"></div>',
              scope: {
                  editData: '='
              },
              link: function ($scope, element, attr) {

                  var $tree = $(element).find('.user-tree');
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

                      var treeData = _.map($scope.editData.departmentUsers, function (item) {
                          return {
                              id: item.id,
                              parent: item.parentId=="D" ?  '#':item.parentId,
                              text: item.displayName,
                              state: {
                                  opened: true,
                                  selected: _.contains($scope.editData.selectedUsers, item.id)
                              }
                          };
                      });

                      $tree.jstree({
                          'core': {
                              data: treeData
                          },
                          "types": {
                              "default": {
                                  "icon": "fa fa-folder tree-item-icon-color icon-lg"
                              },
                              "file": {
                                  "icon": "fa fa-file tree-item-icon-color icon-lg"
                              }
                          },
                          'checkbox': {
                              keep_selected_style: false,
                              three_state: false,
                              cascade: ''
                          },
                          plugins: ['checkbox', 'types']
                      });

                      treeInitializedBefore = true;

                      $tree.on("changed.jstree", function (e, data) {
                          if (!data.node) {
                              return;
                          }

                          var wasInTreeChangeEvent = inTreeChangeEvent;
                          if (!wasInTreeChangeEvent) {
                              inTreeChangeEvent = true;
                          }

                          var childrenNodes;

                          if (data.node.state.selected) {
                              selectNodeAndAllParents($tree.jstree('get_parent', data.node));

                              childrenNodes = $.makeArray($tree.jstree('get_children_dom', data.node));
                              $tree.jstree('select_node', childrenNodes);

                          } else {
                              childrenNodes = $.makeArray($tree.jstree('get_children_dom', data.node));
                              $tree.jstree('deselect_node', childrenNodes);
                          }

                          if (!wasInTreeChangeEvent) {
                              $scope.$apply(function () {
                                  $scope.editData.selectedUsers = getselectedUsersIds();
                              });
                              inTreeChangeEvent = false;
                          }
                      });
                  };

                  function selectNodeAndAllParents(node) {
                      $tree.jstree('select_node', node, true);
                      var parent = $tree.jstree('get_parent', node);
                      if (parent) {
                          selectNodeAndAllParents(parent);
                      }
                  };

                  function getselectedUsersIds() {
                      var usersIds = [];

                      var selectedUsers = $tree.jstree('get_selected', true);
                      for (var i = 0; i < selectedUsers.length; i++) {
                          if(selectedUsers[i].original.id.substring(0,1)!="D"&&selectedUsers[i].original.id.substring(0,1)!="#") {
                              usersIds.push(selectedUsers[i].original.id);
                          }
                      }

                      return usersIds;
                  };
              }
          };
      }]);