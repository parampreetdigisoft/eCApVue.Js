var app = new Vue({
    el: '#myuser',
    data: {
        loading: true,
        selectedRows: [],
        selectedEmailRows: [],
        selectedViewRows: [],
        UsersData:[],
        User: [],
        Products: [],
        AddOns:[],
        TotalUsers: [],
        GetAllUser:[],
        currentPage: 1,
        maxPerPage: 10,
        filterOptions: [
            { id: 'Name', Name: "Name" },
            { id: 'UserID', Name: "UserID" },
            { id: 'Creation_Date', Name: "Creation Date" },
            { id: 'Client_Group', Name: "Client Group" },
            { id: 'Group_Admin', Name: "Group Admin" },
            { id: 'User_No', Name: "User No" }
        ],
        selected: "Name",
        searchBy: "",
        message:"",
    },
    computed: {
        totalResults() {
            return Object.keys(this.TotalUsers).length;
        },
        pageCount() {
            this.paginationTotalPages = Math.ceil(this.totalResults / this.maxPerPage);
            return paginationTotalPages;
        },
        pageOffest() {
            return this.maxPerPage * this.currentPage;
        }
    },
    methods: {
        getUserPageURL(lang, roleType) {
            var language = "en";
            if (lang == "J")
                language = "ja-JP";
           else if (lang == "E")
                language = "en";
            else if (lang == "K")
                language = "ko-KR";
            return roleType == "CA" ? "/CAadmin/Home" : "/user/MemberHome?lang=" + language;
        },
        GetAllManageUser: function (selectedValue, pageNumber) {
            var vm = this;
            vm.loading = true;
            vm.message = "";
            $.ajax({ url: "/User/GetAllManageUser", method: "GET", data: { "filterBy": selectedValue, "searchBy": vm.searchBy } })
                .done(function (data) {
                    vm.loading = false;
                    vm.TotalUsers = [];
                    vm.TotalUsers = data;
                    vm.paginatedUsers(pageNumber);
                    initPagination(data.length);
                }).fail(function () {
                    vm.loading = false;
                    vm.TotalUsers = [];
                    vm.UsersData = [];
                });
        },
        loadMore(pageNumber) {
            this.message = "";
            this.paginatedUsers(pageNumber);
        },
        paginatedUsers(pageNumber) {

            if (pageNumber > 0) {
                this.currentPage = pageNumber;
            }

            this.UsersData = [];
            this.UsersData = this.TotalUsers.slice(this.currentPage == 1 ? 0 : ((this.currentPage - 1) * this.maxPerPage) + 1,
                ((this.currentPage - 1) * this.maxPerPage) + this.maxPerPage);
        },
        tableSort(event) {
            var data_column = event.target.getAttribute("data-column");
            var data_sort = event.target.getAttribute("data-sortorder");

            this.UsersData.sort(predicateBy(data_sort, data_column));

            if (data_sort === 'asc') {
                event.target.setAttribute("data-sortorder", 'desc');
            }
            else {
                event.target.setAttribute("data-sortorder", 'asc');
            }
        },
        applyFilter() {
            this.GetAllManageUser(this.selected, 0);
        },
        deleteUsers() {
            this.message = "";
            $("#deleteRecordModal").modal("show");
        },
        saveEmails() {
            if (this.selectedEmailRows.length > 0) {
            }
        },
        saveViewResults() {
            console.log(this.selectedViewRows)
        },
        ConfirmDeleteUsers() {

            var self = this;
            self.message = "";
            var newModel = {
                ids: self.selectedRows
            };
            $.post("/user/DeleteUser", newModel)
                .done(function (data) {
                    $("#deleteRecordModal").modal("hide");
                    self.message = data.message;
                    self.GetAllManageUser("", self.currentPage);
                }).fail(function () {
                    // .error("Can not update this bug.");
                }).always(function () {
                    //self.clearData();
                });

        },
        UserInfo(id) {
                var vm = this;
              $.ajax({ url: "/User/GetManageUserDetails", method: "GET", data: { "userId": id } })
                  .done(function (data) {
                      vm.User = data.user;
                      vm.Products = data.products;
                      vm.AddOns = data.addons;
                        $("#UserInformatioModal").modal("show")
                    }).fail(function () {
                        vm.loading = false;
                        vm.User = [];
                        vm.Products = [];
                        vm.AddOns = [];
                    });
        },
        markTestComplete(purchaseId, userId) {
            var vm = this;
            $.ajax({ url: "/User/MarkTestComplete", method: "post", data: { "purchaseId": purchaseId, "userId": userId } })
                .done(function (data) {
                    if (data) {
                        $.each(vm.Products, function (index, value) {
                            if (purchaseId === value.purchaseId)
                                value.statusCode = 2;
                        });
                    }
                }).fail(function () {
                    vm.loading = false;
                });
        }
     
    },
    created: function () {
        this.GetAllManageUser("",0);
    }
});


var initPagination = function (totalCounts) {
    if (typeof totalCounts !== 'undefined') {
        $.jqPaginator('#pagination1', {
            totalCounts: totalCounts,
            pageSize: 10,
            visiblePages: 5,
            currentPage: 1,
            onPageChange: function (num, type) {
                app.loadMore(num);
            }
        });
    }
}
// for sorting table colummns
function predicateBy(direction, prop) {
    return function (a, b) {
        if (direction == 'asc') {
            if (a[prop] > b[prop]) {
                return 1;
            } else if (a[prop] < b[prop]) {
                return -1;
            }
        } else {
            if (a[prop] > b[prop]) {
                return -1;
            } else if (a[prop] < b[prop]) {
                return 1;
            }
        }
        return 0;
    }
}
$(function () {
    initPagination();
})