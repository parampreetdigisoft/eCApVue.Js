var app = new Vue({
    el: "#loginForm",
    data: {
        username: "",
        password: "",
        rememberMe: false,
        reason: "",
        modelState: {},
        message: "",
        returnURL :""
    },
    methods: {
        clearData: function () {
            var vm = this;
            vm.username = "";
            vm.password = "";
            vm.rememberMe = false;
            vm.message = "";
        },
        showPassword: function () {
            var e = document.getElementById("password-input");
            "password" === e.type ? (e.type = "text") : (e.type = "password");
        },
        blurEventHandler: function () {
            var self = this;
            self.modelState = {};
            self.message = "";
        },
        loginConfirm: function () {
            var self = this;
            //Clear the modelState before submission
            self.modelState = {};
            self.message = "";

            //validate UserName
            if (self.username === '') {
                self.$set(self.modelState, "UserName", 'Username is required');
                return;
            }

            //validate password
            if (self.password === '') {
                self.$set(self.modelState, "Password", 'Password is required');
                return;
            }

            var newModel = {
                username: self.username,
                password: self.password,
                rememberMe: self.rememberMe,
                returnURL: self.returnURL == null ? "" : self.returnURL
            }

            $.post("/profile/Login", newModel)
                .done(function (data) {
                    if (data.status) {
                        window.location.href = data.callBackUrl;
                    } else {
                        if (data.message) {
                            self.message = data.message;
                        }
                        else {
                            self.modelState = {};
                            if (data.modalStateError.length > 0) {
                                $.each(JSON.parse(data.modalStateError) , function (index, item) {
                                    let message = '';
                                    let stateErrors = item.Value;
                                    if (stateErrors) {
                                        $.each(stateErrors, function (j, stateError) {
                                            if (j > 0) {
                                                message += "; ";
                                            }
                                            message += stateError;
                                        });
                                        self.$set(self.modelState, item.Key, message);
                                    }
                                });
                            }
                        }
                    }
                }).fail(function () {
                    // .error("Can not update this bug.");
                }).always(function () {
                    //self.clearData();
                });
        }
    }
    , created() {
        let uri = window.location.search.substring(1);
        let params = new URLSearchParams(uri);
        this.returnURL = params.get("returnUrl");
    },
});